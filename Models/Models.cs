using System.ComponentModel.DataAnnotations;

namespace ZaposleniAPI;

public class Zaposleni {
    [Key]
    public string Jmbg { get; set; } = null!;
    public string Ime { get; set; } = null!;
    public string Prezime { get; set; } = null!;
    public DateTime DatumRodjenja { get; set; }
    public string? BrojTelefona { get; set; } // NOVO
    public string? Email { get; set; }        // NOVO
}

public class Pozicija {
    public int Id { get; set; }
    public string ZaposleniJmbg { get; set; } = null!;
    public string NazivPozicije { get; set; } = null!;
    public DateTime DatumOd { get; set; }
    public DateTime? DatumDo { get; set; }
}

public class Projekat {
    public int Id { get; set; }
    public string MenadzerJmbg { get; set; } = null!;
    public DateTime DatumOd { get; set; }
    public DateTime? DatumDo { get; set; }
    // Naziv uklonjen po zahtevu
}

public class RadioNa {
    public int Id { get; set; }
    public int ProjekatId { get; set; }
    public int PozicijaId { get; set; }
    public string? Opis { get; set; }
}

public class Plata {
    public int Id { get; set; }
    public string ZaposleniJmbg { get; set; } = null!;
    public decimal Iznos { get; set; }
    public DateTime DatumIsplate { get; set; }
}
