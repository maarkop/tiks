using Microsoft.EntityFrameworkCore;

namespace ZaposleniAPI;

public class FirmaService : IFirmaService
{
    private readonly AppDbContext _context;

    public FirmaService(AppDbContext context)
    {
        _context = context;
    }

    // --- ZAPOSLENI ---
    public async Task<List<Zaposleni>> GetAllZaposleni() => await _context.Zaposleni.ToListAsync();

    public async Task<Zaposleni?> GetZaposleniByJmbg(string jmbg) => await _context.Zaposleni.FindAsync(jmbg);

    public async Task<(bool Success, string Message, Zaposleni? Data)> DodajZaposlenog(Zaposleni z)
    {
        if (z == null) return (false, "Objekat je null.", null);
        if (z.DatumRodjenja > DateTime.Now)
            return (false, "Datum rođenja ne može biti u budućnosti.", null);

        if (await _context.Zaposleni.AnyAsync(x => x.Jmbg == z.Jmbg))
            return (false, "Zaposleni sa tim JMBG već postoji.", null);

        _context.Zaposleni.Add(z);
        await _context.SaveChangesAsync();
        return (true, "Uspešno dodat.", z);
    }

    public async Task<(bool Success, string Message, Zaposleni? Data)> UpdateZaposleni(string jmbg, Zaposleni z)
    {
        // FIX: Provera na null sprečava NullReferenceException u testovima
        if (z == null) return (false, "Podaci su prazni.", null);
        if (jmbg != z.Jmbg) return (false, "JMBG se ne poklapaju.", null);
        
        _context.Entry(z).State = EntityState.Modified;
        try {
            await _context.SaveChangesAsync();
            return (true, "Ažurirano.", z);
        } catch {
            return (false, "Greška pri ažuriranju.", null);
        }
    }

    public async Task<(bool Success, string Message)> DeleteZaposleni(string jmbg)
    {
        var z = await _context.Zaposleni.FindAsync(jmbg);
        if (z == null) return (false, "Nije pronađen.");
        
        _context.Zaposleni.Remove(z);
        await _context.SaveChangesAsync();
        return (true, "Obrisan.");
    }

    // --- POZICIJE ---
    public async Task<List<Pozicija>> GetAllPozicije() => await _context.Pozicije.ToListAsync();

    public async Task<(bool Success, string Message, Pozicija? Data)> DodajPoziciju(Pozicija p)
    {
        if (p.DatumDo.HasValue && p.DatumDo < p.DatumOd)
            return (false, "Kraj ne može biti pre početka.", null);

        var imaAktivnu = await _context.Pozicije.AnyAsync(x => x.ZaposleniJmbg == p.ZaposleniJmbg && x.DatumDo == null);
        if (imaAktivnu)
            return (false, "Zaposleni već ima aktivnu poziciju.", null);

        _context.Pozicije.Add(p);
        await _context.SaveChangesAsync();
        return (true, "Pozicija dodata.", p);
    }

    public async Task<(bool Success, string Message)> DeletePozicija(int id)
    {
        var p = await _context.Pozicije.FindAsync(id);
        if (p == null) return (false, "Nije pronađena.");
        _context.Pozicije.Remove(p);
        await _context.SaveChangesAsync();
        return (true, "Obrisana.");
    }

    // --- PROJEKTI ---
    public async Task<List<Projekat>> GetAllProjekti() => await _context.Projekti.ToListAsync();

    public async Task<Projekat> DodajProjekat(Projekat p)
    {
        _context.Projekti.Add(p);
        await _context.SaveChangesAsync();
        return p;
    }

    public async Task<(bool Success, string Message)> DeleteProjekat(int id)
    {
        var p = await _context.Projekti.FindAsync(id);
        if (p == null) return (false, "Nije pronađen.");
        _context.Projekti.Remove(p);
        await _context.SaveChangesAsync();
        return (true, "Obrisan.");
    }

    // --- PLATE ---
    public async Task<List<Plata>> GetAllPlate() => await _context.Plate.ToListAsync();

    public async Task<(bool Success, string Message, Plata? Data)> DodajPlatu(Plata p)
    {
        if (p.Iznos < 0) return (false, "Plata ne može biti negativna.", null);
        _context.Plate.Add(p);
        await _context.SaveChangesAsync();
        return (true, "Plata dodata.", p);
    }

    // --- RADIO NA ---
    public async Task<List<RadioNa>> GetAllRadioNa() => await _context.RadioNa.ToListAsync();

    public async Task<(bool Success, string Message, RadioNa? Data)> DodajRadioNa(RadioNa r)
    {
        var brojAktivnih = await _context.RadioNa.CountAsync(x => x.PozicijaId == r.PozicijaId);
        if (brojAktivnih >= 3)
            return (false, "Maksimalno 3 projekta po poziciji.", null);

        _context.RadioNa.Add(r);
        await _context.SaveChangesAsync();
        return (true, "Angažovanje dodato.", r);
    }

    public async Task<(bool Success, string Message)> DeleteRadioNa(int id)
    {
        var r = await _context.RadioNa.FindAsync(id);
        if (r == null) return (false, "Nije pronađeno.");
        _context.RadioNa.Remove(r);
        await _context.SaveChangesAsync();
        return (true, "Obrisano.");
    }

    // --- IZVEŠTAJ ---
    public async Task<object> GetIzvestaj()
    {
        return await (from z in _context.Zaposleni
                      join p in _context.Pozicije on z.Jmbg equals p.ZaposleniJmbg
                      join r in _context.RadioNa on p.Id equals r.PozicijaId
                      where p.DatumDo == null
                      select new {
                          Radnik = $"{z.Ime} {z.Prezime}",
                          Kontakt = $"{z.Email} | {z.BrojTelefona}",
                          Pozicija = p.NazivPozicije,
                          ProjekatId = r.ProjekatId
                      }).ToListAsync();
    }

    // --- SEED METODA (Novo!) ---
    public async Task SeedData()
    {
        if (await _context.Zaposleni.AnyAsync()) return; // Ne puni ako već ima podataka

        var z1 = new Zaposleni { Jmbg = "0101990123456", Ime = "Marko", Prezime = "Marković", DatumRodjenja = new DateTime(1990, 1, 1), Email = "marko@firma.com", BrojTelefona = "064123456" };
        var z2 = new Zaposleni { Jmbg = "0202995123456", Ime = "Ana", Prezime = "Anić", DatumRodjenja = new DateTime(1995, 2, 2), Email = "ana@firma.com", BrojTelefona = "065987654" };

        _context.Zaposleni.AddRange(z1, z2);
        
        var proj1 = new Projekat { MenadzerJmbg = z1.Jmbg, DatumOd = DateTime.Now };
        _context.Projekti.Add(proj1);

        await _context.SaveChangesAsync();
    }
}
