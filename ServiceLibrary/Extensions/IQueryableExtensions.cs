using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Models;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Z.EntityFramework.Plus;

public static class IQueryableExtensions
{

    private static IQueryable<T> FiltersApply<T>(IQueryable<T> queryable, List<DataSourceRequestFilter> filters)
    {
        foreach (var filter in filters)
        {
            if (filter.Value == "null" || string.IsNullOrEmpty(filter.Value)) continue;

            if (filter.Operator == FilterOperator.Contains)
            {
                queryable = queryable.Where($"{filter.Field}.Contains(@0) ", (object)filter.Value);
            }
            else if (filter.Operator == FilterOperator.IsGreaterThan)
            {
                queryable = queryable.Where($"{filter.Field} > @0 ", (object)filter.Value);
            }
            else if (filter.Operator == FilterOperator.IsGreaterThanOrEqualTo)
            {
                queryable = queryable.Where($"{filter.Field} >= @0 ", (object)filter.Value);
            }
            else if (filter.Operator == FilterOperator.IsLessThan)
            {
                queryable = queryable.Where($"{filter.Field} < @0 ", (object)filter.Value);
            }
            else if (filter.Operator == FilterOperator.IsLessThanOrEqualTo)
            {
                queryable = queryable.Where($"{filter.Field} <= @0 ", (object)filter.Value);
            }
            else if (filter.Operator == FilterOperator.IsNotEqualTo)
            {
                queryable = queryable.Where($"{filter.Field} <> @0 ", (object)filter.Value);
            }
            else
            {
                queryable = queryable.Where($"{filter.Field} = @0 ", (object)filter.Value);
            }
        }
        return queryable;
    }

    private static IQueryable<T> ScopesFilterApply<T>(IQueryable<T> queryable, List<DataSourceRequestScopeFilter> scopes)
    {
        foreach (var scope in scopes)
        {

            int Index = 0;
            string query = "";


            foreach (var filter in scope.Filters)
            {
                if (filter.Operator == FilterOperator.Contains)
                {
                    query += $" {filter.Field}.Contains(@{Index}) {scope.Condition} ";
                }
                else if (filter.Operator == FilterOperator.IsGreaterThan)
                {
                    query += $" {filter.Field} > @{Index} {scope.Condition} ";

                }
                else if (filter.Operator == FilterOperator.IsGreaterThanOrEqualTo)
                {
                    query += $" {filter.Field} >= @{Index} {scope.Condition} ";
                }
                else if (filter.Operator == FilterOperator.IsLessThan)
                {
                    query += $" {filter.Field} < @{Index} {scope.Condition} ";

                }
                else if (filter.Operator == FilterOperator.IsLessThanOrEqualTo)
                {
                    query += $" {filter.Field} <= @{Index} {scope.Condition} ";
                }
                else if (filter.Operator == FilterOperator.IsNotEqualTo)
                {
                    query += $" {filter.Field} <> @{Index} {scope.Condition} ";
                }
                else
                {
                    query += $" {filter.Field} = @{Index} {scope.Condition} ";
                }

                Index += 1;
            }

            query = query.Substring(0, query.LastIndexOf(scope.Condition.ToString()));
            queryable = queryable.Where(query, scope.Filters.Select(x => x.Value).ToArray());
            queryable = ScopesFilterApply(queryable, scope.Scopes);
        }
        return queryable;
    }

    public static async Task<PagedResultDto<T>> ToPagedResult<T>(this IQueryable<T> queryable, DataSourceRequestDTO request)
    {
        if (request != null)
        {
            queryable = FiltersApply(queryable, request.Filters);
            queryable = ScopesFilterApply(queryable, request.ScopesFilter);
        }

       var ToTalCount = queryable.DeferredCount().FutureValue();
        queryable = queryable.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize);


        var toReturn = new PagedResultDto<T>();
        //toReturn.Items = await queryable.ToListAsync();
        toReturn.Items = await queryable.Future().ToListAsync();

        toReturn.TotalCount = ToTalCount;
        toReturn.TotalCount = ToTalCount.Value;
        return toReturn;
    }
}
