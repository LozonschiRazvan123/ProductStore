using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProductStore.Core.Interface;
using ProductStore.Framework.Page;
using ProductStore.Framework.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductStore.Infrastructure.Repository
{
    public class PaginationRepository<T> : IServicePagination<T>
    {
        public async Task<PageResult<T>> GetCustomersPagination(IQueryable<T> query, PaginationFilter filter)
        {

            if (!string.IsNullOrEmpty(filter.SortByField))
            {
                var parts = filter.SortByField.Split('_');
                if (parts.Length == 2)
                {
                    var columnName = parts[0];
                    var columnSort = parts[1];

                    var entityType = typeof(T);
                    var propertyInfo = entityType.GetProperty(columnName);

                    if (propertyInfo != null)
                    {
                        var parameter = Expression.Parameter(typeof(T));
                        var propertyAccess = Expression.Property(parameter, columnName);
                        var orderByExp = Expression.Lambda(propertyAccess, parameter);

                        var methodName = columnSort.ToLower() == "desc" ? "OrderByDescending" : "OrderBy";

                        query = query.Provider.CreateQuery<T>(
                            Expression.Call(
                                typeof(Queryable),
                                methodName,
                                new[] { typeof(T), propertyInfo.PropertyType },
                                query.Expression,
                                Expression.Quote(orderByExp)
                            )
                        );
                    }
                    else
                    {
                        throw new Exception($"The property '{columnName}' does not exist in the entity '{entityType.Name}'.");
                    }
                }
                else
                {
                    throw new Exception("Invalid SortByField format. It should be in the format 'ColumnName_SortDirection'.");
                }
            }

            /*if (!string.IsNullOrEmpty(filter.Keyword))
            {
                var propsToCheck = typeof(T).GetProperties();
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                var searchValue = Expression.Constant(filter.Keyword, typeof(string));
                var parameter = Expression.Parameter(typeof(T));
                var predicate = PredicateBuilder.New<T>(false);

                foreach (var prop in propsToCheck)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        var propertyValue = Expression.Property(parameter, prop);
                        var containsExpr = Expression.Call(propertyValue, containsMethod, searchValue);
                        predicate = predicate.Or(Expression.Lambda<Func<T, bool>>(containsExpr, parameter));
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        var propertyValue = Expression.Property(parameter, prop);
                        var toStringMethod = typeof(int).GetMethod("ToString", Type.EmptyTypes);
                        var propertyToStringCall = Expression.Call(propertyValue, toStringMethod);
                        var containsExpr = Expression.Call(propertyToStringCall, containsMethod, searchValue);
                        predicate = predicate.Or(Expression.Lambda<Func<T, bool>>(containsExpr, parameter));
                    }
                }

                query = query.Where(predicate);
            }*/

            query = query.Where(Filter(query, filter.Keyword));
            var totalRecords = await query.CountAsync();

            var pagedData = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalRecords / (double)filter.PageSize);

            return new PageResult<T>
            {
                Results = pagedData,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalPages = totalPages,
                TotalRecords = totalRecords
            };
        }


        private static ExpressionStarter<T> Filter<T>(IQueryable<T> query, string searchStr)
        {
            var propsToCheck = typeof(T).GetProperties();
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var searchValue = Expression.Constant(searchStr, typeof(string));
            var parameter = Expression.Parameter(typeof(T));
            var predicate = PredicateBuilder.New<T>(false);

            foreach (var prop in propsToCheck)
            {
                if (prop.PropertyType == typeof(string))
                {
                    var propertyValue = Expression.Property(parameter, prop);
                    var containsExpr = Expression.Call(propertyValue, containsMethod, searchValue);
                    predicate = predicate.Or(Expression.Lambda<Func<T, bool>>(containsExpr, parameter));
                }
                else if (prop.PropertyType == typeof(int))
                {
                    var propertyValue = Expression.Property(parameter, prop);
                    var toStringMethod = typeof(int).GetMethod("ToString", Type.EmptyTypes);
                    var propertyToStringCall = Expression.Call(propertyValue, toStringMethod);
                    var containsExpr = Expression.Call(propertyToStringCall, containsMethod, searchValue);
                    predicate = predicate.Or(Expression.Lambda<Func<T, bool>>(containsExpr, parameter));
                }
            }

            return predicate;
        }
    }
}
