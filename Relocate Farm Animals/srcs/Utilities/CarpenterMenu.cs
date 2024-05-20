using StardewModdingAPI.Utilities;
using StardewValley;

namespace RelocateFarmAnimals.Utilities
{
	internal class CarpenterMenuUtility
	{
		private static readonly PerScreen<GameLocation> mainTargetLocation = new();

		public static GameLocation MainTargetLocation
		{
			get => mainTargetLocation.Value;
			set => mainTargetLocation.Value = value;
		}
	}
}
