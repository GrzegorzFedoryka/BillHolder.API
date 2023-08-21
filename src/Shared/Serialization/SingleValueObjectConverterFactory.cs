using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Shared.Serialization;

public class SingleValueObjectConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
        => typeToConvert?.BaseType == typeof(SingleValueObject<>);

    public override JsonConverter CreateConverter(
        Type type,
        JsonSerializerOptions? options)
    {
        Type wrapperType = type;
        Type valueType = type.BaseType!.GetGenericArguments()[0];

        JsonConverter converter = (JsonConverter)Activator.CreateInstance(
            typeof(SingleValueObjectConverterInner<,>).MakeGenericType(
                new Type[] { wrapperType, valueType }),
            BindingFlags.Instance | BindingFlags.Public,
            binder: null,
            args: new object?[] { options },
            culture: null)!;

        return converter;
    }

    private class SingleValueObjectConverterInner<Twrapper, Tvalue>
        : JsonConverter<Twrapper> where Twrapper : SingleValueObject<Tvalue>
    {
        private readonly JsonConverter<Tvalue> _valueConverter;
        private readonly Type _valueType;

        public SingleValueObjectConverterInner(JsonSerializerOptions options)
        {
            // For performance, use the existing converter
            _valueConverter = (JsonConverter<Tvalue>)options
                .GetConverter(typeof(Tvalue));

            // Cache the value type
            _valueType = typeof(Tvalue);
        }

        public override Twrapper Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType is not JsonTokenType.String or JsonTokenType.Number)
            {
                throw new JsonException();
            }

            Tvalue value = _valueConverter.Read(ref reader, _valueType, options)!;


            return (Twrapper)Activator.CreateInstance(
                typeof(Twrapper),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                binder: null,
                args: new object[] { value },
                culture: null
                )!;
        }

        public override void Write(
            Utf8JsonWriter writer,
            Twrapper value,
            JsonSerializerOptions options)
        {
            _valueConverter.Write(writer, value.Value, options);
        }
    }
}