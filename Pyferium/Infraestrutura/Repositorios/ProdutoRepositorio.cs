using Dapper;
using NHibernate;
using NHibernate.Transform;
using Pyferium.Aplicacao.Produtos.Requests;
using Pyferium.Aplicacao.Produtos.Responses;
using Pyferium.Dominio.Entidades;
using Pyferium.Infraestrutura.Repositorios.Interfaces;
using System.Data;
using NHibernateSession = NHibernate.ISession;

namespace Pyferium.Infraestrutura.Repositorios;

public class ProdutoRepositorio : IProdutoRepositorio
{
    private readonly NHibernateSession _session;

    public ProdutoRepositorio(NHibernateSession session)
    {
        _session = session;
    }

    public async Task<ProdutoCriadoResponse> CriarProdutoAsync(string nomeProduto, int codigoCategoria, decimal valorProduto)
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
                idtAtivo = "S"
            });

        return new ProdutoCriadoResponse
        {
            CodigoProduto = codigoProduto,
            NomeProduto = nomeProduto,
            CodigoCategoria = codigoCategoria,
            ValorProduto = valorProduto,
            IdtAtivo = "S"
        };
    }
    public async Task<ProdutoEditadoResponse?> AtualizarProdutoAsync(int codigoProduto, EditarProdutoRequest request)
    {
        var parameters = new DynamicParameters();

        const string sqlUpdate = @"
        UPDATE GEN_PRODUTO
        SET 
            NOMPRODUTO = COALESCE(@nomeProduto, NOMPRODUTO),
            CODCATEGORIA = COALESCE(@codigoCategoria, CODCATEGORIA),
            VLRPRODUTO = COALESCE(@valorProduto, VLRPRODUTO),
            IDTATIVO = COALESCE(@idtAtivo, IDTATIVO)
        WHERE CODPRODUTO = @codigoProduto;
    ";

        parameters.Add("codigoProduto", codigoProduto, DbType.Int32);
        parameters.Add("nomeProduto", request.NomeProduto, DbType.String);
        parameters.Add("codigoCategoria", request.CodigoCategoria, DbType.Int32);
        parameters.Add("valorProduto", request.ValorProduto, DbType.Decimal);
        parameters.Add("idtAtivo", request.IdtAtivo, DbType.String);

        var linhasAfetadas = await _session.Connection.ExecuteAsync(sqlUpdate, parameters);

        if (linhasAfetadas == 0)
            return null;

        const string sqlSelect = @"
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
            sqlSelect,
            new { codigoProduto });
    }
    public async Task<IEnumerable<ProdutoListagemResponse>> ListarProdutosAsync()
    {
        const string sql = @"
            SELECT 
                P.CODPRODUTO AS CodigoProduto,
                P.NOMPRODUTO AS NomeProduto,
                P.VLRPRODUTO AS ValorProduto,
                C.CODCATEGORIA AS CodigoCategoria,
                C.DSCCATEGORIA AS DescricaoCategoria,
                C.CODNIVEL AS CodigoNivel
            FROM GEN_PRODUTO P
            INNER JOIN GEN_CATEGORIA C 
                ON C.CODCATEGORIA = P.CODCATEGORIA
            WHERE P.IDTATIVO = :idtAtivo
              AND C.IDTATIVO = :idtAtivo
            ORDER BY P.CODPRODUTO;
        ";

        var produtos = await _session
            .CreateSQLQuery(sql)
            .AddScalar("CodigoProduto", NHibernateUtil.Int32)
            .AddScalar("NomeProduto", NHibernateUtil.String)
            .AddScalar("ValorProduto", NHibernateUtil.Decimal)
            .AddScalar("CodigoCategoria", NHibernateUtil.Int32)
            .AddScalar("DescricaoCategoria", NHibernateUtil.String)
            .AddScalar("CodigoNivel", NHibernateUtil.String)
            .SetParameter("idtAtivo", "S")
            .SetResultTransformer(Transformers.AliasToBean<ProdutoListagemResponse>())
            .ListAsync<ProdutoListagemResponse>();

        return produtos;
    }
    public async Task<IEnumerable<ProdutoListagemResponse>> ListarPorCodigoAsync(int codigoProduto)
    {
        const string sql = @"
            SELECT 
                P.CODPRODUTO AS CodigoProduto,
                P.NOMPRODUTO AS NomeProduto,
                P.VLRPRODUTO AS ValorProduto,
                C.CODCATEGORIA AS CodigoCategoria,
                C.DSCCATEGORIA AS DescricaoCategoria,
                C.CODNIVEL AS CodigoNivel
            FROM GEN_PRODUTO P
            INNER JOIN GEN_CATEGORIA C 
                ON C.CODCATEGORIA = P.CODCATEGORIA
            WHERE P.IDTATIVO = :idtAtivo
              AND C.IDTATIVO = :idtAtivo
              AND P.CODPRODUTO = :codigoProduto
            ORDER BY P.CODPRODUTO;
        ";
        var produtos = await _session
            .CreateSQLQuery(sql)
            .AddScalar("CodigoProduto", NHibernateUtil.Int32)
            .AddScalar("NomeProduto", NHibernateUtil.String)
            .AddScalar("ValorProduto", NHibernateUtil.Decimal)
            .AddScalar("CodigoCategoria", NHibernateUtil.Int32)
            .AddScalar("DescricaoCategoria", NHibernateUtil.String)
            .AddScalar("CodigoNivel", NHibernateUtil.String)
            .SetParameter("idtAtivo", "S")
            .SetParameter("codigoProduto", codigoProduto)
            .SetResultTransformer(Transformers.AliasToBean<ProdutoListagemResponse>())
            .ListAsync<ProdutoListagemResponse>();
        return produtos;
    }
    public async Task<bool> ExisteProdutoAtivoComMesmoNomeAsync(string nomeProduto, int codigoCategoria, int? codigoProdutoIgnorar = null)
    {
        const string sql = @"
        SELECT COUNT(1)
        FROM GEN_PRODUTO
        WHERE UPPER(TRIM(NOMPRODUTO)) = UPPER(TRIM(@nomeProduto))
          AND CODCATEGORIA = @codigoCategoria
          AND IDTATIVO = 'S'
          AND (@codigoProdutoIgnorar IS NULL OR CODPRODUTO <> @codigoProdutoIgnorar);
    ";

        var total = await _session.Connection.QuerySingleAsync<int>(
            sql,
            new
            {
                nomeProduto,
                codigoCategoria,
                codigoProdutoIgnorar
            });

        return total > 0;
    }
}