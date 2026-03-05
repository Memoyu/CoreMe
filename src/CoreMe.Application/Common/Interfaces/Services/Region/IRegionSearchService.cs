using System.Net;

namespace CoreMe.Application.Common.Interfaces.Services.Region;

public interface IRegionSearchService
{
    string Search(string ipStr);

    string Search(IPAddress ipAddress);

    RegionInfo SearchInfo(string ipStr);

    RegionInfo SearchInfo(IPAddress ipAddress);

    RegionInfo ToRegionInfo(string? region);
}
