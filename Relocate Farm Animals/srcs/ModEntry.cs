using System;
using HarmonyLib;
using StardewModdingAPI;
using RelocateFarmAnimals.Handlers;
using RelocateFarmAnimals.Patches;

namespace RelocateFarmAnimals
{
	/// <summary>The mod entry point.</summary>
	internal sealed class ModEntry : Mod
	{
		// Shared static helpers
		internal static new IModHelper	Helper { get; private set; }
		internal static new IMonitor	Monitor { get; private set; }
		internal static new IManifest	ModManifest { get; private set; }

		public override void Entry(IModHelper helper)
		{
			Helper = base.Helper;
			Monitor = base.Monitor;
			ModManifest = base.ModManifest;

			// Load Harmony patches
			try
			{
				var harmony = new Harmony(ModManifest.UniqueID);

				// Apply menu patches
				AnimalQueryMenuPatch.Apply(harmony);
				CarpenterMenuPatch.Apply(harmony);
				DialogueBoxPatch.Apply(harmony);

				// Apply location patches
				GameLocationPatch.Apply(harmony);

				// Apply Game1 patches
				OptionsPatch.Apply(harmony);
			}
			catch (Exception e)
			{
				Monitor.Log($"Issue with Harmony patching: {e}", LogLevel.Error);
				return;
			}

			// Subscribe to events
			Helper.Events.Input.ButtonPressed += ButtonPressedHandler.Apply;
		}
	}
}
