using System;
using System.Collections.Generic;
using System.Linq;
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
			new KeyValuePair<int, string>( 2, "Peanut Raid" ),
			new KeyValuePair<int, string>( 3, "Goldfish Attacks" )
		};
		Random rand = new Random();

		public EventHandlers( Plugin plugin ) => this.plugin = plugin;

		public void OnRoundStart()
		{
			if ( rand.Next( 1, 101 ) <= 10 )
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
				case 2: Timing.RunCoroutine( PeanutRaid() ); break;
				case 3: Timing.RunCoroutine( GoldfishAttacks() ); break;
			}
		}

		public IEnumerator<float> DodgeBall()
		{
			ServerConsole.FriendlyFire = true;
			foreach ( ReferenceHub hub in Player.GetHubs() )
			{	
				yield return Timing.WaitForSeconds( 3f );
				if ( hub.IsScp() )
					hub.characterClassManager.SetPlayersClass( RoleType.FacilityGuard, hub.gameObject );
				hub.inventory.AddNewItem( ItemType.KeycardNTFCommander );
				for ( int i = 0; i < 7; i++ )
					hub.inventory.AddNewItem( ItemType.SCP018 );
				hub.Broadcast( 6, "<color=red>The Dodgeball round has started!</color>" );
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
			List<ReferenceHub> PlyList = new List<ReferenceHub>();
			yield return Timing.WaitForSeconds( 3f );
			foreach ( ReferenceHub hub in Player.GetHubs() )
			{
				PlyList.Add( hub );
				hub.Broadcast( 6, "<color=red>The Peanut Raid round has started!</color>" );
			}
			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count - 1 );
			ReferenceHub SelectedDBoi = PlyList.ElementAt( RandPly );
			SelectedDBoi.characterClassManager.SetPlayersClass( RoleType.ClassD, SelectedDBoi.gameObject );
			SelectedDBoi.SetScale( 0.5f );
			PlyList.RemoveAt( RandPly );
			foreach ( ReferenceHub hub in PlyList )
			{
				hub.characterClassManager.SetPlayersClass( RoleType.Scp173, hub.gameObject );
			}
		}

		public void OnPlayerJoin( PlayerJoinEvent ev )
		{
			if ( CurrentGamemode == 2 )
				ev.Player.characterClassManager.SetPlayersClass( RoleType.Scp173, ev.Player.gameObject );
		}

		public IEnumerator<float> GoldfishAttacks()
		{
			yield return Timing.WaitForSeconds( 3f );
			string Name = "The Black Goldfish";
			bool ModeEnabled = false;
			foreach ( ReferenceHub hub in Player.GetHubs() )
			{
				if ( hub.GetNickname() == Name )
				{
					hub.characterClassManager.SetPlayersClass( RoleType.Scp079, hub.gameObject );
					ModeEnabled = true;
				}
				if ( ModeEnabled )
					hub.Broadcast( 6, "<color=red>The Goldfish Attacks round has started!</color>" );
				else
					CurrentGamemode = 0;
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