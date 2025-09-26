# PeliculasApiV2

# 🎬 PeliculasApi (ASP.NET Core + EF Core + Swagger)

API REST para el TP4: gestiona **Usuarios** (login, CRUD) y **Películas** (CRUD) con **SQL Server** mediante **Entity Framework Core**. Pensada para usarse desde una app **.NET MAUI Blazor Hybrid**.

## ✨ Características
- Endpoints REST para **Usuarios** y **Películas**.
- **Login** por email/usuario + contraseña (DTO simple).
- **EF Core** con SQL Server (Code First).
- **Swagger UI** para probar (GET/POST/PUT/DELETE).
- CORS habilitado para permitir la app cliente.
- Manejo básico de imágenes por **ruta/URL** (la app MAUI resuelve las imágenes).

## 🧱 Stack
- .NET 8 (ASP.NET Core Web API)
- Entity Framework Core (SQL Server)
- Swagger / Swashbuckle
- (Opcional) Despliegue en **Somee** + **SQL Server remoto**

## 📦 Estructura rápida
PeliculasApi/
├─ Controllers/
│ ├─ UsuarioController.cs
│ └─ PeliculasController.cs
├─ Data/
│ └─ AppDbContext.cs
├─ Models/
│ ├─ Usuario.cs
│ └─ Pelicula.cs
├─ Program.cs
└─ appsettings.json

bash
Copiar código

## 🔧 Configuración local

1) **Clonar**
```bash
git clone https://github.com/<tu-usuario>/PeliculasApi.git
cd PeliculasApi
Configurar conexión a SQL Server
Editá appsettings.json:
{
  "ConnectionStrings": {
    "Default": "workstation id=MauiPeliculasDB.mssql.somee.com;packet size=4096;user id=Nacho;pwd=12345678;data source=MauiPeliculasDB.mssql.somee.com;persist security info=False;initial catalog=MauiPeliculasDB;TrustServerCertificate=True;MultipleActiveResultSets=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}


🔐 Endpoints (resumen)
Usuarios
GET /api/Usuario — lista de usuarios (DTO)

GET /api/Usuario/{id}

POST /api/Usuario — crear (cuando no uses /register)

PUT /api/Usuario/{id}

DELETE /api/Usuario/{id}

POST /api/Usuario/login — login { userOrEmail, password }

POST /api/Usuario/register — registro { nombre, email, rol, imagen, contraseña } (si lo habilitaste)

Películas
GET /api/Peliculas

GET /api/Peliculas/{id}

POST /api/Peliculas

PUT /api/Peliculas/{id}

DELETE /api/Peliculas/{id}

🌐 CORS
En Program.cs se habilita CORS. Si la app MAUI apunta a producción (Somee), asegurá incluir ese origen.

🖼 Manejo de imágenes
La API no guarda archivos; almacena una cadena con:

URL absoluta (http/https), o

un nombre/ruta relativa (la app MAUI lo resuelve desde wwwroot/Imagenes).

🚀 Despliegue (Somee)
Publicar:

bash
Copiar código
dotnet publish -c Release
Subir a tu hosting (Somee) el contenido de bin/Release/net8.0/publish.

Configurar la connection string remota en appsettings.json del servidor.

Verificar Swagger en la URL pública.

🧪 Datos iniciales:

email: admin@peliculas.com

password: admin123

rol: Admin
