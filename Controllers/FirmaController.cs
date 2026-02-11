using Microsoft.AspNetCore.Mvc;

namespace ZaposleniAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FirmaController : ControllerBase
{
    private readonly IFirmaService _service;

    public FirmaController(IFirmaService service)
    {
        _service = service;
    }

    // --- ZAPOSLENI ---
    [HttpGet("zaposleni")]
    public async Task<IActionResult> GetZaposleni() => Ok(await _service.GetAllZaposleni());

    [HttpGet("zaposleni/{jmbg}")]
    public async Task<IActionResult> GetZaposleni(string jmbg)
    {
        var z = await _service.GetZaposleniByJmbg(jmbg);
        return z == null ? NotFound() : Ok(z);
    }

    [HttpPost("zaposleni")]
    public async Task<IActionResult> DodajZaposlenog(Zaposleni z)
    {
        var result = await _service.DodajZaposlenog(z);
        if (!result.Success) return BadRequest(new { error = result.Message });
        return Ok(result.Data);
    }

    [HttpPut("zaposleni/{jmbg}")]
    public async Task<IActionResult> UpdateZaposleni(string jmbg, Zaposleni z)
    {
        var result = await _service.UpdateZaposleni(jmbg, z);
        if (!result.Success) return BadRequest(new { error = result.Message });
        return Ok(result.Data);
    }

    [HttpDelete("zaposleni/{jmbg}")]
    public async Task<IActionResult> DeleteZaposleni(string jmbg)
    {
        var result = await _service.DeleteZaposleni(jmbg);
        if (!result.Success) return NotFound();
        return Ok(new { message = result.Message });
    }

    // --- POZICIJE ---
    [HttpGet("pozicije")]
    public async Task<IActionResult> GetPozicije() => Ok(await _service.GetAllPozicije());

    [HttpPost("pozicija")]
    public async Task<IActionResult> DodajPoziciju(Pozicija p)
    {
        var result = await _service.DodajPoziciju(p);
        if (!result.Success) return BadRequest(new { error = result.Message });
        return Ok(result.Data);
    }

    [HttpDelete("pozicija/{id}")]
    public async Task<IActionResult> DeletePozicija(int id)
    {
        var result = await _service.DeletePozicija(id);
        return result.Success ? Ok() : NotFound();
    }

    // --- PROJEKTI ---
    [HttpGet("projekti")]
    public async Task<IActionResult> GetProjekti() => Ok(await _service.GetAllProjekti());

    [HttpPost("projekat")]
    public async Task<IActionResult> DodajProjekat(Projekat p) => Ok(await _service.DodajProjekat(p));

    [HttpDelete("projekat/{id}")]
    public async Task<IActionResult> DeleteProjekat(int id)
    {
        var result = await _service.DeleteProjekat(id);
        return result.Success ? Ok() : NotFound();
    }

    // --- PLATE ---
    [HttpGet("plate")]
    public async Task<IActionResult> GetPlate() => Ok(await _service.GetAllPlate());

    [HttpPost("plata")]
    public async Task<IActionResult> DodajPlatu(Plata p)
    {
        var result = await _service.DodajPlatu(p);
        if (!result.Success) return BadRequest(new { error = result.Message });
        return Ok(result.Data);
    }

    // --- RADIO NA ---
    [HttpGet("radio-na")]
    public async Task<IActionResult> GetRadioNa() => Ok(await _service.GetAllRadioNa());

    [HttpPost("radio-na")]
    public async Task<IActionResult> DodajRadioNa(RadioNa r)
    {
        var result = await _service.DodajRadioNa(r);
        if (!result.Success) return BadRequest(new { error = result.Message });
        return Ok(result.Data);
    }

    [HttpDelete("radio-na/{id}")]
    public async Task<IActionResult> DeleteRadioNa(int id)
    {
        var result = await _service.DeleteRadioNa(id);
        return result.Success ? Ok() : NotFound();
    }

    // --- IZVEŠTAJ ---
    [HttpGet("izvestaj")]
    public async Task<IActionResult> GetIzvestaj() => Ok(await _service.GetIzvestaj());
}
