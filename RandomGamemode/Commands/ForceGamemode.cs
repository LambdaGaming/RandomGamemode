using CommandSystem;
using System;

namespace RandomGamemode.Commands
{
	[CommandHandler( typeof( RemoteAdminCommandHandler ) )]
	[CommandHandler( typeof( GameConsoleCommandHandler ) )]
	internal sealed class ForceGamemode : ICommand
	{
		public string Command { get; } = "forcegamemode";
		public string[] Aliases { get; } = Array.Empty<string>();
		public string Description { get; } = "Forces the next round to be a special gamemode.";
		public bool Execute( ArraySegment<string> arguments, ICommandSender sender, out string response )
		{
			if ( !sender.CheckPermission( PlayerPermissions.GameplayData ) )
			{
				response = "<color=red>You don't have permission to use this command!</color>";
				return false;
			}

			if ( arguments.Count < 1 )
			{
				Plugin.NextGamemode = Plugin.EnabledList.RandomItem();
				response = $"The {Plugin.GetGamemodeName( Plugin.NextGamemode )} gamemode will be activated next round.";
				return true;
			}

			try
			{
				Gamemode selectedGamemode = ( Gamemode ) int.Parse( arguments.At( 0 ) );
				Plugin.NextGamemode = selectedGamemode;
				if ( Plugin.NextGamemode == Gamemode.Invalid )
					response = "Manual gamemode override for next round has been disabled.";
				else
					response = $"The {Plugin.GetGamemodeName( Plugin.NextGamemode )} gamemode will be activated next round.";
				return true;
			}
			catch
			{
				response = $"<color=red>Argument must be a number that corresponds to the gamemode you want to select.</color>";
				return false;
			}
		}
	}
}
