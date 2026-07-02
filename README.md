# API Inventory

API REST desarrollada con **.NET 10** para la gestión de productos y categorías de un inventario.

El proyecto implementa operaciones CRUD completas, validaciones de negocio, eliminación lógica (Soft Delete), manejo centralizado de excepciones, pruebas unitarias y soporte para ejecución mediante Docker.

---

# Tecnologías utilizadas

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Swagger / OpenAPI
- Docker & Docker Compose
- xUnit

---

# Arquitectura

La solución está organizada siguiendo una arquitectura en capas.

```
Api-Inventory
│
├── src
│   ├── ApiInventory.Api
│   ├── ApiInventory.Application
│   ├── ApiInventory.Domain
│   └── ApiInventory.Infrastructure
│
└── tests
    └── ApiInventory.Tests
```

### Descripción de las capas

| Proyecto | Responsabilidad |
|----------|-----------------|
| ApiInventory.Api | Controllers, configuración, middleware y punto de entrada de la aplicación |
| ApiInventory.Application | DTOs, interfaces, excepciones, mapeos y reglas de aplicación |
| ApiInventory.Domain | Entidades del dominio |
| ApiInventory.Infrastructure | Entity Framework Core, DbContext, configuraciones y servicios |
| ApiInventory.Tests | Pruebas unitarias |

---

# Funcionalidades implementadas

## Gestión de Categorías

- Obtener todas las categorías
- Obtener una categoría por Id
- Crear categoría
- Modificar categoría
- Eliminación lógica

## Gestión de Productos

- Obtener todos los productos
- Obtener un producto por Id
- Crear producto
- Modificar producto
- Eliminación lógica

---

# Características implementadas

- CRUD completo
- Eliminación lógica (Soft Delete)
- Validaciones de negocio
- Manejo centralizado de excepciones
- Respuestas de error utilizando **ProblemDetails**
- Migraciones automáticas al iniciar la aplicación
- Docker Compose
- Swagger
- Pruebas unitarias

---

# Requisitos

## Ejecución local

- .NET 10 SDK
- SQL Server o SQL Server LocalDB
- Visual Studio 2026 (o compatible)

## Ejecución con Docker

- Docker Desktop

---

# Configuración

La cadena de conexión puede configurarse en:

```
appsettings.json
```

o mediante un archivo:

```
appsettings.Development.json
```

según el entorno utilizado.

> **Importante**
>
> Antes de ejecutar el proyecto es necesario configurar la cadena de conexión correspondiente al entorno donde se ejecutará la aplicación.

Ejemplo utilizando LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=Inventory;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

En caso de utilizar SQL Server, deberá reemplazarse la cadena de conexión por la correspondiente al servidor utilizado.

---

# Ejecución local

Restaurar dependencias:

```
dotnet restore
```

Compilar:

```
dotnet build
```

Ejecutar:

```
dotnet run --project src/ApiInventory.Api
```

Al iniciar la aplicación se ejecutarán automáticamente las migraciones pendientes.

Swagger estará disponible en:

```
https://localhost:xxxx/swagger
```

---

# Ejecución con Docker

Desde la carpeta raíz del proyecto ejecutar:

```
docker compose up --build
```

Docker iniciará automáticamente:

- SQL Server
- API
- Aplicación de migraciones

Swagger estará disponible en:

```
http://localhost:8080/swagger
```

---

# Pruebas realizadas

Se realizaron pruebas funcionales utilizando **Swagger** y **Postman** tanto en ejecución **local** como mediante **Docker Compose**, verificando el correcto funcionamiento de la API en ambos entornos.

## Entornos validados

- ✅ Ejecución local utilizando Visual Studio y SQL Server LocalDB.
- ✅ Ejecución mediante Docker Compose utilizando contenedores para la API y SQL Server.
- ✅ Aplicación automática de migraciones al iniciar la aplicación.
- ✅ Conexión correcta entre la API y la base de datos en ambos entornos.

---

## Categorías

### Casos exitosos

- Crear categoría.
- Obtener todas las categorías.
- Obtener una categoría por Id.
- Modificar una categoría.
- Eliminar una categoría sin productos asociados.

### Casos de error

- Obtener una categoría inexistente.
- Modificar una categoría inexistente.
- Eliminar una categoría inexistente.
- Intentar eliminar una categoría que posee productos asociados.

---

## Productos

### Casos exitosos

- Crear producto.
- Obtener todos los productos.
- Obtener un producto por Id.
- Modificar un producto.
- Eliminar un producto.

### Casos de error

- Obtener un producto inexistente.
- Modificar un producto inexistente.
- Eliminar un producto inexistente.
- Crear un producto utilizando una categoría inexistente.
- Modificar un producto utilizando una categoría inexistente.

---

## Validaciones verificadas

- Campos obligatorios.
- Longitud máxima de textos.
- Relaciones entre entidades.
- Integridad referencial.
- Manejo centralizado de excepciones.
- Eliminación lógica (Soft Delete).
- Respuestas HTTP apropiadas mediante `ProblemDetails`.
- Aplicación automática de migraciones.

---

# Pruebas unitarias

Se implementaron pruebas unitarias utilizando **xUnit**.

Se validaron los siguientes escenarios:

### Caso exitoso

- Creación correcta de un producto cuando la categoría existe.

### Caso de error

- Intentar crear un producto utilizando una categoría inexistente, verificando que se lance la excepción correspondiente.

---

# Decisiones de diseño

Durante el desarrollo se adoptaron las siguientes decisiones:

- Arquitectura en capas para separar responsabilidades.
- Uso de DTOs para desacoplar la API de las entidades de dominio.
- Eliminación lógica implementada mediante filtros globales (`HasQueryFilter`).
- Middleware para el manejo centralizado de excepciones.
- Uso de `ProblemDetails` para respuestas de error estandarizadas.
- Uso de `AsNoTracking()` en consultas de solo lectura para mejorar el rendimiento.
- Aplicación automática de migraciones al iniciar la aplicación.
- Docker Compose para simplificar la puesta en marcha del entorno.

---

# Posibles mejoras

Algunas mejoras que podrían incorporarse en futuras versiones:

- Autenticación y autorización mediante JWT.
- Paginación y filtrado de resultados.
- Logging estructurado (Serilog).
- Versionado de la API.
- Pruebas de integración.

---

# Supuestos y consideraciones

- La aplicación aplica automáticamente las migraciones de Entity Framework Core al iniciar.
- La eliminación de registros se implementó mediante **Soft Delete**, preservando la información en la base de datos.
- Las entidades eliminadas no son devueltas por las consultas gracias a filtros globales de Entity Framework Core.
- Para simplificar la evaluación, no se implementó autenticación ni autorización, ya que no formaban parte de los requerimientos de la prueba técnica.
- La solución fue desarrollada priorizando la separación de responsabilidades, mantenibilidad y facilidad de despliegue.

# Autor

Proyecto desarrollado como prueba técnica utilizando **.NET 10**, **Entity Framework Core** y **SQL Server**.
