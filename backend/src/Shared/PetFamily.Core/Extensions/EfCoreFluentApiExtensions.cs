using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PetFamily.Core.Extensions
{
    public static class EfCoreFluentApiExtensions
    {
        public static PropertyBuilder<IReadOnlyList<TValueObject>> ValueObjectCollectionJsonConversion<TValueObject, TDto>(
            this PropertyBuilder<IReadOnlyList<TValueObject>> propertyBuilder,
            Func<TValueObject, TDto> toDtoSelector,
            Func<TDto, TValueObject> toValueObjectSelector)
        {
            return propertyBuilder.HasConversion(
                valueObjects => SerializeValueObjectsCollection(valueObjects, toDtoSelector),
                json => DeserializeDto(json, toValueObjectSelector),
                   CreateCollectionValueComparer<TValueObject>())
                .HasColumnType("Jsonb");
        }

        private static string SerializeValueObjectsCollection<TValueObject, TDto>(
            IReadOnlyList<TValueObject> valueObjects, Func<TValueObject, TDto> selector)
        {
            var dtos = valueObjects.Select(selector);

            return JsonSerializer.Serialize(dtos, JsonSerializerOptions.Default);
        }

        private static IReadOnlyList<TValueObject> DeserializeDto<TValueObject, TDto>(
            string json, Func<TDto, TValueObject> selector)
        {
            var dtos = JsonSerializer.Deserialize<IEnumerable<TDto>>(json, JsonSerializerOptions.Default) ?? [];

            return dtos.Select(selector).ToList();
        }

        private static ValueComparer<IReadOnlyList<T>> CreateCollectionValueComparer<T>()
        {
            return new ValueComparer<IReadOnlyList<T>>(
                        (c1, c2) => c1!.SequenceEqual(c2!),
                        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v!.GetHashCode())),
                        c => c.ToList());
        }
    }
}
