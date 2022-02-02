#nullable disable
using Garage_2._0.Interfaces;
using Garage_2._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Garage_2._0.Controllers.VehiclesController
{
    public class VehiclesController : Controller
    {
        private readonly GarageVehicleContext _context;

        private readonly double hourlyRate = 20;

        public VehiclesController(GarageVehicleContext context)
        IConfiguration _iConfig;
        public VehiclesController(GarageVehicleContext context, IConfiguration iConfig)
        {
            _context = context;
            _iConfig = iConfig;
        }
        
        
        
        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Vehicle.ToListAsync());
        }

        // GET: Vehicles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.License == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Type,License,Color,Make,Model,Wheels")] Vehicle vehicle)
        {

            
            //Check if license already exists in the database. If it exists, don't add the Vehicle.
            if (_context.Vehicle.Where(v => v.License == vehicle.License).ToList().Count > 0)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                vehicle.Arrival = DateTime.Now;
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                TempData["message"] = $"{vehicle.License} has been successfully parked!";
                return RedirectToAction(nameof(VehiclesOverview));
            }
            return View(vehicle);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyLicense(string license)
        {
            if (_context.Vehicle.Where(v => v.License == license).ToList().Count > 0)
            {
                return Json($"License {license} is already in use.");
            }
            return Json(true);
        }

        // GET: Vehicles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle.FindAsync(id);

            if (vehicle == null)
            {
                return NotFound();
            }
            return View(vehicle);
        }

        // POST: Vehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Type,License,Color,Make,Model,Wheels")] Vehicle vehicle)
        {
            if (id != vehicle.License)
            {
                //TODO: show a popup to the user and ridicule them for trying to break our system :D
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //This was the way I found to not make the arrival date change
                    _context.Entry(vehicle).Property(v => v.Type).IsModified = true;
                    _context.Entry(vehicle).Property(v => v.Color).IsModified = true;
                    _context.Entry(vehicle).Property(v => v.Make).IsModified = true;
                    _context.Entry(vehicle).Property(v => v.Model).IsModified = true;
                    _context.Entry(vehicle).Property(v => v.Wheels).IsModified = true;
                    TempData["message"] = $"Your changes for {vehicle.License} has been applied";
                    //_context.Update(vehicle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleExists(vehicle.License))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(VehiclesOverview));
            }
            return View(vehicle);
        }

        // GET: Vehicles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vehicle = await _context.Vehicle
                .FirstOrDefaultAsync(m => m.License == id);
            if (vehicle == null)
            {
                return NotFound();
            }

            return View(vehicle);
        }

        // POST: Vehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            TempData["message"] = $"{vehicle.License} has been checked out!";
            return RedirectToAction(nameof(VehiclesOverview));
        }

        private bool VehicleExists(string id)
        {
            return _context.Vehicle.Any(e => e.License == id);
        }

        //Calculate Total Parked Time + View Model for the Receipt
        public async Task<IActionResult> ReceiptView(string id)
        {
            //regNo should come from check-out so
            Vehicle vehicle = await _context.Vehicle.FindAsync(id);
            Receipt receipt = new Receipt();
            if (vehicle != null)
            {
                receipt.Type = vehicle.Type;
                receipt.License = vehicle.License;
                receipt.Arrival = vehicle.Arrival;
                receipt.CheckOut = DateTime.Now;

                //Calculating Total Parked Time

                TimeSpan totalParkedTime = DateTime.Now.Subtract(vehicle.Arrival);

                receipt.ParkingDuration = totalParkedTime;
                double cost = (totalParkedTime.Hours * 20) + (totalParkedTime.Minutes * 0.33);
                cost = Math.Round(cost, 2);
                receipt.Price = cost + "Sek";
            }
            else
                return NotFound();

            _context.Vehicle.Remove(vehicle);
            _context.SaveChanges();
            TempData["message"] = $"{vehicle.License} has been checked out";
            return View(nameof(ReceiptView), receipt);
        }
        //This one is used on the detailed view
        public async Task<IActionResult> SearchDetailed(string plate)
        {
            if (plate == null)
            {
                TempData["Error"] = "You need to enter a License plate before you search";
                var m = new List<VehicleViewModel>();
                return View(nameof(VehiclesOverview), m);
            }
             var model = _context.Vehicle.Where(v => v.License.Contains(plate));
                //if (model.Count() == 0)
                //{
                //    TempData["Error"] = "Sorry your search did not yield a result";
                //}

            return View(nameof(Index), await model.ToListAsync());
        }
        //this one is used on the Overview
        public async Task<IActionResult> Search(string plate)
        {
            if(plate == null)
            {
                TempData["Error"] = "You need to enter a License plate before you search";
                var m = new List<VehicleViewModel>();
                ViewBag.Button = "true";
                return View(nameof(VehiclesOverview), m);
            }
            var model = _context.Vehicle.Where(v => v.License.Contains(plate))
                                                             .Select(v => new VehicleViewModel
                                                             {
                                                                 Type = v.Type,
                                                                 License = v.License,
                                                                 Make = v.Make,
                                                                 TimeSpent = DateTime.Now.Subtract(v.Arrival)
                                                             });
            await model.ToListAsync();

            if (!model.Any())
            {
                TempData["Error"] = "Your search did not yield any results";
            }
            ViewBag.Button = "true";
            return View(nameof(VehiclesOverview), model);
    }

        public async Task<IActionResult> VehiclesOverview()
        {
            var simpleViewList = _context.Vehicle.Select(v => new VehicleViewModel
            {
                Type = v.Type,
                License = v.License,
                Make = v.Make,
                TimeSpent = DateTime.Now.Subtract(v.Arrival)
            });
            string GarageStatus=TotalGarageCapacity_and_FreeSpace();
            ViewBag.garageStatus = GarageStatus;
            return View(await simpleViewList.ToListAsync());
        }
        
        //Calculating Available free space
        public string TotalGarageCapacity_and_FreeSpace()
        {
            int recordCount=_context.Vehicle.Count();
            int Total_Garage_Capacity = _iConfig.GetValue<int>("GarageCapacity:Capacity");
            string GarageStatus= $"Total Capacity of the Garage is: {Total_Garage_Capacity}. Available Free Space is:{Total_Garage_Capacity - recordCount}";
            return GarageStatus;
        }

        public async Task<IActionResult> Statistics()
        {
            //Create a list of an anonymous class
            var res = await _context.Vehicle.Select(v => new {Arrival = v.Arrival, Wheels = v.Wheels, Type = v.Type}).ToListAsync();

            Statistics statistics = new Statistics
            {
                TotalWheelAmount = res.Sum(r => r.Wheels),
                TotalCostsGenerated = res.Sum(v =>
                {
                    TimeSpan duration = DateTime.Now.Subtract(v.Arrival);
                    double cost = (duration.Hours + (duration.Minutes * 1.0 / 60)) * hourlyRate;
                    return Math.Round(cost, 2);
                })
            };

            foreach (VehicleTypes type in Enum.GetValues(typeof(VehicleTypes)))
            {
                statistics.VehicleTypeCounter.Add(type, res.Where(v => v.Type == type).Count());
            }

            return View(statistics);
        }
    }
}
