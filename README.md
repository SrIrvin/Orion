# Orión - Sistema de Gestión de Mantenimiento Industrial 🚀

Orión es una solución empresarial de alto rendimiento para la gestión de mantenimiento industrial, diseñada bajo estándares de **Clean Architecture** y principios **SOLID**. El sistema permite una administración integral de activos (maquinaria, componentes), personal técnico y órdenes de servicio, con un enfoque en la seguridad, la trazabilidad y la experiencia de usuario moderna.

[![.NET 9](https://img.shields.io/badge/.NET-9.0-512bd4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/download)
[![WPF](https://img.shields.io/badge/UI-WPF%20%7C%20Material%20Design-blue?style=flat-square)](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)

## 🏗️ Arquitectura del Proyecto

El proyecto implementa una **Arquitectura Limpia (Clean Architecture)** con desacoplamiento total entre la lógica de negocio y la infraestructura:

*   **`Orión.Domain`**: El núcleo del sistema. Contiene las entidades, enums, excepciones personalizadas y reglas de negocio puras. Sin dependencias externas.
*   **`Orión.Application`**: Casos de uso y servicios de aplicación. Gestiona DTOs, validaciones y la orquestación del negocio.
*   **`Orión.Infrastructure`**: Implementación de la persistencia con **EF Core**, repositorios genéricos, y servicios de bajo nivel como seguridad de configuración y generación de PDF.
*   **`Orión.DesktopUI`**: Capa de presentación avanzada en **WPF** utilizando el patrón **MVVM** con generadores de código para una reactividad óptima.

## 🛠️ Stack Tecnológico

*   **Framework:** .NET 9 (C# 13)
*   **Interfaz de Usuario:** WPF + [Material Design In XAML](http://materialdesigninxaml.net/)
*   **Patrón MVVM:** [CommunityToolkit.Mvvm](https://github.com/CommunityToolkit/dotnet) (ObservableProperty, RelayCommand)
*   **Bases de Datos Soportadas:** 
    *   **PostgreSQL** (Producción/Escalabilidad)
    *   **MS Access / Jet** (Portabilidad/Entornos locales)
*   **ORM:** Entity Framework Core 9.0
*   **Reportes:** [QuestPDF](https://www.questpdf.com/) (Motor de diseño fluído para PDFs profesionales)
*   **Seguridad:** 
    *   **BCrypt.Net-Next** para hashing de contraseñas.
    *   **Data Protection API (DPAPI)** para encriptación de configuraciones locales.

## ✨ Características Destacadas

*   **🛡️ Configuración Segura:** Las cadenas de conexión y preferencias de usuario no se guardan en texto plano en `appsettings.json`, sino en un almacén binario encriptado (`SecureConfigService`).
*   **📊 Dashboard Inteligente:** Visualización de salud de maquinaria y mapas de calor (Heatmaps) de actividad global generados dinámicamente.
*   **🔌 Multi-Proveedor de Datos:** Capacidad de alternar entre motores de base de datos (Access vs PostgreSQL) mediante configuración dinámica en tiempo de ejecución.
*   **📄 Reportes Profesionales:** Generación automática de reportes de servicio en PDF con diseño moderno y tablas detalladas.
*   **⏳ Gestión de Sesiones:** Control automático de inactividad con cierre de sesión seguro y persistencia opcional de credenciales ("Recordarme").
*   **♻️ Ciclo de Vida de Activos:** Seguimiento detallado de componentes por máquina, historial de fallas y estados de servicio (Pendiente, En Proceso, Completado).

## 🚀 Instalación y Ejecución

### Requisitos Previos
1.  **.NET 9 SDK** instalado.
2.  (Opcional) Instancia de **PostgreSQL** si se desea usar este motor. Por defecto, el sistema puede inicializarse con **MS Access**.

### Configuración Inicial
El sistema cuenta con un `DbInitializer` que detecta el entorno y:
1.  Crea la base de datos automáticamente.
2.  Aplica las migraciones de EF Core.
3.  Carga datos semilla (Seed Data) para pruebas si no es entorno de producción.

### Compilación y Ejecución
```powershell
# Clonar el repositorio
git clone https://github.com/SrIrvin/Orion.git
cd Orion

# Compilar la solución
dotnet build

# Ejecutar la aplicación UI
dotnet run --project Orión.DesktopUI
```

## 🔐 Acceso de Prueba (Modo Staging)

Si el sistema inicia en modo desarrollo/staging, puedes usar las siguientes credenciales:

| Usuario | Contraseña | Rol |
| :--- | :--- | :--- |
| `admin` | `admin123` | Administrador |
| `operador` | `user123` | Operador |

## 🧪 Calidad y Pruebas

El proyecto mantiene una alta integridad de código validada mediante pruebas automatizadas:

```powershell
# Ejecutar todas las pruebas (xUnit + Moq + FluentAssertions)
dotnet test
```
*Estado actual: 54 pruebas exitosas verificando lógica de servicios y ViewModels.*

## 📄 Licencia

Este proyecto se distribuye bajo la licencia **MIT**. Consulta el archivo `LICENSE` para más información.

---
*Desarrollado con arquitectura empresarial por **SrIrvin**.*
