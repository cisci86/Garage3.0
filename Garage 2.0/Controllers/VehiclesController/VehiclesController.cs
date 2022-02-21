#nullable disable
using Garage_2._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Web;
using System.Linq;

namespace Garage_2._0.Controllers.VehiclesController
{
    public class VehiclesController : Controller
    {
        private readonly GarageVehicleContext _context;
        Vehicle[] parkingSpots;
        public VehiclesController(GarageVehicleContext context,IConfiguration config)
        {
            _context = context;
            Global.Garagecapacity = config.GetValue<int>("GarageCapacity:Capacity");
            Global.HourlyRate = config.GetValue<double>("Price:HourlyRate");

            SetParkingSpots(); //Sets the list with a capacity to the garage capacity.
        }



        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            AddExistingDataToGarage();
            TotalGarageCapacity_and_FreeSpace();
            ViewbagModel.spotsTaken = parkingSpots;
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
            
            CalculateParkingAmount(vehicle);
            return View(vehicle);
        }

        // GET: Vehicles/Create
        public IActionResult Create()
        {
            if (CheckIfGarageIsFull())
            {
                TempData["Error"] = "Sorry the garage is already full!";

                return RedirectToAction(nameof(VehiclesOverview));
            }
            return View();
        }


        // POST: Vehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {

            //Check if license already exists in the database. If it exists, don't add the Vehicle.
            if (_context.Vehicle.Find(vehicle.License) != null)
            {
                return BadRequest();
            }


