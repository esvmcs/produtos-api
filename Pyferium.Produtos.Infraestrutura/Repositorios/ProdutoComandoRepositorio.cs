using Dapper;
using Pyferium.Produtos.Aplicacao.Produtos.Comandos;
using Pyferium.Produtos.Aplicacao.Produtos.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Responses;
using NHibernateSession = NHibernate.ISession;

namespace Pyferium.Produtos.Infraestrutura.Repositorios;

public class ProdutoComandoRepositorio : IProdutoComandoRepositorio
{
    private const string ProdutoAtivo = "S";
    private const string ProdutoInativo = "N";

    private readonly NHibernateSession _session;

    public ProdutoComandoRepositorio(NHibernateSession session)
    {
        _session = session;
    }

    public async Task<ProdutoCriadoResponse> CriarProdutoAsync(
        string nomeProduto,
        int codigoCategoria,
        decimal valorProduto)
    {
        const string sql = @"
            INSERT INTO GEN_PRODUTO 
            (
                NOMPRODUTO,
                CODCATEGORIA,
                VLRPRODUTO,
                IDTATIVO
            )
            VALUES 
            (
                @nomeProduto,
                @codigoCategoria,
                @valorProduto,
                @idtAtivo
            );

            SELECT LAST_INSERT_ID();
        ";

        var codigoProduto = await _session.Connection.QuerySingleAsync<int>(
            sql,
            new
            {
                nomeProduto,
                codigoCategoria,
                valorProduto,
                idtAtivo = ProdutoAtivo
            });

        return new ProdutoCriadoResponse
        {
            CodigoProduto = codigoProduto,
            NomeProduto = nomeProduto,
            CodigoCategoria = codigoCategoria,
            ValorProduto = valorProduto,
            IdtAtivo = ProdutoAtivo
        };
    }

    public async Task<ProdutoEditadoResponse?> AtualizarProdutoAsync(
    int codigoProduto,
    EditarProdutoComando comando)
    {
        const string sql = @"
        UPDATE GEN_PRODUTO
        SET 
            NOMPRODUTO = COALESCE(@nomeProduto, NOMPRODUTO),
            CODCATEGORIA = COALESCE(@codigoCategoria, CODCATEGORIA),
            VLRPRODUTO = COALESCE(@valorProduto, VLRPRODUTO),
            IDTATIVO = COALESCE(@idtAtivo, IDTATIVO)
        WHERE CODPRODUTO = @codigoProduto;
    ";

        var linhasAfetadas = await _session.Connection.ExecuteAsync(
            sql,
            new
            {
                codigoProduto,
                nomeProduto = comando.NomeProduto,
                codigoCategoria = comando.CodigoCategoria,
                valorProduto = comando.ValorProduto,
                idtAtivo = comando.IdtAtivo
            });

        if (linhasAfetadas == 0)
            return null;

        return await ListarProdutoEditadoAsync(codigoProduto);
    }

    public async Task<bool> DeletarProdutoAsync(int codigoProduto)
    {
        const string sql = @"
            UPDATE GEN_PRODUTO
            SET IDTATIVO = @produtoInativo
            WHERE CODPRODUTO = @codigoProduto
              AND IDTATIVO = @produtoAtivo;
        ";

        var linhasAfetadas = await _session.Connection.ExecuteAsync(
            sql,
            new
            {
                codigoProduto,
                produtoInativo = ProdutoInativo,
                produtoAtivo = ProdutoAtivo
            });

        return linhasAfetadas > 0;
    }

    private async Task<ProdutoEditadoResponse?> ListarProdutoEditadoAsync(int codigoProduto)
    {
        const string sql = @"
            SELECT 
                CODPRODUTO AS CodigoProduto,
                NOMPRODUTO AS NomeProduto,
                CODCATEGORIA AS CodigoCategoria,
                VLRPRODUTO AS ValorProduto,
                IDTATIVO AS IdtAtivo
            FROM GEN_PRODUTO
            WHERE CODPRODUTO = @codigoProduto;
        ";

        return await _session.Connection.QuerySingleOrDefaultAsync<ProdutoEditadoResponse>(
            sql,
            new
            {
                codigoProduto
            });
    }
}