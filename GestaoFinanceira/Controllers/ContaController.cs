using GestaoFinanceira.Data;
using GestaoFinanceira.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContaController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Conta>>> GetContas()
    {
        return await _context.Contas
            .Include(c => c.Usuario)
            .ToListAsync();
    }
    
    [HttpPost]
    public async Task<ActionResult<Conta>> PostConta(Conta conta)
    {
        var usuario = await _context.Usuarios.FindAsync(conta.UsuarioId);

        if (usuario == null)
        {
            return BadRequest("Usuário não encontrado.");
        }

        _context.Contas.Add(conta);

        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContas), new { id = conta.Id }, conta);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Conta>> GetConta(int id)
    {
        var conta = await _context.Contas
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (conta == null)
        {
            return NotFound();
        }

        return conta;
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutConta(int id, Conta conta)
    {
        if (id != conta.Id)
        {
            return BadRequest();
        }

        var usuario = await _context.Usuarios.FindAsync(conta.UsuarioId);

        if (usuario == null)
        {
            return BadRequest("Usuário não encontrado.");
        }

        _context.Entry(conta).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConta(int id)
    {
        var conta = await _context.Contas.FindAsync(id);

        if (conta == null)
        {
            return NotFound();
        }

        _context.Contas.Remove(conta);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}