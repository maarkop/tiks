namespace ZaposleniAPI;

public interface IFirmaService
{
    // ZAPOSLENI
    Task<List<Zaposleni>> GetAllZaposleni();
    Task<Zaposleni?> GetZaposleniByJmbg(string jmbg);
    Task<(bool Success, string Message, Zaposleni? Data)> DodajZaposlenog(Zaposleni z);
    Task<(bool Success, string Message, Zaposleni? Data)> UpdateZaposleni(string jmbg, Zaposleni z);
    Task<(bool Success, string Message)> DeleteZaposleni(string jmbg);

    // POZICIJE
    Task<List<Pozicija>> GetAllPozicije();
    Task<(bool Success, string Message, Pozicija? Data)> DodajPoziciju(Pozicija p);
    Task<(bool Success, string Message)> DeletePozicija(int id);

    // PROJEKTI
    Task<List<Projekat>> GetAllProjekti();
    Task<Projekat> DodajProjekat(Projekat p);
    Task<(bool Success, string Message)> DeleteProjekat(int id);

    // PLATE
    Task<List<Plata>> GetAllPlate();
    Task<(bool Success, string Message, Plata? Data)> DodajPlatu(Plata p);

    // RADIO NA
    Task<List<RadioNa>> GetAllRadioNa();
    Task<(bool Success, string Message, RadioNa? Data)> DodajRadioNa(RadioNa r);
    Task<(bool Success, string Message)> DeleteRadioNa(int id);

    // IZVEŠTAJ
    Task<object> GetIzvestaj();
}
