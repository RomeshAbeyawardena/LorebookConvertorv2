namespace Lorebook.Convertor.Web.Api.Exception;

public class TypeCollectionBuilder()
{
    private readonly List<Type> exceptions = [];

    public TypeCollectionBuilder AddRange(params Type[] exceptions)
    {
        this.exceptions.AddRange(exceptions);
        return this;
    }

    public TypeCollection Build()
    {
        return new TypeCollection(exceptions);
    }
}
