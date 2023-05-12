// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds a delegate to another class' method with preceonditions that need to 
/// be satisfied in order to execute the method.
/// Aimed to be used by a HTN.
/// </summary>
public class Task : MonoBehaviour {
	public struct Condition {
		public string condition;
		public bool satisfied;
	}

	public delegate void Method();

	private List<Condition> preconditions = null;
	private List<Condition> postconditions = null;
	private Method taskToExecute = null;

	public Task(List<Condition> preconditions,
		List<Condition> postconditions,
		Method taskToExecute) { 
		this.preconditions = preconditions;
		this.postconditions = postconditions;
		this.taskToExecute = taskToExecute;
	}
}
