﻿#nullable disable
using AutoMapper;
using Garage_2._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Garage_2._0.Controllers.VehiclesController
{
    public class VehiclesController : Controller
    {
        private readonly GarageVehicleContext _context;
        private readonly IMapper mapper;

        public VehiclesController(GarageVehicleContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            Global.Garagecapacity = config.GetValue<int>("GarageCapacity:Capacity");
            Global.HourlyRate = config.GetValue<double>("Price:HourlyRate");
            Global.BaseRate = config.GetValue<double>("Price:BaseRate");
            this.mapper = mapper;
        }



        // GET: Vehicles
        public async Task<IActionResult> Index()
        {
            TotalGarageCapacity_and_FreeSpace();
            ViewbagModel.spotsTaken = _context.ParkinSpot.Include(p => p.Vehicle).ToArray();
            return View(await _context.Vehicle.ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vehicle = await _context.Vehicle
                .Include(v => v.Type)
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
        public async Task<IActionResult> Create(VehicleCreateViewModel vehicleCreate)
        {

            //Check if license already exists in the database. If it exists, don't add the Vehicle.
            if (_context.Vehicle.Find(vehicleCreate.License) != null)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var vehicle = mapper.Map<Vehicle>(vehicleCreate);
                vehicle.License = vehicle.License.ToUpper();
                vehicle.Type = _context.VehicleType.Find(vehicle.VehicleTypeName);
                vehicle.Arrival = DateTime.Now;
                vehicle.ParkingSpot = await _context.ParkinSpot.FirstOrDefaultAsync(p => p.Available);
                vehicle.ParkingSpot.Vehicle = vehicle;
                vehicle.ParkingSpot.Available = false;
                _context.Add(vehicle);
                await _context.SaveChangesAsync();
                TempData["message"] = $"{vehicle.License} has been successfully parked in spot {vehicle.ParkingSpot.Id}!";
                return RedirectToAction(nameof(VehiclesOverview));
            }
            return View(vehicleCreate);
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
            //int OwnerAge = DateTime.Now.Year - (int.Parse(MemberId.AsSpan(0, 4)));
            
            if (MemberId.Length != 13)
                return Json("Enter your Social Security Number in correct Format yyyymmdd-xxxx");
            if (DateTime.Now.Year - (int.Parse(MemberId.AsSpan(0, 4)))<18)
                return Json("You must be over 18 to Park a Vehicle");
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
                    _context.Entry(vehicle).Reference(v => v.ParkingSpot).IsModified = false; //Makes sure that the parking spot don't change
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
                .Include(v => v.ParkingSpot)
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
            var vehicle = _context.Vehicle.Include(v => v.ParkingSpot).FirstOrDefault(v => v.License == id);
            vehicle.ParkingSpot.Available = true;
            vehicle.ParkingSpot.Vehicle = null;
            _context.Vehicle.Remove(vehicle);
            await _context.SaveChangesAsync();
            TempData["message"] = $"{vehicle.License} has been checked out!";
            return RedirectToAction(nameof(VehiclesOverview));
        }

        private bool VehicleExists(string id)
        {
            return _context.Vehicle.Any(e => e.License == id);
        }

        private double BaseDiscountBonus(string type, int parkSpotsOccupied)
        {
            switch (type)
            {
                case "Pro":
                    //The rule for Pro membership: 10% discount iff the vehicle occupies more than one spot
                    return parkSpotsOccupied > 1 ? 0.9 : 1;
                default:
                    return 1;
            }
        }

        private double CalculatePrice(Vehicle vehicle)
        {
            // 1 Spot = baseprice + hourly * time
            // 2 Spot = baseprice * 1.3 + hourly * 1.4 * time
            // 3+ Spot = baseprice * 1.6 + hourly * 1.5 * time

            int parkSpotsOccupied = _context.VehicleType.Find(vehicle.VehicleTypeName).Size;

            double basepriceMultiplier = 1.0d,
                hourlyMultiplier = 1.0d;

            if (parkSpotsOccupied == 2)
            {
                basepriceMultiplier = 1.3d;
                hourlyMultiplier = 1.4d;
            }
            else if (parkSpotsOccupied > 2)
            {
                basepriceMultiplier = 1.6d;
                hourlyMultiplier = 1.5d;
            }

            double totalPrice = 0;

            /* General algorithm:
                1. Get a list of all memberships the member had during the period.
                2. The price for the first element is calculated by membershipEnded - vehicleArrival
                3. The price for the last element is calculated by membershipStarted - datetimeNow
                4. All elements inbetween are calculated iteratively by:
                    a. The price from membershipEnded - membershipStarted
                5. Add all prices and return total.
            */
            // 1. Get all memberships the member had during the time period
            List<MemberHasMembership> hasMemberships = _context.MemberHasMembership.Include(m => m.Membership)
                .Where(m => m.MemberId == vehicle.MemberId)
                .Where(m => m.StartDate <= DateTime.Now)
                .Where(m => m.FinishedDate == null ? true : m.FinishedDate >= vehicle.Arrival)
                .ToList();
            //Check if membership expired during stay
            if (hasMemberships.Count > 1)
            {
                // 2. 
                Membership firstMembership = _context.Membership.FirstOrDefault(m => m.Type == hasMemberships.First().MembershipId);
                TimeSpan firstTotalTime = (TimeSpan)(hasMemberships.First().FinishedDate - vehicle.Arrival);
                double firstHourlyDiscountBonus = Math.Max(1 - 4 * (1 - firstMembership.BenefitHourly), 1 - parkSpotsOccupied * (1 - firstMembership.BenefitHourly));
                double firstBaseDiscountBonus = BaseDiscountBonus(firstMembership.Type, parkSpotsOccupied);
                double firstTimeHours = firstTotalTime.Days * 24 + firstTotalTime.Hours + firstTotalTime.Minutes * 1.0 / 60.0;

                totalPrice += Global.BaseRate * basepriceMultiplier * firstMembership.BenefitBase * firstBaseDiscountBonus 
                    + Global.HourlyRate * hourlyMultiplier * firstHourlyDiscountBonus * firstTimeHours;
                // 3.
                Membership lastMembership = _context.Membership.FirstOrDefault(m => m.Type == hasMemberships.Last().MembershipId);
                TimeSpan lastTotalTime = (DateTime.Now - hasMemberships.Last().StartDate);
                double lastHourlyDiscountBonus = Math.Max(1 - 4 * (1 - lastMembership.BenefitHourly), 1 - parkSpotsOccupied * (1 - lastMembership.BenefitHourly));
                double lastBaseDiscountBonus = BaseDiscountBonus(lastMembership.Type, parkSpotsOccupied);
                double lastTimeHours = lastTotalTime.Days * 24 + lastTotalTime.Hours + lastTotalTime.Minutes * 1.0 / 60.0;

                totalPrice += Global.BaseRate * basepriceMultiplier * lastMembership.BenefitBase * lastBaseDiscountBonus
                    + Global.HourlyRate * hourlyMultiplier * lastHourlyDiscountBonus * lastTimeHours;
                // 4. 
                Membership middleMembership;
                TimeSpan middleTotalTime;
                double middleHourlyDiscountBonus;
                double middleBaseDiscountBonus;
                double middleTimeHours;
                for (int i = 1; i < hasMemberships.Count - 1; i++)
                {
                    middleMembership = _context.Membership.FirstOrDefault(m => m.Type == hasMemberships[i].MembershipId);
                    middleTotalTime = (TimeSpan)(hasMemberships[i].FinishedDate - hasMemberships[i].StartDate);
                    middleHourlyDiscountBonus = Math.Max(1 - 4 * (1 - middleMembership.BenefitHourly), 1 - parkSpotsOccupied * (1 - middleMembership.BenefitHourly));
                    middleBaseDiscountBonus = BaseDiscountBonus(middleMembership.Type, parkSpotsOccupied);
                    middleTimeHours = middleTotalTime.Days * 24 + middleTotalTime.Hours + middleTotalTime.Minutes * 1.0 / 60.0;

                    totalPrice += Global.BaseRate * basepriceMultiplier * middleMembership.BenefitBase * middleBaseDiscountBonus
                    + Global.HourlyRate * hourlyMultiplier * middleHourlyDiscountBonus * middleTimeHours;
                }
            }
            else
            {
                //No changes in the membership
                Membership membership = _context.Membership.FirstOrDefault(m => m.Type == hasMemberships.First().MembershipId);
                TimeSpan totalTime = DateTime.Now.Subtract(vehicle.Arrival);
                double hourlyDiscountBonus = Math.Max(1 - 4 * (1 - membership.BenefitHourly), 1 - parkSpotsOccupied * (1 - membership.BenefitHourly));
                double baseDiscountBonus = BaseDiscountBonus(membership.Type, parkSpotsOccupied);

                double timeHours = totalTime.Days * 24 + totalTime.Hours + totalTime.Minutes * 1.0 / 60;
                totalPrice = Global.BaseRate * basepriceMultiplier * membership.BenefitBase * baseDiscountBonus 
                    + Global.HourlyRate * hourlyMultiplier * hourlyDiscountBonus * timeHours;
            }

            return Math.Round(totalPrice, 2);
        }
        //Calculate Total Parked Time + View Model for the Receipt
        public async Task<IActionResult> ReceiptView(string id)
        {
            //regNo should come from check-out so
            Vehicle vehicle = await _context.Vehicle
                                .Include(v => v.Owner)
                                .Include(v => v.ParkingSpot)
                                .Include(v => v.Type)
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
                receipt.Price = $"{CalculatePrice(vehicle)} kr"; //cost + "Sek";
            }
            else
                return NotFound();
            vehicle.ParkingSpot.Available = true;
            _context.Vehicle.Remove(vehicle);
            _context.SaveChanges();
            TempData["message"] = $"{vehicle.License} has been checked out";
            return View(nameof(ReceiptView), receipt);
        }
        //This one is used on the detailed view
        public async Task<IActionResult> SearchDetailed(string plate, string type)
        {
            if (plate == null && type == null)
            {
                TempData["Error"] = "You need to enter a License plate or select Type before you search";
                return RedirectToAction(nameof(Index));
            }
            IQueryable<Vehicle> model = null!;

            if (plate != null && type == null)
            {
                model = _context.Vehicle.Where(v => v.License.Contains(plate));
                await model.ToListAsync();
            }

            else if (plate == null && type != null)
            {
                model = _context.Vehicle.Where(v => v.Type.Name.Contains(type));
                await model.ToListAsync();
            }

            else
            {
                model = _context.Vehicle.Where(v => v.Type.Name.Contains(type))
                                         .Where(v => v.License.Contains(plate));
                await model.ToListAsync();
            }

            if (!model.Any())
            {
                TempData["Error"] = "Sorry your search did not yield a result";
            }
            ViewBag.Button = "true";

            return View(nameof(Index), await model.ToListAsync());
        }
        
        //this one is used on the Overview
        public async Task<IActionResult> Search(string plate, string type)
        {
            if (plate == null && type == null)
            {
                TempData["Error"] = "You need to enter a License plate or select Type before you search";
                ViewBag.Button = "true";
                return RedirectToAction(nameof(VehiclesOverview));
            }
            IQueryable<VehicleViewModel> model = null!;
            if (plate != null && type == null)
            {
                model = _context.Vehicle.Where(v => v.License.Contains(plate))
                                                                 .Select(v => new VehicleViewModel
                                                                 {
                                                                     Type = v.Type,
                                                                     License = v.License,
                                                                     Make = v.Make,
                                                                     TimeSpent = Global.TimeAsString(DateTime.Now.Subtract(v.Arrival))
                                                                 });
                await model.ToListAsync();
            }
            else if (plate == null && type != null)
            {
                model = _context.Vehicle.Where(v => v.Type.Name == type)
                                        .Select(v => new VehicleViewModel
                                        {
                                            Type = v.Type,
                                            License = v.License,
                                            Make = v.Make,
                                            TimeSpent = Global.TimeAsString(DateTime.Now.Subtract(v.Arrival))
                                        });
                await model.ToListAsync();
            }
            else
            {
                model = _context.Vehicle.Where(v => v.Type.Name == type)
                                        .Where(v => v.License.Contains(plate))
                                        .Select(v => new VehicleViewModel
                                        {
                                            Type = v.Type,
                                            License = v.License,
                                            Make = v.Make,
                                            TimeSpent = Global.TimeAsString(DateTime.Now.Subtract(v.Arrival))
                                        });
                await model.ToListAsync();
            }

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
                TimeSpent = Global.TimeAsString(DateTime.Now.Subtract(v.Arrival))
            });
            TotalGarageCapacity_and_FreeSpace();
            //AddExistingDataToGarage(); //Populates the Array with the existing vehicles on the right indexes.
            ViewbagModel.spotsTaken = _context.ParkinSpot.Include(p => p.Vehicle).Where(p => !p.Available).ToArray();
            CheckIfGarageIsEmpty();
            return View(await simpleViewList.ToListAsync());
        }

        //Calculating Available free space
        public void TotalGarageCapacity_and_FreeSpace()
        {
            int recordCount = _context.Vehicle.Count();
            int Total_Garage_Capacity = Global.Garagecapacity;
            ViewbagModel.garageStatus = $"Total parking spots: {Total_Garage_Capacity}";
            ViewbagModel.freeSpots = $"Free Space :{Total_Garage_Capacity - recordCount}";
        }

        public async Task<IActionResult> Statistics()
        {
            //Create a list of an anonymous class
            var res = await _context.Vehicle.Select(v => new { Arrival = v.Arrival, Wheels = v.Wheels, Type = v.Type }).ToListAsync();

            Statistics statistics = new Statistics
            {
                TotalWheelAmount = res.Sum(r => r.Wheels),
                TotalCostsGenerated = _context.Vehicle.ToList().Sum(v => CalculatePrice(v))
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
        
        private bool CheckIfGarageIsFull()
        {
            return !_context.ParkinSpot.Any(p => p.Available);
    
        }
        private void CheckIfGarageIsEmpty()
        {
            
            ViewbagModel.areEmpty = !_context.ParkinSpot.Any(p => !p.Available);

        }
                
        public void CalculateParkingAmount(Vehicle vehicle)
        {
            ViewbagModel.AmtTitle = "Amount";
            ViewbagModel.amount = $"{CalculatePrice(vehicle)} kr";
        }

        public async Task<IActionResult> VehicleMemberView()
        {
            var newList = await _context.Vehicle.Include(v => v.Owner.Memberships)
                .Select(v => new VehicleMemberViewModel(v.License, v.Arrival, v.Owner, v.Type.Name))
                .ToListAsync();
            return View(newList);
        }
        public async Task<IActionResult> VehicleTypeCreate()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VehicleTypeCreate(VehicleType type)
        {
            if (_context.VehicleType.Find(type.Name) != null)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                _context.VehicleType.Add(type);
                await _context.SaveChangesAsync();
                TempData["message"] = $"{type.Name} has been successfully to Vehicle types!";
                return RedirectToAction(nameof(Create));
            }
            return View(type);
        
        }
    }
}
