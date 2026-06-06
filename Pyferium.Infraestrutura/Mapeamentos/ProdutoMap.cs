using FluentNHibernate.Mapping;
using Pyferium.Dominio.Entidades;
using Pyferium.Infraestrutura.Tipos;

namespace Pyferium.Infraestrutura.Mapeamentos;

public class ProdutoMap : ClassMap<Produto>
{
    public ProdutoMap()
    {
        Table("GEN_PRODUTO");

        Id(x => x.CodigoProduto)
            .Column("CODPRODUTO")
            .GeneratedBy.Identity();

        Map(x => x.NomeProduto)
            .Column("NOMPRODUTO")
            .Length(80)
            .Not.Nullable();

        References(x => x.Categoria)
            .Column("CODCATEGORIA")
            .Not.Nullable()
            .Cascade.None();

        Map(x => x.ValorProduto)
            .Column("VLRPRODUTO")
            .CustomSqlType("decimal(10,2)")
            .Not.Nullable();

        Map(x => x.IdtAtivo)
            .Column("IDTATIVO")
            .CustomType<AtivoEnumTipo>()
            .CustomSqlType("char(1)")
            .Length(1)
            .Not.Nullable();
    }
}