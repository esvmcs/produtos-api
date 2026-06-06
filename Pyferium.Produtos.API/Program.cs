using Microsoft.OpenApi;
using Pyferium.Produtos.API.Middlewares;
using Pyferium.Produtos.Aplicacao.Configuracoes;
using Pyferium.Produtos.Infraestrutura.Configuracoes;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Pyferium Produtos API",
        Version = "v1"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.Services.AddAplicacao();

builder.Services.AddInfraestrutura(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<TratamentoExcecaoMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();