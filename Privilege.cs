using System.Collections.Generic;
using Sandbox;

namespace CAMI;

/// <summary>
///     A privilege defines a unified interface for access to perform actions.
/// </summary>
public class Privilege
{
	/// <summary>
	///     Contains the registered privileges, indexed by name.
	/// </summary>
	private static readonly IDictionary<string, Privilege> Privileges = new Dictionary<string, Privilege>();

	/// <summary>
	///     The name of the privilege.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	///     Optional text describing the purpose of the privilege.
	/// </summary>
	public string Description { get; set; }

	/// <summary>
	///     Retrieves all registered privileges.
	/// </summary>
	/// <returns>All registered privileges.</returns>
	public static IDictionary<string, Privilege> GetAll()
	{
		return Privileges;
	}

	/// <summary>
	///     Retrieves information about a privilege.
	/// </summary>
	/// <param name="name">The name of the privilege.</param>
	/// <returns>The privilege object, or null if it could not be found.</returns>
	public static Privilege Get(string name)
	{
		return Privileges.TryGetValue(name, out var privilege) ? privilege : null;
	}

	/// <summary>
	///     Registers a privilege.
	/// </summary>
	/// <param name="privilege">The privilege object to register.</param>
	/// <returns>The privilege object.</returns>
	public static Privilege Register(Privilege privilege)
	{
		Privileges[privilege.Name] = privilege;
		Event.Run("CAMI.OnPrivilegeRegistered", privilege);
		return privilege;
	}

	/// <summary>
	///     Removes a registered privilege.
	/// </summary>
	/// <param name="name">The name of the privilege to remove.</param>
	/// <returns>Whether or not the privilege was successfully removed.</returns>
	public static bool Unregister(string name)
	{
		if (!Privileges.ContainsKey(name))
			return false;

		Privileges.Remove(name);
		Event.Run("CAMI.OnPrivilegeUnregistered", name);
		return true;
	}
}