using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ZaposleniAPI;
using System;
using System.Threading.Tasks;

namespace ZaposleniAPI.Tests;

[TestFixture]
public class PozicijaTests
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

    // --- TEST 1: Uspešno dodavanje pozicije ---
    [Test]
    public async Task DodajPoziciju_ValidniPodaci_VracaSuccess()
    {
        // Arrange
        var p = new Pozicija { 
            ZaposleniJmbg = "123", 
            NazivPozicije = "Developer", 
            DatumOd = DateTime.Now 
        };

        // Act
        var (success, message, data) = await _service.DodajPoziciju(p);

        // Assert
        Assert.That(success, Is.True);
        Assert.That(_context.Pozicije.CountAsync().Result, Is.EqualTo(1));
    }

    // --- TEST 2: Zabrana duple aktivne pozicije (Glavno pravilo!) ---
    [Test]
    public async Task DodajPoziciju_VecImaAktivnu_VracaError()
    {
        // Arrange
        var prva = new Pozicija { ZaposleniJmbg = "123", NazivPozicije = "Junior", DatumOd = DateTime.Now, DatumDo = null };
        await _service.DodajPoziciju(prva);

        var druga = new Pozicija { ZaposleniJmbg = "123", NazivPozicije = "Senior", DatumOd = DateTime.Now.AddDays(1) };

        // Act
        var (success, message, data) = await _service.DodajPoziciju(druga);

        // Assert
        Assert.Multiple(() => {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo("Zaposleni već ima aktivnu poziciju."));
        });
    }

    // --- TEST 3: Validacija datuma (Kraj pre početka) ---
    [Test]
    public async Task DodajPoziciju_KrajPrePocetka_VracaError()
    {
        // Arrange
        var p = new Pozicija { 
            ZaposleniJmbg = "123", 
            NazivPozicije = "Tester", 
            DatumOd = DateTime.Now, 
            DatumDo = DateTime.Now.AddDays(-1) 
        };

        // Act
        var (success, message, data) = await _service.DodajPoziciju(p);

        // Assert
        Assert.That(success, Is.False);
        Assert.That(message, Is.EqualTo("Kraj ne može biti pre početka."));
    }
}
