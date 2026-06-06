namespace Pyferium.Dominio.Entidades;

public class Produto : EntidadeBase
{
    public virtual int CodigoProduto { get; protected set; }
    public virtual string NomeProduto { get; protected set; } = string.Empty;
    public virtual Categoria Categoria { get; protected set; } = null!;
    public virtual decimal ValorProduto { get; protected set; }

    protected Produto() { }
    public Produto(string nomeProduto, Categoria categoria, decimal valorProduto)
    {
        SetNome(nomeProduto);
        SetCategoria(categoria);
        SetValor(valorProduto);
    }

    public virtual void SetNome(string nomeProduto)
    {
        if (string.IsNullOrWhiteSpace(nomeProduto))
            throw new ArgumentException("O nome do produto não pode ser vazio.");
        if (nomeProduto.Length > 80)
            throw new ArgumentException("O nome do produto deve conter no máximo 80 caracteres.");
        NomeProduto = nomeProduto;
    }
    public virtual void SetCategoria(Categoria categoria)
    {
        if (categoria is null)
            throw new ArgumentException("A categoria do produto não pode ser nula.");
        Categoria = categoria;
    }
    public virtual void SetValor(decimal valorProduto)
    {
        if (valorProduto < 0)
            throw new ArgumentException("O valor do produto não pode ser negativo ou zero.");
        ValorProduto = valorProduto;
    }
}