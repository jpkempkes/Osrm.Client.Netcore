using Osrm.Client.Models;
using Osrm.Client.Models.Requests;
using Osrm.Client.Models.Responses;
using System.Threading.Tasks;

namespace Osrm.Client
{
    public interface IOsrmClient
    {
        string Profile { get; set; }
        int? Timeout { get; set; }
        string Url { get; set; }
        string Version { get; set; }

        Task<MatchResponse> Match(MatchRequest requestParams);
        Task<MatchResponse> Match(params Location[] locs);
        Task<NearestResponse> Nearest(NearestRequest requestParams);
        Task<NearestResponse> Nearest(params Location[] locs);
        Task<RouteResponse> Route(Location[] locs);
        Task<RouteResponse> Route(RouteRequest requestParams);
        Task<TableResponse> Table(params Location[] locs);
        Task<TableResponse> Table(TableRequest requestParams);
        Task<TripResponse> Trip(params Location[] locs);
        Task<TripResponse> Trip(TripRequest requestParams);
    }
}