# Orión - Sistema de Gestión de Mantenimiento 🚀

Orión es una solución integral para la gestión de mantenimiento industrial, diseñada bajo estándares de **Clean Architecture** y principios **SOLID**. El sistema permite administrar maquinaria, personal técnico, componentes y solicitudes de servicio, integrando un control de acceso basado en roles (RBAC) y una interfaz moderna basada en Material Design.

## 🏗️ Arquitectura del Proyecto

El proyecto sigue el patrón de **Arquitectura Limpia**, separando las preocupaciones en cuatro capas bien definidas:

*   **Orión.Domain:** Contiene las entidades de negocio, interfaces de repositorio y excepciones globales. Es la capa central y no tiene dependencias externas.
*   **Orión.Application:** Define la lógica de negocio, interfaces de servicios, DTOs y casos de uso.
*   **Orión.Infrastructure:** Implementa el acceso a datos mediante **Entity Framework Core**, la persistencia en **PostgreSQL**, repositorios genéricos y servicios de infraestructura (como generación de reportes).
*   **Orión.DesktopUI:** Capa de presentación construida con **WPF (Windows Presentation Foundation)** siguiendo el patrón **MVVM (Model-View-ViewModel)**.

## 🛠️ Tecnologías Utilizadas

*   **Lenguaje:** C# 13 / .NET 9
*   **Interfaz:** WPF con [Material Design In XAML](http://materialdesigninxaml.net/)
*   **MVVM:** [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) (Source Generators)
*   **Persistencia:** Entity Framework Core con Npgsql (PostgreSQL)
*   **Seguridad:** BCrypt.Net para el hasheo de contraseñas
*   **Pruebas:** xUnit, FluentAssertions y Moq
*   **Reportes:** QuestPDF / iText7 (según implementación)

## ✨ Funcionalidades Clave

*   **Control de Acceso (RBAC):** Diferenciación entre usuarios **Admin** (gestión total) y **Operadores** (solo lectura/operaciones básicas).
*   **Gestión de Catálogos:** CRUD completo para Maquinaria, Componentes, Técnicos y Usuarios.
*   **Ventanas Flotantes:** Implementación de `DialogHost` para formularios fluidos y modernos sin ventanas emergentes del sistema.
*   **Borrado Lógico:** Sistema de desactivación de registros para preservar la integridad referencial y el historial de mantenimiento.
*   **Búsqueda Reactiva:** Filtrado instantáneo en todas las tablas del sistema.
*   **Generación de Reportes:** Exportación de órdenes de servicio en formato PDF.

## 🚀 Configuración e Instalación

### Requisitos Previos
1.  **PostgreSQL:** Asegúrate de tener una instancia corriendo (puerto por defecto: 5433 o configurar en `appsettings.json` / `OrionDbContext`).
2.  **.NET 9 SDK:** Instalado en tu sistema.

### Configuración de la Base de Datos
El sistema utiliza un inicializador automático (`DbInitializer`) que crea la base de datos, aplica las migraciones y carga los datos de prueba al arrancar.

### Ejecución
```bash
cd Orión
dotnet run --project Orión.DesktopUI
```

## 🔐 Credenciales por Defecto (Pruebas)

| Usuario | Contraseña | Rol | Permisos |
| :--- | :--- | :--- | :--- |
| `admin` | `admin123` | Admin | Acceso Total |
| `operador` | `user123` | Operador | Solo lectura y Solicitudes |

## 🧪 Pruebas Unitarias

El proyecto cuenta con una suite de tests automatizados que validan la lógica de negocio y los ViewModels.

```bash
dotnet test Orión/Orión.sln
```

## 📄 Licencia

Este proyecto está bajo la Licencia **MIT** - consulta el archivo [LICENSE](LICENSE) para más detalles.

---
*Desarrollado con enfoque en Clean Code y Excelencia Técnica por sr_irvin.*
