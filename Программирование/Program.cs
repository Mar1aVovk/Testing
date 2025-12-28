using System;
using System.Collections.Generic;

namespace TravelPlanner
{
    // Класс для хранения информации о пути между городами
    class Path
    {
        public string From { get; set; }
        public string To { get; set; }
        public int Time { get; set; } // в минутах
        public int Cost { get; set; }

        public void Print()
        {
            Console.WriteLine($"{From} -> {To} | Время: {Time} мин | Стоимость: {Cost} руб");
        }
    }

    // Класс для хранения маршрута
    class Route
    {
        public List<Path> Paths = new List<Path>();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public void Print()
        {
            Console.WriteLine($"\nДаты: {StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}");
            Console.WriteLine("--- Маршрут ---");
            foreach (var path in Paths)
            {
                path.Print();
            }
            Console.WriteLine($"Общее время: {GetTotalTime()} мин");
            Console.WriteLine($"Общая стоимость: {GetTotalCost()} руб");
            Console.WriteLine($"Пересадок: {Paths.Count - 1}");
        }

        public int GetTotalTime()
        {
            int total = 0;
            foreach (var path in Paths) total += path.Time;
            return total;
        }

        public int GetTotalCost()
        {
            int total = 0;
            foreach (var path in Paths) total += path.Cost;
            return total;
        }
    }

    // Класс для хранения предпочтений пользователя
    class Preferences
    {
        public int MinTime { get; set; } = 0; // минимальное время
        public int MaxTransfers { get; set; } = 3;
        public int MaxCost { get; set; } = 10000;
    }

    // Класс для планировщика
    class Planner
    {
        // База путей между городами
        private List<Path> allPaths = new List<Path>();

        public Planner()
        {
            // Добавляем тестовые пути
            allPaths.Add(new Path { From = "Москва", To = "СПб", Time = 240, Cost = 1500 });
            allPaths.Add(new Path { From = "Москва", To = "Казань", Time = 360, Cost = 1200 });
            allPaths.Add(new Path { From = "СПб", To = "Казань", Time = 480, Cost = 1800 });
            allPaths.Add(new Path { From = "Казань", To = "Екатеринбург", Time = 420, Cost = 1400 });
            allPaths.Add(new Path { From = "Екатеринбург", To = "Новосибирск", Time = 300, Cost = 2000 });
            allPaths.Add(new Path { From = "Екатеринбург", To = "Новосибирск", Time = 200, Cost = 2000 });
        }

        // 2.1 Поиск всех маршрутов (упрощенный DFS)
        public List<Route> FindAllRoutes(string start, string end, Preferences prefs)
        {
            List<Route> result = new List<Route>();
            FindRoutesDFS(start, end, new List<Path>(), result, prefs, 0);
            return result;
        }

        private void FindRoutesDFS(string current, string end, List<Path> currentPath,
                                  List<Route> result, Preferences prefs, int depth)
        {
            // Если слишком много пересадок - выходим
            if (depth > prefs.MaxTransfers) return;

            // Если дошли до конца - сохраняем маршрут
            if (current == end && currentPath.Count > 0)
            {
                // Проверяем ограничения
                if (CheckPreferences(currentPath, prefs))
                {
                    Route newRoute = new Route();
                    foreach (var path in currentPath) newRoute.Paths.Add(path);
                    result.Add(newRoute);
                }
                return;
            }

            // Ищем все пути из текущего города
            foreach (var path in allPaths)
            {
                if (path.From == current)
                {
                    currentPath.Add(path);
                    FindRoutesDFS(path.To, end, currentPath, result, prefs, depth + 1);
                    currentPath.RemoveAt(currentPath.Count - 1);
                }
            }
        }

        // Проверка ограничений
        private bool CheckPreferences(List<Path> paths, Preferences prefs)
        {
            int totalTime = 0;
            int totalCost = 0;

            foreach (var path in paths)
            {
                totalTime += path.Time;
                totalCost += path.Cost;
            }

            return totalTime >= prefs.MinTime && totalCost <= prefs.MaxCost;
        }

        // 2.2 Поиск лучшего маршрута (линейный поиск)
        public Route FindBestRoute(List<Route> routes, bool byTime = true)
        {
            if (routes.Count == 0) return null;

            Route best = routes[0];

            foreach (var route in routes)
            {
                if (byTime)
                {
                    if (route.GetTotalTime() < best.GetTotalTime()) best = route;
                }
                else
                {
                    if (route.GetTotalCost() < best.GetTotalCost()) best = route;
                }
            }

            return best;
        }

        // Расчет дат поездки
        public void CalculateDates(Route route, DateTime startDate)
        {
            route.StartDate = startDate;
            route.EndDate = startDate.AddMinutes(route.GetTotalTime());
        }
    }

    class Program
    {
        static void Main()

        {
            Planner planner = new Planner();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== ПЛАНИРОВЩИК ПУТЕШЕСТВИЙ ===\n");

                // Главное меню
                Console.WriteLine("1. Найти маршрут");
                Console.WriteLine("2. Выход");
                Console.Write("\nВыберите: ");

                string choice = Console.ReadLine();

                if (choice == "2") break;
                if (choice != "1") continue;

                // Ввод данных
                Console.Write("\nОткуда: ");
                string from = Console.ReadLine();

                Console.Write("Куда: ");
                string to = Console.ReadLine();

                // Ввод даты начала
                Console.Write("Дата начала (дд.мм.гггг): ");
                DateTime startDate;
                while (!DateTime.TryParse(Console.ReadLine(), out startDate))
                {
                    Console.Write("Неверный формат! Введите дату (дд.мм.гггг): ");
                }

                // Ввод предпочтений
                Preferences prefs = new Preferences();

                Console.Write("Минимальное время пути (мин): ");
                prefs.MinTime = int.Parse(Console.ReadLine());

                Console.Write("Максимум пересадок: ");
                prefs.MaxTransfers = int.Parse(Console.ReadLine());

                Console.Write("Максимальная стоимость: ");
                prefs.MaxCost = int.Parse(Console.ReadLine());

                Console.Write("Критерий (1-время, 2-стоимость): ");
                bool byTime = Console.ReadLine() == "1";

                // Поиск всех маршрутов
                Console.WriteLine("\nИщем маршруты...");
                var allRoutes = planner.FindAllRoutes(from, to, prefs);

                if (allRoutes.Count == 0)
                {
                    Console.WriteLine("Маршрутов не найдено!");
                }
                else
                {
                    // Рассчитать даты для всех маршрутов
                    foreach (var route in allRoutes)
                    {
                        planner.CalculateDates(route, startDate);
                    }

                    Console.WriteLine($"\nНайдено маршрутов: {allRoutes.Count}");

                    // Показать все маршруты
                    for (int i = 0; i < allRoutes.Count; i++)
                    {
                        Console.WriteLine($"\nМаршрут #{i + 1}:");
                        allRoutes[i].Print();
                    }

                    // Найти лучший
                    var best = planner.FindBestRoute(allRoutes, byTime);
                    Console.WriteLine("\n=== ЛУЧШИЙ МАРШРУТ ===");
                    best.Print();
                }

                Console.Write("\nНажмите Enter для нового поиска...");
                Console.ReadLine();
            }

            Console.WriteLine("До свидания!");
        }
    }
}