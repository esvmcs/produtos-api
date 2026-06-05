using System.Data;
using Dapper;
using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Responses;
using Pyferium.Aplicacao.Produtos.Repositorios;

using NHibernateSession = NHibernate.ISession;

namespace Pyferium.Infraestrutura.Repositorios;

public class ProdutoRepositorio : IProdutoRepositorio
{
    private const string ProdutoAtivo = "S";
    private const string ProdutoInativo = "N";

    private readonly NHibernateSession _session;

    public ProdutoRepositorio(NHibernateSession session)
    {
        _session = session;
    }

    private IDbConnection Connection => _session.Connection;

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

        var codigoProduto = await Connection.QuerySingleAsync<int>(
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
        ProdutoRequest request)
    {
        const string sqlUpdate = @"
            UPDATE GEN_PRODUTO
            SET 
                NOMPRODUTO = COALESCE(@nomeProduto, NOMPRODUTO),
                CODCATEGORIA = COALESCE(@codigoCategoria, CODCATEGORIA),
                VLRPRODUTO = COALESCE(@valorProduto, VLRPRODUTO),
                IDTATIVO = COALESCE(@idtAtivo, IDTATIVO)
            WHERE CODPRODUTO = @codigoProduto;
        ";

        var linhasAfetadas = await Connection.ExecuteAsync(
            sqlUpdate,
            new
            {
                codigoProduto,
                nomeProduto = request.NomeProduto,
                codigoCategoria = request.CodigoCategoria,
                valorProduto = request.ValorProduto,
                idtAtivo = request.IdtAtivo
            });

        if (linhasAfetadas == 0)
            return null;

        return await ListarProdutoEditadoAsync(codigoProduto);
    }

    public async Task<IReadOnlyList<ProdutoListagemResponse>> ListarProdutosAsync()
    {
        const string sql = @"
        SELECT 
            P.CODPRODUTO AS CodigoProduto,
            P.NOMPRODUTO AS NomeProduto,
            P.VLRPRODUTO AS ValorProduto,
            C.CODCATEGORIA AS CodigoCategoria,
            C.DSCCATEGORIA AS DescricaoCategoria,
            C.CODNIVEL AS CodigoNivel,
            P.IDTATIVO AS IdtAtivo
        FROM GEN_PRODUTO P
        INNER JOIN GEN_CATEGORIA C 
            ON C.CODCATEGORIA = P.CODCATEGORIA
        WHERE P.IDTATIVO = @idtAtivo
          AND C.IDTATIVO = @idtAtivo
        ORDER BY P.CODPRODUTO;
    ";

        var produtos = await Connection.QueryAsync<ProdutoListagemResponse>(
            sql,
            new
            {
                idtAtivo = "S"
            });

        return produtos.ToList();
    }

    public async Task<ProdutoListagemResponse?> ListarPorCodigoAsync(int codigoProduto)
    {
        const string sql = @"
            SELECT 
                P.CODPRODUTO AS CodigoProduto,
                P.NOMPRODUTO AS NomeProduto,
                P.VLRPRODUTO AS ValorProduto,
                C.CODCATEGORIA AS CodigoCategoria,
                C.DSCCATEGORIA AS DescricaoCategoria,
                C.CODNIVEL AS CodigoNivel,
                P.IDTATIVO AS IdtAtivo
            FROM GEN_PRODUTO P
            INNER JOIN GEN_CATEGORIA C 
                ON C.CODCATEGORIA = P.CODCATEGORIA
            WHERE P.CODPRODUTO = @codigoProduto;
        ";

        return await Connection.QuerySingleOrDefaultAsync<ProdutoListagemResponse>(
            sql,
            new
            {
                codigoProduto
            });
    }

    public async Task<bool> ExisteProdutoAtivoComMesmoNomeAsync(
        string nomeProduto,
        int codigoCategoria,
        int? codigoProdutoIgnorar = null)
    {
        const string sql = @"
            SELECT COUNT(1)
            FROM GEN_PRODUTO
            WHERE UPPER(TRIM(NOMPRODUTO)) = UPPER(TRIM(@nomeProduto))
              AND CODCATEGORIA = @codigoCategoria
              AND IDTATIVO = @idtAtivo
              AND (@codigoProdutoIgnorar IS NULL OR CODPRODUTO <> @codigoProdutoIgnorar);
        ";

        var total = await Connection.QuerySingleAsync<int>(
            sql,
            new
            {
                nomeProduto,
                codigoCategoria,
                codigoProdutoIgnorar,
                idtAtivo = ProdutoAtivo
            });

        return total > 0;
    }

    public async Task<bool> DeletarProdutoAsync(int codigoProduto)
    {
        const string sql = @"
            UPDATE GEN_PRODUTO
            SET IDTATIVO = @idtAtivo
            WHERE CODPRODUTO = @codigoProduto
              AND IDTATIVO = @produtoAtivo;
        ";

        var linhasAfetadas = await Connection.ExecuteAsync(
            sql,
            new
            {
                codigoProduto,
                idtAtivo = ProdutoInativo,
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

        return await Connection.QuerySingleOrDefaultAsync<ProdutoEditadoResponse>(
            sql,
            new
            {
                codigoProduto
            });
    }
}