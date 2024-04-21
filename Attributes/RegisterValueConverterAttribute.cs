namespace SpaceCore.Attributes
{

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class RegisterValueConverterAttribute : Attribute
	{

		public Type ValueConverter { get; }
		public Type? ValueComparer { get; }


		public RegisterValueConverterAttribute(Type valueConverter, Type? valueComparer = null)
		{
			ValueConverter = valueConverter;
			ValueComparer = valueComparer;
		}

	}
}
