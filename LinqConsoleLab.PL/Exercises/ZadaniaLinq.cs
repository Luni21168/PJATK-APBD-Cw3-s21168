using LinqConsoleLab.PL.Data;

namespace LinqConsoleLab.PL.Exercises;

public sealed class ZadaniaLinq
{

    public IEnumerable<string> Zadanie01_StudenciZWarszawy()
    {
        return DaneUczelni.Studenci
            .Where(s => s.Miasto == "Warsaw")
            .Select(s => $"{s.NumerIndeksu} | {s.Imie} {s.Nazwisko} | {s.Miasto}");
    }


    public IEnumerable<string> Zadanie02_AdresyEmailStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Email);
    }


    public IEnumerable<string> Zadanie03_StudenciPosortowani()
    {
        return DaneUczelni.Studenci
            .OrderBy(s => s.Nazwisko)
            .ThenBy(s => s.Imie)
            .Select(s => $"{s.NumerIndeksu} | {s.Imie} {s.Nazwisko}");
    }


    public IEnumerable<string> Zadanie04_PierwszyPrzedmiotAnalityczny()
    {
        var przedmiot = DaneUczelni.Przedmioty
            .FirstOrDefault(p => p.Kategoria == "Analytics");

        if (przedmiot == null)
        {
            return ["Brak przedmiotu z kategorii Analytics."];
        }

        return [$"{przedmiot.Nazwa} | start: {przedmiot.DataStartu:yyyy-MM-dd}"];
    }


    public IEnumerable<string> Zadanie05_CzyIstniejeNieaktywneZapisanie()
    {
        var istnieje = DaneUczelni.Zapisy
            .Any(z => !z.CzyAktywny);

        return [$"Czy istnieje nieaktywne zapisanie: {(istnieje ? "Tak" : "Nie")}"];
    }


    public IEnumerable<string> Zadanie06_CzyWszyscyProwadzacyMajaKatedre()
    {
        var wszyscyMajaKatedre = DaneUczelni.Prowadzacy
            .All(p => !string.IsNullOrWhiteSpace(p.Katedra));

        return [$"Czy wszyscy prowadzący mają katedrę: {(wszyscyMajaKatedre ? "Tak" : "Nie")}"];
    }


    public IEnumerable<string> Zadanie07_LiczbaAktywnychZapisow()
    {
        var liczba = DaneUczelni.Zapisy
            .Count(z => z.CzyAktywny);

        return [$"Liczba aktywnych zapisów: {liczba}"];
    }


    public IEnumerable<string> Zadanie08_UnikalneMiastaStudentow()
    {
        return DaneUczelni.Studenci
            .Select(s => s.Miasto)
            .Distinct()
            .OrderBy(m => m);
    }


    public IEnumerable<string> Zadanie09_TrzyNajnowszeZapisy()
    {
        return DaneUczelni.Zapisy
            .OrderByDescending(z => z.DataZapisu)
            .Take(3)
            .Select(z => $"{z.DataZapisu:yyyy-MM-dd} | StudentId: {z.StudentId} | PrzedmiotId: {z.PrzedmiotId}");
    }


    public IEnumerable<string> Zadanie10_DrugaStronaPrzedmiotow()
    {
        return DaneUczelni.Przedmioty
            .OrderBy(p => p.Nazwa)
            .Skip(2)
            .Take(2)
            .Select(p => $"{p.Nazwa} | {p.Kategoria}");
    }


    public IEnumerable<string> Zadanie11_PolaczStudentowIZapisy()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy,
                student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => $"{student.Imie} {student.Nazwisko} | data zapisu: {zapis.DataZapisu:yyyy-MM-dd}"
            );
    }


    public IEnumerable<string> Zadanie12_ParyStudentPrzedmiot()
    {
        return DaneUczelni.Studenci
            .SelectMany(student => DaneUczelni.Zapisy
                .Where(z => z.StudentId == student.Id)
                .Join(
                    DaneUczelni.Przedmioty,
                    zapis => zapis.PrzedmiotId,
                    przedmiot => przedmiot.Id,
                    (zapis, przedmiot) => $"{student.Imie} {student.Nazwisko} | {przedmiot.Nazwa}"
                )
            );
    }


    public IEnumerable<string> Zadanie13_GrupowanieZapisowWedlugPrzedmiotu()
    {
        return DaneUczelni.Zapisy
            .Join(
                DaneUczelni.Przedmioty,
                zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (zapis, przedmiot) => przedmiot.Nazwa
            )
            .GroupBy(nazwa => nazwa)
            .Select(g => $"{g.Key} | liczba zapisów: {g.Count()}");
    }


    public IEnumerable<string> Zadanie14_SredniaOcenaNaPrzedmiot()
    {
        return DaneUczelni.Zapisy
            .Where(z => z.OcenaKoncowa.HasValue)
            .Join(
                DaneUczelni.Przedmioty,
                zapis => zapis.PrzedmiotId,
                przedmiot => przedmiot.Id,
                (zapis, przedmiot) => new
                {
                    przedmiot.Nazwa,
                    Ocena = zapis.OcenaKoncowa!.Value
                }
            )
            .GroupBy(x => x.Nazwa)
            .Select(g => $"{g.Key} | średnia ocena: {g.Average(x => x.Ocena):0.00}");
    }


    public IEnumerable<string> Zadanie15_ProwadzacyILiczbaPrzedmiotow()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                prowadzacy => prowadzacy.Id,
                przedmiot => przedmiot.ProwadzacyId,
                (prowadzacy, przedmioty) =>
                    $"{prowadzacy.Imie} {prowadzacy.Nazwisko} | liczba przedmiotów: {przedmioty.Count()}"
            );
    }


    public IEnumerable<string> Zadanie16_NajwyzszaOcenaKazdegoStudenta()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa.HasValue),
                student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => new
                {
                    student.Imie,
                    student.Nazwisko,
                    Ocena = zapis.OcenaKoncowa!.Value
                }
            )
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko} | najwyższa ocena: {g.Max(x => x.Ocena):0.0}");
    }

 
    public IEnumerable<string> Wyzwanie01_StudenciZWiecejNizJednymAktywnymPrzedmiotem()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => new { student.Imie, student.Nazwisko }
            )
            .GroupBy(x => new { x.Imie, x.Nazwisko })
            .Where(g => g.Count() > 1)
            .Select(g => $"{g.Key.Imie} {g.Key.Nazwisko} | aktywne przedmioty: {g.Count()}");
    }


    public IEnumerable<string> Wyzwanie02_PrzedmiotyStartujaceWKwietniuBezOcenKoncowych()
    {
        return DaneUczelni.Przedmioty
            .Where(p => p.DataStartu.Year == 2026 && p.DataStartu.Month == 4)
            .GroupJoin(
                DaneUczelni.Zapisy,
                przedmiot => przedmiot.Id,
                zapis => zapis.PrzedmiotId,
                (przedmiot, zapisy) => new
                {
                    przedmiot.Nazwa,
                    MaOcenyKoncowe = zapisy.Any(z => z.OcenaKoncowa.HasValue)
                }
            )
            .Where(x => !x.MaOcenyKoncowe)
            .Select(x => x.Nazwa);
    }

    
    public IEnumerable<string> Wyzwanie03_ProwadzacyISredniaOcenNaIchPrzedmiotach()
    {
        return DaneUczelni.Prowadzacy
            .GroupJoin(
                DaneUczelni.Przedmioty,
                prowadzacy => prowadzacy.Id,
                przedmiot => przedmiot.ProwadzacyId,
                (prowadzacy, przedmioty) => new
                {
                    Prowadzacy = prowadzacy,
                    Przedmioty = przedmioty
                }
            )
            .Select(x => new
            {
                x.Prowadzacy.Imie,
                x.Prowadzacy.Nazwisko,
                Oceny = x.Przedmioty
                    .Join(
                        DaneUczelni.Zapisy.Where(z => z.OcenaKoncowa.HasValue),
                        przedmiot => przedmiot.Id,
                        zapis => zapis.PrzedmiotId,
                        (przedmiot, zapis) => zapis.OcenaKoncowa!.Value
                    )
            })
            .Select(x => x.Oceny.Any()
                ? $"{x.Imie} {x.Nazwisko} | średnia ocen: {x.Oceny.Average():0.00}"
                : $"{x.Imie} {x.Nazwisko} | średnia ocen: brak");
    }


    public IEnumerable<string> Wyzwanie04_MiastaILiczbaAktywnychZapisow()
    {
        return DaneUczelni.Studenci
            .Join(
                DaneUczelni.Zapisy.Where(z => z.CzyAktywny),
                student => student.Id,
                zapis => zapis.StudentId,
                (student, zapis) => student.Miasto
            )
            .GroupBy(miasto => miasto)
            .OrderByDescending(g => g.Count())
            .Select(g => $"{g.Key} | aktywne zapisy: {g.Count()}");
    }
}
