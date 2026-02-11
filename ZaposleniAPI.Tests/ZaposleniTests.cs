using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ZaposleniAPI;
using System;
using System.Threading.Tasks;

namespace ZaposleniAPI.Tests;

[TestFixture]
public class ZaposleniTests
{
    private AppDbContext _context;
    private FirmaService _service;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _service = new FirmaService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Dispose();
    }

    // --- DODAVANJE (CREATE) ---

    [Test]
    public async Task DodajZaposlenog_ValidniPodaci_VracaSuccess()
    {
        var novi = new Zaposleni { Jmbg = "123", Ime = "Marko", Prezime = "Markovic", DatumRodjenja = new DateTime(1990, 1, 1) };
        var (success, _, data) = await _service.DodajZaposlenog(novi);

        Assert.Multiple(() =>
        {
            Assert.That(success, Is.True);
            Assert.That(data?.Jmbg, Is.EqualTo("123"));
            Assert.That(_context.Zaposleni.CountAsync().Result, Is.EqualTo(1));
        });
    }

    [Test]
    public async Task DodajZaposlenog_BuduciDatum_VracaError()
    {
        var novi = new Zaposleni { Jmbg = "456", Ime = "Test", Prezime = "User", DatumRodjenja = DateTime.Now.AddDays(1) };
        var (success, message, _) = await _service.DodajZaposlenog(novi);

        Assert.Multiple(() =>
        {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo("Datum rođenja ne može biti u budućnosti."));
        });
    }

    [Test]
    public async Task DodajZaposlenog_DupliJmbg_VracaError()
    {
        var prvi = new Zaposleni { Jmbg = "111", Ime = "Ime1", Prezime = "Prez1" };
        await _service.DodajZaposlenog(prvi);

        var dupli = new Zaposleni { Jmbg = "111", Ime = "Ime2", Prezime = "Prez2" };
        var (success, message, _) = await _service.DodajZaposlenog(dupli);

        Assert.That(success, Is.False);
        Assert.That(message, Is.EqualTo("Zaposleni sa tim JMBG već postoji."));
    }

    // --- AŽURIRANJE (UPDATE) ---

    [Test]
    public async Task UpdateZaposleni_Uspesno_VracaTrue() 
    {
        var z = new Zaposleni { Jmbg = "123", Ime = "Staro", Prezime = "Prezime" };
        _context.Zaposleni.Add(z);
        await _context.SaveChangesAsync();

        z.Ime = "NovoIme";
        var (success, _, data) = await _service.UpdateZaposleni("123", z);

        Assert.Multiple(() => {
            Assert.That(success, Is.True);
            Assert.That(data?.Ime, Is.EqualTo("NovoIme"));
        });
    }

    [Test]
    public async Task UpdateZaposleni_PogresanJmbg_VracaFalse() 
    {
        var z = new Zaposleni { Jmbg = "123", Ime = "Ime" };
        var (success, _, _) = await _service.UpdateZaposleni("POGRESAN_JMBG", z);
        Assert.That(success, Is.False);
    }

    [Test]
    public async Task UpdateZaposleni_NullObjekat_VracaFalse() 
    {
        var (success, _, _) = await _service.UpdateZaposleni("123", null!);
        Assert.That(success, Is.False);
    }

    // --- BRISANJE (DELETE) ---

    [Test]
    public async Task DeleteZaposleni_Postojeci_VracaTrue() 
    {
        _context.Zaposleni.Add(new Zaposleni { Jmbg = "123", Ime = "T", Prezime = "T" });
        await _context.SaveChangesAsync();

        var (success, _) = await _service.DeleteZaposleni("123");
        Assert.That(success, Is.True);
    }

    [Test]
    public async Task DeleteZaposleni_Nepostojeci_VracaFalse() 
    {
        var (success, message) = await _service.DeleteZaposleni("ne-postoji");
        Assert.Multiple(() => {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo("Nije pronađen."));
        });
    }

    [Test]
    public async Task DeleteZaposleni_ProveraBaze_SmanjujeBroj() 
    {
        _context.Zaposleni.Add(new Zaposleni { Jmbg = "123", Ime = "T", Prezime = "T" });
        await _context.SaveChangesAsync();

        await _service.DeleteZaposleni("123");
        var count = await _context.Zaposleni.CountAsync();
        Assert.That(count, Is.EqualTo(0));
    }
}
