namespace Pyferium.Infraestrutura.Repositorios.Interfaces;

public interface ICategoriaRepositorio
{
    Task<bool> VerificarExistenciaCategoriaAsync(int codigoCategoria);
}
