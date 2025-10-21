using DomainLayer.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEnitiy> CreateQuery<TEnitiy, TKey>(IQueryable<TEnitiy> inputQuery, ISpecifications<TEnitiy, TKey> specifications) where TEnitiy : DomainLayer.Models.BaseEntity<TKey>
        {
            var query = inputQuery;
            if (specifications.Criteria != null)
            {
                query = query.Where(specifications.Criteria);
            }
            if (specifications.OrderBy != null)
            {
                query = query.OrderBy(specifications.OrderBy);
            }
            else if (specifications.OrderByDescending != null)
            {
                query = query.OrderByDescending(specifications.OrderByDescending);
            }
            if (specifications.IncludeExpression == null || !specifications.IncludeExpression.Any())
            {
                return query;
            }
            query = specifications.IncludeExpression.Aggregate(query, (current, include) => current.Include(include));
            if(specifications.IsPaginated)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }
            return query;
        }   
    }
}
