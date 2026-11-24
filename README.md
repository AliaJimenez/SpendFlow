<h1 align="center">💸 SpendFlow</h1>

<p align="center">
  <b>Personal expense manager for the console, built with a clean N-tier architecture.</b><br/>
  <i>Gestor de gastos personales para consola, construido con arquitectura en N capas.</i>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white" alt="C#"/>
  <img src="https://img.shields.io/badge/.NET-9-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 9"/>
  <img src="https://img.shields.io/badge/Storage-JSON-000000?style=for-the-badge&logo=json&logoColor=white" alt="JSON"/>
</p>

---

## ✨ Features · Funcionalidades

| EN | ES |
|---|---|
| Register expenses with amount, description, date and category | Registro de gastos con monto, descripción, fecha y categoría |
| Categories with a monthly budget and an **over-budget warning** | Categorías con presupuesto mensual y **alerta al excederlo** |
| Reports and spending summary by category | Reportes y resumen de gastos por categoría |
| Export a monthly summary to JSON | Exportación del resumen mensual a JSON |
| Data persisted automatically in local JSON files | Persistencia automática en archivos JSON locales |

## 🏗️ Architecture · Arquitectura

```
SpendFlow.sln
├─ Presentacion  →  Console UI (entry point)              · Interfaz de consola
├─ Negocio       →  GestorFinanzas: rules, budgets, reports · Reglas de negocio
├─ Datos         →  RepositorioJson<T>: generic JSON store  · Acceso a datos
└─ Entidades     →  Gasto, Categoria                        · Modelos
```

Each layer only depends on the layers below it: `Presentacion → Negocio → Datos → Entidades`.

## 🚀 Run · Ejecutar

**Requirements · Requisitos:** .NET 9 SDK.

```bash
git clone https://github.com/AliaJimenez/SpendFlow.git
cd SpendFlow/SistemaDeRegistro_GastosPersonales
dotnet run --project SistemaDeRegistroGastosPersonales.Presentacion
```

> Start by creating a category (option **3**) — expenses must belong to one.
> *Primero crea una categoría (opción **3**): todo gasto debe pertenecer a una.*

## 👩‍💻 Author · Autora

**Aliandy Jimenez** — [@AliaJimenez](https://github.com/AliaJimenez)
