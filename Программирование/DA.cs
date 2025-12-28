using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TravelPlanner.Tests
{
    [TestClass]
    public class PlannerTests
    {
        private Planner planner;

        [TestInitialize]
        public void Setup()
        {
            planner = new Planner();
        }

        // T1: Пустой список маршрутов
        [TestMethod]
        public void FindBestRoute_EmptyList_ReturnsNull()
        {
            // Arrange
            List<Route> emptyRoutes = new List<Route>();

            // Act
            var result = planner.FindBestRoute(emptyRoutes);

            // Assert
            Assert.IsNull(result, "Для пустого списка должен возвращаться null");
        }

        // T2: Один маршрут, поиск по времени
        [TestMethod]
        public void FindBestRoute_SingleRouteByTime_ReturnsSameRoute()
        {
            // Arrange
            var route = new Route();
            route.Paths.Add(new Path { From = "A", To = "B", Time = 100, Cost = 500 });

            List<Route> routes = new List<Route> { route };

            // Act
            var result = planner.FindBestRoute(routes, true);

            // Assert
            Assert.AreEqual(route, result, "Для одного маршрута должен возвращаться он же");
            Assert.AreEqual(100, result.GetTotalTime());
        }

        // T3: Два маршрута по времени, первый лучше
        [TestMethod]
        public void FindBestRoute_TwoRoutesByTime_FirstBetter_ReturnsFirst()
        {
            // Arrange
            var route1 = new Route();
            route1.Paths.Add(new Path { From = "A", To = "B", Time = 100, Cost = 500 });

            var route2 = new Route();
            route2.Paths.Add(new Path { From = "A", To = "B", Time = 200, Cost = 400 });

            List<Route> routes = new List<Route> { route1, route2 };

            // Act
            var result = planner.FindBestRoute(routes, true);

            // Assert
            Assert.AreEqual(route1, result, "Должен возвращаться маршрут с меньшим временем");
            Assert.AreEqual(100, result.GetTotalTime());
        }

        // T4: Два маршрута по времени, второй лучше
        [TestMethod]
        public void FindBestRoute_TwoRoutesByTime_SecondBetter_ReturnsSecond()
        {
            // Arrange
            var route1 = new Route();
            route1.Paths.Add(new Path { From = "A", To = "B", Time = 200, Cost = 500 });

            var route2 = new Route();
            route2.Paths.Add(new Path { From = "A", To = "B", Time = 100, Cost = 600 });

            List<Route> routes = new List<Route> { route1, route2 };

            // Act
            var result = planner.FindBestRoute(routes, true);

            // Assert
            Assert.AreEqual(route2, result, "Должен возвращаться маршрут с меньшим временем");
            Assert.AreEqual(100, result.GetTotalTime());
        }

        // T5: Два маршрута по стоимости, первый лучше
        [TestMethod]
        public void FindBestRoute_TwoRoutesByCost_FirstBetter_ReturnsFirst()
        {
            // Arrange
            var route1 = new Route();
            route1.Paths.Add(new Path { From = "A", To = "B", Time = 150, Cost = 400 });

            var route2 = new Route();
            route2.Paths.Add(new Path { From = "A", To = "B", Time = 100, Cost = 600 });

            List<Route> routes = new List<Route> { route1, route2 };

            // Act
            var result = planner.FindBestRoute(routes, false);

            // Assert
            Assert.AreEqual(route1, result, "Должен возвращаться маршрут с меньшей стоимостью");
            Assert.AreEqual(400, result.GetTotalCost());
        }

        // T6: Два маршрута по стоимости, второй лучше
        [TestMethod]
        public void FindBestRoute_TwoRoutesByCost_SecondBetter_ReturnsSecond()
        {
            // Arrange
            var route1 = new Route();
            route1.Paths.Add(new Path { From = "A", To = "B", Time = 100, Cost = 600 });

            var route2 = new Route();
            route2.Paths.Add(new Path { From = "A", To = "B", Time = 150, Cost = 400 });

            List<Route> routes = new List<Route> { route1, route2 };

            // Act
            var result = planner.FindBestRoute(routes, false);

            // Assert
            Assert.AreEqual(route2, result, "Должен возвращаться маршрут с меньшей стоимостью");
            Assert.AreEqual(400, result.GetTotalCost());
        }
        //Т7: Null вместо списка
        [TestMethod]
        public void FindBestRoute_NullList_ReturnsNull()
        {
            // Act
            var result = planner.FindBestRoute(null);

            // Assert
            Assert.IsNull(result, "Для null должен возвращаться null");
        }
    }

