using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomGamemode
{
	public class EventHandlers
	{
		private Plugin plugin;
		public int CurrentGamemode;
		Random rand = new Random();

		public EventHandlers( Plugin plugin ) => this.plugin = plugin;

		public string GetGamemodeName()
		{
			switch ( CurrentGamemode )
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

		public bool IsSCP( Player ply )
		{
			RoleType[] SCPs = {
				RoleType.Scp049,
				RoleType.Scp0492,
				RoleType.Scp079,
				RoleType.Scp096,
				RoleType.Scp106,
				RoleType.Scp173,
				RoleType.Scp93953,
				RoleType.Scp93989
			};
			foreach ( RoleType role in SCPs )
			{
				if ( ply.Role == role )
					return true;
			}
			return false;
		}

		public IEnumerator<float> DodgeBall()
		{
			if ( !plugin.Config.DodgeBallEnabled ) yield break;
			ServerConsole.FriendlyFire = true;
			foreach ( Player ply in Player.List )
			{
				yield return Timing.WaitForSeconds( 3f );
				if ( IsSCP( ply ) )
					ply.SetRole( RoleType.FacilityGuard );
				yield return Timing.WaitForSeconds( 3f );
				ply.ClearInventory();
				for ( int i = 0; i < 7; i++ )
					ply.Inventory.AddNewItem( ItemType.SCP018 );
				ply.Position = Map.GetRandomSpawnPoint( RoleType.Scp106 );
			}
			Map.Broadcast( 6, "<color=red>The Dodgeball round has started!</color>" );
		}

		public void OnGrenadeThrown( ThrowingGrenadeEventArgs ev )
		{
			if ( CurrentGamemode == 1 )
				ev.Player.Inventory.AddNewItem( ItemType.SCP018 );
		}

		public void OnItemDropped( DroppingItemEventArgs ev )
		{
			if ( CurrentGamemode == 1 )
				ev.IsAllowed = false;
		}

		public IEnumerator<float> PeanutRaid()
		{
			if ( !plugin.Config.PeanutRaidEnabled ) yield break;
			List<Player> PlyList = new List<Player>();
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
				PlyList.Add( ply );
			Map.Broadcast( 6, "<color=red>The Peanut Raid round has started!</color>" );

			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count );
			Player SelectedDBoi = PlyList[RandPly];
			SelectedDBoi.SetRole( RoleType.ClassD );
			SelectedDBoi.Scale /= 2;
			PlyList.RemoveAt( RandPly );
			foreach ( Player ply in PlyList )
				ply.SetRole( RoleType.Scp173 );
		}

		public void OnPlayerJoin( JoinedEventArgs ev )
		{
			if ( CurrentGamemode == 2 )
				ev.Player.SetRole( RoleType.Scp173 );
		}

		public IEnumerator<float> GoldfishAttacks()
		{
			if ( !plugin.Config.GoldfishEnabled ) yield break;
			yield return Timing.WaitForSeconds( 3f );
			string Name = "The Black Goldfish";
			bool ModeEnabled = false;
			foreach ( Player ply in Player.List )
			{
				if ( ply.Nickname == Name )
				{
					ply.SetRole( RoleType.Scp079 );
					ModeEnabled = true;
				}
				else
					CurrentGamemode = 0;
			}
			if ( ModeEnabled )
				Map.Broadcast( 6, "<color=red>The Goldfish Attacks round has started!</color>" );
		}

		public IEnumerator<float> NightOfTheLivingNerd()
		{
			if ( !plugin.Config.LivingNerdEnabled ) yield break;
			List<Player> PlyList = new List<Player>();
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
				PlyList.Add( ply );
			Map.Broadcast( 6, "<color=red>The Night of the Living Nerd round has started!</color>" );

			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count );
			Player SelectedNerd = PlyList[RandPly];
			SelectedNerd.SetRole( RoleType.Scientist );
			SelectedNerd.Inventory.AddNewItem( ItemType.GunLogicer );
			SelectedNerd.Inventory.AddNewItem( ItemType.Flashlight );
			SelectedNerd.SetAmmo( AmmoType.Nato762, 1000 );
			PlyList.RemoveAt( RandPly );
			Map.TurnOffAllLights( 5000 );
			foreach ( Player ply in PlyList )
			{
				ply.SetRole( RoleType.ClassD );
				ply.Inventory.AddNewItem( ItemType.Flashlight );
			}
		}

		public IEnumerator<float> SCP682Containment()
		{
			if ( !plugin.Config.SCP682ContainmentEnabled ) yield break;
			List<Player> PlyList = new List<Player>();
			yield return Timing.WaitForSeconds( 3f );
			if ( Player.List.Count() < 3 ) // The round ends too early if there's only 2 players
			{
				CurrentGamemode = 0;
				yield break;
			}
			foreach ( Player ply in Player.List )
				PlyList.Add( ply );
			Map.Broadcast( 6, "<color=red>The SCP-682 Containment round has started!</color>" );

			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count );
			Player Selected682 = PlyList[RandPly];
			Selected682.SetRole( RoleType.Scp93953 );
			yield return Timing.WaitForSeconds( 3f );
			Selected682.Position = Map.GetRandomSpawnPoint( RoleType.ChaosInsurgency );
			Selected682.Scale *= 2;
			Selected682.Health = 8000;
			PlyList.RemoveAt( RandPly );
			Warhead.Start();
			foreach ( Player ply in PlyList )
			{
				ply.SetRole( RoleType.NtfCommander );
				ply.SetAmmo( AmmoType.Nato556, 1000 );
			}
		}

		public void OnRoundEnd( RoundEndedEventArgs ev )
		{
			if ( CurrentGamemode > 0 )
			{
				Map.Broadcast( 6, "<color=red>The " + GetGamemodeName() + " round has ended.</color>" );
				CurrentGamemode = 0;
				ServerConsole.FriendlyFire = false;
			}
		}
	}
}
