# Furniture Constructor

🇷🇺 [Читать на русском](README.ru.md)

A Unity prototype for configuring furniture objects at runtime — switch materials, styles, and sizes through a data-driven UI.

---

## Demo

> Coming soon

---

## About

Furniture Constructor is a portfolio project focused on clean architecture in Unity. Furniture items are defined in a structured JSON database and loaded at runtime. Users can select a model and adjust its material, style, and dimensions through a panel-based UI. Each modifier type is isolated into its own class, keeping the domain layer clean and extensible.

---

## Features

- Runtime furniture spawning from a `FurnitureCatalog` ScriptableObject
- Material switching per furniture part
- Style switching via configurable keys
- Size modification using blend shape morphing
- UV data handling for morphed mesh parts
- Data-driven UI — dropdowns and sliders generated from furniture data
- Visibility toggle — only the selected model is shown

---

## Architecture

| Pattern | Implementation |
|---|---|
| Entry Point | `FurnitureEntryPoint` — wires presenter, panel and catalog in the correct order |
| Presenter | `FurniturePresenter` — orchestrates creation, selection and modification |
| Interfaces | `IFurniturePresenter`, `IFurnitureLoader`, `IFurnitureFactory` |
| Modifier system | `SizeModifier`, `MaterialModifier`, `StyleModifier` — one responsibility per type |
| Data layer | `FurnitureCatalog` SO + JSON database via Newtonsoft.Json |
| UI | `FurniturePanel` with `Bind / Unbind` — no upward events, calls presenter directly |

---

## Tech Stack

- **Engine:** Unity 6000.3 LTS
- **Language:** C#
- **Render Pipeline:** URP
- **3D Format:** glTFast
- **UI:** TextMeshPro
- **Serialization:** Newtonsoft.Json

---

## Getting Started

1. Clone the repository
2. Open in Unity 6000.3 or newer
3. Open `Assets/Scenes/SampleScene`
4. Create a `FurnitureCatalog` asset: `Assets → Create → FurnitureConstructor → Catalog`
5. Add furniture prefabs to the catalog and assign it to `FurniturePresenter` in the Inspector
6. Press Play

---

## Contact

- GitHub: [nnivis](https://github.com/nnivis)
