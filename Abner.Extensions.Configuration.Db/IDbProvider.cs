using Microsoft.Extensions.Primitives;

namespace Abner.Extensions.Configuration.Db
{
    public interface IDbProvider
    {
        IChangeToken Watch(string tableName);
    }
}
