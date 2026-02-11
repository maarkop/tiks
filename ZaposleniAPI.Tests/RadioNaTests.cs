using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ZaposleniAPI;
using System;
using System.Threading.Tasks;

namespace ZaposleniAPI.Tests;

[TestFixture]
public class RadioNaTests
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
    public void TearDown() => _context.Dispose();

    // --- DODAJ RADIO NA ---

    [Test]
    public async Task DodajRadioNa_CetvrtiProjekat_VracaError()
    {
        // Dodajemo 3 angažovanja za poziciju 1
        _context.RadioNa.Add(new RadioNa { PozicijaId = 1, ProjekatId = 101 });
        _context.RadioNa.Add(new RadioNa { PozicijaId = 1, ProjekatId = 102 });
        _context.RadioNa.Add(new RadioNa { PozicijaId = 1, ProjekatId = 103 });
        await _context.SaveChangesAsync();

        var (success, message, _) = await _service.DodajRadioNa(new RadioNa { PozicijaId = 1, ProjekatId = 104 });

        Assert.Multiple(() => {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo("Maksimalno 3 projekta po poziciji."));
        });
    }

    [Test]
    public async Task DodajRadioNa_Uspesno_VracaSuccess()
    {
        var r = new RadioNa { PozicijaId = 1, ProjekatId = 101, Opis = "Programiranje" };
        var (success, _, _) = await _service.DodajRadioNa(r);
        Assert.That(success, Is.True);
    }

    [Test]
    public async Task DodajRadioNa_PovecavaBrojUBazi()
    {
        await _service.DodajRadioNa(new RadioNa { PozicijaId = 1, ProjekatId = 101 });
        Assert.That(await _context.RadioNa.CountAsync(), Is.EqualTo(1));
    }

    // --- DELETE RADIO NA ---

    [Test]
    public async Task DeleteRadioNa_Postojeci_VracaTrue()
    {
        var r = new RadioNa { Id = 1, PozicijaId = 1, ProjekatId = 101 };
        _context.RadioNa.Add(r);
        await _context.SaveChangesAsync();

        var (success, _) = await _service.DeleteRadioNa(1);
        Assert.That(success, Is.True);
    }

    [Test]
    public async Task DeleteRadioNa_Nepostojeci_VracaFalse()
    {
        var (success, _) = await _service.DeleteRadioNa(99);
        Assert.That(success, Is.False);
    }

    [Test]
    public async Task DeleteRadioNa_UklanjaZapis()
    {
        var r = new RadioNa { Id = 1, PozicijaId = 1, ProjekatId = 101 };
        _context.RadioNa.Add(r);
        await _context.SaveChangesAsync();

        await _service.DeleteRadioNa(1);
        Assert.That(await _context.RadioNa.AnyAsync(), Is.False);
    }
}
