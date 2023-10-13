using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RandomGamemode
{
	enum Gamemode
	{
		Invalid,
		Dodgeball,
		PeanutRaid,
		GoldfishAttacks,
		NightOfTheLivingNerd,
		SCP682Containment,
		Randomizer,
	}

	public class EventHandlers
	{
		private Plugin plugin;
		private Gamemode CurrentGamemode;
		private bool FriendlyFireDefault;
		System.Random rand = new System.Random();

		public EventHandlers( Plugin plugin ) => this.plugin = plugin;

		public string GetGamemodeName()
		{
			switch ( CurrentGamemode )
			{
				case Gamemode.Dodgeball: return "Dodgeball";
				case Gamemode.PeanutRaid: return "Peanut Raid";
				case Gamemode.GoldfishAttacks: return "Goldfish Attacks";
				case Gamemode.NightOfTheLivingNerd: return "Night of the Living Nerd";
				case Gamemode.SCP682Containment: return "SCP-682 Containment";
				case Gamemode.Randomizer: return "Randomizer";
				default: return "Invalid Gamemode";
			}
		}

		public void OnRoundStart()
		{
			if ( rand.Next( 1, 101 ) <= plugin.Config.GamemodeChance )
			{
				CurrentGamemode = ( Gamemode ) Plugin.EnabledList[rand.Next( Plugin.EnabledList.Count )];
				switch ( CurrentGamemode )
				{
					case Gamemode.Dodgeball: Timing.RunCoroutine( DodgeBall() ); break;
					case Gamemode.PeanutRaid: Timing.RunCoroutine( PeanutRaid() ); break;
					case Gamemode.GoldfishAttacks: Timing.RunCoroutine( GoldfishAttacks() ); break;
					case Gamemode.NightOfTheLivingNerd: Timing.RunCoroutine( NightOfTheLivingNerd() ); break;
					case Gamemode.SCP682Containment: Timing.RunCoroutine( SCP682Containment() ); break;
					case Gamemode.Randomizer: Timing.RunCoroutine( Randomizer() ); break;
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
					ply.Role.Set( RoleTypeId.FacilityGuard );
				}

				yield return Timing.WaitForSeconds( 0.5f );
				ply.ClearInventory();

				for ( int i = 0; i < 7; i++ )
				{
					ply.AddItem( ItemType.SCP018 );
				}

				ply.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.NtfCaptain ).Position;
			}
		}

		public void OnGrenadeThrown( ThrownProjectileEventArgs ev )
		{
			if ( CurrentGamemode == Gamemode.Dodgeball && ev.Projectile.ProjectileType == ProjectileType.Scp018 )
			{
				ev.Projectile.Scale *= 3;
				( ev.Projectile as Scp018Projectile ).FuseTime = 3;
				ev.Player.AddItem( ItemType.SCP018 );
			}
		}

		public void OnItemDropped( DroppingItemEventArgs ev )
		{
			if ( CurrentGamemode == Gamemode.Dodgeball )
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
			SelectedDBoi.Role.Set( RoleTypeId.ClassD );
			PlyList.RemoveAt( RandPly );

			foreach ( Player ply in PlyList )
			{
				ply.Role.Set( RoleTypeId.Scp173 );
			}

			yield return Timing.WaitForSeconds( 0.5f );
			SelectedDBoi.Scale /= 2;
		}

		public void OnPlayerJoin( JoinedEventArgs ev )
		{
			if ( CurrentGamemode == Gamemode.PeanutRaid )
			{
				ev.Player.Role.Set( RoleTypeId.Scp173 );
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
					ply.Role.Set( RoleTypeId.Scp079 );
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
			SelectedNerd.Role.Set( RoleTypeId.Scientist );
			SelectedNerd.ClearInventory();
			SelectedNerd.AddItem( ItemType.GunLogicer );
			SelectedNerd.AddItem( ItemType.Flashlight );
			SelectedNerd.AddItem( ItemType.KeycardFacilityManager );
			SelectedNerd.SetAmmo( AmmoType.Nato762, plugin.Config.NerdAmmoAmount );
			SelectedNerd.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.Scp939 ).Position;
			SelectedNerd.EnableEffect( EffectType.MovementBoost );
			PlyList.RemoveAt( RandPly );
			Map.TurnOffAllLights( 5000 );

			foreach ( Player ply in PlyList )
			{
				ply.Role.Set( RoleTypeId.ClassD );
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
			Selected682.Role.Set( RoleTypeId.Scp939 );
			yield return Timing.WaitForSeconds( 3f );
			Selected682.Scale *= 1.75f; // Any larger and players are hard to kill due to hitbox issues
			Selected682.MaxHealth = plugin.Config.SCP682Health;
			Selected682.Health = plugin.Config.SCP682Health;
			PlyList.RemoveAt( RandPly );

			foreach ( Player ply in PlyList )
			{
				ply.Role.Set( RoleTypeId.NtfCaptain );
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

			RoleTypeId[] roles = new RoleTypeId[] {
				RoleTypeId.NtfCaptain, RoleTypeId.ChaosConscript, RoleTypeId.ClassD,
				RoleTypeId.FacilityGuard, RoleTypeId.Scientist
			};

			RoleTypeId[] scps = new RoleTypeId[] {
				RoleTypeId.Scp049, RoleTypeId.Scp0492, RoleTypeId.Scp096,
				RoleTypeId.Scp106, RoleTypeId.Scp173, RoleTypeId.Scp939
			};

			// Set random SCP
			foreach ( Player ply in Player.List )
			{
				PlyList.Add( ply );
			}

			yield return Timing.WaitForSeconds( 1f );

			int RandPly = rand.Next( PlyList.Count );
			Player scp = PlyList[RandPly];
			scp.Role.Set( scps[rand.Next( scps.Length )] );
			PlyList.RemoveAt( RandPly );

			// Set random roles for the rest of the players
			foreach ( Player ply in PlyList )
			{
				ply.Role.Set( roles[rand.Next( roles.Length )] );
			}

			yield return Timing.WaitForSeconds( 1f );

			// Set random spawns
			foreach ( Player ply in Player.List )
			{
				if ( ply.Role == RoleTypeId.Scp0492 )
				{
					ply.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.ClassD ).Position;
				}
				else
				{
					Vector3 pos = RoleExtensions.GetRandomSpawnLocation( ( RoleTypeId ) rand.Next( roles.Length ) ).Position;
					while ( pos == Vector3.zero || pos == RoleExtensions.GetRandomSpawnLocation( RoleTypeId.Scp079 ).Position )
					{
						// Prevent players from spawning in areas they can't escape
						pos = RoleExtensions.GetRandomSpawnLocation( ( RoleTypeId ) rand.Next( roles.Length ) ).Position;
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

			if ( CurrentGamemode == Gamemode.Randomizer && totalalive > 1 )
			{
				ev.IsRoundEnded = false;
			}
		}

		public void OnRoundEnd( RoundEndedEventArgs ev )
		{
			if ( CurrentGamemode > 0 )
			{
				Map.Broadcast( 6, "<color=red>The " + GetGamemodeName() + " round has ended.</color>" );
				CurrentGamemode = 0;
				ServerConsole.FriendlyFire = FriendlyFireDefault;
			}
		}
	}
}
