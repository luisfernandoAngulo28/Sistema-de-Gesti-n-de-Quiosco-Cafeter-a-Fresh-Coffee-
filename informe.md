# INFORME DE PROYECTO FINAL

---

**INSTITUCIÓN:** Universidad Pública de El Alto — Posgrado UPEA  
**PROGRAMA:** Desarrollo Backend con .NET y C# — Nivel Intermedio  
**DOCENTE:** Lic. Ing. Limber Mamani Canaza  
**ESTUDIANTE:** Luis Fernando Angulo Heredia  
**INICIALES:** LFAH  
**FECHA:** Mayo 2026  

---

## 1. Introducción

El presente informe documenta el desarrollo del **Sistema de Gestión de Quiosco Cafetería "Fresh Coffee"**, proyecto final del curso de Desarrollo Backend con .NET y C# Nivel Intermedio.

El sistema fue desarrollado aplicando los principios y técnicas impartidos durante el curso: arquitectura en N-Capas, principios SOLID, uso de interfaces, AutoMapper, Entity Framework Core y JWT para autenticación. El objetivo fue construir una API REST completa y profesional que sea consumida por un frontend en React 18.

El sistema permite a los usuarios registrar pedidos de productos de cafetería por categorías, asignar mesas, registrar el método de pago y llevar un control del estado de cada pedido. Los administradores cuentan con un panel especializado para gestionar mesas, confirmar pedidos y consultar estadísticas del negocio.

---

## 2. Capturas del Sistema en Funcionamiento

### 2.1 Pantalla de Login

![Login del sistema](imagenes/01_login.png)

*Formulario de inicio de sesión con validación de credenciales y generación de token JWT.*

### 2.2 Vista Principal del Quiosco

![Vista principal del quiosco](imagenes/02_quiosco.png)

*Navegación por categorías en el sidebar izquierdo y productos en el área central. Resumen del pedido en el panel derecho.*

### 2.3 Modal de Selección de Producto

![Modal de producto](imagenes/03_modal_producto.png)

*Modal que se abre al hacer clic en un producto, permite ajustar la cantidad antes de agregar al pedido.*

### 2.4 Resumen del Pedido y Método de Pago

![Resumen del pedido](imagenes/04_resumen_pedido.png)

*Panel derecho con los productos seleccionados, total calculado, selección de mesa y método de pago.*

### 2.5 Confirmación de Pago (PAGADO)

![Página de confirmación](imagenes/05_gracias_pagado.png)

*Página de confirmación tras registrar el pedido y el pago exitosamente.*

### 2.6 Historial de Pedidos

![Mis pedidos](imagenes/06_mis_pedidos.png)

*Vista de historial de pedidos del usuario autenticado con estado y detalle de productos.*

### 2.7 Panel Admin — Órdenes

![Admin órdenes](imagenes/07_admin_ordenes.png)

*Vista administrativa de todas las órdenes con método de pago, mesa asignada y estado.*

### 2.8 Panel Admin — Gestión de Mesas

![Admin mesas](imagenes/08_admin_mesas.png)

*Panel de administración de mesas con estado en tiempo real: Libre, Ocupada o Inactiva.*

### 2.9 Documentación Swagger

![Swagger UI](imagenes/09_swagger.png)

*Documentación interactiva de la API generada automáticamente con Swashbuckle, agrupada por módulos.*

### 2.10 Modo Oscuro

![Modo oscuro](imagenes/10_dark_mode.png)

*Interfaz en modo oscuro, con soporte para preferencia del sistema operativo y persistencia en localStorage.*

---

## 4. Descripción del Sistema

**Fresh Coffee — Sistema Quiosco Cafetería** es una aplicación web full-stack compuesta por:

- **Backend:** ASP.NET Core 8 Web API con arquitectura N-Capas
- **Frontend:** React 18 + Vite con Tailwind CSS (modo oscuro incluido)
- **Base de datos:** PostgreSQL gestionado con Entity Framework Core (Code First)

### Funcionalidades principales

