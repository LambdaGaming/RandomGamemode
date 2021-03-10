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

		[Description( "Health that SCP-682 starts with for the SCP-682 Containment gamemode." )]
		public int SCP682Health { get; private set; } = 5000;

		[Description( "Amount of ammo MTF units should get for the SCP-682 Containment gamemode." )]
		public uint SCP682MTFAmmo { get; private set; } = 1000;

		[Description( "Max amount of dodgeballs that can be active in the world at once. Setting this too high will cause the server to hang." )]
		public int MaxDodgeballs { get; private set; } = 20;

		[Description( "Amount of ammo the scientist should get for the Night of the Living Nerd gamemode." )]
		public uint NerdAmmoAmount { get; private set; } = 1000;
	}
}
