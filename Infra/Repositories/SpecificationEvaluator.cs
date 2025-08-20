using Challenge.Core.Abstraction;
using Microsoft.EntityFrameworkCore;


namespace Infra.Repositories
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> input, ISpecification<T>? spec) where T : class
        {
            if (spec?.Criteria != null) input = input.Where(spec.Criteria);
            if (spec != null)
                input = spec.Includes.Aggregate(input, (q, include) => q.Include(include));
            return input;
        }
    }
}
