namespace Core.Game.Components
{
	/// <summary>
	/// The different rarity types that control spawning rates, prices, and frequencies of items,
	/// buildings, and other objects. 
	/// <br/>There is no public constructor. As such, the rarities that exist are controlled
	/// internally.
	/// </summary>
	public enum Rarity
	{
		Common,
		Uncommon,
		Unique,
		Rare,
		Unreal,
		Legendary,
		Mythic,
		Godly
	}
}
