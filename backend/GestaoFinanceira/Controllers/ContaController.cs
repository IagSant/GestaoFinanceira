using GestaoFinanceira.Data;
using GestaoFinanceira.DTOs;
using GestaoFinanceira.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoFinanceira.Controllers;

[Authorize]
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
    public async Task<ActionResult<IEnumerable<ContaResponseDTO>>> GetContas()
    {
        var contas = await _context.Contas
            .Include(c => c.Usuario)
            .Select(c => new ContaResponseDTO
            {
                Id = c.Id,
                Nome = c.Nome,
                Saldo = c.Saldo,
                UsuarioId = c.UsuarioId,
                NomeUsuario = c.Usuario.Nome
            })
            .ToListAsync();

        return Ok(contas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ContaResponseDTO>> GetConta(int id)
    {
        var conta = await _context.Contas
            .Include(c => c.Usuario)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (conta == null)
        {
            return NotFound(new
            {
                mensagem = "Conta não encontrada"
            });
        }

        var response = new ContaResponseDTO
        {
            Id = conta.Id,
            Nome = conta.Nome,
            Saldo = conta.Saldo,
            UsuarioId = conta.UsuarioId,
            NomeUsuario = conta.Usuario.Nome
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult> PostConta(ContaCreateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);

        if (usuario == null)
        {
            return BadRequest(new
            {
                mensagem = "Usuário não encontrado"
            });
        }

        var conta = new Conta
        {
            Nome = dto.Nome,
            Saldo = dto.Saldo,
            UsuarioId = dto.UsuarioId
        };

        _context.Contas.Add(conta);

        await _context.SaveChangesAsync();

        var response = new ContaResponseDTO
        {
            Id = conta.Id,
            Nome = conta.Nome,
            Saldo = conta.Saldo,
            UsuarioId = conta.UsuarioId,
            NomeUsuario = usuario.Nome
        };

        return CreatedAtAction(nameof(GetConta), new { id = conta.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutConta(int id, ContaCreateDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var conta = await _context.Contas.FindAsync(id);

        if (conta == null)
        {
            return NotFound(new
            {
                mensagem = "Conta não encontrada"
            });
        }

        var usuario = await _context.Usuarios.FindAsync(dto.UsuarioId);

        if (usuario == null)
        {
            return BadRequest(new
            {
                mensagem = "Usuário não encontrado"
            });
        }

        conta.Nome = dto.Nome;
        conta.Saldo = dto.Saldo;
        conta.UsuarioId = dto.UsuarioId;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Conta atualizada com sucesso"
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteConta(int id)
    {
        var conta = await _context.Contas.FindAsync(id);

        if (conta == null)
        {
            return NotFound(new
            {
                mensagem = "Conta não encontrada"
            });
        }

        _context.Contas.Remove(conta);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Conta removida com sucesso"
        });
    }
}