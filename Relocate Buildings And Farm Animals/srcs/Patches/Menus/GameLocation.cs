using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Buildings;
using RelocateBuildingsAndFarmAnimals.Utilities;

namespace RelocateBuildingsAndFarmAnimals.Patches
{
	internal class GameLocationPatch
	{
		internal static void Apply(Harmony harmony)
		{
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.buildStructure), new Type[] { typeof(Building), typeof(Vector2), typeof(Farmer), typeof(bool) }),
				prefix: new HarmonyMethod(typeof(GameLocationPatch), nameof(BuildStructurePrefix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.buildStructure), new Type[] { typeof(Building), typeof(Vector2), typeof(Farmer), typeof(bool) }),
				postfix: new HarmonyMethod(typeof(GameLocationPatch), nameof(BuildStructurePostfix))
			);
			harmony.Patch(
				original: AccessTools.Method(typeof(GameLocation), nameof(GameLocation.AddDefaultBuilding), new Type[] { typeof(string), typeof(Vector2), typeof(bool) }),
				prefix: new HarmonyMethod(typeof(GameLocationPatch), nameof(AddDefaultBuildingPrefix))
			);
		}

		private static void BuildStructurePrefix(GameLocation __instance, Building building)
		{
			if (Game1.activeClickableMenu is not CarpenterMenu || CarpenterMenuUtility.MainTargetLocation is null)
				return;

			CarpenterMenuUtility.MainTargetLocation.buildings.Remove(building);
			__instance.buildings.Add(building);
		}

		private static void BuildStructurePostfix(GameLocation __instance, Building building, bool __result)
		{
			if (Game1.activeClickableMenu is not CarpenterMenu carpenterMenu || CarpenterMenuUtility.MainTargetLocation is null)
				return;

			if (!__result)
			{
				__instance.buildings.Remove(building);
				CarpenterMenuUtility.MainTargetLocation.buildings.Add(building);
			}
			else
			{
				Farm farm = Game1.getFarm();
				GameLocation indoors = building.GetIndoors();
				string key = $"{ModEntry.ModManifest.UniqueID}_IsDefaultGreenhouse";

				if (indoors is not null)
				{
					foreach (Warp warp in indoors.warps)
					{
						warp.TargetName = carpenterMenu.TargetLocation.NameOrUniqueName;
					}
					building.updateInteriorWarps();
					foreach (FarmAnimal animal in CarpenterMenuUtility.MainTargetLocation.animals.Values)
					{
						if (animal.home == building)
						{
							animal.warpHome();
						}
					}
				}
				if (building is GreenhouseBuilding)
				{
					if (CarpenterMenuUtility.MainTargetLocation == farm)
					{
						if (!building.modData.ContainsKey(key))
						{
							building.modData.Add(key, "T");
						}
					}
					if (carpenterMenu.TargetLocation == farm)
					{
						if (building.modData.ContainsKey(key))
						{
							building.modData.Remove(key);
						}
					}
				}
				carpenterMenu.TargetLocation = CarpenterMenuUtility.MainTargetLocation;
				CarpenterMenuUtility.MainTargetLocation = null;
				Game1.globalFadeToBlack(carpenterMenu.setUpForBuildingPlacement, 0.04f);
			}
		}

		private static bool AddDefaultBuildingPrefix(GameLocation __instance, string id)
		{
			Farm farm = Game1.getFarm();
			string key = $"{ModEntry.ModManifest.UniqueID}_IsDefaultGreenhouse";

			if (id == "Greenhouse" && __instance == farm)
			{
				foreach (GameLocation location in Game1.locations)
				{
					if (location != farm && location.IsBuildableLocation())
					{
						foreach (Building building in location.buildings)
						{
							if (building.modData.ContainsKey(key))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}
	}
}
