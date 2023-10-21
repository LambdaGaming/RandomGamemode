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
		public override Version Version { get; } = new Version( 2, 0, 0 );
		public override Version RequiredExiledVersion { get; } = new Version( 8, 0, 0 );
		public override PluginPriority Priority { get; } = PluginPriority.Medium;
		public static List<int> EnabledList = new List<int>();

		public override void OnEnabled()
		{
			base.OnEnabled();
			EventHandlers = new EventHandlers( this );
			events.Server.RoundStarted += EventHandlers.OnRoundStart;
			events.Server.RoundEnded += EventHandlers.OnRoundEnd;
			events.Player.ThrownProjectile += EventHandlers.OnGrenadeThrown;
			events.Player.DroppingItem += EventHandlers.OnItemDropped;
			events.Server.EndingRound += EventHandlers.OnRoundEnding;
			events.Scp079.ChangingCamera += EventHandlers.OnChangeCamera;
			events.Player.ActivatingGenerator += EventHandlers.OnGeneratorActivate;
			events.Player.StoppingGenerator += EventHandlers.OnGeneratorDeactivate;

			if ( Config.DodgeBallEnabled ) // This ensures that the chances of a gamemode being selected are still the same, even if some are disabled
				EnabledList.Add( 1 );

			if ( Config.PeanutRaidEnabled )
				EnabledList.Add( 2 );

			if ( Config.BlueScreenOfDeathEnabled )
				EnabledList.Add( 3 );

			if ( Config.LivingNerdEnabled )
				EnabledList.Add( 4 );

			if ( Config.Scp682ContainmentEnabled )
				EnabledList.Add( 5 );

			if ( Config.RandomizerEnabled )
				EnabledList.Add( 6 );

			Log.Info( "Successfully loaded." );
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			events.Server.RoundStarted -= EventHandlers.OnRoundStart;
			events.Server.RoundEnded -= EventHandlers.OnRoundEnd;
			events.Player.ThrownProjectile -= EventHandlers.OnGrenadeThrown;
			events.Player.DroppingItem -= EventHandlers.OnItemDropped;
			events.Server.EndingRound -= EventHandlers.OnRoundEnding;
			events.Scp079.ChangingCamera -= EventHandlers.OnChangeCamera;
			events.Player.ActivatingGenerator -= EventHandlers.OnGeneratorActivate;
			events.Player.StoppingGenerator -= EventHandlers.OnGeneratorDeactivate;
			EventHandlers = null;
		}
	}
}
