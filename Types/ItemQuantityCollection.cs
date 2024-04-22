using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Core.Attributes;
using Core.Game.Components;
using System.Collections;
using System.Collections.ObjectModel;
using System.Text;

namespace Core.Types
{
	public class ItemQuantityCollectionJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType == typeof(ItemQuantityCollection);

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			string? value = reader.Value as string;
			
			if (value is null) return null;

			return ItemQuantityCollection.Parse(value);
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (value is not ItemQuantityCollection collection)
			{
				writer.WriteNull();
				return;
			}

			writer.WriteValue(collection.ToString());
		}
	}

	public class ItemQuantityCollectionValueConverter : ValueConverter<ItemQuantityCollection, string>
	{
		public ItemQuantityCollectionValueConverter()
		: base(collection => collection.ToString(),
			  asString => ItemQuantityCollection.Parse(asString))
		{ }
	}

	public class ItemQuantityCollectionValueComparer : ValueComparer<ItemQuantityCollection>
	{
		public ItemQuantityCollectionValueComparer() : base(
			(A, B) => ReferenceEquals(A, B) || A.SequenceEqual(B), 
			A => A.ToString().GetHashCode())
		{
		}
	}

	[JsonConverter(typeof(ItemQuantityCollectionJsonConverter))]
	[RegisterValueConverter(typeof(ItemQuantityCollectionValueConverter), typeof(ItemQuantityCollectionValueComparer))]
	public class ItemQuantityCollection : IEnumerable<ItemQuantity>
	{
		private List<ItemQuantity> items { get; } = new();

		public IEnumerator<ItemQuantity> GetEnumerator() => items.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();


		public ExpoNumber this[int index]
		{
			get => items[index].Quantity;
			set => items[index].Quantity = value;
		}

		public ExpoNumber this[string item]
		{
			get => items.Find(i => i.Item == item)?.Quantity ?? throw new KeyNotFoundException("Key " + item + " not found");
			set
			{
				var find = items.Find(i => i.Item == item);
				if (find is not null)
				{
					find.Quantity = value;
				}
				else
				{
					var newItem = new ItemQuantity(item, value);
					items.Add(newItem);
				}
			}
		}

		public ExpoNumber this[Resource resource]
		{
			get => this[resource.Id];
			set => this[resource.Id] = value;
		}

		public ReadOnlyCollection<ItemQuantity> AsReadOnly() => items.AsReadOnly();

		public ItemQuantityCollection Add(ItemQuantity itemQuantity)
		{
			items.Add(itemQuantity);
			return this;
		}

		public ItemQuantityCollection Add(string item, ExpoNumber? quantity = null) 
			=> Add(new ItemQuantity(item, quantity));

		public ItemQuantityCollection Add(Resource resource, ExpoNumber? quantity = null)
			=> Add(new ItemQuantity(resource.Id, quantity));




		public ItemQuantityCollection Remove(string item)
		{
			int index = items.FindIndex(i => i.Equals(item));
			if (index >= 0) items.RemoveAt(index);

			return this;
		}

		public ItemQuantityCollection Remove(Resource resource)
			=> Remove(resource.Id);

		public ItemQuantityCollection Remove(ItemQuantity itemQuantity)
		{
			items.Remove(itemQuantity);
			return this;
		}

		public ItemQuantityCollection RemoveAt(int index)
		{
			items.RemoveAt(index);
			return this;
		}

		public override string ToString()
		{
			StringBuilder builder = new();

			for (int i = 0; i < items.Count; i++)
			{
				ItemQuantity item = items[i];

				builder.Append(item.ToString());
				if (i < items.Count - 1)
					builder.Append(';');
			}

			return builder.ToString();
		}


		public static ItemQuantityCollection Parse(string input)
		{
			if (input == "") return new();

			string[] items = input.Split(';');

			ItemQuantityCollection collection = new();
			foreach(string itemPair in items)
			{
				if (itemPair == "") continue;

				collection.Add(ItemQuantity.Parse(itemPair));
			}


			return collection;
		}


		public bool ValidateItems(Func<string, bool> validater, bool autoRemove = false)
		{
			bool allIsValid = true;
			List<ItemQuantity> toRemove = new();

			foreach(var item in items)
			{
				bool isValid = validater.Invoke(item.Item);
				if(!isValid) toRemove.Add(item);

				allIsValid = allIsValid && isValid;
			}

			if (autoRemove) toRemove.ForEach(i => Remove(i));

			return allIsValid;
		}

		public bool HasItem(string item) => items.Find(i => i.Item == item) != null;
		public bool HasItem(Resource resource) => HasItem(resource.Id);

	}




	public class ItemQuantityJsonConverter : JsonConverter
	{
		public override bool CanConvert(Type objectType) => objectType == typeof(ItemQuantity);

		public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
		{
			string? value = reader.Value as string;
			if (value is null) return null;

			return ItemQuantity.Parse(value);
		}

		public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
		{
			if (value is not ItemQuantity item)
			{
				writer.WriteNull();
				return;
			}
				
			writer.WriteValue(item.ToString());
		}

	}

	public class ItemQuantityValueConverter : ValueConverter<ItemQuantity, string>
	{
		public ItemQuantityValueConverter()
		: base(item => item.ToString(),
			  asString => ItemQuantity.Parse(asString))
		{ }
	}

	public class ItemQuantityValueComparer : ValueComparer<ItemQuantity>
	{
		public ItemQuantityValueComparer() : base((A, B) => A.Equals(B), A => A.GetHashCode())
		{

		}
	}

	[JsonConverter(typeof(ItemQuantityJsonConverter))]
	[RegisterValueConverter(typeof(ItemQuantityValueConverter), typeof(ItemQuantityValueComparer))]
	public class ItemQuantity(string item, ExpoNumber? quantity = null)
	{
		[JsonProperty("item")]
		public string Item { get; } = item;


		[JsonProperty("quantity")]
		public ExpoNumber Quantity { get; set; } = quantity ?? 0;


		public override string ToString()
		{
			return $"{Item}:{Quantity.ToString()}";
		}

		public override bool Equals(object? obj)
		{
			if (obj is not ItemQuantity iq) return false;
			return Item == iq.Item && Quantity == iq.Quantity;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Item, Quantity);
		}


		public static ItemQuantity Parse(string data)
		{
			string[] split = data.Split(':');
			if (split.Length != 2) throw new InvalidOperationException("Could not parse into item quantity: " + data);

			string item = split[0];
			string quantityStr = split[1];

			ExpoNumber quantity = ExpoNumber.Parse(quantityStr);

			return new(item, quantity);
		} 
	}

}
