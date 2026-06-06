namespace Pyferium.Produtos.Dominio.Entidades;

public class Categoria : EntidadeBase
{
    public virtual int CodigoCategoria { get; protected set; }
    public virtual string DescricaoCategoria { get; protected set; } = string.Empty;
    public virtual string CodigoNivel { get; protected set; } = string.Empty;

    protected Categoria() { }
    public Categoria(string descricaoCategoria, string codigoNivel)
    {
        SetDescricao(descricaoCategoria);
        SetCodigoNivel(codigoNivel);
    }

    public virtual void SetDescricao(string descricaoCategoria)
    {
        if (string.IsNullOrWhiteSpace(descricaoCategoria))
            throw new ArgumentException("A descrição da categoria não pode ser vazia.");
        if (descricaoCategoria.Length > 45)
            throw new ArgumentException("A descrição da categoria deve conter no máximo 45 caracteres.");
        DescricaoCategoria = descricaoCategoria;
    }
    public virtual void SetCodigoNivel(string codigoNivel)
    {
        if (string.IsNullOrWhiteSpace(codigoNivel))
            throw new ArgumentException("O código de nível não pode ser vazio.");
        if (codigoNivel.Length > 2)
            throw new ArgumentException("O código de nível deve conter no máximo 2 caracteres.");
        CodigoNivel = codigoNivel;
    }
}