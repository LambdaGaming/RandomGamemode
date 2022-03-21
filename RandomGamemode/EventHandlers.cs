using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomGamemode
{
	public class EventHandlers
	{
		private Plugin plugin;
		public int CurrentGamemode;
		private bool FriendlyFireDefault;
		private int TotalBalls = 0;
		System.Random rand = new System.Random();

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
				case 6: return "Randomizer";
				default: return "Invalid Gamemode";
			}
		}

		public void OnRoundStart()
		{
			if ( rand.Next( 1, 101 ) <= plugin.Config.GamemodeChance )
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
					case 6: Timing.RunCoroutine( Randomizer() ); break;
				}
				Map.Broadcast( 6, "<color=red>The " + GetGamemodeName() + " round has started!</color>" );
			}
		}

		public IEnumerator<float> DodgeBall()
		{
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
			List<Player> PlyList = new List<Player>();
			yield return Timing.WaitForSeconds( 3f );

			foreach ( Player ply in Player.List )
			{
				PlyList.Add( ply );
			}

			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( PlyList.Count );
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
			yield return Timing.WaitForSeconds( 3f );
			string Name = "The Black Goldfish";

			foreach ( Player ply in Player.List )
			{
				if ( ply.Nickname == Name )
				{
					ply.SetRole( RoleType.Scp079 );
				}
				else
				{
					CurrentGamemode = 0;
				}
			}
		}

		public IEnumerator<float> NightOfTheLivingNerd()
		{
			List<Player> PlyList = new List<Player>();
			yield return Timing.WaitForSeconds( 3f );

			foreach ( Player ply in Player.List )
			{
				PlyList.Add( ply );
			}

			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( PlyList.Count );
			Player SelectedNerd = PlyList[RandPly];
			SelectedNerd.SetRole( RoleType.Scientist );
			SelectedNerd.AddItem( ItemType.GunLogicer );
			SelectedNerd.AddItem( ItemType.Flashlight );
			SelectedNerd.SetAmmo( AmmoType.Nato762, plugin.Config.NerdAmmoAmount );
			SelectedNerd.Position = RoleExtensions.GetRandomSpawnProperties( RoleType.Scp93953 ).Item1;
			SelectedNerd.EnableEffect( EffectType.Scp207 );
			SelectedNerd.EnableEffect( EffectType.Deafened );
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

			yield return Timing.WaitForSeconds( 1f );
			int RandPly = rand.Next( 0, PlyList.Count );
			Player Selected682 = PlyList[RandPly];
			Selected682.SetRole( RoleType.Scp93953 );
			yield return Timing.WaitForSeconds( 3f );
			Selected682.Scale *= 1.75f; // Any larger and players are hard to kill due to hitbox issues
			Selected682.MaxHealth = plugin.Config.SCP682Health;
			Selected682.Health = plugin.Config.SCP682Health;
			PlyList.RemoveAt( RandPly );

			foreach ( Player ply in PlyList )
			{
				ply.SetRole( RoleType.NtfCaptain );
				ply.SetAmmo( AmmoType.Nato556, plugin.Config.SCP682MTFAmmo );
				ply.AddItem( ItemType.KeycardO5 );
			}
		}

		public IEnumerator<float> Randomizer()
		{
			List<Player> PlyList = new List<Player>();
			FriendlyFireDefault = ServerConsole.FriendlyFire;
			ServerConsole.FriendlyFire = true;
			yield return Timing.WaitForSeconds( 3f );

			RoleType[] roles = new RoleType[] {
				RoleType.NtfCaptain, RoleType.ChaosConscript, RoleType.ClassD,
				RoleType.FacilityGuard, RoleType.Scientist
			};

			RoleType[] scps = new RoleType[] {
				RoleType.Scp049, RoleType.Scp0492, RoleType.Scp096,
				RoleType.Scp106, RoleType.Scp173, RoleType.Scp93953
			};

			// Set random SCP
			foreach ( Player ply in Player.List )
			{
				PlyList.Add( ply );
			}

			yield return Timing.WaitForSeconds( 1f );

			int RandPly = rand.Next( PlyList.Count );
			Player scp = PlyList[RandPly];
			scp.SetRole( scps[rand.Next( scps.Length )] );
			PlyList.RemoveAt( RandPly );

			// Set random roles for the rest of the players
			foreach ( Player ply in PlyList )
			{
				ply.SetRole( roles[rand.Next( roles.Length )] );
			}

			yield return Timing.WaitForSeconds( 1f );

			// Set random spawns
			foreach ( Player ply in Player.List )
			{
				if ( ply.Role == RoleType.Scp0492 )
				{
					ply.Position = RoleExtensions.GetRandomSpawnProperties( RoleType.ClassD ).Item1;
				}
				else
				{
					Vector3 pos = RoleExtensions.GetRandomSpawnProperties( ( RoleType ) rand.Next( roles.Length ) ).Item1;
					while ( pos == Vector3.zero || pos == RoleExtensions.GetRandomSpawnProperties( RoleType.Scp079 ).Item1 )
					{
						// Prevent players from spawning in areas they can't escape
						pos = RoleExtensions.GetRandomSpawnProperties( ( RoleType ) rand.Next( roles.Length ) ).Item1;
					}
					ply.Position = pos;
				}
			}

			// Set random inventory items
			Array items = Enum.GetValues( typeof( ItemType ) );
			foreach ( Player ply in Player.List )
			{
				ply.ClearInventory();
				ply.AddItem( ItemType.KeycardO5 );
				for ( int i = 0; i < 7; i++ )
				{
					ply.AddItem( ( ItemType ) items.GetValue( rand.Next( items.Length ) ) );
				}
			}
		}

		// Prevents randomizer round from ending if everyone is on the same team
		public void OnRoundEnding( EndingRoundEventArgs ev )
		{
			int totalalive = 0;
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsAlive )
					totalalive++;
			}

			if ( CurrentGamemode == 6 && totalalive > 1 )
			{
				ev.IsAllowed = false;
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
