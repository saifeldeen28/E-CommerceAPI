using DomainLayer.Contracts;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
    public abstract class BaseSpecifications<TEnitiy, TKey> : ISpecifications<TEnitiy, TKey> where TEnitiy : BaseEntity<TKey>
    {
        public BaseSpecifications(Expression<Func<TEnitiy, bool>> CriteriaExpression) 
        { 
            Criteria = CriteriaExpression;
        }
        public Expression<Func<TEnitiy, bool>>? Criteria { get; private set; }

        public List<Expression<Func<TEnitiy, object>>> IncludeExpression { get; } = [];

        public Expression<Func<TEnitiy, object>> OrderBy { get; private set; }

        public Expression<Func<TEnitiy, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }
        protected void ApplyPagination(int PageSize, int PageIndex)
        {
            Skip = (PageIndex-1)*PageSize;
            Take = PageSize;
            IsPaginated = true;
        }

        protected void AddInclude(Expression<Func<TEnitiy, object>> includeExpression)
        {
            IncludeExpression.Add(includeExpression);
        }
        protected void AddOrderBy(Expression<Func<TEnitiy, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<TEnitiy, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
    }
}
