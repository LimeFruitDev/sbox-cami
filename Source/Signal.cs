using Sandbox;

namespace CAMI;

/// <summary>
///     Used to signal various events.
/// </summary>
public static class Signal
{
    /// <summary>
    ///     Used to signal a change in a player's usergroup.
    /// </summary>
    /// <param name="client">The player whose usergroup is being modified.</param>
    /// <param name="oldUsergroup">The previous usergroup the player was assigned to.</param>
    /// <param name="newUsergroup">The new usergroup the player is assigned to.</param>
    /// <param name="source">The source of the change (admin mod identifier).</param>
    public static void UserGroupChanged(IClient client, string oldUsergroup, string newUsergroup,
        string source = "undefined")
    {
        Event.Run("CAMI.PlayerUsergroupChanged", client, new UserGroupChangedSignal
        {
            OldUsergroup = oldUsergroup,
            NewUsergroup = newUsergroup,
            Source = source
        });
    }

    /// <summary>
    ///     Used to signal a change in a player's usergroup, identified via their Steam ID.
    /// </summary>
    /// <param name="steamId">The SteamID of the player whose usergroup is being modified.</param>
    /// <param name="oldUsergroup">The previous usergroup the player was assigned to.</param>
    /// <param name="newUsergroup">The new usergroup the player is assigned to.</param>
    /// <param name="source">The source of the change (admin mod identifier).</param>
    public static void SteamIdUserGroupChanged(string steamId, string oldUsergroup, string newUsergroup,
        string source = "undefined")
    {
        Event.Run("CAMI.SteamIDUsergroupChanged", steamId, new UserGroupChangedSignal
        {
            OldUsergroup = oldUsergroup,
            NewUsergroup = newUsergroup,
            Source = source
        });
    }

    /// <summary>
    ///     Used to pass on information regarding the changed usergroup.
    /// </summary>
    public record UserGroupChangedSignal
    {
        /// <summary>
        ///     The previous usergroup the player was assigned to.
        /// </summary>
        public required string OldUsergroup { get; set; }

        /// <summary>
        ///     The new usergroup the player is assigned to.
        /// </summary>
        public required string NewUsergroup { get; set; }

        /// <summary>
        ///     The source of the change (admin mod identifier).
        /// </summary>
        public required string Source { get; set; }
    }
}