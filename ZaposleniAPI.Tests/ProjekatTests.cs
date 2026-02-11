using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ZaposleniAPI;
using System;
using System.Threading.Tasks;

namespace ZaposleniAPI.Tests;

[TestFixture]
public class ProjekatTests
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

    // --- DODAJ PROJEKAT ---

    [Test]
    public async Task DodajProjekat_Uspesno_VracaObjekat()
    {
        var p = new Projekat { MenadzerJmbg = "123", DatumOd = DateTime.Now };
        var result = await _service.DodajProjekat(p);
        Assert.That(result.MenadzerJmbg, Is.EqualTo("123"));
    }

    [Test]
    public async Task DodajProjekat_CuvaUBazu()
    {
        var p = new Projekat { MenadzerJmbg = "999", DatumOd = DateTime.Now };
        await _service.DodajProjekat(p);
        Assert.That(await _context.Projekti.AnyAsync(x => x.MenadzerJmbg == "999"), Is.True);
    }

    [Test]
    public async Task DodajProjekat_IdSeAutomatskiGenerise()
    {
        var p1 = await _service.DodajProjekat(new Projekat { MenadzerJmbg = "1", DatumOd = DateTime.Now });
        var p2 = await _service.DodajProjekat(new Projekat { MenadzerJmbg = "2", DatumOd = DateTime.Now });
        Assert.That(p1.Id, Is.Not.EqualTo(p2.Id));
    }

    // --- DELETE PROJEKAT ---

    [Test]
    public async Task DeleteProjekat_Postojeci_VracaTrue()
    {
        var p = new Projekat { Id = 10, MenadzerJmbg = "123", DatumOd = DateTime.Now };
        _context.Projekti.Add(p);
        await _context.SaveChangesAsync();

        var (success, _) = await _service.DeleteProjekat(10);
        Assert.That(success, Is.True);
    }

    [Test]
    public async Task DeleteProjekat_Nepostojeci_VracaFalse()
    {
        var (success, _) = await _service.DeleteProjekat(999);
        Assert.That(success, Is.False);
    }

    [Test]
    public async Task DeleteProjekat_UklanjaIzBaze()
    {
        var p = new Projekat { Id = 5, MenadzerJmbg = "123", DatumOd = DateTime.Now };
        _context.Projekti.Add(p);
        await _context.SaveChangesAsync();

        await _service.DeleteProjekat(5);
        Assert.That(await _context.Projekti.CountAsync(), Is.EqualTo(0));
    }
}
