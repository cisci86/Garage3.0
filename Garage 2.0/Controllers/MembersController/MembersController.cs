﻿#nullable disable
using AutoMapper;
using Garage_2._0.Models;
using Garage_2._0.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using PagedList.Mvc;

namespace Garage_2._0.Controllers.MembersController
{
    public class MembersController : Controller
    {
        private readonly GarageVehicleContext _context;
        private readonly IMapper mapper;

        public MembersController(GarageVehicleContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
            return View(await _context.Member.ToListAsync());
        }
        public async Task<IActionResult> MemberOverviewIndex(string sortOrder, string currentFilter, string ssn,int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "FirstName_desc" : "";
            
            
                       
            

            ViewBag.CurrentFilter = ssn;

            var viewmodel = _context.Member.Include(m => m.Memberships).Include(m => m.Vehicles)
                .Select(m => new MemberOverViewModel
                {
                    SocialSecurityNumber = m.SocialSecurityNumber,
                    FirstName = m.Name.FirstName,
                    LastName = m.Name.LastName,
                    Membership = m.Memberships.OrderBy(m => m.Id).Last().MembershipId,
                    VehicleCount = m.Vehicles.Count
                }).AsEnumerable();

            if (ssn != null)
            {
                pageNumber = 1;
                ViewBag.Button = "true";
            }
            else
            {
                ViewBag.Button = "";
                ssn = currentFilter;
                
            }
            if (!String.IsNullOrEmpty(ssn))
            {
                viewmodel = viewmodel.Where(s => s.SocialSecurityNumber.Contains(ssn));
            }
           
            switch (sortOrder)
            {
                case "FirstName_desc":
                    viewmodel = viewmodel.OrderByDescending(x => x.FirstName.Substring(0, 2), StringComparer.Ordinal).ToList();
                    break;

                default:
                    viewmodel = viewmodel.OrderBy(x => x.FirstName.Substring(0, 2), StringComparer.Ordinal).ToList();
                    break;
            }
            int pageSize = 5;

           
            return View(await PaginatedList<MemberOverViewModel>.CreateAsync(viewmodel.AsEnumerable().ToList(), pageNumber ?? 1, pageSize));

        }
        // GET: Members/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.Include(m => m.Vehicles).Include(m => m.Memberships)
                .FirstOrDefaultAsync(m => m.SocialSecurityNumber == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // GET: Members/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MemberCreateViewModel viewModel)
        {
            if (_context.Member.Find(viewModel.SocialSecurityNumber) != null)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var member = mapper.Map<Member>(viewModel);
                var memberHasMemberShip = new MemberHasMembership(member.SocialSecurityNumber)
                {
                    Member = member,
                    Membership = _context.Membership.Find("Standard"),
                    StartDate = DateTime.Now,

                };
                member.Memberships.Add(memberHasMemberShip);
                //member.Memberships.Add(_context.MemberHasMembership.Find(_context.Membership.Find("Standard")));
                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MemberOverviewIndex));
            }
            return View(viewModel);
        }

        // GET: Members/Edit/5
        public async Task<IActionResult> Edit(string Id)

        {
            if (Id == null)
            {
                return NotFound();
            }

            var member = await _context.Member.FindAsync(Id);// var member = await _context.Member.FindAsync(id);
            var memberx = mapper.Map<MemberEditviewModel>(member);
            if (memberx == null)
            {
                return NotFound();
            }
            return View(memberx);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, MemberEditviewModel viewModel)
        {
            var member = mapper.Map<Member>(viewModel);
            if (id != member.SocialSecurityNumber)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    _context.Entry(member).Property(m => m.SocialSecurityNumber).IsModified = false;
                    _context.Entry(member).Collection(m => m.Memberships).IsModified = false;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.SocialSecurityNumber))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MemberOverviewIndex));
            }
            return View(member);
        }


        // GET: Members/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .FirstOrDefaultAsync(m => m.SocialSecurityNumber == id);

            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var member = await _context.Member
                                .Include(m => m.Vehicles)
                                .FirstOrDefaultAsync(m => m.SocialSecurityNumber == id);
            foreach (var vehicle in member.Vehicles)
            {
                var p = await _context.ParkinSpot.FirstOrDefaultAsync(p => p.Vehicle == vehicle);
                p.Available = true;
                _context.Remove(vehicle);
            }

            _context.Member.Remove(member);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MemberOverviewIndex));
        }

        private bool MemberExists(string id)
        {
            return _context.Member.Any(e => e.SocialSecurityNumber == id);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult CheckForDuplicateMembers(string SocialSecurityNumber)
        {
            //Check if ssn already exists in the database. Sends a warning if it exists
            if (_context.Member.Find(SocialSecurityNumber) != null)
            {
                return Json($"Your social security number: {SocialSecurityNumber} is already in use.");
            }
            return Json(true);
        }

 
    }
}
