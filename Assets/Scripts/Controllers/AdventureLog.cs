using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureLog
{
	Dictionary<string, bool> eventList = new Dictionary<string, bool>();
	Dictionary<string, System.Action<bool> > eventDict = new Dictionary<string, System.Action<bool> >();

	// Set or unset a flag and call any registered callbacks to that flag
	public void SetEvent(string flag, bool value)
	{
		if (this.eventList.ContainsKey(flag)) {
			this.eventList[flag] = value;
		} else {
			Debug.LogWarning("flag '" + flag + "' does not exist in the eventList.");
		}

		if (this.eventDict.ContainsKey(flag) && this.eventDict[flag] != null) {
			this.eventDict[flag].Invoke(value);
		}
	}

	public List<string> GetAllEvents()
	{
		return null;
	}

	// Add a callback to an event
	public void RegisterEvent(string flag, System.Action<bool> callback)
	{
		if (this.eventDict.ContainsKey(flag)) {
			this.eventDict[flag] += callback;
		}
	}

	// Remove a callback from an event
	public void DeregisterEvent(string flag, System.Action<bool> callback)
	{
		if (this.eventDict.ContainsKey(flag)) {
			this.eventDict[flag] -= callback;
		}
	}
}
