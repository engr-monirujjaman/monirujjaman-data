﻿using CleanArchitecture.Data.Enums;
using FluentValidation;

namespace Monirujjaman.Data.Models;

public class SearchRequestBaseValidator<TModel> : AbstractValidator<SearchRequestBaseModel> where TModel : class
{
     private readonly List<(string Name, Type Type)> _columnNames = new(typeof(TModel).GetProperties()
            .Select(a =>
            {
                if (a.PropertyType.IsGenericType && a.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return (a.Name, Type: Nullable.GetUnderlyingType(a.PropertyType)!);
                }

                return (a.Name, Type: a.PropertyType);
            }).ToList());

        private readonly IDictionary<OperatorType, IList<Type>> _operatorTypeDictionary = new Dictionary<OperatorType, IList<Type>>
        {
            { OperatorType.GreaterThan, new List<Type> { typeof(int), typeof(DateTime), typeof(bool), typeof(double), typeof(float), typeof(long) } },
            { OperatorType.GreaterThanEquals, new List<Type> { typeof(int), typeof(DateTime), typeof(bool), typeof(double), typeof(float), typeof(long) } },
            { OperatorType.LessThan, new List<Type> { typeof(int), typeof(DateTime), typeof(bool), typeof(double), typeof(float), typeof(long) } },
            { OperatorType.LessThanEquals, new List<Type> { typeof(int), typeof(DateTime), typeof(bool), typeof(double), typeof(float), typeof(long) } },
            { OperatorType.Equals, new List<Type> { typeof(int), typeof(string), typeof(DateTime), typeof(bool), typeof(double), typeof(float), typeof(long) } },
            { OperatorType.StartsWith, new List<Type> { typeof(string) } },
            { OperatorType.EndsWith, new List<Type> { typeof(string) } },
            { OperatorType.Contains, new List<Type> { typeof(string) } },
        };

        public SearchRequestBaseValidator()
        {
            RuleFor(x => x.PageSize)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("PageSize is required.")
                .NotEmpty().WithMessage("PageSize is required.")
                .GreaterThanOrEqualTo(1).WithMessage("PageSize is invalid.");

            RuleFor(x => x.PageIndex)
                .Cascade(CascadeMode.Stop)
                .NotNull().WithMessage("PageIndex is required.")
                .NotEmpty().WithMessage("PageIndex is required.")
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber is invalid.");

            RuleForEach(x => x.Filters)
                .Cascade(CascadeMode.Stop)
                .Must(item => !string.IsNullOrWhiteSpace(item.Value))
                .WithMessage((_, filter) => $"{nameof(filter.Value)} '{filter.Value}' is not valid.")
                .Must(item => _columnNames.Any(a => a.Name.Equals(item.ColumnName, StringComparison.OrdinalIgnoreCase)))
                .WithMessage((_, filter) => $"{nameof(filter.ColumnName)} '{filter.ColumnName}' is not valid.")
                .Must(IsOperatorAllowed)
                .WithMessage((_, filter) => $"{nameof(filter.Operator)} '{filter.Operator}' is not valid for '{filter.ColumnName}'.")
                .Must(IsValidValue)
                .WithMessage((_, filter) => $"{nameof(filter.Value)} '{filter.Value}' is not valid for '{filter.ColumnName}'.");
            
            RuleForEach(x => x.Sorts)
                .Must(item => _columnNames.Any(a => a.Name.Equals(item.SortBy, StringComparison.OrdinalIgnoreCase)))
                .WithMessage((_, sortModel) => $"{nameof(sortModel.SortBy)} '{sortModel.SortBy}' is not valid.");
        }

        private bool IsValidValue(FilterColumnModel item)
        {
            try
            {
                var type = _columnNames.First(a => a.Name == item.ColumnName).Type;
                
                var _ = Convert.ChangeType(item.Value, type);

                return true;
            }
            catch (InvalidCastException)
            {
                return false;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (Exception)

            {
                return false;
            }
        }

        private bool IsOperatorAllowed(FilterColumnModel item)
        {
            var type = _columnNames.First(a => a.Name == item.ColumnName).Type;

            return _operatorTypeDictionary[item.Operator].Any(a => a == type);
        }
}