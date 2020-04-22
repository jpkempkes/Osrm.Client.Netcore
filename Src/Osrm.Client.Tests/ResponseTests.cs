using Microsoft.VisualStudio.TestTools.UnitTesting;
using Osrm.Client.Models;
using Osrm.Client.v5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Osrm.Client.Tests
{
    [TestClass]
    public class ResponseTests
    {
        protected Osrm5x osrm = new Osrm5x(new HttpClient(), "http://router.project-osrm.org/");

        [TestMethod]
        public void Route_Response()
        {
            var locations = new Location[] {
                new Location(52.503033, 13.420526),
                new Location(52.516582, 13.429290),
            };

            var result = osrm.Route(locations).GetAwaiter().GetResult();

            Assert.AreEqual<string>("Ok", result.Code);
            Assert.IsTrue(result.Routes.Length > 0);
            Assert.IsTrue(result.Waypoints.Length > 0);
            Assert.IsTrue(result.Routes[0].Legs.Length > 0);

            var result2 = osrm.Route(new RouteRequest()
            {
                Coordinates = locations,
                Alternative = false
            }).GetAwaiter().GetResult();

            Assert.AreEqual<string>("Ok", result2.Code);
            Assert.IsTrue(result2.Routes.Length > 0);
            Assert.IsTrue(result2.Waypoints.Length > 0);
            Assert.IsTrue(result2.Routes[0].Legs.Length > 0);

            var result3 = osrm.Route(new RouteRequest()
            {
                Coordinates = locations,
                Alternative = true
            }).GetAwaiter().GetResult();

            Assert.AreEqual<string>("Ok", result3.Code);
            Assert.IsTrue(result3.Routes.Length > 0);
            Assert.IsTrue(result3.Waypoints.Length > 0);
            Assert.IsTrue(result3.Routes[0].Legs.Length > 0);
        }

        [TestMethod]
        public void Table_Response()
        {
            var locations = new Location[] {
                new Location(52.554070, 13.160621),
                new Location(52.431272, 13.720654),
                new Location(52.554070, 13.720654),
                new Location(52.554070, 13.160621),
            };

            var result = osrm.Table(locations).GetAwaiter().GetResult();
            Assert.AreEqual<string>("Ok", result.Code);
            Assert.AreEqual<int>(4, result.Durations.Length);
            Assert.AreEqual<int>(4, result.Durations[0].Length);
            Assert.AreEqual<int>(4, result.Durations[1].Length);
            Assert.AreEqual<int>(4, result.Durations[2].Length);
            Assert.AreEqual<int>(4, result.Durations[3].Length);

            var srcAndDests = new Location[] {
                new Location(52.554070, 13.160621),
                new Location(52.431272, 13.720654),
                new Location(52.554070, 13.720654),
                new Location(52.554070, 13.160621),
            };

            var result2 = osrm.Table(new TableRequest()
            {
                Coordinates = srcAndDests,
                Sources = new uint[] { 0 },
                Destinations = new uint[] { 1, 2, 3 }
            }).GetAwaiter().GetResult();

            Assert.AreEqual<string>("Ok", result.Code);
            Assert.AreEqual<int>(1, result2.Durations.Length);
            Assert.AreEqual<int>(3, result2.Durations[0].Length);
        }

        [TestMethod]
        public void Match_Response()
        {
            var locations = new Location[] {
                new Location(52.542648, 13.393252),
                new Location(52.543079, 13.394780),
                new Location(52.542107, 13.397389)
            };

            var req = new MatchRequest()
            {
                Coordinates = locations,
                Timestamps = new int[] { 1424684612, 1424684616, 1424684620 }
            };

            var result = osrm.Match(req).GetAwaiter().GetResult();

            Assert.AreEqual<string>("Ok", result.Code);
            Assert.IsTrue(result.Matchings.Length > 0);
            Assert.IsTrue(result.Matchings[0].Legs.Length > 0);
            Assert.IsNotNull(result.Matchings[0].Confidence);
        }

        [TestMethod]
        public void Nearest_Response()
        {
            var result = osrm.Nearest(new Location(52.4224, 13.333086)).GetAwaiter().GetResult();

            Assert.AreEqual<string>("Ok", result.Code);
            Assert.IsNotNull(result.Waypoints);
        }

        [TestMethod]
        public void Trip_Response()
        {
            var locations = new Location[] {
                new Location(52.503033, 13.420526),
                new Location(52.516582, 13.429290),
            };

            var result =  osrm.Trip(locations).GetAwaiter().GetResult();

            Assert.AreEqual<string>("Ok", result.Code);
            Assert.AreEqual<int>(1, result.Trips.Length);
            Assert.IsTrue(result.Trips[0].Legs.Length > 0);
        }
    }
}