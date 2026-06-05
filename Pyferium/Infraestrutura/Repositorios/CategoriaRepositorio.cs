using Dapper;
using Pyferium.Aplicacao.Categorias.Repositorios;
using NHibernateSession = NHibernate.ISession;

namespace Pyferium.Infraestrutura.Repositorios;

public class CategoriaRepositorio : ICategoriaRepositorio
{
    private readonly NHibernateSession _session;
    public CategoriaRepositorio(NHibernateSession session)
    {
        _session = session;
    }

    public async Task<bool> VerificarExistenciaCategoriaAsync(int codigoCategoria)
    {
        const string sql = @"
            SELECT COUNT(1)
            FROM GEN_CATEGORIA
            WHERE CODCATEGORIA = @codigoCategoria
              AND IDTATIVO = @idtAtivo;
        ";

        var total = await _session.Connection.QuerySingleAsync<int>(
            sql,
            new
            {
                codigoCategoria,
                idtAtivo = "S"
            });

        return total > 0;
    }
}
