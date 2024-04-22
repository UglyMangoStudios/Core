using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Core.Extensions
{
	public static class JsonUtility
	{
		public static JsonSerializerSettings ConstructDefaultSettings()
		{
			return new()
			{
				TypeNameHandling = TypeNameHandling.All,
				SerializationBinder = new DefaultSerializationBinder(),
				ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
			};
		}


		public static string SerializeWithSettings<T>(T objuct) => SerializeWithSettings<T>(objuct, null);
		public static string SerializeWithSettings<T>(T objuct, JsonSerializerSettings? settings)
		{
			settings ??= ConstructDefaultSettings();

			return JsonConvert.SerializeObject(objuct, Formatting.None, settings);
		}



		public static T? DeserializeWithSettings<T>(string json) => DeserializeWithSettings<T>(json, null);
		public static T? DeserializeWithSettings<T>(string json, JsonSerializerSettings? settings)
		{
			settings ??= ConstructDefaultSettings();

			return JsonConvert.DeserializeObject<T>(json, settings);
		}


		public static void StreamSerialize(object value, Stream s)
		{
			using StreamWriter writer = new(s);
			using JsonTextWriter jsonWriter = new(writer);

			JsonSerializer serializer = new();
			serializer.TypeNameHandling = TypeNameHandling.All;

			serializer.Serialize(jsonWriter, value);

			jsonWriter.Flush();
		}

		public static T? StreamDeserialize<T>(Stream s)
		{
			using StreamReader reader = new(s);
			using JsonTextReader jsonReader = new(reader);

			JsonSerializer serializer = new();
			serializer.TypeNameHandling = TypeNameHandling.All;

			return serializer.Deserialize<T>(jsonReader);
		}



		//public (ValueConverter, ValueComparer) CreateJsonValueConverter(Type type)
		//{
		//	var converterType = typeof(JsonValueConverter<>);
		//	var comparerType = typeof(JsonValueComparer<>);

		//	var genericConverterType = converterType.MakeGenericType(type);
		//	var genericComparerType = comparerType.MakeGenericType(type);

		//	ValueConverter converterInstance = Activator.CreateInstance(genericConverterType) 
		//		as ValueConverter ?? 
		//		throw new Exception("Could not generically create value converter!");

		//	ValueComparer comparerInstance = Activator.CreateInstance(genericComparerType)
		//		as ValueComparer ??
		//		throw new Exception("Could not generically create value converter!");

		//	return (converterInstance, comparerInstance);

		//}

		//public (ValueConverter, ValueComparer) CreateJsonValueConverter<T>()
		//	=> (new JsonValueConverter<T>(), new JsonValueComparer<T>());


	}




	public class JsonValueConverter<T> : ValueConverter<T, string>
	{
		public JsonValueConverter() 
			: base(type => JsonUtility.SerializeWithSettings(type, null), 
				  str => JsonUtility.DeserializeWithSettings<T>(str, null),
				  null)
		{

		}
	}

	public class JsonValueComparer<T> : ValueComparer<T>
	{
		public JsonValueComparer() : base(
			(A, B) => ReferenceEquals(A, B) || JsonUtility.SerializeWithSettings(A).Equals(JsonUtility.SerializeWithSettings(B)),
			t => JsonUtility.SerializeWithSettings(t).GetHashCode())
		{
		}
	}
}
