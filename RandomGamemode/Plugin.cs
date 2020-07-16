using Exiled.API.Enums;
using Exiled.API.Features;
using events = Exiled.Events.Handlers;

namespace RandomGamemode
{
	public class Plugin : Plugin<Config>
	{
		private EventHandlers EventHandlers;

		public override PluginPriority Priority { get; } = PluginPriority.Medium;

		public override void OnEnabled()
		{
			base.OnEnabled();
			EventHandlers = new EventHandlers( this );
			events.Server.RoundStarted += EventHandlers.OnRoundStart;
			events.Server.RoundEnded += EventHandlers.OnRoundEnd;
			events.Player.ThrowingGrenade += EventHandlers.OnGrenadeThrown;
			events.Player.DroppingItem += EventHandlers.OnItemDropped;
			events.Player.Joined += EventHandlers.OnPlayerJoin;
			Log.Info( $"Successfully loaded." );
		}

		public override void OnDisabled()
		{
			base.OnDisabled();
			events.Server.RoundStarted -= EventHandlers.OnRoundStart;
			events.Server.RoundEnded -= EventHandlers.OnRoundEnd;
			events.Player.ThrowingGrenade -= EventHandlers.OnGrenadeThrown;
			events.Player.DroppingItem -= EventHandlers.OnItemDropped;
			events.Player.Joined -= EventHandlers.OnPlayerJoin;
			EventHandlers = null;
		}
	}
}
