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
		public int GamemodeChance { get; set; } = 10;

		[Description( "Text that displays when a gamemode round starts. Use {0} for the name of the gamemode." )]
		public string StartText { get; set; } = "<color=red>The {0} round has started!</color>";

		[Description( "Text that displays when a gamemode round ends. Use {0} for the name of the gamemode." )]
		public string EndText { get; set; } = "<color=red>The {0} round has ended.</color>";


		[Description( "Enable/disable the Dodgeball gamemode." )]
		public bool DodgeBallEnabled { get; set; } = true;

		[Description( "Instructions that appear at the beginning of a Dodgeball round. Should be less than 250 characters." )]
		public string DodgeBallText { get; set; } = "Eliminate players by throwing dodgeballs at them to win!";


		[Description( "Enable/disable the Peanut Raid gamemode." )]
		public bool PeanutRaidEnabled { get; set; } = true;

		[Description( "Instructions that appear at the beginning of a Peanut Raid round. Should be less than 250 characters." )]
		public string PeanutRaidText { get; set; } = "Peanuts must stop the Class Ds before they escape and become Chaos Insurgency!";


		[Description( "Enable/disable the Blue Screen of Death gamemode." )]
		public bool BlueScreenOfDeathEnabled { get; set; } = true;

		[Description( "Instructions that appear at the beginning of a Blue Screen of Death round. Should be less than 250 characters." )]
		public string BlueScreenOfDeathText { get; set; } = "Scientists have 15 minutes to disable SCP-079!";


		[Description( "Enable/disable the Night of the Living Nerd gamemode." )]
		public bool LivingNerdEnabled { get; set; } = true;

		[Description( "Instructions that appear at the beginning of a Night of the Living Nerd round. Should be less than 250 characters." )]
		public string LivingNerdText { get; set; } = "Class Ds must defend themselves from the killer scientist!";


		[Description( "Enable/disable the Randomizer gamemode." )]
		public bool RandomizerEnabled { get; set; } = true;

		[Description( "Instructions that appear at the beginning of a Randomizer round. Should be less than 250 characters." )]
		public string RandomizerText { get; set; } = "Everyone has been given randomized roles, items, and spawn positions. Friendly fire is enabled. The last player alive wins!";


		[Description( "Enable/disable the Annoying Mimicry gamemode." )]
		public bool AnnoyingMimicryEnabled { get; set; } = true;

		[Description( "Instructions that appear at the beginning of an Annoying Mimicry round. Should be less than 250 characters." )]
		public string AnnoyingMimicryText { get; set; } = "SCP-939 can replicate by killing Class Ds. Class Ds must fight back. Escaping to surface is disabled.";


		[Description( "Enable/disable the Locked In gamemode." )]
		public bool LockedInEnabled { get; set; } = true;

		[Description( "Instructions that appear at the beginning of a Locked In round. Should be less than 250 characters." )]
		public string LockedInText { get; set; } = "Escaping to surface and light containment lockdown are disabled. MTF will spawn in entrance zone and Chaos will spawn in heavy containment.";


		[Description( "Enable/disable the Infection gamemode." )]
		public bool InfectionEnabled { get; set; } = true;

		[Description( "Instructions that appear at the beginning of an Infection round. Should be less than 250 characters." )]
		public string InfectionText { get; set; } = "Players who are killed by SCP-049 are automatically revived as a stronger SCP-049-2.";


		[Description( "Enable/disable the Living Like Larry gamemode." )]
		public bool LivingLikeLarryEnabled { get; set; } = true;

		[Description( "Instructions that appear at the beginning of a Living Like Larry round. Should be less than 250 characters." )]
		public string LivingLikeLarryText { get; set; } = "Class Ds must escape the hoard of SCP-106s and become Chaos Insurgency!";
	}
}
