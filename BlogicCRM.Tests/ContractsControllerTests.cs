using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogicCRM.Controllers;
using BlogicCRM.Models;
using BlogicCRM.ViewModels;
using Xunit;

namespace BlogicCRM.Tests
{
    public class ContractsControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public void Create_PostAction_ManagerIsNotSelectedAsParticipant_AutomaticallyAddsManagerToContractAdvisors()
        {
            using var context = GetInMemoryDbContext();

            var client = new Client { Id = 1, Jmeno = "John", Prijmeni = "Doe", Email = "j@d.com", Telefon = "123", RodneCislo = "111", Vek = 30 };
            var advisor = new Advisor { Id = 5, Jmeno = "Peter", Prijmeni = "Smart", Email = "p@s.com", Telefon = "456", RodneCislo = "222", Vek = 40 };

            context.Clients.Add(client);
            context.Advisors.Add(advisor);
            context.SaveChanges();

            var model = new ContractFormViewModel
            {
                EvidencniCislo = "CON-2026-001",
                Instituce = "Allianz",
                ClientId = client.Id,
                ManagerId = advisor.Id,
                DostupniUcastnici = new List<AdvisorCheckboxViewModel>
                {
                    new AdvisorCheckboxViewModel { AdvisorId = advisor.Id, JmenoPrijmeni = "Peter Smart", IsSelected = false }
                }
            };

            var controller = new ContractsController(context);

            var result = controller.Create(model);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            var savedContract = context.Contracts.FirstOrDefault(c => c.EvidencniCislo == "CON-2026-001");
            Assert.NotNull(savedContract);

            var relationExists = context.ContractAdvisors.Any(ca => ca.ContractId == savedContract.Id && ca.AdvisorId == advisor.Id);
            Assert.True(relationExists);
        }

        [Fact]
        public void Create_PostAction_InvalidModelState_DoesNotSaveToDatabaseAndReturnsView()
        {
            using var context = GetInMemoryDbContext();
            var controller = new ContractsController(context);

            controller.ModelState.AddModelError("EvidencniCislo", "Evidenční číslo je povinné.");

            var model = new ContractFormViewModel
            {
                Instituce = "ČSOB",
                ClientId = 1,
                ManagerId = 2
            };

            var result = controller.Create(model);

            Assert.IsType<ViewResult>(result);
            Assert.Equal(0, context.Contracts.Count());
        }

        [Fact]
        public void DeleteConfirmed_PostAction_RemovesContractAndItsAdvisorsRelations()
        {
            using var context = GetInMemoryDbContext();

            var contract = new Contract { Id = 99, EvidencniCislo = "TEST-DEL", Instituce = "KB", ClientId = 1, ManagerId = 1 };
            context.Contracts.Add(contract);

            var relation = new ContractAdvisor { ContractId = 99, AdvisorId = 1 };
            context.ContractAdvisors.Add(relation);

            context.SaveChanges();

            var controller = new ContractsController(context);

            var result = controller.DeleteConfirmed(99);

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            var deletedContract = context.Contracts.FirstOrDefault(c => c.Id == 99);
            Assert.Null(deletedContract);

            var deletedRelations = context.ContractAdvisors.Where(ca => ca.ContractId == 99).ToList();
            Assert.Empty(deletedRelations);
        }
    }
}