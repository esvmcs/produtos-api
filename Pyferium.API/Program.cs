using Pyferium.Aplicacao.Configuracoes;
using Pyferium.Infraestrutura.Configuracoes;
using Pyferium.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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