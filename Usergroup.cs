using System.Collections.Generic;
using Sandbox;

namespace CAMI;

public class Usergroup
{
	/// <summary>
	///     Contains the registered usergroups, indexed by name.
	/// </summary>
	private static readonly IDictionary<string, Usergroup> Usergroups = new Dictionary<string, Usergroup>();

	/// <summary>
	///     The name of the usergroup
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	///     The name of the usergroup this usergroup inherits from
	/// </summary>
	public string Inherits { get; set; }

	/// <summary>
	///     The source specified by the admin mod which registered this usergroup (if any, converted to a string)
	/// </summary>
	public string Source { get; set; }

	/// <summary>
	///     Retrieves all registered usergroups.
	/// </summary>
	/// <returns>A dictionary of usergroups indexed by their names.</returns>
	public static IDictionary<string, Usergroup> GetAll()
	{
		return Usergroups;
	}

	/// <summary>
	///     Receives information about a usergroup.
	/// </summary>
	/// <param name="name">the name of the usergroup.</param>
	/// <returns>The usergroup, if it is registered, or null.</returns>
	public static Usergroup Get(string name)
	{
		return Usergroups.TryGetValue(name, out var usergroup) ? usergroup : null;
	}

	/// <summary>
	///     Registers a usergroup with CAMI.
	/// </summary>
	/// <param name="usergroup">The name of the usergroup to register.</param>
	/// <param name="source">Identifier of the admin mod registering the usergroup.</param>
	/// <returns>The usergroup being registered.</returns>
	public static Usergroup Register(Usergroup usergroup, string source = "undefined")
	{
		Usergroups[usergroup.Name] = usergroup;
		Event.Run("CAMI.OnUsergroupRegistered", usergroup, source);
		return usergroup;
	}

	/// <summary>
	///     Removes a registered CAMI usergroup.
	/// </summary>
	/// <param name="name">The name of the usergroup to remove.</param>
	/// <param name="source">Identifier of the admin mod removing the usergroup.</param>
	/// <returns>Whether or not the usergroup was successfully removed.</returns>
	public static bool Unregister(string name, string source = "undefined")
	{
		if (!Usergroups.ContainsKey(name))
			return false;

		Usergroups.Remove(name);
		Event.Run("CAMI.OnUsergroupUnregistered", name, source);
		return true;
	}

	/// <summary>
	///     Checks if the usergroup inherits from another.
	/// </summary>
	/// <param name="potentialAncestor">The possible ancestor of the usergroup.</param>
	/// <returns>Whether or not the usergroup inherits from the potential ancestor.</returns>
	public bool DoesInherit(string potentialAncestor)
	{
		if (Name == potentialAncestor || Inherits == Name)
			return true;

		if (string.IsNullOrEmpty(Inherits) || !Usergroups.TryGetValue(Inherits, out var ancestor))
			return false;

		return ancestor.DoesInherit(potentialAncestor);
	}

	/// <summary>
	///     Get the root ancestor of the usergroup.
	/// </summary>
	/// <returns>The root ancestor of the usergroup.</returns>
	public string GetRootAncestor()
	{
		return string.IsNullOrEmpty(Inherits) ? Name : Usergroups[Inherits].GetRootAncestor();
	}
}