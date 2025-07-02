using NWRWS.Model.Service;
using NWRWS.Model.System;

namespace NWRWS.IService.Service
{
    public interface IDropDownService : IDisposable
    {
        List<dynamic> GetAllCountries();
        List<dynamic> GetAllStatesByCountry(long country);
        List<dynamic> GetAllDistrictsByState(long state);
    }
}
