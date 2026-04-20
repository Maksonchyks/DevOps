# 🥗 NutriPlan — Мікросервісна система планування харчування

Лабораторна робота №1 з предмету "Технології DevOps"  
Тема: Віртуалізація та контейнеризація

---

## Архітектура

```
┌──────────────────────┐     ┌────────────────────────┐
│   NutritionService   │     │  NotificationService   │
│  (REST API, EF Core) │     │  (REST + SignalR Hub)  │
│      :5001           │     │       :5002            │
└──────────┬───────────┘     └────────────────────────┘
           │
   ┌───────▼────────┐
   │   PostgreSQL   │
   │     :5432      │
   └────────────────┘
```

### Мікросервіси

| Сервіс | Порт | Опис |
|--------|------|------|
| **NutritionService** | 5001 | CRUD продуктів + генерація плану харчування |
| **NotificationService** | 5002 | Real-time нагадування про прийом їжі (SignalR) |
| **PostgreSQL** | 5432 | База даних для NutritionService |

---

## Швидкий старт

### Запуск через Docker Compose

```bash
git clone <repo-url>
cd nutriplan
docker-compose up --build
```

Після запуску:
- **NutritionService Swagger**: http://localhost:5001/swagger
- **NotificationService Swagger**: http://localhost:5002/swagger
- **SignalR Hub**: `ws://localhost:5002/hubs/meals`

---

## API — NutritionService

### Продукти (`/api/foods`)

| Метод | URL | Опис |
|-------|-----|------|
| GET | `/api/foods` | Отримати всі продукти |
| GET | `/api/foods/{id}` | Отримати продукт за ID |
| POST | `/api/foods` | Додати новий продукт |
| PUT | `/api/foods/{id}` | Оновити продукт |
| DELETE | `/api/foods/{id}` | Видалити продукт |

### Генерація плану (`/api/mealplan`)

```http
POST /api/mealplan/generate
Content-Type: application/json

{
  "targetCalories": 2000,
  "days": 3,
  "goal": "lose"
}
```

**goal**: `lose` | `gain` | `maintain`

**Приклад відповіді:**
```json
{
  "targetCalories": 1600,
  "goal": "lose",
  "days": [
    {
      "dayNumber": 1,
      "breakfast": [ { "foodName": "Вівсянка", "gramsAmount": 294, "calories": 200, ... } ],
      "lunch":     [ ... ],
      "dinner":    [ ... ],
      "snacks":    [ ... ],
      "totalCalories": 1598,
      "totalProtein": 94.2,
      "totalCarbs": 180.5,
      "totalFat": 44.1
    }
  ]
}
```

---

## API — NotificationService

### REST

```http
POST /api/notifications/broadcast
{ "meal": "Обід", "message": "Час поїсти!" }

POST /api/notifications/user/{userId}
{ "meal": "Сніданок", "message": "Доброго ранку!" }
```

### SignalR (real-time)

Підключення:
```javascript
const connection = new signalR.HubConnectionBuilder()
  .withUrl("http://localhost:5002/hubs/meals")
  .build();

connection.on("MealReminder", (data) => {
  console.log(`${data.meal} о ${data.time}: ${data.message}`);
});

await connection.start();

// Підписатись на нагадування конкретного користувача
await connection.invoke("JoinUserGroup", "user123");
```

Автоматичні нагадування спрацьовують о:
- 🌅 **08:00** — Сніданок
- 🥗 **13:00** — Обід
- 🍎 **16:00** — Перекус
- 🍽️ **19:00** — Вечеря

---

## Тести

```bash
cd NutritionService.Tests
dotnet test
```

Покриття:
- `MealPlannerService.AdjustCaloriesForGoal` — 4 тести
- `MealPlannerService.BuildMealEntry` — 2 тести
- `MealPlannerService.GeneratePlan` — 5 тестів

---

## Структура проекту

```
nutriplan/
├── NutritionService/           # Мікросервіс харчування
│   ├── Controllers/
│   │   ├── FoodsController.cs
│   │   └── MealPlanController.cs
│   ├── Data/
│   │   └── NutritionDbContext.cs
│   ├── Migrations/
│   ├── Models/
│   │   └── Models.cs
│   ├── Services/
│   │   └── MealPlannerService.cs
│   ├── Dockerfile
│   └── Program.cs
├── NutritionService.Tests/     # Юніт-тести
│   └── MealPlannerServiceTests.cs
├── NotificationService/        # Мікросервіс нотифікацій
│   ├── Controllers/
│   │   └── NotificationsController.cs
│   ├── Hubs/
│   │   └── MealHub.cs
│   ├── Services/
│   │   └── ReminderScheduler.cs
│   ├── Dockerfile
│   └── Program.cs
├── docker-compose.yml
├── NutriPlan.sln
└── README.md
```
