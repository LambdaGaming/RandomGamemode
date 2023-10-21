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

		[Description( "Whether the Dodgeball gamemode is enabled or not." )]
		public bool DodgeBallEnabled { get; private set; } = true;

		[Description( "Whether the Peanut Raid gamemode is enabled or not." )]
		public bool PeanutRaidEnabled { get; private set; } = true;

		[Description( "Whether the Blue Screen of Death gamemode is enabled or not." )]
		public bool BlueScreenOfDeathEnabled { get; private set; } = true;

		[Description( "Whether the Night of the Living Nerd gamemode is enabled or not." )]
		public bool LivingNerdEnabled { get; private set; } = true;

		[Description( "Whether the Randomizer gamemode is enabled or not." )]
		public bool RandomizerEnabled { get; private set; } = true;

		[Description( "Amount of ammo the scientist should get for the Night of the Living Nerd gamemode." )]
		public ushort NerdAmmoAmount { get; private set; } = 1000;
	}
}
