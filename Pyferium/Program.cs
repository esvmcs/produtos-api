using Pyferium.Infraestrutura.Dados;
using Pyferium.Infraestrutura.Repositorios;

using NHibernateSession = NHibernate.ISession;
using ISessionFactory = NHibernate.ISessionFactory;
using Pyferium.Aplicacao.Produtos.Servicos;
using Pyferium.Infraestrutura.Repositorios.Interfaces;
using Pyferium.Aplicacao.Produtos.Servicos.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// NHibernate - SessionFactory
builder.Services.AddSingleton<ISessionFactory>(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    return NHibernateSessionFactory.Criar(configuration);
});

// NHibernate - Session por requisição
builder.Services.AddScoped<NHibernateSession>(serviceProvider =>
{
    var sessionFactory = serviceProvider.GetRequiredService<ISessionFactory>();

    return sessionFactory.OpenSession();
});

// Repositórios
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();

// Serviços
builder.Services.AddScoped<IListarProdutoService, ListarProdutoService>();
builder.Services.AddScoped<ICriarProdutoService, CriarProdutoService>();
builder.Services.AddScoped<IEditarProdutoService, EditarProdutoService>();

var app = builder.Build();

// Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();