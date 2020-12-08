using Exiled.API.Enums;
using Exiled.API.Features;
using System.Collections.Generic;
using events = Exiled.Events.Handlers;

namespace RandomGamemode
{
	public class Plugin : Plugin<Config>
	{
		private EventHandlers EventHandlers;

		public override PluginPriority Priority { get; } = PluginPriority.Medium;
		public static List<int> EnabledList = new List<int>();

		public override void OnEnabled()
		{
			base.OnEnabled();
			EventHandlers = new EventHandlers( this );
			events.Server.RoundStarted += EventHandlers.OnRoundStart;
			events.Server.RoundEnded += EventHandlers.OnRoundEnd;
			events.Player.ThrowingGrenade += EventHandlers.OnGrenadeThrown;
			events.Map.ExplodingGrenade += EventHandlers.OnGrenadeExplode;
			events.Player.DroppingItem += EventHandlers.OnItemDropped;
			events.Player.Joined += EventHandlers.OnPlayerJoin;

			if ( Config.DodgeBallEnabled ) // This ensures that the chances of a gamemode being selected are still the same, even if some are disabled
				EnabledList.Add( 1 );

			if ( Config.PeanutRaidEnabled )
				EnabledList.Add( 2 );

			if ( Config.GoldfishEnabled )
				EnabledList.Add( 3 );

			if ( Config.LivingNerdEnabled )
				EnabledList.Add( 4 );

			if ( Config.SCP682ContainmentEnabled )
				EnabledList.Add( 5 );

			Log.Info( $"Successfully loaded." );
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			events.Server.RoundStarted -= EventHandlers.OnRoundStart;
			events.Server.RoundEnded -= EventHandlers.OnRoundEnd;
			events.Player.ThrowingGrenade -= EventHandlers.OnGrenadeThrown;
			events.Map.ExplodingGrenade -= EventHandlers.OnGrenadeExplode;
			events.Player.DroppingItem -= EventHandlers.OnItemDropped;
			events.Player.Joined -= EventHandlers.OnPlayerJoin;
			EventHandlers = null;
		}
	}
}
