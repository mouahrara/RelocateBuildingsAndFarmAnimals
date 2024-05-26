using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using RelocateBuildingsAndFarmAnimals.Utilities;

namespace RelocateBuildingsAndFarmAnimals.Handlers
{
	internal static class ButtonPressedHandler
	{
		internal static bool ReloadContent = false;

		/// <inheritdoc cref="IInputEvents.ButtonPressed"/>
		/// <param name="sender">The event sender.</param>
		/// <param name="e">The event data.</param>
		internal static void Apply(object sender, ButtonPressedEventArgs e)
		{
			if (!Context.IsWorldReady)
				return;

			if (Game1.options.menuButton.Any((menuButton) => menuButton.ToSButton().Equals(e.Button)))
			{
				PagedResponsesMenuUtility.ReceiveMenuButtonKeyPress(e.Button);
			}
		}
	}
}
