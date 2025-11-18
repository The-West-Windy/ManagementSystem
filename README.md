# ManagementSystem

Система для управління **Librarians**, **Books**, **BorrowRequests** з MVC-адмін-панеллю (ASP.NET Core) та мобільним .NET MAUI застосунком (MVVM).

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

## Налаштування
1. Клонувати репозиторій або відкрити solution `ManagementSystem.sln`.
2. Запустити `Server/ServerApp`.
3. Запустити `Client/ClientApp` на Android-емуляторі (API 33).

## Предметна область
Предметна область - бібліотека, база даних якої підтримує 3 таблиці: **Librarians**, **Books**, **BorrowRequests**.

## Git
- `master` — стабільна гілка.
- Розробка — через feature-бренчі з PR.