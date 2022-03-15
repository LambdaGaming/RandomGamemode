using Exiled.API.Extensions;
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
		private bool FriendlyFireDefault;
		private int TotalBalls = 0;
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
			if ( rand.Next( 1, 101 ) <= plugin.Config.GamemodeChance )
			{
				ChooseGamemode();
			}
		}

		public void ChooseGamemode()
		{
			int RandomGamemode = Plugin.EnabledList[rand.Next( 0, Plugin.EnabledList.Count )];
			CurrentGamemode = RandomGamemode;
			switch ( RandomGamemode )
			{
				case 1: Timing.RunCoroutine( DodgeBall() ); break;
				case 2: Timing.RunCoroutine( PeanutRaid() ); break;
				case 3: Timing.RunCoroutine( GoldfishAttacks() ); break;
				case 4: Timing.RunCoroutine( NightOfTheLivingNerd() ); break;
				case 5: Timing.RunCoroutine( SCP682Containment() ); break;
			}
		}

		public IEnumerator<float> DodgeBall()
		{
			if ( !plugin.Config.DodgeBallEnabled ) yield break;

			FriendlyFireDefault = ServerConsole.FriendlyFire;
			ServerConsole.FriendlyFire = true;
			yield return Timing.WaitForSeconds( 3f );

			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.SetRole( RoleType.FacilityGuard );
				}

				yield return Timing.WaitForSeconds( 0.5f );
				ply.ClearInventory();

				for ( int i = 0; i < 7; i++ )
				{
					ply.AddItem( ItemType.SCP018 );
				}

				ply.Position = RoleExtensions.GetRandomSpawnProperties( RoleType.Scp106 ).Item1;
			}

			Map.Broadcast( 6, "<color=red>The Dodgeball round has started!</color>" );
		}

		public void OnGrenadeThrown( ThrowingItemEventArgs ev )
		{
			if ( CurrentGamemode == 1 )
			{
				if ( TotalBalls >= plugin.Config.MaxDodgeballs )
				{
					ev.IsAllowed = false;
				}

				if ( ev.IsAllowed )
				{
					ev.Player.AddItem( ItemType.SCP018 );
					TotalBalls++;
				}
			}
		}

		public void OnGrenadeExplode( ExplodingGrenadeEventArgs ev )
		{
			if ( CurrentGamemode == 1 )
			{
				TotalBalls--;
			}
		}

		public void OnItemDropped( DroppingItemEventArgs ev )
		{
			if ( CurrentGamemode == 1 )
			{
				ev.IsAllowed = false;
			}
		}

		public IEnumerator<float> PeanutRaid()
		{
			if ( !plugin.Config.PeanutRaidEnabled ) yield break;

			List<Player> PlyList = new List<Player>();
			yield return Timing.WaitForSeconds( 3f );

			foreach ( Player ply in Player.List )
			{
				PlyList.Add( ply );
			}

			Map.Broadcast( 6, "<color=red>The Peanut Raid round has started!</color>" );
			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count );
			Player SelectedDBoi = PlyList[RandPly];
			SelectedDBoi.SetRole( RoleType.ClassD );
			PlyList.RemoveAt( RandPly );

			foreach ( Player ply in PlyList )
			{
				ply.SetRole( RoleType.Scp173 );
			}

			yield return Timing.WaitForSeconds( 0.5f );
			SelectedDBoi.Scale /= 2;
		}

		public void OnPlayerJoin( JoinedEventArgs ev )
		{
			if ( CurrentGamemode == 2 )
			{
				ev.Player.SetRole( RoleType.Scp173 );
			}
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
				{
					CurrentGamemode = 0;
				}
			}

			if ( ModeEnabled )
			{
				Map.Broadcast( 6, "<color=red>The Goldfish Attacks round has started!</color>" );
			}
		}

		public IEnumerator<float> NightOfTheLivingNerd()
		{
			if ( !plugin.Config.LivingNerdEnabled ) yield break;

			List<Player> PlyList = new List<Player>();
			yield return Timing.WaitForSeconds( 3f );

			foreach ( Player ply in Player.List )
			{
				PlyList.Add( ply );
			}

			Map.Broadcast( 6, "<color=red>The Night of the Living Nerd round has started!</color>" );
			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count );
			Player SelectedNerd = PlyList[RandPly];
			SelectedNerd.SetRole( RoleType.Scientist );
			SelectedNerd.AddItem( ItemType.GunLogicer );
			SelectedNerd.AddItem( ItemType.Flashlight );
			SelectedNerd.Ammo[ItemType.Ammo762x39] = plugin.Config.NerdAmmoAmount;
			PlyList.RemoveAt( RandPly );
			Map.TurnOffAllLights( 5000 );

			foreach ( Player ply in PlyList )
			{
				ply.SetRole( RoleType.ClassD );
				ply.AddItem( ItemType.Flashlight );
				ply.AddItem( ItemType.SCP268 );
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
			{
				PlyList.Add( ply );
			}

			Map.Broadcast( 6, "<color=red>The SCP-682 Containment round has started!</color>" );
			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count );
			Player Selected682 = PlyList[RandPly];
			Selected682.SetRole( RoleType.Scp93953 );
			yield return Timing.WaitForSeconds( 3f );
			Selected682.Position = RoleExtensions.GetRandomSpawnProperties( RoleType.ChaosConscript ).Item1;
			Selected682.Scale *= 1.75f;
			Selected682.MaxHealth = plugin.Config.SCP682Health;
			Selected682.Health = plugin.Config.SCP682Health;
			PlyList.RemoveAt( RandPly );
			Warhead.Start();

			foreach ( Player ply in PlyList )
			{
				ply.SetRole( RoleType.NtfCaptain );
				ply.Ammo[ItemType.Ammo556x45] = plugin.Config.SCP682MTFAmmo;
			}
		}

		public void OnRoundEnd( RoundEndedEventArgs ev )
		{
			if ( CurrentGamemode > 0 )
			{
				Map.Broadcast( 6, "<color=red>The " + GetGamemodeName() + " round has ended.</color>" );
				CurrentGamemode = 0;
				TotalBalls = 0;
				ServerConsole.FriendlyFire = FriendlyFireDefault;
			}
		}
	}
}
