﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace SharedKernel;

public abstract class Specification<TEntity>(
    Expression<Func<TEntity, bool>>? criteria = null,
    bool criteriaCondition = true)
    where TEntity : class
{
    private readonly List<(Expression<Func<TEntity, bool>>, bool)> _criteria = criteria is null ? [] : [(criteria, criteriaCondition)];

    public IReadOnlyList<(Expression<Func<TEntity, bool>>, bool)> Criteria => _criteria.AsReadOnly();

    public bool IsSplitQuery { get; protected set; }

    public Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? IncludeExpression { get; protected set; }

    public Specification<TEntity> AddInclude(
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeExpression)
    {
        if (includeExpression is null)
        {
            throw new InvalidOperationException("IncludeExpression is null");
        }

        IncludeExpression = includeExpression;

        return this;
    }

    protected Specification<TEntity> AddCriteria(
        Expression<Func<TEntity, bool>> criteria,
        bool condition = true)
    {
        if (criteria is null)
        {
            throw new InvalidOperationException("Criteria is null");
        }

        _criteria.Add((criteria, condition));

        return this;
    }
}

public static class SpecificationEvaluator
{
    public static IQueryable<TEntity> GetQuery<TEntity>(
        this IQueryable<TEntity> queryable,
        Specification<TEntity> specification)
        where TEntity : class
    {
        if (specification.Criteria is not null)
        {
            for (int i = 0; i < specification.Criteria.Count; i++)
            {
                queryable = queryable.WhereIf(specification.Criteria[i].Item2, specification.Criteria[i].Item1);
            }
        }

        if (specification.IncludeExpression is not null)
        {
            queryable = specification.IncludeExpression(queryable);
        }

        return queryable;
    }
}