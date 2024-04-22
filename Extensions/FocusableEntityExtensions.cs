using Core.Game.Components;

namespace Core.Extensions
{
	internal static class FocusableEntityExtensions
	{
		internal static void AssignSelfToChildren(this IFocusable focusable)
		{
			foreach(var child in focusable.GetChildren())
			{
				child.GetType().GetProperty("Parent")?.SetValue(child, focusable);
			}
		}
	}
}
