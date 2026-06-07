# BlogicCRM

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)
![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET_Core-MVC-blue)
![EF Core](https://img.shields.io/badge/EF_Core-10.0.8-orange)
![SQL Server](https://img.shields.io/badge/Database-SQL_Server-CC2927?logo=microsoftsqlserver)
![C#](https://img.shields.io/badge/C%23-54.6%25-239120?logo=csharp)

---

Tento projekt vznikl v rámci ucházení se o stáže ve společnosti **Blogic** v roce 2026.
Byl vypracován za využití znalostí nabytých ve škole a z předchozí praxe.
Na místech, kde mé znalosti nestačily nebo jsem si nebyl jistý správným postupem, jsem se opřel o pomoc AI nástrojů — konkrétně **Claude** a **Gemini**.
Při tvorbě jsem vždy nejprve přemýšlel sám, snažil se problém logicky rozebrat a teprve pak případně hledal pomoc.
Věřím, že kombinace vlastního uvažování a moderních nástrojů je přirozená součást dnešního vývoje.

---

## Dokumentace

| Dokument | Obsah |
|----------|-------|
| [📘 Technická dokumentace](TECHNICKA_DOKUMENTACE.md) | Architektura, datový model, použité technologie, popis řešení |
| [📗 Uživatelská dokumentace](UZIVATELSKA_DOKUMENTACE.md) | Jak s aplikací pracovat z pohledu uživatele |

---

## O aplikaci

BlogicCRM je webová aplikace pro **správu pojišťovacích smluv**. Umožňuje evidovat smlouvy s vazbou na klienty a poradce, spravovat všechny záznamy přes přehledné seznamy a detaily, a chránit úpravy pomocí přihlášení.

---

## Rychlý start

### Požadavky

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server nebo SQL Server Express / LocalDB

### Spuštění

```bash
# 1. Naklonuj repozitář
git clone https://github.com/Pavlis12/BlogicCRM_internship_2026.git
cd BlogicCRM_internship_2026

# 2. Nastav connection string v appsettings.json
# "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogicCRM;Trusted_Connection=True;"

# 3. Vytvoř databázi
dotnet ef database update

# 4. Spusť aplikaci
dotnet run
```

Aplikace poté běží na `https://localhost:5001`.

### Přihlášení (testovací účet)

| Pole | Hodnota |
|------|---------|
| Uživatelské jméno | `admin` |
| Heslo | `heslo` |

---

## Struktura projektu

```
BlogicCRM/
├── Controllers/          # MVC controllery (Contract, Client, Advisor, Account)
├── Models/               # Datové modely + ApplicationDbContext
├── Views/                # Razor šablony (.cshtml)
├── Helpers/              # Pomocné třídy
├── Migrations/           # EF Core migrace databáze
├── wwwroot/              # Statické soubory (CSS, JS)
├── *ViewModel.cs         # ViewModely (ve složce projektu)
├── Program.cs            # Vstupní bod aplikace, DI konfigurace
├── appsettings.json      # Konfigurace (connection string)
└── BlogicCRM.csproj      # Definice projektu a NuGet balíčků
```

---

## Technologie

| Vrstva | Technologie |
|--------|-------------|
| Backend | ASP.NET Core MVC / .NET 10 |
| ORM | Entity Framework Core 10.0.8 |
| Databáze | Microsoft SQL Server |
| Frontend | Razor + Bootstrap 5 |
| Autentizace | ASP.NET Core Cookie Authentication |
| CI/CD | GitHub Actions |

---

## Splněné požadavky

- [x] Evidence smluv — evidenční číslo, instituce, klient, správce, datum uzavření, datum platnosti, datum ukončení
- [x] Více účastníků smlouvy (poradců), minimálně jeden je zároveň správce
- [x] Evidence klientů — jméno, příjmení, e-mail, telefon, rodné číslo, věk
- [x] Evidence poradců — jméno, příjmení, e-mail, telefon, rodné číslo, věk
- [x] Seznamy smluv, klientů a poradců s proklikem na detail
- [x] Detail s proklikem na svázané entity
- [x] CRUD operace pro Smlouvu, Klienta a Poradce
- [x] Responzivní design (Bootstrap 5)
- [x] Jednoduchá autentizace — úpravy a mazání jen po přihlášení
- [ ] Filtrování *(neimplementováno)*
- [x] Export do CSV *(neimplementováno)*

---

*Stáž 2026 