using System.ComponentModel;
using Exiled.API.Interfaces;

namespace RandomGamemode
{
	public sealed class Config : IConfig
	{
		[Description( "Whether the plugin is enabled or not." )]
		public bool IsEnabled { get; set; } = true;

		[Description( "Whether or not debug messages should be shown in the console." )]
		public bool Debug { get; set; } = false;

		[Description( "Chance of a gamemode being activated at the start of every round. Must be a whole number." )]
		public int GamemodeChance { get; private set; } = 10;

		[Description( "Enable/disable the Dodgeball gamemode." )]
		public bool DodgeBallEnabled { get; private set; } = true;

		[Description( "Enable/disable the Peanut Raid gamemode." )]
		public bool PeanutRaidEnabled { get; private set; } = true;

		[Description( "Enable/disable the Blue Screen of Death gamemode." )]
		public bool BlueScreenOfDeathEnabled { get; private set; } = true;

		[Description( "Enable/disable the Night of the Living Nerd gamemode." )]
		public bool LivingNerdEnabled { get; private set; } = true;

		[Description( "Enable/disable the Randomizer gamemode." )]
		public bool RandomizerEnabled { get; private set; } = true;

		[Description( "Enable/disable the Annoying Mimicry gamemode." )]
		public bool AnnoyingMimicryEnabled { get; private set; } = true;
	}
}
