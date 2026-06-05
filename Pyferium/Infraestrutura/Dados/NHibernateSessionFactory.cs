using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Connection;
using NHibernate.Dialect;
using NHibernate.Driver.MySqlConnector;
using NHibernate.Tool.hbm2ddl;
using Pyferium.Infraestrutura.Mapeamentos;

namespace Pyferium.Infraestrutura.Dados;

public static class NHibernateSessionFactory
{
    public static ISessionFactory Criar(IConfiguration configuration)
    {
        var cfg = new Configuration();

        cfg.DataBaseIntegration(db =>
        {
            db.ConnectionString = configuration.GetConnectionString("MySql");
            db.ConnectionProvider<DriverConnectionProvider>();
            db.Driver<MySqlConnectorDriver>();
            db.Dialect<MySQLDialect>();

            db.LogSqlInConsole = true;
            db.LogFormattedSql = true;
        });

        return Fluently.Configure(cfg)
            .Mappings(m =>
            {
                m.FluentMappings.AddFromAssemblyOf<ProdutoMap>();
            })
            .ExposeConfiguration(c =>
            {
                var atualizarSchema = configuration.GetValue<bool>("NHibernate:AtualizarSchema");

                if (atualizarSchema)
                {
                    new SchemaUpdate(c).Execute(useStdOut: true, doUpdate: true);
                }
            })
            .BuildSessionFactory();
    }
}