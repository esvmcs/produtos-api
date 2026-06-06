using System.Data;
using System.Data.Common;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Pyferium.Dominio.Enumeradores;

namespace Pyferium.Infraestrutura.Tipos;

public class AtivoEnumTipo : IUserType
{
    public SqlType[] SqlTypes => new[]
    {
        new SqlType(DbType.StringFixedLength, 1)
    };

    public Type ReturnedType => typeof(AtivoEnum);

    public bool IsMutable => false;

    public new bool Equals(object? x, object? y)
    {
        if (ReferenceEquals(x, y))
            return true;

        if (x is null || y is null)
            return false;

        return x.Equals(y);
    }

    public int GetHashCode(object? x)
    {
        return x?.GetHashCode() ?? 0;
    }

    public object? NullSafeGet(
        DbDataReader rs,
        string[] names,
        ISessionImplementor session,
        object owner)
    {
        var valorBanco = NHibernateUtil.String
            .NullSafeGet(rs, names[0], session) as string;

        valorBanco = valorBanco?.Trim().ToUpperInvariant();

        return valorBanco switch
        {
            "S" => AtivoEnum.Ativo,
            "N" => AtivoEnum.Inativo,
            null => null,
            "" => null,
            _ => throw new InvalidOperationException(
                $"Valor inválido para IDTATIVO: {valorBanco}")
        };
    }

    public void NullSafeSet(
        DbCommand cmd,
        object? value,
        int index,
        ISessionImplementor session)
    {
        var valorBanco = value switch
        {
            AtivoEnum.Ativo => "S",
            AtivoEnum.Inativo => "N",
            null => null,
            _ => throw new InvalidOperationException(
                $"Valor inválido para {nameof(AtivoEnum)}: {value}")
        };

        NHibernateUtil.String.NullSafeSet(cmd, valorBanco, index, session);
    }

    public object? DeepCopy(object? value)
    {
        return value;
    }

    public object? Replace(object? original, object? target, object owner)
    {
        return original;
    }

    public object? Assemble(object? cached, object owner)
    {
        return cached;
    }

    public object? Disassemble(object? value)
    {
        return value;
    }
}