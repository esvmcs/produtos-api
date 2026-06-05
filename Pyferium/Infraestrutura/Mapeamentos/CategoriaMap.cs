using FluentNHibernate.Mapping;
using Pyferium.Dominio.Entidades;
using Pyferium.Infraestrutura.Tipos;

namespace Pyferium.Infraestrutura.Mapeamentos;

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
            .CustomType<AtivoEnumType>()
            .CustomSqlType("char(1)")
            .Length(1)
            .Not.Nullable();
    }
}