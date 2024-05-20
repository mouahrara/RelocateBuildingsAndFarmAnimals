using HarmonyLib;
using StardewValley.Menus;
using RelocateFarmAnimals.Utilities;

namespace RelocateFarmAnimals.Patches
{
	internal class DialogueBoxPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(DialogueBox), nameof(DialogueBox.closeDialogue)),
				postfix: new HarmonyMethod(typeof(PagedResponsesMenuUtility), nameof(PagedResponsesMenuUtility.AfterClose))
			);
		}
	}
}
