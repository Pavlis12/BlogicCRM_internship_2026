using BlogicCRM.Helpers;
using BlogicCRM.Models;
using BlogicCRM.ViewModels;
using Microsoft.AspNetCore.Mvc;


namespace BlogicCRM.Controllers
{
    public class AdvisorsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public IActionResult ExportToCsv()
        {
            var advisors = _context.Advisors.ToList();
            var csvBytes = CsvExportHelper.GenerateCsv(advisors);
            return File(csvBytes, "text/csv", "advisors_export.csv");
        }
        public AdvisorsController(ApplicationDbContext context)
        {
            _context = context;
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

        public IActionResult Create()
        {
            return View(new AdvisorFormViewModel());
        }

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
        public IActionResult Delete(int id)
        {
            var poradce = _context.Advisors.Find(id);
            if (poradce == null) return NotFound();
            return View(poradce);
        }

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