using System.Globalization;
using EfficientDynamoDb.Converters;
using EfficientDynamoDb.DocumentModel;
using Newtonsoft.Json.Linq;

namespace DynamoOutboxPattern.Lambda.BuildingBlocks.Infrastructure.DynamoDb.Converters;

public class JTokenConverter : DdbConverter<JToken>
{
    public override JToken Read(in AttributeValue attributeValue) => MapAttributeValue(attributeValue);

    public override AttributeValue Write(ref JToken value) => MapJToken(value);

    private static JToken MapAttributeValue(AttributeValue value)
        => value.Type switch
        {
            AttributeType.String => JToken.FromObject(value.AsString()),
            AttributeType.Number => MapNumberAttributeValue(value.AsNumberAttribute()),
            AttributeType.Bool => JToken.FromObject(value.AsBool()),
            AttributeType.Map => MapDocument(value.AsDocument()),
            AttributeType.List => MapAttributeValueList(value.AsListAttribute()),
            AttributeType.Null => JValue.CreateNull(),
            _ => throw new ArgumentOutOfRangeException(nameof(AttributeValue.Type), value.Type.ToString())
        };

    private static JToken MapNumberAttributeValue(NumberAttributeValue value)
        => value.Value.Contains(".") ? JToken.FromObject(value.ToDecimal()) : JToken.FromObject(value.ToLong());

    private static JObject MapDocument(Document document)
    {
        var jObject = new JObject();

        foreach (var (key, value) in document)
        {
            jObject.TryAdd(key, MapAttributeValue(value));
        }

        return jObject;
    }

    private static JToken MapAttributeValueList(ListAttributeValue valueList)
    {
        var list = valueList.Items.Select(MapAttributeValue).ToList();
        return JToken.FromObject(list);
    }

    private static AttributeValue MapJToken(JToken jToken)
        => jToken.Type switch
        {
            JTokenType.Object => MapJObject((JObject)jToken),
            JTokenType.Guid => new StringAttributeValue(jToken.Value<Guid>().ToString()),
            JTokenType.Array => MapJTokenArray(jToken),
            JTokenType.Integer => new NumberAttributeValue(jToken.Value<long>().ToString(CultureInfo.InvariantCulture)),
            JTokenType.Float => new NumberAttributeValue(jToken.Value<decimal>().ToString(CultureInfo.InvariantCulture)),
            JTokenType.String => new StringAttributeValue(jToken.Value<string>()!),
            JTokenType.Boolean => new BoolAttributeValue(jToken.Value<bool>()),
            JTokenType.Date => new StringAttributeValue(
                jToken.Value<DateTime>().ToUniversalTime().ToString("O", CultureInfo.InvariantCulture)
            ),
            JTokenType.Null => new NullAttributeValue(),
            _ => throw new ArgumentOutOfRangeException(nameof(jToken.Type), jToken.Type.ToString())
        };

    private static Document MapJObject(JObject jObject)
    {
        var doc = new Document();
        foreach (var (key, jToken) in jObject)
        {
            doc[key] = MapJToken(jToken!);
        }

        return doc;
    }

    private static ListAttributeValue MapJTokenArray(JToken jTokenArray)
    {
        var list = jTokenArray.Select(MapJToken).ToList();
        return new ListAttributeValue(list);
    }
}