            if (ModelState.IsValid)
            {
                vehicle.License = vehicle.License.ToUpper();
                vehicle.Arrival = DateTime.Now;
                vehicle.ParkingSpot = FindFirstEmptySpot();
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                TempData["message"] = $"{vehicle.License} has been successfully parked in spot {vehicle.ParkingSpot}!";
                return RedirectToAction(nameof(VehiclesOverview));
            }
            return View(vehicle);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyLicense(string license)
        {
            //Check if license already exists in the database. Sends a warning if it exists
            if (_context.Vehicle.Find(license) != null)
            {
                return Json($"License {license} is already in use.");
            }
            return Json(true);
        }
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyMember(string MemberId)
        {
            // Check if a vehicle's owner is a registered member and over 18
           
            if (MemberId.Length != 13)
                return Json("Enter your Social Security Number in correct Format yyyymmdd-xxxx");
            
            if (_context.Member.Find(MemberId) == null)
            {
                return Json("You have to be a Registered member to Park a Vehicle");
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
        public async Task<IActionResult> Edit(string id, Vehicle vehicle)
        {
            if (id != vehicle.License)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vehicle);
                    _context.Entry(vehicle).Property(v => v.Arrival).IsModified = false; //Makes sure that the Arrival time don't change
                    _context.Entry(vehicle).Property(v => v.ParkingSpot).IsModified = false; //Makes sure that the parking spot don't change
                    TempData["message"] = $"Your changes for {vehicle.License} has been applied";
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
                .Include(v => v.Owner)
                .FirstOrDefaultAsync(m => m.License == id);
            if (vehicle == null)
            {
                return NotFound();
            }
            CalculateParkingAmount(vehicle);
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
            Vehicle vehicle = await _context.Vehicle
                                .Include(v => v.Owner)
                                .FirstOrDefaultAsync(v => v.License == id);

            Receipt receipt = new Receipt();
            if (vehicle != null)
            {
                receipt.MemberName = vehicle.Owner.Name.FirstName + vehicle.Owner.Name.LastName;
                receipt.MemberId = vehicle.MemberId;
                receipt.VehicleTypeName = vehicle.VehicleTypeName;
                receipt.License = vehicle.License;
                receipt.ParkingSpot = vehicle.ParkingSpot;
                receipt.Arrival = vehicle.Arrival;
                receipt.CheckOut = DateTime.Now;

                //Calculating Total Parked Time

                TimeSpan totalParkedTime = DateTime.Now.Subtract(vehicle.Arrival);

                receipt.ParkingDuration = totalParkedTime;
                double hourlyRate = Global.HourlyRate;
                double cost = (totalParkedTime.Hours * hourlyRate) + (totalParkedTime.Minutes * hourlyRate / 60.0);
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
                return RedirectToAction(nameof(Index));
            }
            var model = _context.Vehicle.Where(v => v.License.Contains(plate));

            await model.ToListAsync();

            if (!model.Any())
            {
                TempData["Error"] = "Sorry your search did not yield a result";
            }
            ViewBag.Button = "true";

            return View(nameof(Index), await model.ToListAsync());
        }
        //this one is used on the Overview
        public async Task<IActionResult> Search(string plate)
        {
            if (plate == null)
            {
                TempData["Error"] = "You need to enter a License plate before you search";
                ViewBag.Button = "true";
                return RedirectToAction(nameof(VehiclesOverview));
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
            TotalGarageCapacity_and_FreeSpace();
            AddExistingDataToGarage(); //Populates the Array with the existing vehicles on the right indexes.
            ViewbagModel.spotsTaken = parkingSpots;
            CheckIfGarageIsEmpty();
            return View(await simpleViewList.ToListAsync());
        }

        //Calculating Available free space
        public void TotalGarageCapacity_and_FreeSpace()
        {
            int recordCount = _context.Vehicle.Count();
            int Total_Garage_Capacity = Global.Garagecapacity;
            ViewbagModel.garageStatus = $"Total parking spots: {Total_Garage_Capacity}";
            ViewbagModel.freeSpots=$"Free Space :{Total_Garage_Capacity - recordCount}";
        }

        public async Task<IActionResult> Statistics()
        {
            //Create a list of an anonymous class
            var res = await _context.Vehicle.Select(v => new { Arrival = v.Arrival, Wheels = v.Wheels, Type = v.Type }).ToListAsync();

            Statistics statistics = new Statistics
            {
                TotalWheelAmount = res.Sum(r => r.Wheels),
                TotalCostsGenerated = res.Sum(v =>
                {
                    double hourlyRate = Global.HourlyRate;
                    TimeSpan duration = DateTime.Now.Subtract(v.Arrival);
                    double cost = (duration.Hours + (duration.Minutes * 1.0 / 60)) * hourlyRate;
                    return Math.Round(cost, 2);
                })
            };

            //Initialise the statistics vehicle type counter
            foreach (VehicleType type in _context.VehicleType)
            {
                statistics.VehicleTypeCounter.Add(type.Name, 0);
            }

            //Count each vehicle and count them
            foreach (Vehicle vehicle in _context.Vehicle)
            {
                statistics.VehicleTypeCounter[vehicle.Type.Name]++;
            }

            return View(statistics);
        }
        //Set the parking spots Array to the capacity of the garage.
        private void SetParkingSpots()
        {
            int spotCount = Global.Garagecapacity;
            parkingSpots = new Vehicle[spotCount];
        }
        private bool CheckIfGarageIsFull()
        {
            AddExistingDataToGarage(); //Populates the Array with the existing vehicles on the right indexes.
            bool isFull = true;
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                if (parkingSpots[i] == null)
                    isFull = false;
            }
            return isFull;
        }
        private void CheckIfGarageIsEmpty()
        {
            bool isEmpty = true;
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                if (parkingSpots[i] != null)
                    isEmpty = false;
            }
            ViewbagModel.areEmpty = isEmpty;

        }
        //Checks for the first empty spot in the array and gets that index. Then adds the vehicle to the array.
        private int FindFirstEmptySpot()
        {
            AddExistingDataToGarage(); //Populates the Array with the existing vehicles on the right indexes.
            int emptySpot = -1;
            for (int i = 0; i < parkingSpots.Length; i++)
            {
                if (parkingSpots[i] == null)
                {
                    emptySpot = i;
                    break;
                }
            }
            return emptySpot + 1;
        }
        //Goes through the database and gets the parking spot and adds it to the correct place in the array.
        private void AddExistingDataToGarage()
        {
            foreach (var item in _context.Vehicle)
            {
                int garageSpot = item.ParkingSpot - 1;
                parkingSpots[garageSpot] = item;
            }
        }

        public void CalculateParkingAmount(Vehicle vehicle)
        {
            
            double hourlyRate = Global.HourlyRate;
            TimeSpan totalParkedTime = DateTime.Now.Subtract(vehicle.Arrival);
            double cost = (totalParkedTime.Hours * hourlyRate) + (totalParkedTime.Minutes * hourlyRate / 60.0);
            cost = Math.Round(cost, 2);
            ViewbagModel.AmtTitle = "Amount";
            ViewbagModel.amount = cost + "Sek";

        }

         public async Task<IActionResult> VehicleMemberView()
        {
            var newList = await _context.Vehicle
                .Select(v => new VehicleMemberViewModel (v.License, v.Arrival, v.Owner, v.Type.Name) )
                .ToListAsync();
            return View(newList);
        }
    }
}
    