| Módulo | Descripción |
|---|---|
| Autenticación | Registro, login con JWT, roles usuario/admin |
| Catálogo | Navegación de productos por categoría con disponibilidad |
| Pedidos | Creación de pedidos con selección de mesa y productos |
| Pagos | Registro de cobro (efectivo, tarjeta, transferencia) |
| Panel Admin | Gestión de mesas, visualización de órdenes, estadísticas |
| Modo oscuro | Interfaz adaptable a preferencia del sistema operativo |

---

## 3. Arquitectura N-Capas

El proyecto sigue estrictamente la separación por capas trabajada en clase:

```
┌─────────────────────────────────────────┐
│              Controllers                │  ← Recibe peticiones HTTP
│  (AuthController, PedidosController…)  │
├─────────────────────────────────────────┤
│               Services                  │  ← Lógica de negocio
│  (PedidoServiceLFAH, PagoServiceLFAH…) │
├─────────────────────────────────────────┤
│             Repositories                │  ← Acceso a datos
│  (PedidoRepositoryLFAH, MesaRepo…)     │
├─────────────────────────────────────────┤
│        Entity Framework Core            │  ← ORM / DbContext
│           PostgreSQL                    │  ← Base de datos
└─────────────────────────────────────────┘
```

### Patrones y prácticas aplicadas

| Patrón | Aplicación |
|---|---|
| **Repository Pattern** | Abstrae el acceso a datos de la lógica de negocio |
| **Service Layer** | Encapsula la lógica de negocio y orquesta repositorios |
| **DTO Pattern** | Separa los modelos de BD de lo que se expone en la API |
| **Dependency Injection** | Todos los servicios y repos registrados en `Program.cs` |
| **AutoMapper** | Mapeo automático entre entidades y DTOs (`MappingProfileLFAH`) |
| **Middleware** | Manejo global de excepciones (`ExceptionMiddlewareLFAH`) |

---

## 4. Principios SOLID Aplicados

### S — Responsabilidad Única
Cada clase tiene una sola responsabilidad:
- `PedidoRepositoryLFAH` → solo accede a datos de pedidos
- `PedidoServiceLFAH` → solo contiene lógica de negocio de pedidos
- `PagosController` → solo maneja peticiones HTTP de pagos

### O — Abierto/Cerrado
Las clases están abiertas a extensión mediante interfaces:
```csharp
// Agregar un nuevo repositorio no modifica el servicio existente
public class PedidoServiceLFAH : IPedidoServiceLFAH
{
    private readonly IPedidoRepositoryLFAH _repository;
}
```

### I — Segregación de Interfaces
Cada entidad tiene su propia interfaz específica:
- `IMesaRepositoryLFAH`, `IMesaServiceLFAH`
- `IPagoRepositoryLFAH`, `IPagoServiceLFAH`
- No existe una interfaz genérica que obligue a implementar métodos innecesarios

### D — Inversión de Dependencias
Los controladores dependen de abstracciones, nunca de implementaciones concretas:
```csharp
// Correcto: depende de la interfaz
public PedidosController(IPedidoServiceLFAH service) { ... }

// NO de la clase concreta PedidoServiceLFAH
```

---

## 5. Convención de Nombres (Iniciales LFAH)

Todas las clases e interfaces del proyecto terminan con las iniciales **LFAH** (Luis Fernando Angulo Heredia):

| Tipo | Ejemplo |
|---|---|
| DTO | `MesaDtoLFAH`, `PagoDtoLFAH`, `CreatePedidoDtoLFAH` |
| Repository Interface | `IMesaRepositoryLFAH`, `IPagoRepositoryLFAH` |
| Repository | `MesaRepositoryLFAH`, `PagoRepositoryLFAH` |
| Service Interface | `IMesaServiceLFAH`, `IPedidoServiceLFAH` |
| Service | `MesaServiceLFAH`, `PedidoServiceLFAH` |
| Mapping | `MappingProfileLFAH` |
| Middleware | `ExceptionMiddlewareLFAH` |
| Seeder | `DataSeederLFAH` |

> Los métodos NO llevan iniciales: `GetAll()`, `Create()`, `Delete()`, etc.

---

