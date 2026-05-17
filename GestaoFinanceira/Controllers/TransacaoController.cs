using GestaoFinanceira.Data;
using GestaoFinanceira.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace GestaoFinanceira.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransacaoController : ControllerBase
{
    private readonly AppDbContext _context;

    public TransacaoController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transacao>>> GetTransacoes()
    {
        return await _context.Transacoes
            .Include(t => t.Conta)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transacao>> GetTransacao(int id)
    {
        var transacao = await _context.Transacoes
            .Include(t => t.Conta)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (transacao == null)
        {
            return NotFound();
        }

        return transacao;
    }

    [HttpPost]
    public async Task<ActionResult<Transacao>> PostTransacao(Transacao transacao)
    {
        var conta = await _context.Contas.FindAsync(transacao.ContaId);

        if (conta == null)
            return BadRequest("Conta não encontrada.");

        if (transacao.Tipo == "credito")
            conta.Saldo += transacao.Valor;
        else if (transacao.Tipo == "debito")
            conta.Saldo -= transacao.Valor;

        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTransacao), new { id = transacao.Id }, transacao);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutTransacao(int id, Transacao transacao)
    {
        if (id != transacao.Id)
            return BadRequest();

        var transacaoAntiga = await _context.Transacoes.FindAsync(id);
        if (transacaoAntiga == null)
            return NotFound();

        var conta = await _context.Contas.FindAsync(transacao.ContaId);
        if (conta == null)
            return BadRequest("Conta não encontrada.");
        
        if (transacaoAntiga.Tipo == "credito")
            conta.Saldo -= transacaoAntiga.Valor;
        else if (transacaoAntiga.Tipo == "debito")
            conta.Saldo += transacaoAntiga.Valor;
        
        if (transacao.Tipo == "credito")
            conta.Saldo += transacao.Valor;
        else if (transacao.Tipo == "debito")
            conta.Saldo -= transacao.Valor;

        _context.Entry(transacaoAntiga).CurrentValues.SetValues(transacao);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransacao(int id)
    {
        var transacao = await _context.Transacoes.FindAsync(id);
        if (transacao == null)
            return NotFound();

        var conta = await _context.Contas.FindAsync(transacao.ContaId);

        if (conta != null)
        {
            if (transacao.Tipo == "credito")
                conta.Saldo -= transacao.Valor;
            else if (transacao.Tipo == "debito")
                conta.Saldo += transacao.Valor;
        }

        _context.Transacoes.Remove(transacao);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}