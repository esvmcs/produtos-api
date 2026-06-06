using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using NHibernateSession = NHibernate.ISession;
using ISessionFactory = NHibernate.ISessionFactory;
using Pyferium.Produtos.Aplicacao.Categorias.Repositorios;
using Pyferium.Produtos.Aplicacao.Produtos.Repositorios;
using Pyferium.Produtos.Infraestrutura.Dados;
using Pyferium.Produtos.Infraestrutura.Repositorios;

namespace Pyferium.Produtos.Infraestrutura.Configuracoes;

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