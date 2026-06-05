using Pyferium.Aplicacao.Produtos.Servicos;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

namespace Pyferium.Aplicacao.Configuracoes;

public static class InjecaoDependenciaAplicacao
{
    public static IServiceCollection AddAplicacao(this IServiceCollection services)
    {
        services.AddScoped<IListarProdutoService, ListarProdutoService>();
        services.AddScoped<ICriarProdutoService, CriarProdutoService>();
        services.AddScoped<IEditarProdutoService, EditarProdutoService>();
        services.AddScoped<IDeletarProdutoService, DeletarProdutoService>();

        return services;
    }
}