using System.Net;

namespace CoreMe.Application.Common.Interfaces.Services.Region;

public interface IRegionSearchService
{
    string Search(string ipStr);

    string Search(IPAddress ipAddress);

    string Search(uint ipAddress);

    RegionInfo SearchInfo(string ipStr);

    RegionInfo SearchInfo(IPAddress ipAddress);

    RegionInfo SearchInfo(uint ipAddress);

    RegionInfo ToRegionInfo(string? region);
}
