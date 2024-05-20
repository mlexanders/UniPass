using System.Linq.Expressions;
using Serialize.Linq.Serializers;

namespace UniPass.Infrastructure;

public class Specification<TEntity> where TEntity : class
{
    private static ExpressionSerializer Serializer => new(new JsonSerializer())
    {
        AutoAddKnownTypesAsListTypes = true
        // AutoAddKnownTypesAsArrayTypes = true
    };


    public List<string>? Includes { get; set; }
    public string? Filter { get; set; }

    public Specification<TEntity> Where(Expression<Func<TEntity, bool>> filter)
    {
        Filter = Serializer.SerializeText(filter);
        return this;
    }

    public Specification<TEntity> Include(params Expression<Func<TEntity, object>>[] includes)
    {
        Includes = includes?.Select(i => Serializer.SerializeText(i)).ToList();
        return this;
    }

    public (Expression<Func<TEntity, bool>> filter, List<Expression<Func<TEntity, object>>>? includes) GetExpressions()
    {
        var includes = Includes?
            .Select(i => (Expression<Func<TEntity, object>>)Serializer.DeserializeText(i))
            .ToList();

        var filter = (Expression<Func<TEntity, bool>>)Serializer.DeserializeText(Filter);

        return (filter, includes);
    }
}