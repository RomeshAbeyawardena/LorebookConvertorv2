using System.Collections;
using System.Collections.Concurrent;

namespace Lorebook.Convertor.Web.Api.Exception;

public class TypeCollection(IEnumerable<Type> exceptions) 
    : IEnumerable<Type>
{
    private readonly ConcurrentBag<Type> exceptions = new(exceptions);

    public IEnumerator<Type> GetEnumerator()
    {
        return exceptions.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return exceptions.GetEnumerator();
    }
}
