using Microsoft.AspNetCore.Mvc;
using BlogicCRM.Models;
using BlogicCRM.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BlogicCRM.Helpers;

namespace BlogicCRM.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ExportToCsv()
        {
            var clients = _context.Clients.ToList();
            var csvBytes = CsvExportHelper.GenerateCsv(clients);
            return File(csvBytes, "text/csv", "clients_export.csv");
        }

        public IActionResult Index()
        {
            var seznamKlientu = _context.Clients.ToList();
            return View(seznamKlientu);
        }

        public IActionResult Details(int id)
        {
            var klient = _context.Clients.Find(id);
            if (klient == null) return NotFound();
            return View(klient);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View(new ClientFormViewModel());
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ClientFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var novyKlient = new Client
                {
                    Jmeno = model.Jmeno,
                    Prijmeni = model.Prijmeni,
                    Email = model.Email,
                    Telefon = model.Telefon,
                    RodneCislo = model.RodneCislo,
                    Vek = model.Vek
                };
                _context.Clients.Add(novyKlient);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Authorize]
        public IActionResult Edit(int id)
        {
            var klient = _context.Clients.Find(id);
            if (klient == null) return NotFound();

            var model = new ClientEditViewModel
            {
                Id = klient.Id,
                Jmeno = klient.Jmeno,
                Prijmeni = klient.Prijmeni,
                Email = klient.Email,
                Telefon = klient.Telefon,
                RodneCislo = klient.RodneCislo,
                Vek = klient.Vek
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ClientEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var klient = _context.Clients.Find(id);
                if (klient == null) return NotFound();
                klient.Jmeno = model.Jmeno;
                klient.Prijmeni = model.Prijmeni;
                klient.Email = model.Email;
                klient.Telefon = model.Telefon;
                klient.RodneCislo = model.RodneCislo;
                klient.Vek = model.Vek;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var klient = _context.Clients.Find(id);
            if (klient == null) return NotFound();
            return View(klient);
        }

        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var klient = _context.Clients.Find(id);
            if (klient != null)
            {
                _context.Clients.Remove(klient);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}