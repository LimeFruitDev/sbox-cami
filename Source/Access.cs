using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace CAMI;

/// <summary>
///     Used to check for privilege access.
/// </summary>
public static class Access
{
	/// <summary>
	///     Retrieves a list of players with access to the given privilege.
	/// </summary>
	/// <param name="privilege">The privilege to retrieve players with access for.</param>
	/// <returns>The list of players with access to the given privilege.</returns>
	public static IList<IClient> GetPlayers(string privilege)
	{
		return Game.Clients.Where(player => CheckPlayer(player, privilege)).ToList();
	}

	/// <summary>
	///     Checks if a player has access to a privilege.
	/// </summary>
	/// <param name="player">The player to query.</param>
	/// <param name="privilege">The privilege to check access for.</param>
	/// <returns>Whether or not the player has access to the given privilege.</returns>
	public static bool CheckPlayer(IClient player, string privilege)
	{
		var status = new Status();
		Event.Run("CAMI.PlayerHasAccess", player, privilege, status);

		return status.HasAccess;
	}

	/// <summary>
	///     Checks if a SteamID has access to a privilege.
	/// </summary>
	/// <param name="steamId">The SteamID to query.</param>
	/// <param name="privilege">The privilege to check access for.</param>
	/// <returns>Whether or not the SteamID has access to the given privilege.</returns>
	public static bool CheckSteamId(string steamId, string privilege)
	{
		var status = new Status();
		Event.Run("CAMI.SteamIDHasAccess", steamId, privilege, status);

		return status.HasAccess;
	}

	/// <summary>
	///     A status object to use in the events, to declare whether or not a player has access.
	/// </summary>
	public class Status
	{
		/// <summary>
		///     Whether or not the player has access to the given privilege.
		/// </summary>
		public bool HasAccess { get; private set; }

		/// <summary>
		///     Whether or not the status is locked (when override-rejected).
		/// </summary>
		public bool IsLocked { get; private set; }

		/// <summary>
		///     Accepts a player's access.
		/// </summary>
		/// <returns>True if the status wasn't locked when attempting to set the access.</returns>
		public bool Accept()
		{
			if (IsLocked)
				return false;

			HasAccess = true;
			return true;
		}

		/// <summary>
		///     Rejects a player's access. This should effectively only be used to override other admin mods.
		///     Imagine this as being the third state of a privilege; You can have a "Yes", "No" and "Never".
		///     If you want a Yes, you accept. If you want a No, you do nothing. If you want a Never, you reject.
		/// </summary>
		/// <returns>True if the status wasn't locked when attempting to set the access.</returns>
		public bool Reject()
		{
			if (IsLocked)
				return false;

			IsLocked = true;
			HasAccess = false;
			return true;
		}
	}
}
