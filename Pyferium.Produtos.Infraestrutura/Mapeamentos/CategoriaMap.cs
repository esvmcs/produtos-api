using FluentNHibernate.Mapping;
using Pyferium.Produtos.Dominio.Entidades;
using Pyferium.Produtos.Infraestrutura.Tipos;

namespace Pyferium.Produtos.Infraestrutura.Mapeamentos;

public class CategoriaMap : ClassMap<Categoria>
{
    public CategoriaMap()
    {
        Table("GEN_CATEGORIA");

        Id(x => x.CodigoCategoria)
            .Column("CODCATEGORIA")
            .GeneratedBy.Identity();

        Map(x => x.DescricaoCategoria)
            .Column("DSCCATEGORIA")
            .Length(45)
            .Not.Nullable();

        Map(x => x.CodigoNivel)
            .Column("CODNIVEL")
            .Length(2)
            .Not.Nullable();

        Map(x => x.IdtAtivo)
            .Column("IDTATIVO")
            .CustomType<AtivoEnumTipo>()
            .CustomSqlType("char(1)")
            .Length(1)
            .Not.Nullable();
    }
}