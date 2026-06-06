using Pyferium.Produtos.Dominio.Enumeradores;

namespace Pyferium.Produtos.Dominio.Entidades;

public abstract class EntidadeBase
{
    public virtual AtivoEnum IdtAtivo { get; protected set; } = AtivoEnum.Ativo;

    public virtual bool EstaAtivo => IdtAtivo == AtivoEnum.Ativo;

    public virtual void Ativar()
    {
        IdtAtivo = AtivoEnum.Ativo;
    }

    public virtual void Inativar()
    {
        IdtAtivo = AtivoEnum.Inativo;
    }
}