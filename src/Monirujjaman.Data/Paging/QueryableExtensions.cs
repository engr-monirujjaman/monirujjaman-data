﻿using System.Globalization;
using System.Linq.Dynamic.Core;
using Monirujjaman.Data.Enums;
using Monirujjaman.Data.Models;

namespace Monirujjaman.Data.Paging;

public static class QueryableExtensions
{
    public static IQueryable<TSource> Where<TSource>(this IQueryable<TSource> query, IList<FilterColumnModel> filters)
        where TSource : class
    {
        filters.ToList().ForEach(filter =>
        {
            if (!filter.FilterBy.Contains('.'))
            {
                var op = GetOperator(filter.Operator);
                var (name, type) = GetPropertyType(filter.FilterBy);

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    Nullable.GetUnderlyingType(type);

                var value = type == typeof(string)
                    ? filter.Value
                    : Convert.ChangeType(filter.Value, type, CultureInfo.InvariantCulture);

                string predicate;

                if (filter.Operator is OperatorType.GreaterThan or OperatorType.GreaterThanEquals
                    or OperatorType.LessThan or OperatorType.LessThanEquals)
                {
                    predicate = name + " != null && " + name + " " + op + " @0";
                }
                else if (type == typeof(string))
                {
                    predicate = name + ".ToLower()." + op + "(@0.ToLower())";
                }
                else
                {
                    predicate = name + "." + op + "(@0)";
                }

                query = query.Where(predicate, value);
            }
        });

        (string Name, Type Type) GetPropertyType(string name) => typeof(TSource).GetProperties()
            .Select(x => (x.Name, x.PropertyType))
            .FirstOrDefault(x => x.Name.Equals(name));

        string GetOperator(OperatorType op) => op switch
        {
            OperatorType.Equals => "Equals",
            OperatorType.Contains => "Contains",
            OperatorType.StartsWith => "StartsWith",
            OperatorType.EndsWith => "EndsWith",
            OperatorType.GreaterThan => ">",
            OperatorType.GreaterThanEquals => ">=",
            OperatorType.LessThan => "<",
            OperatorType.LessThanEquals => "<=",
            _ => "Equals"
        };

        return query;
    }

    public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> query, IList<SortOrderModel> sorts)
        where TSource : class
    {
        if (sorts.Any())
        {
            var firstColumn = sorts.First();

            IOrderedQueryable<TSource> source =
                query.OrderBy($"{firstColumn.SortBy} {GetColumnSortOrder(firstColumn.Order)}");

            sorts.Skip(1).ToList().ForEach(sortColumn =>
            {
                source = source.ThenBy($"{sortColumn.SortBy} {GetColumnSortOrder(sortColumn.Order)}");
            });

            string GetColumnSortOrder(SortOrderType type) => type switch
            {
                SortOrderType.Descending => "DESC",
                _ => "ASC"
            };

            return source;
        }

        return query;
    }
}