## 6. AutoMapper

El archivo `MappingProfileLFAH.cs` centraliza todos los mapeos del sistema:

```csharp
// Mapeo con campo calculado — mesa "ocupada" si tiene pedidos pendientes
CreateMap<Mesa, MesaDtoLFAH>()
    .ForMember(dest => dest.Ocupada,
        opt => opt.MapFrom(src => src.Pedidos.Any(p => !p.Estado)));

// Mapeo relacional — pedido incluye datos del usuario y sus productos
CreateMap<Pedido, PedidoDtoLFAH>()
    .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.User))
    .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.PedidoProductos));

// Mapeo con dato de entidad relacionada
CreateMap<PedidoProducto, PedidoProductoDtoLFAH>()
    .ForMember(dest => dest.Precio,
        opt => opt.MapFrom(src => src.Producto.Precio));
```

Entidades mapeadas: `User`, `Categoria`, `Producto`, `Mesa`, `Pedido`, `PedidoProducto`, `Pago`

---

## 7. Base de Datos

**Motor:** PostgreSQL  
**ORM:** Entity Framework Core 8 (Code First + Migrations)  
**Migraciones:** 3 migraciones aplicadas

### Tablas del sistema (7 tablas — supera el mínimo de 5)

| Tabla | Campos clave | Descripción |
|---|---|---|
| `users` | `id`, `name`, `email`, `password`, `admin` | Usuarios con rol admin |
| `categorias` | `id`, `nombre`, `icono` | Categorías de productos |
| `productos` | `id`, `nombre`, `precio`, `imagen`, `disponible`, `categoria_id` | Productos del menú |
| `mesas` | `id`, `numero`, `capacidad`, `disponible` | Mesas del local |
| `pedidos` | `id`, `user_id`, `mesa_id`, `total`, `estado` | Pedidos realizados |
| `pedido_productos` | `id`, `pedido_id`, `producto_id`, `cantidad` | Detalle de productos por pedido |
| `pagos` | `id`, `pedido_id`, `metodo`, `monto`, `fecha_pago` | Registro de cobros |

### Diagrama Entidad-Relación

El diagrama de entidad-relación fue elaborado en **Lucidchart** e incluye las 7 entidades con sus atributos, tipos de datos y relaciones con multiplicidades.

> Ver diagrama adjunto: `diagramadeclases_EA.xml` (importable en Enterprise Architect)  
> Ver diagrama PlantUML: `diagramadeclases.puml`

---

## 8. CRUD Funcional

Se implementaron operaciones CRUD completas en las entidades principales:

### Operaciones por entidad

| Entidad | GET (todos) | GET (por ID) | POST | PUT | DELETE |
|---|---|---|---|---|---|
| Categorías | ✅ | ✅ | ✅ | ✅ | ✅ |
| Productos | ✅ | ✅ | ✅ | ✅ | ✅ |
| Mesas | ✅ | ✅ | ✅ | ✅ Admin | ✅ Admin |
| Pedidos | ✅ | ✅ | ✅ | ✅ Admin | ✅ |
| Pagos | ✅ | ✅ | ✅ | — | ✅ Admin |
| Usuarios | ✅ | ✅ | ✅ (registro) | — | — |

---

## 9. GET Relacional

El endpoint `GET /api/pedidos` es el más completo del sistema. Retorna en un solo response datos de **5 tablas relacionadas** simultáneamente:

```json
{
  "data": [
    {
      "id": 12,
      "total": 45.50,
      "estado": false,
      "created_at": "2026-05-22T22:00:00Z",
      "user": {
        "id": 3,
        "name": "Luis Fernando",
        "email": "lfah@upea.edu.bo"
      },
      "mesa": {
        "id": 2,
        "numero": 5,
        "capacidad": 4,
        "disponible": true
      },
      "pago": null,
      "productos": [
        {
          "id": 1,
          "nombre": "Café Americano",
          "precio": 15.00,
          "imagen": "cafe_americano",
          "pivot": { "cantidad": 2 }
        }
      ]
    }
  ]
}
```

