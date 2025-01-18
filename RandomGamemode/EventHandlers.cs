using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RandomGamemode
{
	enum Gamemode
	{
		Invalid,
		Dodgeball,
		PeanutRaid,
		BlueScreenOfDeath,
		NightOfTheLivingNerd,
		Randomizer,
		AnnoyingMimicry,
		LockedIn,
		Infection,
		LivingLikeLarry
	}

	public class EventHandlers
	{
		private Plugin plugin;
		private Gamemode CurrentGamemode;
		private bool FriendlyFireDefault;
		System.Random rand = new System.Random();

		public EventHandlers( Plugin plugin ) => this.plugin = plugin;

		private string GetGamemodeName()
		{
			switch ( CurrentGamemode )
			{
				case Gamemode.Dodgeball: return "Dodgeball";
				case Gamemode.PeanutRaid: return "Peanut Raid";
				case Gamemode.BlueScreenOfDeath: return "Blue Screen of Death";
				case Gamemode.NightOfTheLivingNerd: return "Night of the Living Nerd";
				case Gamemode.Randomizer: return "Randomizer";
				case Gamemode.AnnoyingMimicry: return "Annoying Mimicry";
				case Gamemode.LockedIn: return "Locked In";
				case Gamemode.Infection: return "Infection";
				case Gamemode.LivingLikeLarry: return "Living Like Larry";
				default: return "Invalid Gamemode";
			}
		}

		private IEnumerator<float> DelayedRoundEnd( float delay )
		{
			yield return Timing.WaitForSeconds( delay );
			Round.EndRound( true );
		}

		#region Gamemodes
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

		public IEnumerator<float> PeanutRaid()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.Role.Set( RoleTypeId.ClassD );
					yield return Timing.WaitForSeconds( 0.5f );
					ply.Scale *= 0.25f;
				}
				else
				{
					ply.Role.Set( RoleTypeId.Scp173 );
				}
			}
		}

		public IEnumerator<float> BlueScreenOfDeath()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.Role.Set( RoleTypeId.Scp079 );
					( ply.Role as Scp079Role ).Level = 3;
				}
				else
				{
					ply.Role.Set( RoleTypeId.Scientist );
					ply.ClearInventory();
				}
			}
			Map.ChangeLightsColor( Color.blue );
		}

		public IEnumerator<float> NightOfTheLivingNerd()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.Role.Set( RoleTypeId.Scientist );
					ply.ClearInventory();
					ply.AddItem( ItemType.GunLogicer );
					ply.AddItem( ItemType.Flashlight );
					ply.AddItem( ItemType.KeycardFacilityManager );
					ply.SetAmmo( AmmoType.Nato762, 1000 );
					ply.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.Scp939 ).Position;
					ply.EnableEffect( EffectType.MovementBoost );
				}
				else
				{
					ply.Role.Set( RoleTypeId.ClassD );
					ply.AddItem( ItemType.Flashlight );
					ply.AddItem( ItemType.SCP268 );
				}
			}
			Map.TurnOffAllLights( 5000 );
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

			Player scp = PlyList.RandomItem();
			scp.Role.Set( scps.RandomItem() );
			PlyList.RemoveAt( PlyList.IndexOf( scp ) );

			// Set random roles for the rest of the players
			foreach ( Player ply in PlyList )
			{
				ply.Role.Set( roles.RandomItem() );
			}

			yield return Timing.WaitForSeconds( 1f );

			// Set random spawns
			foreach ( Player ply in Player.List )
			{
				if ( ply.Role == RoleTypeId.Scp0492 )
					ply.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.ClassD ).Position;
				else
					ply.Position = RoleExtensions.GetRandomSpawnLocation( roles.RandomItem() ).Position;
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

		public IEnumerator<float> AnnoyingMimicry()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
                    ply.Role.Set( RoleTypeId.Scp939 );
                }
				else
				{
                    ply.Role.Set( RoleTypeId.ClassD );
					ply.AddItem( ItemType.Jailbird );
                }
			}
		}

		public IEnumerator<float> Infection()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
					ply.Role.Set( RoleTypeId.Scp049 );
			}
		}

		public IEnumerator<float> LivingLikeLarry()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
				{
					ply.Role.Set( RoleTypeId.ClassD );
					ply.Position = Door.Get( DoorType.Scp173Bottom ).Position + Vector3.up * 2;
					ply.AddItem( ItemType.KeycardO5 );
				}
				else
				{
					ply.Role.Set( RoleTypeId.Scp106 );
					ply.Position = Door.Get( DoorType.Scp173Gate ).Position + Vector3.up * 2;
				}
			}
		}
		#endregion

		#region Events
		public void OnRoundStart()
		{
			if ( rand.Next( 1, 101 ) <= plugin.Config.GamemodeChance )
			{
				CurrentGamemode = Plugin.EnabledList.RandomItem();
				switch ( CurrentGamemode )
				{
					case Gamemode.Dodgeball: Timing.RunCoroutine( DodgeBall() ); break;
					case Gamemode.PeanutRaid: Timing.RunCoroutine( PeanutRaid() ); break;
					case Gamemode.BlueScreenOfDeath: Timing.RunCoroutine( BlueScreenOfDeath() ); break;
					case Gamemode.NightOfTheLivingNerd: Timing.RunCoroutine( NightOfTheLivingNerd() ); break;
					case Gamemode.Randomizer: Timing.RunCoroutine( Randomizer() ); break;
					case Gamemode.AnnoyingMimicry: Timing.RunCoroutine( AnnoyingMimicry() ); break;
					case Gamemode.Infection: Timing.RunCoroutine( Infection() ); break;
					case Gamemode.LivingLikeLarry: Timing.RunCoroutine( LivingLikeLarry() ); break;
				}
				Map.Broadcast( 6, "<color=red>The " + GetGamemodeName() + " round has started!</color>" );
			}
			if ( CurrentGamemode == Gamemode.BlueScreenOfDeath )
			{
				Timing.RunCoroutine( DelayedRoundEnd( 900 ), "DelayedRoundEnd" );
			}
		}
		
		public void OnRoundEnding( EndingRoundEventArgs ev )
		{
			// Prevents randomizer round from ending if everyone is on the same team
			int totalalive = 0;
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsAlive )
					totalalive++;
			}

			if ( CurrentGamemode == Gamemode.Randomizer && totalalive > 1 )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnRoundEnd( RoundEndedEventArgs ev )
		{
			// Broadcast that the gamemode has ended
			if ( CurrentGamemode > 0 )
			{
				Map.Broadcast( 6, "<color=red>The " + GetGamemodeName() + " round has ended.</color>" );
				CurrentGamemode = 0;
				ServerConsole.FriendlyFire = FriendlyFireDefault;
				Timing.KillCoroutines( "DelayedRoundEnd" );
			}
		}

		public void OnGrenadeThrown( ThrownProjectileEventArgs ev )
		{
			// Increase size and decrease fuse time of dodgeballs
			if ( CurrentGamemode == Gamemode.Dodgeball && ev.Projectile.ProjectileType == ProjectileType.Scp018 )
			{
				ev.Projectile.Scale *= 3;
				( ev.Projectile as Scp018Projectile ).FuseTime = 1;
				ev.Player.AddItem( ItemType.SCP018 );
			}
		}

		public void OnItemDropped( DroppingItemEventArgs ev )
		{
			// Disable dropping dodgeballs so players don't lose all of them
			if ( CurrentGamemode == Gamemode.Dodgeball )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnGeneratorDeactivate( StoppingGeneratorEventArgs ev )
		{
			// Prevent generators from being deactivated once they're active
			if ( CurrentGamemode == Gamemode.BlueScreenOfDeath )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnRespawn( RespawningTeamEventArgs ev )
		{
			// Disable respawning for Annoying Mimicry gamemode and change spawn location for Locked in gamemode
			if ( CurrentGamemode == Gamemode.AnnoyingMimicry )
			{
				ev.IsAllowed = false;
			}
			else if ( CurrentGamemode == Gamemode.LockedIn )
			{
				if ( ev.Wave.Team == Team.FoundationForces )
				{
					Timing.CallDelayed( 3, () => {
						foreach( Player ply in ev.Players )
						{
							ply.Position = RoleExtensions.GetRandomSpawnLocation( RoleTypeId.FacilityGuard ).Position;
						}
					} );
				}
				else if ( ev.Wave.Team == Team.ChaosInsurgency )
				{
					Timing.CallDelayed( 3, () => {
						foreach ( Player ply in ev.Players )
						{
							List<RoleTypeId> scps = new List<RoleTypeId>() {
								RoleTypeId.Scp939, RoleTypeId.Scp049, RoleTypeId.Scp096,
								RoleTypeId.Scp173, RoleTypeId.Scp106
							};
							ply.Position = RoleExtensions.GetRandomSpawnLocation( scps.RandomItem() ).Position;
						}
					} );
				}
			}
		}

		public void OnPlayerDied( DiedEventArgs ev )
		{
			// Respawn killed players as 939 for Annoying Mimicry gamemode, and 049-2 for Infection
			if ( CurrentGamemode == Gamemode.AnnoyingMimicry )
			{
				Timing.CallDelayed( 3, () => ev.Player.Role.Set( RoleTypeId.Scp939 ) );
			}
			else if ( CurrentGamemode == Gamemode.Infection )
			{
				Timing.CallDelayed( 3, () => {
					ev.Player.Role.Set( RoleTypeId.Scp0492 );
					ev.Player.Scale *= 1.15f;
					ev.Player.MaxHealth = 400;
					ev.Player.Health = 400;
				} );
			}
		}

		public void OnDoorUse( InteractingDoorEventArgs ev )
		{
			// Disable using gates for Annoying Mimicry and Locked In gamemodes
			if ( ( CurrentGamemode == Gamemode.AnnoyingMimicry || CurrentGamemode == Gamemode.LockedIn ) && ( ev.Door == Door.Get( "GATE_A" ) || ev.Door == Door.Get( "GATE_B" ) ) )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnDecon( DecontaminatingEventArgs ev )
		{
			if ( CurrentGamemode == Gamemode.LockedIn )
			{
				ev.IsAllowed = false;
			}
		}
		#endregion
	}
}
