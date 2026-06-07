# Technická dokumentace — BlogicCRM

> Stručná technická dokumentace popisující samotné řešení.


---

## Obsah

1. [Architektura](#1-architektura)
2. [Technologický stack](#2-technologický-stack)
3. [Struktura projektu](#3-struktura-projektu)
4. [Datový model](#4-datový-model)
5. [ViewModely](#5-viewmodely)
6. [Autentizace](#6-autentizace)
7. [CI/CD](#7-cicd)
8. [Konfigurace a spuštění](#8-konfigurace-a-spuštění)

---

## 1. Architektura

Aplikace je serverová webová aplikace postavená na vzoru **MVC (Model–View–Controller)**. Veškerá logika a přístup k datům běží na serveru; klient dostává hotové HTML stránky.

```
Prohlížeč
    │
    │  HTTP požadavek
    ▼
┌─────────────────────┐
│     Controller      │  ← přijme požadavek, rozhoduje o logice
│  (C# třída, .NET)   │
└────────┬────────────┘
         │  načte / uloží data
         ▼
┌─────────────────────┐
│  ApplicationDbContext│  ← EF Core – ORM vrstva
│   (Entity Framework)│
└────────┬────────────┘
         │  SQL dotazy
         ▼
┌─────────────────────┐
│    SQL Server DB    │
└─────────────────────┘
         │
         │  data zpět do Controlleru
         ▼
┌─────────────────────┐
│    Razor View       │  ← .cshtml šablona → vygeneruje HTML
│    (.cshtml)        │
└─────────────────────┘
    │
    │  HTML odpověď
    ▼
Prohlížeč
```

---

## 2. Technologický stack

| Vrstva | Technologie | Verze |
|--------|-------------|-------|
| Backend framework | ASP.NET Core MVC | .NET 10.0 |
| ORM | Entity Framework Core | 10.0.8 |
| Databáze | Microsoft SQL Server | Express / LocalDB |
| Šablonovací engine | Razor (`.cshtml`) | součást .NET 10 |
| Frontend CSS | Bootstrap | 5.x |
| Autentizace | ASP.NET Core Cookie Auth | součást .NET 10 |
| CI/CD | GitHub Actions | – |

### NuGet balíčky (`BlogicCRM.csproj`)

```xml
<PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="10.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer"           Version="10.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design"              Version="10.0.8" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools"               Version="10.0.8" />
```

> Balíček `Identity.EntityFrameworkCore` je zahrnut pro případné budoucí rozšíření (správa uživatelů přes databázi). V aktuální verzi se využívá pouze cookie autentizace bez Identity UI.

### Jazyky v repozitáři

| Jazyk | Podíl |
|-------|-------|
| C# | 54,6 % |
| HTML (Razor) | 42,9 % |
| CSS | 2,2 % |
| JavaScript | 0,3 % |

---

## 3. Struktura projektu

```
BlogicCRM/
│
├── .github/
│   └── workflows/            # GitHub Actions – CI pipeline
│
├── Controllers/
│   ├── ContractController.cs # CRUD pro smlouvy
│   ├── ClientController.cs   # CRUD pro klienty
│   ├── AdvisorController.cs  # CRUD pro poradce
│   ├── AccountController.cs  # Přihlášení / odhlášení
│   └── HomeController.cs     # Úvodní stránka
│
├── Models/
│   ├── Contract.cs           # Entita smlouvy
│   ├── Client.cs             # Entita klienta
│   ├── Advisor.cs            # Entita poradce
│   └── ApplicationDbContext.cs
│
├── Helpers/                  # Pomocné třídy
│
├── Migrations/               # EF Core migrace
│
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml    # Hlavní layout (navbar, footer)
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Contract/             # Index, Details, Create, Edit, Delete
│   ├── Client/
│   ├── Advisor/
│   ├── Account/
│   │   └── Login.cshtml
│   └── Home/
│
├── wwwroot/                  # CSS, JS, obrázky
│
│   # ViewModely jsou umístěny přímo v kořeni projektu:
├── ContractFormViewModel.cs
├── ContractEditViewModel.cs
├── ClientFormViewModel.cs
├── ClientEditViewModel.cs
├── AdvisorFormViewModel.cs
├── AdvisorEditViewModel.cs
├── AdvisorCheckboxViewModel.cs
│
├── Program.cs                # Vstupní bod, konfigurace DI
├── appsettings.json
└── BlogicCRM.csproj
```

---

## 4. Datový model

### Diagram vztahů

```
┌──────────────┐         ┌────────────────────────┐         ┌──────────────┐
│    Client    │         │        Contract         │         │   Advisor    │
│──────────────│         │────────────────────────-│         │──────────────│
│ Id (PK)      │ 1     N │ Id (PK)                 │ N     1 │ Id (PK)      │
│ Jmeno        ├────────►│ EvidencniCislo           │◄────────┤ Jmeno        │
│ Prijmeni     │         │ Instituce               │ (Mgr)   │ Prijmeni     │
│ Email        │         │ ClientId (FK)           │         │ Email        │
│ Telefon      │         │ ManagerId (FK)          │         │ Telefon      │
│ RodneCislo   │         │ DatumUzavreni           │         │ RodneCislo   │
│ Vek          │         │ DatumPlatnosti          │         │ Vek          │
└──────────────┘         │ DatumUkonceni (null.)   │         └──────┬───────┘
                         │ Participants ───────────┼──── M:N ───────┘
                         └────────────────────────-┘  (spojovací tabulka)
```

### Popis entit

**Contract (Smlouva)**

| Sloupec | Typ | Validace |
|---------|-----|---------|
| Id | int | PK, autoincrement |
| EvidencniCislo | string | Required |
| Instituce | string | Required (ČSOB, AEGON, Axa…) |
| ClientId | int | FK → Client, Required |
| ManagerId | int | FK → Advisor (správce), Required |
| DatumUzavreni | DateTime | Required |
| DatumPlatnosti | DateTime | Required, může být > DatumUzavreni |
| DatumUkonceni | DateTime? | Nepovinné (nullable) |
| Participants | ICollection\<Advisor\> | M:N, min. 1 (správce je vždy zahrnut) |

**Client (Klient) & Advisor (Poradce)**

Obě entity mají totožnou strukturu polí:

| Sloupec | Typ | Validace |
|---------|-----|---------|
| Id | int | PK, autoincrement |
| Jmeno | string | Required |
| Prijmeni | string | Required |
| Email | string | Required, formát e-mailu |
| Telefon | string | Required |
| RodneCislo | string | Required |
| Vek | int | Required, min. 18 |

---

## 5. ViewModely

Projekt odděluje doménové modely (Entity Framework entity) od formulářových vstupů pomocí ViewModelů. Každá entita má dvojici:

- `*FormViewModel` — pole pro formulář při vytváření záznamu (Create)
- `*EditViewModel` — dědí z `*FormViewModel`, přidává `Id` pro úpravy (Edit)

### ContractFormViewModel

```csharp
namespace BlogicCRM.ViewModels
{
    public class ContractFormViewModel
    {
        [Required] public string EvidencniCislo { get; set; }
        [Required] public string Instituce { get; set; }

        [Required] public int ClientId { get; set; }
        public List<SelectListItem>? DostupniKlienti { get; set; }   // dropdown klientů

        [Required] public int ManagerId { get; set; }
        public List<SelectListItem>? DostupniManageri { get; set; }  // dropdown poradců

        public List<AdvisorCheckboxViewModel> DostupniUcastnici { get; set; } = new();
    }
}
```

### AdvisorCheckboxViewModel

Používá se k zobrazení seznamu poradců jako checkboxů ve formuláři smlouvy.

```csharp
public class AdvisorCheckboxViewModel
{
    public int AdvisorId   { get; set; }
    public string Jmeno    { get; set; }
    public string Prijmeni { get; set; }
    public bool IsSelected { get; set; }
}
```

---

## 6. Autentizace

Aplikace používá **ASP.NET Core Cookie Authentication** — bez databázové tabulky uživatelů. Přihlašovací údaje jsou pro účely stáže uloženy přímo v kódu.

### Konfigurace (`Program.cs`)

```csharp
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath        = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan   = TimeSpan.FromHours(8);
        options.Cookie.Name      = "BlogicCRM.Auth";
    });

// Pořadí middleware je klíčové:
app.UseAuthentication();   // ← musí být před Authorization
app.UseAuthorization();
```

### Tok přihlášení (`AccountController`)

1. Uživatel odešle jméno a heslo přes formulář
2. Controller porovná vstup s hodnotami `admin` / `heslo`
3. Při shodě vytvoří `ClaimsPrincipal` a zavolá `HttpContext.SignInAsync()`
4. Prohlížeč dostane šifrovaný cookie platný 8 hodin

### Ochrana akcí

Akce `Create`, `Edit` a `Delete` ve všech controllerech jsou označeny atributem `[Authorize]`. Nepřihlášený uživatel je automaticky přesměrován na `/Account/Login`.

Tlačítka v šablonách jsou podmíněně skryta:
```html
@if (User.Identity?.IsAuthenticated == true)
{
    <a asp-action="Edit" ...>Upravit</a>
    <a asp-action="Delete" ...>Smazat</a>
}
```

### Poznámka k bezpečnosti

Tato implementace je určena výhradně pro demo/testování v rámci stáže. V produkci by bylo nutné hesla hashovat a přihlašovací údaje ukládat do databáze (k tomu je v projektu připraven balíček `Identity.EntityFrameworkCore`).

---

## 7. CI/CD

Repozitář obsahuje konfiguraci GitHub Actions v `.github/workflows/`. Pipeline se spouští automaticky při každém push na větev `master` a ověřuje, že projekt jde sestavit (`dotnet build`).

---

## 8. Konfigurace a spuštění

### `appsettings.json`

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogicCRM;Trusted_Connection=True;"
  }
}
```

Pro SQL Server Express změň na:
```
Server=.\\SQLEXPRESS;Database=BlogicCRM;Trusted_Connection=True;
```

### Vytvoření databáze

```bash
dotnet ef database update
```

### Spuštění aplikace

```bash
dotnet run
# nebo F5 ve Visual Studio / Rider
```