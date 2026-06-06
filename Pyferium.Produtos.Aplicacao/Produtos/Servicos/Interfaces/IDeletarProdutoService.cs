namespace Pyferium.Produtos.Aplicacao.Produtos.Servicos.Interfaces;

public interface IDeletarProdutoService
{
    Task<bool> DeletarProdutoAsync(int codigoProduto);
}
