using System.ComponentModel;
using Exiled.API.Interfaces;

namespace RandomGamemode
{
	public sealed class Config : IConfig
	{
		[Description( "Whether the plugin is enabled or not." )]
		public bool IsEnabled { get; set; } = true;

		[Description( "Whether the Dodgeball gamemode is enabled or not." )]
		public bool DodgeBallEnabled { get; private set; } = true;

		[Description( "Whether the Peanut Raid gamemode is enabled or not." )]
		public bool PeanutRaidEnabled { get; private set; } = true;

		[Description( "Whether the Goldfish Attacks gamemode is enabled or not." )]
		public bool GoldfishEnabled { get; private set; } = false;

		[Description( "Whether the Night of the Living Nerd gamemode is enabled or not." )]
		public bool LivingNerdEnabled { get; private set; } = true;

		[Description( "Whether the SCP-682 Containment gamemode is enabled or not." )]
		public bool SCP682ContainmentEnabled { get; private set; } = true;

		[Description( "Chance of a gamemode being activated at the start of every round. Must be a whole number." )]
		public int GamemodeChance { get; private set; } = 10;
	}
}
