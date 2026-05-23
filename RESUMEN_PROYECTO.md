# Resumen del Proyecto Final – Sistema de Quiosco/Cafetería
**Curso:** .NET Intermedio  
**Iniciales del estudiante:** LFAH  
**Puntaje mínimo requerido:** 60 pts

---

## Temática elegida

**Sistema de Quiosco/Cafetería** — API REST desarrollada en ASP.NET Core que gestiona productos, categorías, pedidos y usuarios de una cafetería. Incluye un frontend en React y un backend adicional en Laravel (versión previa).

---

## Arquitectura N-Capas

El proyecto respeta la separación `Controllers → Services → Repositories`:

```
QuioscoAPI/
├── Controllers/          ← Reciben HTTP, delegan al Service
│   ├── AdminController.cs
│   ├── AuthController.cs
│   ├── CategoriasController.cs
│   ├── PedidosController.cs
│   ├── ProductosController.cs
│   └── UsersController.cs
├── Services/             ← Lógica de negocio
│   ├── Interfaces/       ← IXxxServiceLFAH
│   ├── CategoriaServiceLFAH.cs
│   ├── PedidoServiceLFAH.cs
│   ├── ProductoServiceLFAH.cs
│   ├── UserServiceLFAH.cs
│   └── JwtServiceLFAH.cs
├── Repositories/         ← Acceso a datos (EF Core)
│   ├── Interfaces/       ← IXxxRepositoryLFAH
│   ├── CategoriaRepositoryLFAH.cs
│   ├── PedidoRepositoryLFAH.cs
│   ├── ProductoRepositoryLFAH.cs
│   └── UserRepositoryLFAH.cs
├── Models/               ← Entidades de la base de datos
├── DTOs/                 ← Objetos de transferencia de datos
├── Mapping/              ← Perfiles de AutoMapper
└── Middleware/           ← Middleware personalizado
```

---

## Requisitos cumplidos

### 1. Arquitectura N-Capas ✅
Separación correcta: los Controllers solo llaman a Services, los Services usan Repositories, y los Repositories acceden a la DB.

### 2. Uso de Interfaces ✅
Todos los Services y Repositories exponen una interfaz:

| Interfaz | Implementación |
|---|---|
| `ICategoriaServiceLFAH` | `CategoriaServiceLFAH` |
| `IPedidoServiceLFAH` | `PedidoServiceLFAH` |
| `IProductoServiceLFAH` | `ProductoServiceLFAH` |
| `IUserServiceLFAH` | `UserServiceLFAH` |
| `IJwtServiceLFAH` | `JwtServiceLFAH` |
| `ICategoriaRepositoryLFAH` | `CategoriaRepositoryLFAH` |
| `IPedidoRepositoryLFAH` | `PedidoRepositoryLFAH` |
| `IProductoRepositoryLFAH` | `ProductoRepositoryLFAH` |
| `IUserRepositoryLFAH` | `UserRepositoryLFAH` |

### 3. Principios SOLID ✅
- **S** – Cada clase tiene una única responsabilidad (Controller, Service, Repository separados)
- **O** – Las interfaces permiten extender sin modificar implementaciones existentes
- **D** – Inyección de dependencias vía constructor en todos los Controllers y Services

### 4. AutoMapper ✅
Perfil configurado en `MappingProfileLFAH.cs` con mapeos para:
- `User` ↔ `UserDtoLFAH`
- `Categoria` ↔ `CategoriaDtoLFAH`
- `Producto` → `ProductoDtoLFAH` (incluye `CategoriaNombre` desde relación)
- `Pedido` → `PedidoDtoLFAH` (incluye `Usuario` y lista de `Productos`)
- `PedidoProducto` → `PedidoProductoDtoLFAH` (incluye `ProductoNombre` y `Precio`)

### 5. Mínimo 5 tablas relacionadas ✅

