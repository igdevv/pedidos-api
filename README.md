# Pedidos API — ASP.NET Core 10

API REST educativa de gestión de productos y pedidos, con autenticación JWT, ASP.NET Core Identity PasswordHasher, Entity Framework Core y SQL Server.

## Qué demuestra

- CRUD de productos y opción de incluir productos inactivos.
- Creación y consulta de pedidos mediante servicios y DTOs.
- Registro, inicio de sesión y emisión de tokens JWT.
- Mapeo de entidades a DTOs con AutoMapper.
- Migraciones de EF Core y persistencia SQL Server.

## Arquitectura

`Controllers/` recibe solicitudes HTTP; `Services/Interfaces` define contratos; `Services/Implementations` implementa casos de uso; `Data/` gestiona EF Core; `Entities/` representa el modelo; `DTOs/` separa contratos de transporte.

## Preparación

Requiere .NET SDK 10 y SQL Server o LocalDB. Configura `ConnectionStrings__DefaultConnection`, `Jwt__Key`, `Jwt__Issuer` y `Jwt__Audience` mediante variables de entorno o secretos locales. Utiliza una clave JWT larga y aleatoria; no publiques secretos.

```powershell
dotnet restore PedidosApi.slnx
dotnet build PedidosApi.slnx
dotnet ef database update --project PedidosApi
dotnet run --project PedidosApi
```

## Rutas destacadas

| Método | Ruta | Función |
| --- | --- | --- |
| POST | `/api/auth/registro` | Registrar usuario |
| POST | `/api/auth/login` | Obtener JWT |
| GET | `/api/productos` | Listar productos |
| GET | `/api/productos/{id}` | Consultar producto |
| POST | `/api/productos` | Crear producto |
| PUT | `/api/productos/{id}` | Actualizar producto |
| DELETE | `/api/productos/{id}` | Eliminar producto |
| GET | `/api/pedidos` | Listar pedidos (requiere autorización) |
| GET | `/api/pedidos/{id}` | Consultar pedido |
| POST | `/api/pedidos` | Crear pedido |

## Seguridad y alcance

**Proyecto de aprendizaje, no listo para producción.** La autorización no está aplicada de manera uniforme en todos los endpoints de pedidos y productos. No debe exponerse públicamente con datos reales sin reforzar permisos, pruebas, validación y configuración de secretos. El workflow de CI verifica compilación; no reemplaza pruebas de seguridad.

**Autor:** José Ignacio Suero.
