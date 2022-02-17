using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Services
{
    public class VehicleTypeSelectListService : IVehicleTypeSelectListService
    {
        private readonly GarageVehicleContext db;
        public VehicleTypeSelectListService(GarageVehicleContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<SelectListItem>> GetVehicleTypesAsync()
        {
            return await db.VehicleType.Select(v => new SelectListItem
            {
                Text = v.Name,
                Value = v.Name
            })
                .ToListAsync();
        }
    }
}
