# Sports Training Management System (ASP.NET Core + MAUI)

Повноцінна система для управління тренуваннями у спортзалі **Coaches**, **Classes**, **Bookings** з MVC-адмін-панеллю (ASP.NET Core), мобільним .NET MAUI застосунком (MVVM), REST API (JWT-аутентифікація) та SQL Server базою даних.

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
ClientApp/ # .NET MAUI App (.NET 8), MVVM (папка ViewModels)

## Налаштування
1. Клонувати репозиторій або відкрити solution `ManagementSystem.sln`.
Запуск локально для сервера:
- dotnet build
- dotnet ef database update
- dotnet run
2. Запустити `Server/ServerApp`.
3. Запустити `Client/ClientApp` на Android-емуляторі (API 33).

## Предметна область
Система керує розкладом занять у спортзалі. База даних підтримує 3 таблиці: **Coaches**, **Classes**, **Bookings**.

## Git
- `master` — стабільна гілка.
- Розробка — через feature-бренчі з PR.

## Функціональність

### 1. Адмін-панель (MVC)
- Авторизація адміністратора
- CRUD-операції:
  - Coaches
  - Classes
  - Bookings

### 2. REST API (ASP.NET Core Web API)
- `POST /api/auth/login` — отримання JWT
- `GET /api/classes` — список занять
- `POST /api/bookings` — створення бронювання
- Swagger доступний за /swagger

### 3. MAUI Mobile App
- Логін тренера
- Перегляд занять (ItemsPage)
- Створення Booking
- Кешування списку занять

---

## Аутентифікація через JWT

Приклад login-запиту:
```http
POST /api/auth/login
{
  "email": "olena.fit@fitgym.com",
  "password": "********"
}
```
