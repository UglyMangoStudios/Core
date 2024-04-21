namespace SpaceCore.Game.Components
{
	public interface IFocusable
	{
		public IFocusable? GetChild(string id);

		public IReadOnlyList<IFocusable> GetChildren();

		public string FocusId { get; }


		/// <summary>
		/// Returns true if this entity has at least one child
		/// </summary>
		public bool HasChildren() => GetChildren().Count > 0;

		/// <summary>
		/// Returns true if reflections is able to find a nonnull value for the Parent field for this object
		/// </summary>
		public bool HasParent() => this.GetParent() is not null;

		/// <summary>
		/// Attempts to use reflection to attempt parent retrieval 
		/// </summary>
		public IFocusable? GetParent() => this.GetType().GetProperty("Parent")?.GetValue(this) as IFocusable;


		public IFocusable[] GetParents()
		{
			List<IFocusable> parents = [];

			IFocusable? parent = this.GetParent();

			while(parent != null)
			{
				parents.Add(parent);
				parent = parent.GetParent();
			}

			return [.. parents];
		}

	}
}
