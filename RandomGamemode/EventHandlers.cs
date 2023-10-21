﻿using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
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
		AnnoyingMimicry
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
				case Gamemode.BlueScreenOfDeath: return "Blue Screen of Death";
				case Gamemode.NightOfTheLivingNerd: return "Night of the Living Nerd";
				case Gamemode.Randomizer: return "Randomizer";
				case Gamemode.AnnoyingMimicry: return "Annoying Mimicry";
				default: return "Invalid Gamemode";
			}
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
			SelectedDBoi.Scale *= 0.5f;
		}

		public IEnumerator<float> BlueScreenOfDeath()
		{
			List<Player> PlyList = new List<Player>();
			yield return Timing.WaitForSeconds( 3f );

			foreach ( Player ply in Player.List )
			{
				PlyList.Add( ply );
			}

			int RandPly = rand.Next( PlyList.Count );
			Player pc = PlyList[RandPly];
			pc.Role.Set( RoleTypeId.Scp079 );
			( pc.Role as Scp079Role ).Level = 3;
			PlyList.RemoveAt( RandPly );

			foreach ( Player ply in PlyList )
			{
				ply.Role.Set( RoleTypeId.Scientist );
				ply.ClearInventory();
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
			SelectedNerd.SetAmmo( AmmoType.Nato762, 1000 );
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

		public IEnumerator<float> AnnoyingMimicry()
		{
			yield return Timing.WaitForSeconds( 3f );
			foreach ( Player ply in Player.List )
			{
				if ( ply.IsScp )
					ply.Role.Set( RoleTypeId.Scp939 );
				else
					ply.Role.Set( RoleTypeId.ClassD );
			}
		}
		#endregion

		#region Events
		public void OnRoundStart()
		{
			if ( rand.Next( 1, 101 ) <= plugin.Config.GamemodeChance )
			{
				CurrentGamemode = Plugin.EnabledList[rand.Next( Plugin.EnabledList.Count )];
				switch ( CurrentGamemode )
				{
					case Gamemode.Dodgeball: Timing.RunCoroutine( DodgeBall() ); break;
					case Gamemode.PeanutRaid: Timing.RunCoroutine( PeanutRaid() ); break;
					case Gamemode.BlueScreenOfDeath: Timing.RunCoroutine( BlueScreenOfDeath() ); break;
					case Gamemode.NightOfTheLivingNerd: Timing.RunCoroutine( NightOfTheLivingNerd() ); break;
					case Gamemode.Randomizer: Timing.RunCoroutine( Randomizer() ); break;
					case Gamemode.AnnoyingMimicry: Timing.RunCoroutine( AnnoyingMimicry() ); break;
				}
				Map.Broadcast( 6, "<color=red>The " + GetGamemodeName() + " round has started!</color>" );
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
				ev.IsRoundEnded = false;
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

		public void OnChangeCamera( ChangingCameraEventArgs ev )
		{
			// Set room light color to blue when 079 views it
			if ( CurrentGamemode == Gamemode.BlueScreenOfDeath )
			{
				ev.Camera.Room.RoomLightController.NetworkOverrideColor = Color.blue;
			}
		}

		public void OnGeneratorActivate( ActivatingGeneratorEventArgs ev )
		{
			// Set generator activation time to 5 minutes
			if ( CurrentGamemode == Gamemode.BlueScreenOfDeath )
			{
				ev.Generator.ActivationTime = 300;
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
			// Disable MTF and chaos respawning for Annoying Mimicry gamemode
			if ( CurrentGamemode == Gamemode.AnnoyingMimicry )
			{
				ev.IsAllowed = false;
			}
		}

		public void OnPlayerDied( DiedEventArgs ev )
		{
			// Respawn killed players as 939 for Annoying Mimicry gamemode
			if ( CurrentGamemode == Gamemode.AnnoyingMimicry )
			{
				Timing.CallDelayed( 3, () => ev.Player.Role.Set( RoleTypeId.Scp939 ) );
			}
		}

		public void OnDoorUse( InteractingDoorEventArgs ev )
		{
			// Disable using gates for Annoying Mimicry gamemode
			if ( CurrentGamemode == Gamemode.AnnoyingMimicry && ev.Door.IsGate )
			{
				ev.IsAllowed = false;
			}
		}
		#endregion
	}
}
