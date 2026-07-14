# 💸 SpendFlow

> Un gestor de finanzas y gastos personales interactivo desde la consola, diseñado para ser rápido, robusto y modular. Construido en **C# (.NET 9)** bajo una arquitectura sólida de desarrollo[cite: 1].

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![.NET 9](https://img.shields.io/badge/.NET%209.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![JSON](https://img.shields.io/badge/JSON-000000?style=for-the-badge&logo=json&logoColor=white)

---

## 🛠️ Arquitectura del Proyecto

Para garantizar la mantenibilidad y escalabilidad del código, la aplicación está estructurada utilizando una **Arquitectura en N-Capas (N-Tier)**:

*   **`Presentacion` (UI):** Capa interactiva de consola para la comunicación con el usuario.
*   **`Negocio` (BLL):** Lógica que gestiona las operaciones financieras, validaciones de montos y generación de resúmenes (`GestorFinanzas.cs`).
*   **`Datos` (DAL):** Responsable de la persistencia de datos persistiendo la información de categorías y gastos en archivos JSON locales (`RepositorioJson.cs`).
*   **`Entidades` (Entities):** Modelos puros que estructuran los datos del negocio (`Gasto.cs`, `Categoria.cs`).

---

## ✨ Características Principales

*   ➕ **Registro de Gastos:** Permite ingresar montos, descripciones, fechas y vincularlos a categorías específicas.
*   🗂️ **Categorización Dinámica:** Gestión y organización de tus transacciones por etiquetas.
*   📂 **Persistencia JSON:** Los datos no se pierden al cerrar la aplicación; se guardan automáticamente en formato JSON local.
*   📊 **Resúmenes Mensuales:** Muestra reportes y un desglose de en qué se está gastando el dinero.

---

## 🚀 Cómo Ejecutar el Proyecto

### Requisitos previos
*   Tener instalado el **SDK de .NET 9**.

### Clonar y arrancar
1. **Clona el repositorio:**
   ```bash
   git clone https://github.com/AliaJimenez/SpendFlow.git
