using System;
using System.Collections.Generic;
using System.Linq;
using EXILED;
using EXILED.ApiObjects;
using EXILED.Extensions;
using MEC;

namespace RandomGamemode
{
	public class EventHandlers
	{
		public Plugin plugin;
		public int CurrentGamemode;
		Random rand = new Random();

		public EventHandlers( Plugin plugin ) => this.plugin = plugin;

		public string GetGamemodeName()
		{
			switch( CurrentGamemode )
			{
				case 1: return "Dodgeball";
				case 2: return "Peanut Raid";
				case 3: return "Goldfish Attacks"; // There's only like 10 people who might get this reference but I'm still adding it for the memes
				case 4: return "Night of the Living Nerd";
				case 5: return "SCP-682 Containment";
				default: return "Invalid Gamemode";
			}
		}

		public void OnRoundStart()
		{
			if ( rand.Next( 1, 101 ) <= 10 )
				ChooseGamemode();
		}

		public void ChooseGamemode()
		{
			int RandomGamemode = rand.Next( 1, 6 );
			CurrentGamemode = RandomGamemode;
			switch ( RandomGamemode )
			{
				case 1: Timing.RunCoroutine( DodgeBall() ); break;
				case 2: Timing.RunCoroutine( PeanutRaid() ); break;
				case 3: Timing.RunCoroutine( GoldfishAttacks() ); break;
				case 4: Timing.RunCoroutine( NightOfTheLivingNerd() ); break;
				case 5: Timing.RunCoroutine( SCP682Containment() ); break;
				//default: Timing.RunCoroutine( SCP682Containment() ); break; // Used for debugging a single gamemode
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
			int RandPly = rand.Next( 0, PlyList.Count );
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

		public IEnumerator<float> NightOfTheLivingNerd()
		{
			List<ReferenceHub> PlyList = new List<ReferenceHub>();
			yield return Timing.WaitForSeconds( 3f );
			foreach ( ReferenceHub hub in Player.GetHubs() )
			{
				PlyList.Add( hub );
				hub.Broadcast( 6, "<color=red>The Night of the Living Nerd round has started!</color>" );
			}
			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count );
			ReferenceHub SelectedNerd = PlyList.ElementAt( RandPly );
			SelectedNerd.characterClassManager.SetPlayersClass( RoleType.Scientist, SelectedNerd.gameObject );
			SelectedNerd.inventory.AddNewItem( ItemType.GunLogicer );
			SelectedNerd.inventory.AddNewItem( ItemType.Flashlight );
			SelectedNerd.SetAmmo( AmmoType.Dropped7, 1000 );
			PlyList.RemoveAt( RandPly );
			Map.TurnOffAllLights( 5000 );
			foreach ( ReferenceHub hub in PlyList )
			{
				hub.characterClassManager.SetPlayersClass( RoleType.ClassD, hub.gameObject );
				hub.inventory.AddNewItem( ItemType.Flashlight );
			}
		}

		public IEnumerator<float> SCP682Containment()
		{
			List<ReferenceHub> PlyList = new List<ReferenceHub>();
			yield return Timing.WaitForSeconds( 3f );
			if ( Player.GetHubs().Count() < 3 ) // The round ends too early if there's only 2 players
			{
				CurrentGamemode = 0;
				yield break;
			}
			foreach ( ReferenceHub hub in Player.GetHubs() )
			{
				PlyList.Add( hub );
				hub.Broadcast( 6, "<color=red>The SCP-682 Containment round has started!</color>" );
			}
			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count );
			ReferenceHub Selected682 = PlyList.ElementAt( RandPly );
			Selected682.characterClassManager.SetPlayersClass( RoleType.Scp93953, Selected682.gameObject );
			yield return Timing.WaitForSeconds( 3f );
			Selected682.SetPosition( Map.GetRandomSpawnPoint( RoleType.ChaosInsurgency ) );
			Selected682.SetScale( 2f );
			Selected682.SetHealth( 5000 );
			PlyList.RemoveAt( RandPly );
			Map.StartNuke();
			foreach ( ReferenceHub hub in PlyList )
			{
				hub.characterClassManager.SetPlayersClass( RoleType.NtfCommander, hub.gameObject );
				hub.SetAmmo( AmmoType.Dropped5, 1000 );
			}
		}

		public void OnRoundEnd()
		{
			if ( CurrentGamemode > 0 )
			{
				foreach ( ReferenceHub hub in Player.GetHubs() )
				{
					hub.Broadcast( 6, "<color=red>The " + GetGamemodeName() + " round has ended.</color>" );
				}
				CurrentGamemode = 0;
				ServerConsole.FriendlyFire = false;
			}
		}
	}
}