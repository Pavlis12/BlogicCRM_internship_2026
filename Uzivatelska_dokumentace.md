# Uživatelská dokumentace — BlogicCRM

> Stručná dokumentace popisující chování aplikace z pohledu uživatele.


---

## Obsah

1. [Co aplikace dělá](#1-co-aplikace-dělá)
2. [Navigace v aplikaci](#2-navigace-v-aplikaci)
3. [Přihlášení a odhlášení](#3-přihlášení-a-odhlášení)
4. [Práce se smlouvami](#4-práce-se-smlouvami)
5. [Práce s klienty](#5-práce-s-klienty)
6. [Práce s poradci](#6-práce-s-poradci)
7. [Přehled přístupových práv](#7-přehled-přístupových-práv)

---

## 1. Co aplikace dělá

BlogicCRM slouží ke správě pojišťovacích smluv a osob s nimi spojených. Aplikace pracuje se třemi typy záznamů:

- **Smlouvy** — centrální entita; každá smlouva váže klienta a jednoho nebo více poradců
- **Klienti** — fyzické osoby, pro které jsou smlouvy uzavírány
- **Poradci** — zaměstnanci nebo spolupracovníci, kteří smlouvy spravují

Každý záznam lze **prohlížet** bez přihlášení. **Vytváření, úpravy a mazání** jsou dostupné pouze po přihlášení.

---

## 2. Navigace v aplikaci

Horní lišta (navbar) je viditelná na každé stránce:

```
[ BlogicCRM ]  |  [ Smlouvy ]  [ Klienti ]  [ Poradci ]  [ 🤖 AI ]    [ 👤 Přihlásit se ]
```

| Odkaz | Kam vede |
|-------|---------|
| **BlogicCRM** | Úvodní stránka |
| **Smlouvy** | Seznam všech smluv |
| **Klienti** | Seznam všech klientů |
| **Poradci** | Seznam všech poradců |
| **🤖 AI** | AI asistent aplikace |
| **👤 Přihlásit se** | Přihlašovací formulář (zobrazí se jen nepřihlášeným) |
| **admin ▾** | Rozbalovací menu s odhlášením (zobrazí se přihlášeným) |

---

## 3. Přihlášení a odhlášení

### Přihlášení

1. Klikněte na ikonu **👤 Přihlásit se** vpravo nahoře
2. Vyplňte přihlašovací údaje:

   | Pole | Hodnota |
   |------|---------|
   | Uživatelské jméno | `admin` |
   | Heslo | `heslo` |

3. Stiskněte **Přihlásit se**
4. Po úspěšném přihlášení se v navbaru zobrazí jméno **admin**

Pokud zadáte nesprávné údaje, formulář zůstane otevřený a zobrazí se chybová hláška.

### Odhlášení

1. Klikněte na jméno **admin** v pravém horním rohu
2. Z rozbalovacího menu vyberte **Odhlásit se**
3. Budete přesměrováni na hlavní stránku

### Automatické přesměrování

Pokud se nepřihlášený uživatel pokusí otevřít stránku pro vytvoření, úpravu nebo smazání záznamu (i přímou URL), aplikace ho automaticky přesměruje na přihlašovací formulář.

---

## 4. Práce se smlouvami

### Seznam smluv

Otevřete přes **Smlouvy** v horní liště. Tabulka zobrazuje:

- Evidenční číslo
- Instituce (pojišťovna / banka)
- Klient *(odkaz na detail klienta)*
- Správce smlouvy *(odkaz na detail poradce)*
- Datum uzavření, datum platnosti, datum ukončení

Na konci každého řádku jsou tlačítka **Detail** (vždy) a — pro přihlášené — **Upravit** a **Smazat**.

### Detail smlouvy

Zobrazuje všechna data smlouvy a navíc:

- Proklik na **klienta** smlouvy
- Proklik na **správce** (hlavního poradce)
- Seznam všech **účastníků (poradců)** s proklikem na každého z nich

### Vytvoření smlouvy *(vyžaduje přihlášení)*

Klikněte na **Nová smlouva** na stránce se seznamem. Vyplňte formulář:

| Pole | Popis | Povinné |
|------|-------|:-------:|
| Evidenční číslo | Unikátní identifikátor smlouvy | ✓ |
| Instituce | Název pojišťovny nebo banky (ČSOB, AEGON, Axa…) | ✓ |
| Klient | Výběr ze seznamu klientů | ✓ |
| Správce smlouvy | Výběr poradce — automaticky se stane účastníkem | ✓ |
| Datum uzavření | Datum podpisu | ✓ |
| Datum platnosti | Může být pozdější než datum uzavření | ✓ |
| Datum ukončení | Konec platnosti | – |
| Další účastníci | Checkboxy — přidání dalších poradců ke smlouvě | – |

Klikněte na **Uložit** pro vytvoření záznamu.

### Úprava smlouvy *(vyžaduje přihlášení)*

1. Na seznamu nebo v detailu klikněte na **Upravit**
2. Formulář je předvyplněn stávajícími hodnotami
3. Změňte požadovaná pole a klikněte na **Uložit**

### Smazání smlouvy *(vyžaduje přihlášení)*

1. Na seznamu nebo v detailu klikněte na **Smazat**
2. Zobrazí se potvrzovací stránka se souhrnem smlouvy
3. **Potvrdit smazání** — trvale odstraní záznam
4. **Zpět** — vrátí se bez smazání

---

## 5. Práce s klienty

### Seznam a detail klientů

Otevřete přes **Klienti** v horní liště. Detail klienta zobrazuje kontaktní údaje a seznam smluv, které jsou na tohoto klienta vedeny — s proklikem na každou smlouvu.

### Vytvoření klienta *(vyžaduje přihlášení)*

Klikněte na **Nový klient**. Formulář obsahuje:

| Pole | Validace |
|------|---------|
| Jméno | Povinné |
| Příjmení | Povinné |
| E-mail | Povinné, správný formát (např. jan@email.cz) |
| Telefon | Povinné |
| Rodné číslo | Povinné |
| Věk | Povinné, minimálně 18 let |

Formulář při odeslání zkontroluje všechna pole. Pokud něco chybí nebo je zadáno chybně, zobrazí se upozornění přímo u daného pole.

### Úprava a smazání klienta *(vyžaduje přihlášení)*

Postup je totožný jako u smluv — tlačítka jsou dostupná na seznamu i v detailu.

> **Upozornění:** Klienta není možné smazat, pokud na něj existují aktivní smlouvy.

---

## 6. Práce s poradci

### Seznam a detail poradců

Otevřete přes **Poradci** v horní liště. Detail poradce zobrazuje kontaktní údaje a seznam smluv, na kterých poradce figuruje jako správce nebo účastník — s proklikem na každou smlouvu.

### Vytvoření poradce *(vyžaduje přihlášení)*

Klikněte na **Nový poradce**. Formulář je totožný s formulářem pro klienta:

| Pole | Validace |
|------|---------|
| Jméno | Povinné |
| Příjmení | Povinné |
| E-mail | Povinné, správný formát |
| Telefon | Povinné |
| Rodné číslo | Povinné |
| Věk | Povinné, minimálně 18 let |

### Úprava a smazání poradce *(vyžaduje přihlášení)*

Postup je totožný jako u smluv a klientů.

> **Upozornění:** Poradce není možné smazat, pokud je správcem nebo účastníkem aktivní smlouvy.

---

## 7. Přehled přístupových práv

| Akce | Bez přihlášení | Po přihlášení |
|------|:--------------:|:-------------:|
| Zobrazit seznam smluv | ✅ | ✅ |
| Zobrazit detail smlouvy | ✅ | ✅ |
| Zobrazit seznam klientů | ✅ | ✅ |
| Zobrazit detail klienta | ✅ | ✅ |
| Zobrazit seznam poradců | ✅ | ✅ |
| Zobrazit detail poradce | ✅ | ✅ |
| Vytvořit nový záznam | ❌ | ✅ |
| Upravit existující záznam | ❌ | ✅ |
| Smazat záznam | ❌ | ✅ |