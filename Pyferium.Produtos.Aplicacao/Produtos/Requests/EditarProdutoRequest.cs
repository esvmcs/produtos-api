using System.ComponentModel.DataAnnotations;

namespace Pyferium.Produtos.Aplicacao.Produtos.Requests;

public class EditarProdutoRequest
{
    [StringLength(80, ErrorMessage = "O nome do produto deve conter no máximo 80 caracteres.")]
    public string? NomeProduto { get; set; }

    [Range(1, 9, ErrorMessage = "O código da categoria deve ser maior que zero.")]
    public int? CodigoCategoria { get; set; }

    [Range(typeof(decimal), "0,01", "999999999", ErrorMessage = "O valor do produto deve ser maior que zero.")]
    public decimal? ValorProduto { get; set; }

    [RegularExpression("^[SsNn]$", ErrorMessage = "O campo IdtAtivo deve ser 'S' ou 'N'.")]
    public string? IdtAtivo { get; set; }
}