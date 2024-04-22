using Newtonsoft.Json;

namespace Core.Extensions
{
	public class ServiceJsonConverter<T> : JsonConverter
	{

		private Func<string, T?> _service { get; }

		private Func<T, string> _toString { get; }

		public ServiceJsonConverter(Func<string, T?> service, Func<T, string> toString)
		{
			_service = service;
			_toString = toString;
		}

		public override bool CanConvert(Type objectType) => objectType == typeof(T);

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			string? id = reader.ReadAsString();
			if (id is null) return null;

			return _service.Invoke(id);
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (value is not T t)
			{
				writer.WriteNull();
				return;
			}

			writer.WriteValue(_toString.Invoke(t));
		}
	}
}
