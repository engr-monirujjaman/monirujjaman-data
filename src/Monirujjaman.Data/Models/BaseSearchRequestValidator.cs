using FluentValidation;
using Monirujjaman.Data.Enums;
using Newtonsoft.Json;

namespace Monirujjaman.Data.Models;

public class BaseSearchRequestValidator<TModel> : AbstractValidator<SearchRequestModel> where TModel : class
{
    private readonly List<(string Name, Type Type)> _columnNames = new(typeof(TModel).GetProperties()
        .Select(a =>
        {
            if (a.PropertyType.IsGenericType && a.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                return (a.Name, Type: Nullable.GetUnderlyingType(a.PropertyType)!);

            return (a.Name, Type: a.PropertyType);
        }).ToList());

    private readonly IDictionary<OperatorType, IList<Type>> _operatorTypeDictionary =
        new Dictionary<OperatorType, IList<Type>>
        {
            {
                OperatorType.GreaterThan,
                new List<Type>
                    { typeof(int), typeof(DateTime), typeof(bool), typeof(double), typeof(float), typeof(long) }
            },
            {
                OperatorType.GreaterThanEquals,
                new List<Type>
                    { typeof(int), typeof(DateTime), typeof(bool), typeof(double), typeof(float), typeof(long) }
            },
            {
                OperatorType.LessThan,
                new List<Type>
                    { typeof(int), typeof(DateTime), typeof(bool), typeof(double), typeof(float), typeof(long) }
            },
            {
                OperatorType.LessThanEquals,
                new List<Type>
                    { typeof(int), typeof(DateTime), typeof(bool), typeof(double), typeof(float), typeof(long) }
            },
            {
                OperatorType.Equals,
                new List<Type>
                {
                    typeof(int), typeof(string), typeof(DateTime), typeof(bool), typeof(double), typeof(float),
                    typeof(long), typeof(Enum)
                }
            },
            { OperatorType.StartsWith, new List<Type> { typeof(string) } },
            { OperatorType.EndsWith, new List<Type> { typeof(string) } },
            {
                OperatorType.Contains,
                new List<Type>
                {
                    typeof(string), typeof(List<string>), typeof(List<int>), typeof(List<float>), typeof(List<double>),
                    typeof(List<long>), typeof(List<Guid>), typeof(List<Enum>)
                }
            }
        };

    public BaseSearchRequestValidator()
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
            .Must(item => _columnNames.Any(a => a.Name.Equals(item.FilterBy, StringComparison.OrdinalIgnoreCase)))
            .WithMessage((_, filter) => $"{nameof(filter.FilterBy)} '{filter.FilterBy}' is not valid.")
            .Must(IsOperatorAllowed)
            .WithMessage((_, filter) =>
                $"{nameof(filter.Operator)} '{filter.Operator}' is not valid for '{filter.FilterBy}'.")
            .Must(IsValidValue)
            .WithMessage(
                (_, filter) => $"{nameof(filter.Value)} '{filter.Value}' is not valid for '{filter.FilterBy}'.");

        RuleForEach(x => x.Sorts)
            .Must(item => _columnNames.Any(a => a.Name.Equals(item.SortBy, StringComparison.OrdinalIgnoreCase)))
            .WithMessage((_, sortModel) => $"{nameof(sortModel.SortBy)} '{sortModel.SortBy}' is not valid.");
    }

    private bool IsValidValue(FilterColumnModel item)
    {
        try
        {
            var column = _columnNames.FirstOrDefault(a => a.Name == item.FilterBy);

            if (column.Type is null) return false;

            if (column.Type.IsGenericType || column.Type.IsEnum)
            {
                var obj = JsonConvert.DeserializeObject(item.Value, column.Type);

                return obj is not null;
            }

            var _ = Convert.ChangeType(item.Value, column.Type);

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
        var column = _columnNames.FirstOrDefault(a => a.Name == item.FilterBy);
        return column.Type is not null && _operatorTypeDictionary[item.Operator].Any(a => a == column.Type);
    }
}