| Tabla | Descripción |
|---|---|
| `Users` | Usuarios del sistema (admin y clientes) |
| `Categorias` | Categorías de productos (ej. bebidas, comidas) |
| `Productos` | Productos del quiosco con precio e imagen |
| `Pedidos` | Pedidos realizados por usuarios, con número de mesa |
| `PedidoProductos` | Tabla pivote — relación muchos a muchos entre Pedidos y Productos |

**Relaciones:**
- `User` → `Pedidos` (1:N)
- `Categoria` → `Productos` (1:N)
- `Pedido` ↔ `Productos` (N:M via `PedidoProducto`)

### 6. CRUD funcional ✅

| Entidad | GET todos | GET por ID | POST | PUT | DELETE |
|---|---|---|---|---|---|
| Categorias | ✅ | ✅ | ✅ | ✅ | ✅ |
| Productos | ✅ | ✅ | ✅ | ✅ | ✅ |
| Pedidos | ✅ | ✅ | ✅ | ✅ | ✅ |
| Users | ✅ | ✅ | ✅ | ✅ | ✅ |

### 7. GET Relacional ✅
El endpoint `GET /api/pedidos` retorna pedidos con datos de múltiples tablas relacionadas:
- Datos del **Pedido**
- Datos del **Usuario** que realizó el pedido
- Lista de **Productos** del pedido con nombre y precio (desde `PedidoProducto` → `Producto`)

También: `GET /api/productos/categoria/{id}` retorna productos con su categoría incluida.

### 8. Convención de nombres ✅
Todas las clases e interfaces terminan con las iniciales **LFAH**:
- `UserDtoLFAH`, `ProductoDtoLFAH`, `PedidoDtoLFAH`, etc.
- `CategoriaServiceLFAH`, `PedidoRepositoryLFAH`, etc.
- `ICategoriaServiceLFAH`, `IPedidoRepositoryLFAH`, etc.

---

## Criterios de Evaluación

| Criterio | Puntaje | Estado |
|---|---|---|
| Uso correcto de AutoMapper | 10 pts | ✅ Implementado con mapeos relacionales |
| CRUD funcional | 45 pts | ✅ CRUD completo en todas las entidades |
| GET relacional | 25 pts | ✅ Pedidos con Usuario y Productos anidados |
| Tablas relacionadas (mín. 5) | 10 pts | ✅ 5 tablas con relaciones 1:N y N:M |
| Entidad Relación (Lucidchart) | 10 pts | ⚠️ Pendiente exportar desde `diagrama_ER_BDQuiosco.csv` |
| **Total** | **100 pts** | |

---

## Endpoints disponibles

### Auth
- `POST /api/auth/register` — Registro de usuario
- `POST /api/auth/login` — Login, retorna JWT

### Categorias
- `GET /api/categorias`
- `GET /api/categorias/{id}`
- `POST /api/categorias`
- `PUT /api/categorias/{id}`
- `DELETE /api/categorias/{id}`

### Productos
- `GET /api/productos`
- `GET /api/productos/{id}`
- `GET /api/productos/categoria/{categoriaId}` ← **GET relacional**
- `POST /api/productos`
- `PUT /api/productos/{id}`
- `DELETE /api/productos/{id}`

### Pedidos (requiere autenticación JWT)
- `GET /api/pedidos` ← **GET relacional principal** (pedido + usuario + productos)
- `GET /api/pedidos/{id}`
- `GET /api/pedidos/mios` — historial del usuario autenticado
- `POST /api/pedidos`
- `PUT /api/pedidos/{id}` — marcar como completado
- `DELETE /api/pedidos/{id}`

### Users / Admin
- CRUD de usuarios
- Panel de administración

---

## Tecnologías utilizadas

- **Backend:** ASP.NET Core Web API, Entity Framework Core, AutoMapper, JWT Bearer Auth
- **Frontend:** React
- **Base de datos:** SQL Server / compatible con EF Core Migrations
- **Diagramas:** Lucidchart (ER), PlantUML (clases)
