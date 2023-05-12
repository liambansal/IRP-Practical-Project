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
	public struct Precondition {
		public string condition;
		public bool satisfied;
	}

	public delegate void Method();

	private List<Precondition> preconditions = null;
	private Method taskToExecute = null;

	public Task(List<Precondition> preconditions, Method taskToExecute) { 
		this.preconditions = preconditions;
		this.taskToExecute = taskToExecute;
	}
}
