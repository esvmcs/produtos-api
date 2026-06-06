namespace Pyferium.Aplicacao.Categorias.Repositorios;

public interface ICategoriaRepositorio
{
    Task<bool> VerificarExistenciaCategoriaAsync(int codigoCategoria);
}
