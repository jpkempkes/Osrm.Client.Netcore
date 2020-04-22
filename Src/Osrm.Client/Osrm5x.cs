using System.Text.Json;
using Osrm.Client.Models;
using Osrm.Client.Models.Requests;
using Osrm.Client.Models.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Osrm.Client
{
    public class Osrm5x : IOsrmClient
    {
        private readonly HttpClient Client;

        public string Url { get; set; }

        /// <summary>
        /// Version of the protocol implemented by the service.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Mode of transportation, is determined by the profile that is used to prepare the data
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Timeout for web request. If not specified default value will be used.
        /// </summary>
        public int? Timeout { get; set; }

        protected readonly string RouteServiceName = "route";
        protected readonly string NearestServiceName = "nearest";
        protected readonly string TableServiceName = "table";
        protected readonly string MatchServiceName = "match";
        protected readonly string TripServiceName = "trip";
        protected readonly string TileServiceName = "tile";

        public Osrm5x(HttpClient client, string url, string version = "v1", string profile = "driving")
        {
            Client = client;
            Url = url;
            Version = version;
            Profile = profile;
        }

        /// <summary>
        /// This service provides shortest path queries with multiple via locations.
        /// It supports the computation of alternative paths as well as giving turn instructions.
        /// </summary>
        /// <param name="locs"></param>
        /// <returns></returns>
        public async Task<RouteResponse> Route(Location[] locs)
        {
            return await Route(new RouteRequest()
            {
                Coordinates = locs
            });
        }

        /// <summary>
        /// This service provides shortest path queries with multiple via locations.
        /// It supports the computation of alternative paths as well as giving turn instructions.
        /// </summary>
        /// <param name="requestParams"></param>
        /// <returns></returns>
        public async Task<RouteResponse> Route(RouteRequest requestParams)
        {
            return await Send<RouteResponse>(RouteServiceName, requestParams);
        }

        public async Task<Models.Responses.NearestResponse> Nearest(params Location[] locs)
        {
            return await Nearest(new NearestRequest()
            {
                Coordinates = locs
            });
        }

        public async Task<NearestResponse> Nearest(NearestRequest requestParams)
        {
            return await Send<NearestResponse>(NearestServiceName, requestParams);
        }

        public async Task<TableResponse> Table(params Location[] locs)
        {
            return await Table(new TableRequest()
            {
                Coordinates = locs
            });
        }

        public async Task<TableResponse> Table(TableRequest requestParams)
        {
            return await Send<TableResponse>(TableServiceName, requestParams);
        }

        public async Task<MatchResponse> Match(params Location[] locs)
        {
            return await Match(new MatchRequest()
            {
                Coordinates = locs
            });
        }

        public async Task<MatchResponse> Match(MatchRequest requestParams)
        {
            return await Send<MatchResponse>(MatchServiceName, requestParams);
        }

        public async Task<TripResponse> Trip(params Location[] locs)
        {
            return await Trip(new TripRequest()
            {
                Coordinates = locs
            });
        }

        public async Task<TripResponse> Trip(TripRequest requestParams)
        {
            return await Send<TripResponse>(TripServiceName, requestParams);
        }

        protected async Task<T> Send<T>(string service, BaseRequest request) //string coordinatesStr, List<Tuple<string, string>> urlParams)
        {
            var coordinatesStr = request.CoordinatesUrlPart;
            List<Tuple<string, string>> urlParams = request.UrlParams;
            var fullUrl = OsrmRequestBuilder.GetUrl(Url, service, Version, Profile, coordinatesStr, urlParams);

            try
            {
                string responseBody = await Client.GetStringAsync(fullUrl);

                return await Task.FromResult(JsonSerializer.Deserialize<T>(responseBody));
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                throw;
            }

        }

    }
}