using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using ZaposleniAPI;
using System;
using System.Threading.Tasks;

namespace ZaposleniAPI.Tests;

[TestFixture]
public class PlataTests
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

    // --- DODAJ PLATU ---

    [Test]
    public async Task DodajPlatu_ValidniPodaci_VracaSuccess()
    {
        var p = new Plata { ZaposleniJmbg = "123", Iznos = 1000, DatumIsplate = DateTime.Now };
        var (success, _, data) = await _service.DodajPlatu(p);
        
        Assert.Multiple(() => {
            Assert.That(success, Is.True);
            Assert.That(data?.Iznos, Is.EqualTo(1000));
        });
    }

    [Test]
    public async Task DodajPlatu_NegativanIznos_VracaError()
    {
        var p = new Plata { ZaposleniJmbg = "123", Iznos = -50, DatumIsplate = DateTime.Now };
        var (success, message, _) = await _service.DodajPlatu(p);

        Assert.Multiple(() => {
            Assert.That(success, Is.False);
            Assert.That(message, Is.EqualTo("Plata ne može biti negativna."));
        });
    }

    [Test]
    public async Task DodajPlatu_ProveraUBazi_PovecaBrojZapisa()
    {
        var p = new Plata { ZaposleniJmbg = "123", Iznos = 500, DatumIsplate = DateTime.Now };
        await _service.DodajPlatu(p);
        
        var count = await _context.Plate.CountAsync();
        Assert.That(count, Is.EqualTo(1));
    }
}
