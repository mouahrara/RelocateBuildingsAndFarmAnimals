using HarmonyLib;
using StardewValley;
using StardewValley.Menus;
using RelocateBuildingsAndFarmAnimals.Utilities;

namespace RelocateBuildingsAndFarmAnimals.Patches
{
	internal class OptionsPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.PropertyGetter(typeof(Options), nameof(Options.zoomLevel)),
				postfix: new HarmonyMethod(typeof(OptionsPatch), nameof(ZoomLevelPostfix))
			);
		}

		private static void ZoomLevelPostfix(ref float __result)
		{
			if (Game1.activeClickableMenu is not DialogueBox || !PagedResponsesMenuUtility.IsPagedResponsesMenu)
				return;

			__result = PagedResponsesMenuUtility.ZoomLevel;
		}
	}
}
