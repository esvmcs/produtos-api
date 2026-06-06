using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pyferium.Aplicacao.Categorias.Repositorios;
using Pyferium.Aplicacao.Produtos.Repositorios;
using Pyferium.Infraestrutura.Dados;
using Pyferium.Infraestrutura.Repositorios;

using NHibernateSession = NHibernate.ISession;
using ISessionFactory = NHibernate.ISessionFactory;

namespace Pyferium.Infraestrutura.Configuracoes;

public static class InjecaoDependenciaInfraestrutura
{
    public static IServiceCollection AddInfraestrutura(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<ISessionFactory>(_ =>
        {
            return NHibernateSessionFactory.Criar(configuration);
        });

        services.AddScoped<NHibernateSession>(serviceProvider =>
        {
            var sessionFactory = serviceProvider.GetRequiredService<ISessionFactory>();
            return sessionFactory.OpenSession();
        });

        services.AddScoped<IProdutoConsultaRepositorio, ProdutoConsultaRepositorio>();
        services.AddScoped<IProdutoComandoRepositorio, ProdutoComandoRepositorio>();
        services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();

        return services;
    }
}