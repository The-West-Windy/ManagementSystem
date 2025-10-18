# ManagementSystem

Система для управління **Users**, **Items**, **Actions** з MVC-адмін-панеллю (ASP.NET Core) та мобільним .NET MAUI застосунком (MVVM).

## Вимоги
- Visual Studio 2022 Community
- .NET 8 SDK
- Робочі навантаження: ASP.NET and web development, .NET MAUI
- Android Emulator API 33

## Структура
ManagementSystem/
Server/
ServerApp/ # ASP.NET Core (Empty, .NET 8)
Client/
ClientApp/ # .NET MAUI App (.NET 8), MVVM ( папка ViewModels )

markdown
Копіювати код

## Налаштування
1. Клонувати репозиторій або відкрити solution `ManagementSystem.sln`.
2. Запустити `Server/ServerApp` (F5) — порожня сторінка/Hello World.
3. Запустити `Client/ClientApp` на Android-емуляторі (API 33).

## Предметна область
Підтримує 3 таблиці: **Users**, **Items**, **Actions** (можна адаптувати під освіту/бібліотеку/готель тощо).

## Git
- `main` — стабільна гілка.
- Розробка — через feature-бренчі з PR.