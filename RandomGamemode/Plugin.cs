using System;
using EXILED;

namespace RandomGamemode
{
	public class Plugin : EXILED.Plugin
	{
		public EventHandlers EventHandlers;

		public override void OnEnable()
		{
			EventHandlers = new EventHandlers( this );
			Events.RoundStartEvent += EventHandlers.OnRoundStart;
			Events.RoundEndEvent += EventHandlers.OnRoundEnd;
			Events.GrenadeThrownEvent += EventHandlers.OnGrenadeThrown;
			Log.Info( "Successfully loaded." );
		}

		public override void OnDisable()
		{
			Events.RoundStartEvent -= EventHandlers.OnRoundStart;
			Events.RoundEndEvent -= EventHandlers.OnRoundEnd;
			Events.GrenadeThrownEvent -= EventHandlers.OnGrenadeThrown;
			EventHandlers = null;
		}

		public override void OnReload(){}

		public override string getName { get; } = "Random Gamemode";
	}
}