using System;
using System.Collections.Generic;
using EXILED;
using EXILED.Extensions;
using MEC;

namespace RandomGamemode
{
	public class EventHandlers
	{
		public Plugin plugin;
		public int CurrentGamemode;
		public List<KeyValuePair<int, string>> GamemodeList = new List<KeyValuePair<int, string>> {
			new KeyValuePair<int, string>( 1, "Dodgeball" ),
			new KeyValuePair<int, string>( 2, "Peanut Raid" )
		};
		Random rand = new Random();

		public EventHandlers( Plugin plugin ) => this.plugin = plugin;

		public void OnRoundStart()
		{
			if ( rand.Next( 1, 101 ) <= 100 )
			{
				ChooseGamemode();
			}
		}

		public void ChooseGamemode()
		{
			int RandomGamemode = rand.Next( 1, GamemodeList.Count + 1 );
			CurrentGamemode = RandomGamemode;
			switch ( RandomGamemode )
			{
				case 1: Timing.RunCoroutine( DodgeBall() ); break;
				//case 2: Timing.RunCoroutine( PeanutRaid() ); break;
			}
		}

		public IEnumerator<float> DodgeBall()
		{
			ServerConsole.FriendlyFire = true;
			foreach ( ReferenceHub hub in Player.GetHubs() )
			{	
				yield return Timing.WaitForSeconds( 3f );
				//hub.SetRole( RoleType.ClassD, hub.gameObject );
				hub.inventory.AddNewItem( ItemType.KeycardNTFCommander );
				for ( int i = 0; i < 7; i++ )
					hub.inventory.AddNewItem( ItemType.SCP018 );
			}
		}

		public void OnGrenadeThrown( ref GrenadeThrownEvent ev )
		{
			if ( CurrentGamemode == 1 )
				ev.Player.inventory.AddNewItem( ItemType.SCP018 );
		}

		public void OnItemDropped( ref DropItemEvent ev )
		{
			if ( CurrentGamemode == 1 )
				ev.Allow = false;
		}

		public IEnumerator<float> PeanutRaid()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( ReferenceHub hub in Player.GetHubs() )
			{
				hub.Broadcast( 6, "Peanut raid working" );
			}
		}

		public void OnRoundEnd()
		{
			if ( CurrentGamemode != 0 )
			{
				string GamemodeName = "";
				foreach ( KeyValuePair<int, string> keyValue in GamemodeList )
				{
					if ( keyValue.Key == CurrentGamemode )
					{
						GamemodeName = keyValue.Value;
					}
				}
				foreach ( ReferenceHub hub in Player.GetHubs() )
				{
					hub.Broadcast( 6, "<color=red>The " + GamemodeName + " round has ended.</color>" );
				}
				CurrentGamemode = 0;
				ServerConsole.FriendlyFire = false;
			}
		}
	}
}