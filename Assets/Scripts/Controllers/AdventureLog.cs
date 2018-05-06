using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdventureLog
{
	private static AdventureLog instance;
	public static AdventureLog Instance {
		get {
			if (instance == null) {
				instance = new AdventureLog();
			}
			return instance;
		}
	}

	private Dictionary<string, bool> flagStatus = new Dictionary<string, bool>();
	private Dictionary<string, System.Action<bool> > flagDict = new Dictionary<string, System.Action<bool> >();
	private event System.Action<string, bool> allFlags;
	public AdventureLogView logView = null;

	private AdventureLog()
	{
		DataManager.Instance.DoOnLoaded(this.HandleDataManagerLoaded);
	}

	public void HandleDataManagerLoaded()
	{
		List<NodeData> nodes = DataManager.Instance.allNodes;
		List<string> flags = new List<string>();
		for (int i = 0; i < nodes.Count; ++i) {
			flags.AddRange(nodes[i].flagsRequired);
			if (nodes[i] is InteractionNodeData) {
				flags.AddRange(((InteractionNodeData)nodes[i]).flagsCreated);
				flags.AddRange(((InteractionNodeData)nodes[i]).flagsRemoved);
			}
		}
		flags.Sort();

		for (int i = 0; i < flags.Count; ++i) {
			if (i == 0) {
				if (string.IsNullOrEmpty(flags[i])) {
					flags.RemoveAt(i--);
				}
				continue;
			}

			if (flags[i] == flags[i - 1]) {
				flags.RemoveAt(i--);
			}
		}

		foreach (string flag in flags) {
			Debug.Log(flag);
			this.flagStatus.Add(flag, false);
			this.flagDict.Add(flag, null);
		}
	}

	public void ResetFlags()
	{
		Dictionary<string, bool>.Enumerator status = this.flagStatus.GetEnumerator();
		while (status.MoveNext()) {
			this.flagStatus[status.Current.Key] = false;
		}
	}

	// Set or unset a flag and call any registered callbacks to that flag
	public void SetFlag(string flag, bool value, string logLine = "")
	{
		if (this.flagStatus.ContainsKey(flag)) {
			this.flagStatus[flag] = value;
		} else {
			Debug.LogWarning("flag '" + flag + "' does not exist in flagStatus.");
		}

		if (this.flagDict.ContainsKey(flag) && this.flagDict[flag] != null) {
			this.flagDict[flag].Invoke(value);
		}

		if (this.allFlags != null) {
			this.allFlags.Invoke(flag, value);
		}

		if (!string.IsNullOrEmpty(logLine) && this.logView != null) {
			this.logView.AddLogLine(logLine);
		}
	}

	public bool GetFlag(string flag)
	{
		if (this.flagStatus.ContainsKey(flag)) {
			return this.flagStatus[flag];
		}
		return false;
	}

	public string[] GetAllEvents()
	{
		string[] keys = new string[this.flagDict.Keys.Count];
		this.flagDict.Keys.CopyTo(keys, 0);
		return keys;
	}

	public void RegisterAllEvents(System.Action<string, bool> callback)
	{
		this.allFlags += callback;
	}

	public void DeregisterAllEvents(System.Action<string, bool> callback)
	{
		this.allFlags -= callback;
	}

	// Add a callback to an event
	public void RegisterEvent(string flag, System.Action<bool> callback)
	{
		if (this.flagDict.ContainsKey(flag)) {
			this.flagDict[flag] += callback;
		}
	}

	// Remove a callback from an event
	public void DeregisterEvent(string flag, System.Action<bool> callback)
	{
		if (this.flagDict.ContainsKey(flag)) {
			this.flagDict[flag] -= callback;
		}
	}

	// Given a list of nodes, return only those that are currently available.
	public List<NodeData> FilterAvailableNodes(List<NodeData> nodes) {
		List<NodeData> available = new List<NodeData>();
		foreach (NodeData node in nodes) {
			bool isAvailable = true;
			foreach (string flag in node.flagsRequired) {
				isAvailable = isAvailable && (this.flagStatus.ContainsKey(flag) && this.flagStatus[flag]);
			}
			if (isAvailable) {
				// Override another node, or this one is overridden?
				NodeData deleteExistingNode = null;
				foreach (NodeData existingNode in available) {
					if (existingNode.id == node.id) {
						if (existingNode.flagsRequired.Count > node.flagsRequired.Count) {
							isAvailable = false;
						} else if (existingNode.flagsRequired.Count < node.flagsRequired.Count) {
							deleteExistingNode = existingNode;
						}
					}
				}
				if (deleteExistingNode != null) {
					available.Remove(deleteExistingNode);
				}
				if (isAvailable) {
					available.Add(node);
				}
			}
		}
		return available;
	}
}
