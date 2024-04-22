namespace Core.Game.Components
{
	public interface IFocusableEntity<TParent> : IFocusable where TParent : IFocusable
	{
		public TParent? Parent { get; set; }
	}
}
