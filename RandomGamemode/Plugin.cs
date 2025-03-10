using Exiled.API.Enums;
using Exiled.API.Features;
using System;
using System.Collections.Generic;
using events = Exiled.Events.Handlers;

namespace RandomGamemode
{
	public class Plugin : Plugin<Config>
	{
		private EventHandlers EventHandlers;
		public override Version Version { get; } = new Version( 2, 7, 0 );
		public override Version RequiredExiledVersion { get; } = new Version( 9, 4, 0 );
		public override string Author { get; } = "OPGman";
		public override PluginPriority Priority { get; } = PluginPriority.Medium;

		internal static List<Gamemode> EnabledList = new List<Gamemode>();
		internal static Gamemode NextGamemode = Gamemode.Invalid;

		internal static string GetGamemodeName( Gamemode gm )
		{
			switch ( gm )
			{
				case Gamemode.Dodgeball: return "Dodgeball";
				case Gamemode.PeanutRaid: return "Peanut Raid";
				case Gamemode.BlueScreenOfDeath: return "Blue Screen of Death";
				case Gamemode.NightOfTheLivingNerd: return "Night of the Living Nerd";
				case Gamemode.Randomizer: return "Randomizer";
				case Gamemode.AnnoyingMimicry: return "Annoying Mimicry";
				case Gamemode.LockedIn: return "Locked In";
				case Gamemode.Infection: return "Infection";
				case Gamemode.LivingLikeLarry: return "Living Like Larry";
				default: return "Invalid Gamemode";
			}
		}

		public override void OnEnabled()
		{
			base.OnEnabled();
			EventHandlers = new EventHandlers( this );
			events.Server.RoundStarted += EventHandlers.OnRoundStart;
			events.Server.RoundEnded += EventHandlers.OnRoundEnd;
			events.Player.ThrownProjectile += EventHandlers.OnGrenadeThrown;
			events.Player.DroppingItem += EventHandlers.OnItemDropped;
			events.Server.EndingRound += EventHandlers.OnRoundEnding;
			events.Player.StoppingGenerator += EventHandlers.OnGeneratorDeactivate;
			events.Server.RespawningTeam += EventHandlers.OnRespawn;
			events.Player.Died += EventHandlers.OnPlayerDied;
			events.Player.InteractingDoor += EventHandlers.OnDoorUse;
			events.Map.Decontaminating += EventHandlers.OnDecon;

			// This ensures that the chances of a gamemode being selected are still the same, even if some are disabled
			if ( Config.DodgeBallEnabled )
				EnabledList.Add( Gamemode.Dodgeball );
			if ( Config.PeanutRaidEnabled )
				EnabledList.Add( Gamemode.PeanutRaid );
			if ( Config.BlueScreenOfDeathEnabled )
				EnabledList.Add( Gamemode.BlueScreenOfDeath );
			if ( Config.LivingNerdEnabled )
				EnabledList.Add( Gamemode.NightOfTheLivingNerd );
			if ( Config.RandomizerEnabled )
				EnabledList.Add( Gamemode.Randomizer );
			if ( Config.AnnoyingMimicryEnabled )
				EnabledList.Add( Gamemode.AnnoyingMimicry );
			if ( Config.LockedInEnabled )
				EnabledList.Add( Gamemode.LockedIn );
			if ( Config.InfectionEnabled )
				EnabledList.Add( Gamemode.Infection );
			if ( Config.LivingLikeLarryEnabled )
				EnabledList.Add( Gamemode.LivingLikeLarry );
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			events.Server.RoundStarted -= EventHandlers.OnRoundStart;
			events.Server.RoundEnded -= EventHandlers.OnRoundEnd;
			events.Player.ThrownProjectile -= EventHandlers.OnGrenadeThrown;
			events.Player.DroppingItem -= EventHandlers.OnItemDropped;
			events.Server.EndingRound -= EventHandlers.OnRoundEnding;
			events.Player.StoppingGenerator -= EventHandlers.OnGeneratorDeactivate;
			events.Server.RespawningTeam -= EventHandlers.OnRespawn;
			events.Player.Died -= EventHandlers.OnPlayerDied;
			events.Player.InteractingDoor -= EventHandlers.OnDoorUse;
			events.Map.Decontaminating -= EventHandlers.OnDecon;
			EventHandlers = null;
		}
	}
}
