using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PedidosApi.Data;
using PedidosApi.DTOs.Auth;
using PedidosApi.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PedidosApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController: ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IPasswordHasher<Usuario> passwordHasher, IMapper mapper, 
            IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        [HttpPost("registro")]
        public async Task<IActionResult> Registro(RegistroUsuarioDTO registroUsuarioDTO)
        {
            var correoNormalizado = registroUsuarioDTO.Correo.Trim().ToLower();

            var correoExistente = await _context.Usuarios.AnyAsync(correo => correo.Correo == correoNormalizado);

            if (correoExistente)
            {
                return BadRequest("El correo proporcionado ya está registrado.");
            }

            var usuario = _mapper.Map<Usuario>(registroUsuarioDTO);

            usuario.Nombre = usuario.Nombre.Trim();
            usuario.Correo = correoNormalizado;

            usuario.PasswordHash = _passwordHasher.HashPassword(usuario, registroUsuarioDTO.Password);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return StatusCode(201, new
            {
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                usuario.Rol,
                usuario.FechaCreacion
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUsuarioDTO loginUsuarioDTO)
        {
            var correoNormalizado = loginUsuarioDTO.Correo.Trim().ToLower();

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(usuario => usuario.Correo == correoNormalizado);

            if(usuario is null)
            {
                return Unauthorized("Correo o contraseña incorrectos.");
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, loginUsuarioDTO.Password);

            if(resultado == PasswordVerificationResult.Failed)
            {
                return Unauthorized("Correo o contraseña incorrectos.");
            }

            var token = GenerarToken(usuario);

            return Ok(new
            {
                msg = "Inicio de sesión exitoso.",
                token,
                usuario.Id,
                usuario.Nombre,
                usuario.Correo,
                usuario.Rol
            });
        }

        private string GenerarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var minutosExpiracion = int.Parse(_configuration["Jwt:ExpirationMinutes"]!);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutosExpiracion),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

