using Shared.Serialization;
using System.Text.Json;

namespace Shared.UnitTests;

public class Serialization
{
    [Theory]
    [InlineData("test")]
    [InlineData("")]
    public void TestNestedSingleValueObject_ReturnsEquals(string value)
    {
        var dummyObj = new DummyOuter()
        {
            Id = new DummyOuter.DummyInner(value),
            DummyProperty = "xD"
        };

        var converter = new SingleValueObjectConverterFactory().CreateConverter(typeof(DummyOuter.DummyInner), new JsonSerializerOptions());
        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                converter
            }
        };

        var jsonString = JsonSerializer.Serialize(dummyObj, serializeOptions);
        var expected = $"{{\r\n  \"Id\": \"{value}\",\r\n  \"DummyProperty\": \"xD\"\r\n}}";
        
        Assert.Equal(expected, jsonString);
    }

    [Theory]
    [InlineData("test")]
    public void DeserializeTestNestedSingleValueObjectWithOtherProperty_ReturnsEquals(string value)
    {
        var dummyObj = new DummyOuter()
        {
            Id = new DummyOuter.DummyInner(value),
            DummyProperty = "xD"
        };

        var converter = new SingleValueObjectConverterFactory().CreateConverter(typeof(DummyOuter.DummyInner), new JsonSerializerOptions());
        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                converter
            }
        };

        var jsonString = JsonSerializer.Serialize(dummyObj, serializeOptions);
        var expected = $"{{\r\n  \"Id\": \"{value}\",\r\n  \"DummyProperty\": \"xD\"\r\n}}";

        var test = JsonSerializer.Deserialize<DummyOuter>(jsonString, serializeOptions);

        Assert.Equal(dummyObj.Id.Value, test.Id.Value);
    }
}

internal class DummyOuter
{
    public DummyInner Id { get; init; } = default!;
    public string DummyProperty { get; init; } = default!;

    internal class DummyInner : SingleValueObject<string>
    {
        public DummyInner(string value) : base(value)
        {

        }
    }
}
