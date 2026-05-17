using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoFinanceira.Models;

public class Transacao
{
    public int Id { get; set; }
    [Required]
    public string Descricao { get; set; }
    
    public decimal Valor { get; set; }
    [Required]
    public string Tipo { get; set; }
    
    public DateTime Data { get; set; }
    [ForeignKey("Conta")]
    public int ContaId { get; set; }
    
    public Conta? Conta { get; set; }
    
    
}