**Tablas joinadas:** `pedidos` + `users` + `mesas` + `pagos` + `productos` (a través de `pedido_productos`)

Este mapeo es realizado por `PedidoServiceLFAH.MapResponse()` con carga de relaciones vía EF Core `Include()`.

---

## 10. Autenticación y Autorización

- **JWT Bearer** generado al hacer login en `POST /api/auth/login`
- Token almacenado en `localStorage` del frontend
- Claims incluidos en el token:

| Claim | Valor | Uso |
|---|---|---|
| `sub` | ID del usuario | Identificar al usuario en cada request |
| `email` | Correo | Información del usuario |
| `name` | Nombre | Mostrar en la interfaz |
| `admin` | `"true"` / `"false"` | Control de acceso a endpoints de admin |

- Endpoints protegidos con `[Authorize]`
- Endpoints de admin verifican: `User.FindFirst("admin")?.Value == "true"`
- Respuesta `403 Forbidden` si el usuario no tiene permisos de admin

---

## 11. Estructura del Proyecto

### Backend — `QuioscoAPI/`

```
Controllers/          → 8 controladores (Auth, Users, Categorias, Productos,
                         Mesas, Pedidos, Pagos, Admin)
Models/               → 7 modelos (User, Categoria, Producto, Mesa,
                         Pedido, PedidoProducto, Pago)
DTOs/                 → 15+ DTOs con sufijo LFAH
Repositories/
  Interfaces/         → 6 interfaces (I*RepositoryLFAH)
  *.cs                → 6 implementaciones (*RepositoryLFAH)
Services/
  Interfaces/         → 7 interfaces (I*ServiceLFAH)
  *.cs                → 7 implementaciones (*ServiceLFAH)
Mapping/              → MappingProfileLFAH.cs
Middleware/           → ExceptionMiddlewareLFAH.cs
Data/                 → QuioscoDbContext.cs, DataSeederLFAH.cs
Migrations/           → 3 migraciones EF Core
Program.cs            → Registro de DI, JWT, CORS, AutoMapper
```

### Frontend — `react-quiosco/src/`

```
context/              → QuioscoProvider (estado global del quiosco)
hooks/                → useQuiosco, useDarkMode, useAuth
layouts/              → Layout.jsx, AdminLayout.jsx
views/                → Inicio, MisPedidos, Gracias, Ordenes, AdminMesas
components/           → Sidebar, AdminSidebar, Resumen, Producto, ModalProducto
router.jsx            → Definición de rutas con React Router v6
```

---

## 12. Endpoints de la API

### Autenticación
| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| POST | `/api/auth/registro` | No | Registrar nuevo usuario |
| POST | `/api/auth/login` | No | Login → retorna JWT |
| GET | `/api/auth/me` | Sí | Datos del usuario autenticado |

### Categorías
| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/api/categorias` | No | Listar todas las categorías |
| GET | `/api/categorias/{id}` | No | Obtener categoría por ID |
| POST | `/api/categorias` | Sí | Crear categoría |
| PUT | `/api/categorias/{id}` | Sí | Actualizar categoría |
| DELETE | `/api/categorias/{id}` | Sí | Eliminar categoría |

### Productos
| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/api/productos` | No | Listar todos los productos |
| GET | `/api/productos/{id}` | No | Obtener producto por ID |
| GET | `/api/productos/categoria/{id}` | No | Productos por categoría |
| POST | `/api/productos` | Sí | Crear producto |
| PUT | `/api/productos/{id}` | Sí | Actualizar producto/disponibilidad |
| DELETE | `/api/productos/{id}` | Sí | Eliminar producto |

### Mesas
| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/api/mesas` | Sí | Listar mesas con estado de ocupación |
| GET | `/api/mesas/{id}` | Sí | Obtener mesa por ID |
| POST | `/api/mesas` | Sí | Crear mesa |
| PUT | `/api/mesas/{id}` | Admin | Actualizar mesa |
| DELETE | `/api/mesas/{id}` | Admin | Eliminar mesa |

### Pedidos
| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/api/pedidos` | Admin | Todos los pedidos (relacional — 5 tablas) |
| GET | `/api/pedidos/{id}` | Sí | Pedido por ID |
| GET | `/api/pedidos/mios` | Sí | Historial del usuario autenticado |
| POST | `/api/pedidos` | Sí | Crear pedido → `{ message, id }` |
| PUT | `/api/pedidos/{id}` | Admin | Completar pedido manualmente |
| DELETE | `/api/pedidos/{id}` | Sí | Eliminar pedido |

