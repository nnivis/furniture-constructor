# Furniture Constructor

🇬🇧 [Read in English](README.md)

Unity-прототип для конфигурации мебели в реальном времени — смена материалов, стилей и размеров через data-driven UI.

---

## Демо

> Скоро

---

## О проекте

Furniture Constructor — портфолио-проект с акцентом на чистую архитектуру в Unity. Мебель описывается в структурированной JSON-базе данных и загружается в рантайме. Пользователь выбирает модель и настраивает материал, стиль и габариты через панель управления. Каждый тип модификации изолирован в отдельный класс — доменный слой остаётся чистым и расширяемым.

---

## Механики

- Спаун мебели в рантайме из `FurnitureCatalog` ScriptableObject
- Смена материала по части мебели
- Смена стиля через настраиваемые ключи
- Изменение размера через blend shape морфинг
- Обработка UV-данных для изменённых частей меша
- Data-driven UI — дропдауны и слайдеры генерируются из данных мебели
- Видимость — показывается только выбранная модель

---

## Архитектура

| Паттерн | Реализация |
|---|---|
| Entry Point | `FurnitureEntryPoint` — связывает presenter, panel и каталог в правильном порядке |
| Presenter | `FurniturePresenter` — оркестрирует создание, выбор и модификацию |
| Интерфейсы | `IFurniturePresenter`, `IFurnitureLoader`, `IFurnitureFactory` |
| Система модификаторов | `SizeModifier`, `MaterialModifier`, `StyleModifier` — одна ответственность на тип |
| Слой данных | `FurnitureCatalog` SO + JSON-база через Newtonsoft.Json |
| UI | `FurniturePanel` с `Bind / Unbind` — нет событий наружу, вызывает presenter напрямую |

---

## Технологии

- **Движок:** Unity 6000.3 LTS
- **Язык:** C#
- **Render Pipeline:** URP
- **3D Формат:** glTFast
- **UI:** TextMeshPro
- **Сериализация:** Newtonsoft.Json

---

## Запуск

1. Клонировать репозиторий
2. Открыть в Unity 6000.3 или новее
3. Открыть `Assets/Scenes/SampleScene`
4. Создать ассет каталога: `Assets → Create → FurnitureConstructor → Catalog`
5. Добавить prefab'ы мебели в каталог и назначить его на `FurniturePresenter` в Inspector
6. Нажать Play

---

## Контакты

- GitHub: [nnivis](https://github.com/nnivis)
