using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogicCRM.Models;
using BlogicCRM.ViewModels;
using BlogicCRM.Helpers;
using Microsoft.AspNetCore.Authorization; 
namespace BlogicCRM.Controllers
{
    public class ContractsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContractsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult ExportToCsv()
        {
            var contracts = _context.Contracts
                .Select(c => new { c.Id, c.EvidencniCislo, c.Instituce, c.ClientId, c.ManagerId })
                .ToList();

            var csvBytes = CsvExportHelper.GenerateCsv(contracts);
            return File(csvBytes, "text/csv", "contracts_export.csv");
        }

        public IActionResult Index()
        {
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.Manager)
                .ToList();

            return View(contracts);
        }
        public IActionResult Details(int id)
        {
            var contract = _context.Contracts
                .Include(c => c.Client)
                .Include(c => c.Manager)
                .Include(c => c.ContractAdvisors).ThenInclude(ca => ca.Advisor)
                .FirstOrDefault(c => c.Id == id);

            if (contract == null) return NotFound();

            return View(contract);
        }
        [Authorize]
        public IActionResult Create()
        {
            var model = new ContractFormViewModel();
            PopulateDropdownsAndCheckboxes(model);
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContractFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newContract = new Contract
                {
                    EvidencniCislo = model.EvidencniCislo,
                    Instituce = model.Instituce,
                    ClientId = model.ClientId,
                    ManagerId = model.ManagerId
                };

                _context.Contracts.Add(newContract);
                _context.SaveChanges();

                var selectedAdvisorIds = model.DostupniUcastnici
                    .Where(x => x.IsSelected)
                    .Select(x => x.AdvisorId)
                    .ToList();

                if (!selectedAdvisorIds.Contains(model.ManagerId))
                {
                    selectedAdvisorIds.Add(model.ManagerId);
                }

                foreach (var advisorId in selectedAdvisorIds)
                {
                    var contractAdvisor = new ContractAdvisor
                    {
                        ContractId = newContract.Id,
                        AdvisorId = advisorId
                    };
                    _context.ContractAdvisors.Add(contractAdvisor);
                }

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            PopulateDropdownsAndCheckboxes(model);
            return View(model);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var contract = _context.Contracts
                .Include(c => c.ContractAdvisors)
                .FirstOrDefault(c => c.Id == id);

            if (contract == null) return NotFound();

            var model = new ContractEditViewModel
            {
                Id = contract.Id,
                EvidencniCislo = contract.EvidencniCislo,
                Instituce = contract.Instituce,
                ClientId = contract.ClientId,
                ManagerId = contract.ManagerId
            };

            PopulateDropdownsAndCheckboxes(model);

            var currentAdvisorIds = contract.ContractAdvisors.Select(ca => ca.AdvisorId).ToList();

            foreach (var participant in model.DostupniUcastnici)
            {
                participant.IsSelected = currentAdvisorIds.Contains(participant.AdvisorId);
            }

            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ContractEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var contract = _context.Contracts.Find(id);
                if (contract == null) return NotFound();

                contract.EvidencniCislo = model.EvidencniCislo;
                contract.Instituce = model.Instituce;
                contract.ClientId = model.ClientId;
                contract.ManagerId = model.ManagerId;

                var oldRelations = _context.ContractAdvisors.Where(ca => ca.ContractId == id);
                _context.ContractAdvisors.RemoveRange(oldRelations);

                var selectedAdvisorIds = model.DostupniUcastnici
                    .Where(x => x.IsSelected)
                    .Select(x => x.AdvisorId)
                    .ToList();

                if (!selectedAdvisorIds.Contains(model.ManagerId))
                {
                    selectedAdvisorIds.Add(model.ManagerId);
                }

                foreach (var advisorId in selectedAdvisorIds)
                {
                    _context.ContractAdvisors.Add(new ContractAdvisor { ContractId = id, AdvisorId = advisorId });
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            PopulateDropdownsAndCheckboxes(model);
            return View(model);
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            var contract = _context.Contracts
                .Include(c => c.Client)
                .FirstOrDefault(c => c.Id == id);

            if (contract == null) return NotFound();
            return View(contract);
        }
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var contract = _context.Contracts.Find(id);
            if (contract != null)
            {
                var relations = _context.ContractAdvisors.Where(ca => ca.ContractId == id);
                _context.ContractAdvisors.RemoveRange(relations);

                _context.Contracts.Remove(contract);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        private void PopulateDropdownsAndCheckboxes(ContractFormViewModel model)
        {
            model.DostupniKlienti = _context.Clients
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = $"{c.Jmeno} {c.Prijmeni}"
                }).ToList();

            model.DostupniManageri = _context.Advisors
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"{a.Jmeno} {a.Prijmeni}"
                }).ToList();

            model.DostupniUcastnici = _context.Advisors
                .Select(a => new AdvisorCheckboxViewModel
                {
                    AdvisorId = a.Id,
                    JmenoPrijmeni = $"{a.Jmeno} {a.Prijmeni}",
                    IsSelected = false
                }).ToList();
        }
    }
}