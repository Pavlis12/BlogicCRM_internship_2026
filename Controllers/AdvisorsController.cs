using BlogicCRM.Helpers;
using BlogicCRM.Models;
using BlogicCRM.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BlogicCRM.Controllers
{
    public class AdvisorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdvisorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ExportToCsv()
        {
            var advisors = _context.Advisors.ToList();
            var csvBytes = CsvExportHelper.GenerateCsv(advisors);
            return File(csvBytes, "text/csv", "advisors_export.csv");
        }

        public IActionResult Index()
        {
            var poradci = _context.Advisors.ToList();
            return View(poradci);
        }

        public IActionResult Details(int id)
        {
            var poradce = _context.Advisors.Find(id);
            if (poradce == null) return NotFound();
            return View(poradce);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View(new AdvisorFormViewModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AdvisorFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var novyPoradce = new Advisor
                {
                    Jmeno = model.Jmeno,
                    Prijmeni = model.Prijmeni,
                    Email = model.Email,
                    Telefon = model.Telefon,
                    RodneCislo = model.RodneCislo,
                    Vek = model.Vek
                };

                _context.Advisors.Add(novyPoradce);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var poradce = _context.Advisors.Find(id);
            if (poradce == null) return NotFound();

            var model = new AdvisorEditViewModel
            {
                Id = poradce.Id,
                Jmeno = poradce.Jmeno,
                Prijmeni = poradce.Prijmeni,
                Email = poradce.Email,
                Telefon = poradce.Telefon,
                RodneCislo = poradce.RodneCislo,
                Vek = poradce.Vek
            };
            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, AdvisorEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var poradce = _context.Advisors.Find(id);
                if (poradce == null) return NotFound();

                poradce.Jmeno = model.Jmeno;
                poradce.Prijmeni = model.Prijmeni;
                poradce.Email = model.Email;
                poradce.Telefon = model.Telefon;
                poradce.RodneCislo = model.RodneCislo;
                poradce.Vek = model.Vek;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var poradce = _context.Advisors.Find(id);
            if (poradce == null) return NotFound();
            return View(poradce);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var poradce = _context.Advisors.Find(id);
            if (poradce != null)
            {
                _context.Advisors.Remove(poradce);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}