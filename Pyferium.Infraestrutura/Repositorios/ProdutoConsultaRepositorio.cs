using Dapper;
using Pyferium.Aplicacao.Produtos.Repositorios;
using Pyferium.Aplicacao.Produtos.Responses;

using NHibernateSession = NHibernate.ISession;

namespace Pyferium.Infraestrutura.Repositorios;

public class ProdutoConsultaRepositorio : IProdutoConsultaRepositorio
{
    private const string ProdutoAtivo = "S";

    private readonly NHibernateSession _session;

    public ProdutoConsultaRepositorio(NHibernateSession session)
    {
        _session = session;
    }

    public async Task<IReadOnlyList<ProdutoListagemResponse>> ListarProdutosAsync()
    {
        const string sql = @"
            SELECT 
                P.CODPRODUTO AS CodigoProduto,
                P.NOMPRODUTO AS NomeProduto,
                P.VLRPRODUTO AS ValorProduto,
                P.IDTATIVO AS IdtAtivo,
                C.CODCATEGORIA AS CodigoCategoria,
                C.DSCCATEGORIA AS DescricaoCategoria,
                C.CODNIVEL AS CodigoNivel
            FROM GEN_PRODUTO P
            INNER JOIN GEN_CATEGORIA C 
                ON C.CODCATEGORIA = P.CODCATEGORIA
            WHERE P.IDTATIVO = @idtAtivo
              AND C.IDTATIVO = @idtAtivo
            ORDER BY P.CODPRODUTO;
        ";

        var produtos = await _session.Connection.QueryAsync<ProdutoListagemResponse>(
            sql,
            new
            {
                idtAtivo = ProdutoAtivo
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
                P.IDTATIVO AS IdtAtivo,
                C.CODCATEGORIA AS CodigoCategoria,
                C.DSCCATEGORIA AS DescricaoCategoria,
                C.CODNIVEL AS CodigoNivel
            FROM GEN_PRODUTO P
            INNER JOIN GEN_CATEGORIA C 
                ON C.CODCATEGORIA = P.CODCATEGORIA
            WHERE P.CODPRODUTO = @codigoProduto
              AND P.IDTATIVO = @idtAtivo
              AND C.IDTATIVO = @idtAtivo;
        ";

        return await _session.Connection.QuerySingleOrDefaultAsync<ProdutoListagemResponse>(
            sql,
            new
            {
                codigoProduto,
                idtAtivo = ProdutoAtivo
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
              AND (
                    @codigoProdutoIgnorar IS NULL 
                    OR CODPRODUTO <> @codigoProdutoIgnorar
                  );
        ";

        var total = await _session.Connection.QuerySingleAsync<int>(
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
}