// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The decision making process that will control what actions an AI agent 
/// performs to fulfill their order(s).
/// </summary>
public class HierarchicalTaskNetwork : MonoBehaviour {
	private Task task = null;
	private Task[] availableTasks = null;
	private Task[] executableTasks = null;

	public HierarchicalTaskNetwork(Task[] availableTasks) { 
		this.task = null;
		this.availableTasks = availableTasks;
		this.executableTasks = new Task[availableTasks.Length];
	}

	private void CreatePlan() { }
}
