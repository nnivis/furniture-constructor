# Furniture Constructor

A Unity-based furniture customization prototype focused on runtime model configuration, modular modification logic, and data-driven UI generation.

The project demonstrates a furniture constructor system where furniture items can be loaded from structured data and modified through runtime controls such as size, material, style, and visibility options.

> Portfolio project by Yana Shushkina  
> Unity / C# / Runtime UI / Data-driven architecture

---

## Overview

Furniture Constructor is a Unity prototype for configuring furniture objects at runtime.

The system is built around a structured furniture database and a set of modifier classes responsible for applying changes to the selected model. Instead of hardcoding every furniture variation directly in the scene, the project uses external structured data to describe available furniture categories, morph parameters, materials, styles, and model parts.

The main goal of the project was to explore how a configurable 3D product constructor can be organized in Unity using a clear separation between data, domain logic, services, infrastructure, and UI.

---

## Features

- Runtime furniture generation from predefined data
- Furniture category and object data model
- Material switching by furniture part
- Style switching by configurable keys and labels
- Size modification using morph-like parameters
- UV data handling for modified mesh parts
- UI controls for furniture customization
- Modular modifier system for extending object behavior
- Layered project structure:
  - `Data`
  - `Domain`
  - `Infrastructure`
  - `Services`
  - `UI`

---

## Technical Highlights

### Data-driven configuration

Furniture data is represented through serializable C# models and loaded from a structured database. The data includes:

- furniture categories
- furniture objects
- morph parameters
- material options
- style options
- part-specific customization data

This allows furniture behavior and customization options to be described outside of scene logic.

### Modifier-based architecture

The customization logic is separated into modifier classes:

- `SizeModifier`
- `MaterialModifier`
- `StyleModifier`
- `BlockStackModifier`
- base `Modifier`

This keeps the furniture object itself relatively clean and makes it easier to extend the system with new modification types.

### Runtime UI

The UI layer contains reusable view components for dropdowns, sliders, furniture panels, and visibility controls. These components are used to expose available customization options to the user at runtime.

### Separation of responsibilities

The project is organized into several code layers:

```text
Assets/CodeBase
├── Data
├── Domain
├── Infrastructure
├── Services
└── UI
```

This structure separates raw data models, business/domain logic, loading infrastructure, runtime services, and presentation logic.

---

## Tech Stack

- Unity
- C#
- Universal Render Pipeline
- Unity UI
- Unity Input System
- Newtonsoft Json
- glTFast
- TextMesh Pro

---

## Project Structure

```text
Assets/
├── CodeBase/
│   ├── Data/
│   │   └── FurnitureConstructor/
│   │       └── FurnitureData.cs
│   │
│   ├── Domain/
│   │   └── FurnitureConstructor/
│   │       ├── Furniture.cs
│   │       └── Modifiers/
│   │
│   ├── Infrastructure/
│   │   ├── Bootstrapper.cs
│   │   └── DataProvider/
│   │       └── FurnitureLoader.cs
│   │
│   ├── Services/
│   │   └── FurnitureConstructor/
│   │
│   └── UI/
│       └── FurnitureConstructor/
│
├── Resources/
├── Scenes/
└── Settings/
```

---

## Main Systems

### Furniture Data Model

The data layer describes all information needed to construct and modify furniture objects:

- model name
- start size value
- morph options
- material groups
- style groups
- part-specific material/style data
- UV data for modified meshes

### Furniture Loader

`FurnitureLoader` is responsible for loading the furniture database from Unity Resources, preparing the JSON content, deserializing it, and building lookup data for runtime usage.

### Furniture Entity

`Furniture` acts as a runtime component that stores the current furniture data and delegates customization actions to the modifier system.

Supported operations include:

```csharp
ApplyNewMaterial(...)
ApplyNewStyle(...)
ApplyNewSize(...)
```

### Modifier System

Modifiers encapsulate the logic for applying specific types of changes to the furniture model. This makes the system easier to extend without putting all customization logic into one monolithic class.

### UI Layer

The UI layer contains view components for interacting with the constructor:

- dropdown-based options
- slider-based size controls
- furniture selection panel
- visibility panel

---

## How to Run

1. Clone the repository:

```bash
git clone https://github.com/nnivis/furnitureConstructor_1223id.git
```

2. Open the project in Unity.

3. Open the main scene:

```text
Assets/Scenes/SampleScene.unity
```

4. Press Play.

---

## Current Status

The project is a working prototype focused on the architecture and core logic of a runtime furniture constructor.

Implemented:

- data loading
- furniture runtime initialization
- modifier-based customization
- material/style/size change logic
- UI components for interaction

Planned improvements:

- add a polished demo scene
- add screenshots and GIFs to README
- improve UI visual design
- add more furniture presets
- add WebGL build
- add automated validation for furniture data
- improve naming and cleanup for public portfolio presentation

---

## Portfolio Notes

This project demonstrates:

- Unity runtime systems development
- C# architecture organization
- data-driven feature implementation
- UI-to-domain interaction
- runtime model customization
- separation of concerns in a Unity project

The project is intended as a portfolio example for Unity Developer positions.

