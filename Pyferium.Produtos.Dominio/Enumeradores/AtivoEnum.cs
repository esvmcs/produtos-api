using System.ComponentModel;

namespace Pyferium.Produtos.Dominio.Enumeradores
{
    public enum AtivoEnum
    {
        [Description("Ativo")]
        Ativo = 'S',
        [Description("Inativo")]
        Inativo = 'N'
    }
}