### Pagos
| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/api/pagos` | Admin | Listar todos los pagos |
| POST | `/api/pagos` | Sí | Registrar pago y marcar pedido como completado |
| DELETE | `/api/pagos/{id}` | Admin | Eliminar pago |

### Admin
| Método | Ruta | Auth | Descripción |
|---|---|---|---|
| GET | `/api/admin/estadisticas` | Admin | Total pedidos, pendientes, ingresos |

---

## 13. Tecnologías Utilizadas

### Backend
| Tecnología | Versión | Uso |
|---|---|---|
| ASP.NET Core | 8.0 | Framework principal de la API |
| Entity Framework Core | 8.0 | ORM para acceso a base de datos |
| Npgsql | 8.0 | Proveedor PostgreSQL para EF Core |
| AutoMapper | 13.0 | Mapeo entre entidades y DTOs |
| Microsoft.IdentityModel.Tokens | 7.0 | Generación y validación de JWT |
| BCrypt.Net-Next | 4.0 | Hashing de contraseñas |

### Frontend
| Tecnología | Versión | Uso |
|---|---|---|
| React | 18 | Biblioteca UI |
| Vite | 5 | Bundler y servidor de desarrollo |
| Tailwind CSS | 3.2.4 | Estilos utilitarios con modo oscuro |
| Axios | 1.6 | Cliente HTTP para consumir la API |
| SWR | 2.2 | Fetching de datos con caché |
| React Router | 6 | Enrutamiento del lado del cliente |
| react-toastify | — | Notificaciones toast |
| Lucide React | — | Iconografía |

### Base de datos e infraestructura
| Tecnología | Uso |
|---|---|
| PostgreSQL 16 | Motor de base de datos relacional |
| pgAdmin 4 | Administración de la base de datos |

---

## 14. Conclusiones

El desarrollo del Sistema Quiosco Cafetería permitió aplicar de manera integral los conocimientos adquiridos durante el curso de Desarrollo Backend con .NET y C# Nivel Intermedio:

1. **Arquitectura N-Capas:** La separación en Controllers, Services y Repositories demostró ser fundamental para mantener el código organizado, testeable y mantenible. Cada capa tiene una responsabilidad clara y bien definida.

2. **Principios SOLID:** La aplicación de responsabilidad única e inversión de dependencias facilitó el crecimiento del sistema. Agregar el módulo de Mesas y Pagos no requirió modificar el código existente, sino solo crear nuevas implementaciones.

3. **AutoMapper:** Eliminó el código repetitivo de mapeo manual y permitió transformaciones complejas (como el campo calculado `Ocupada` en mesas) de forma declarativa y centralizada.

4. **JWT y Roles:** La implementación de autenticación con claims personalizados (`admin`) demostró cómo proteger endpoints de manera granular sin necesidad de una tabla de roles compleja.

5. **Entity Framework Core Code First:** Las migraciones permitieron evolucionar el esquema de base de datos de forma controlada, desde la estructura inicial hasta la incorporación de Mesas y Pagos.

6. **Desafíos técnicos resueltos:** El proyecto presentó retos reales como la colisión de caché en SWR, la serialización snake_case en la comunicación frontend-backend, y la prevención del flash de tema en modo oscuro — problemas que se resolvieron con soluciones concretas y fundamentadas.

El resultado es una API REST completa, documentada con Swagger, con autenticación JWT, roles de usuario, 7 tablas relacionadas, CRUD funcional en todas las entidades y endpoints relacionales que consolidan datos de múltiples tablas en una sola respuesta.

---

*Informe elaborado por Luis Fernando Angulo Heredia — Posgrado UPEA — Mayo 2026*
