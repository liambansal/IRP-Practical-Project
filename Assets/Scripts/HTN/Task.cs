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

	public enum TaskState {
		NotStarted,
		Started,
		Executing,
		Finished,
		Cancelled
	}

	public delegate void Method();

	/// <summary>
	/// The current progress made towards completing the task.
	/// </summary>
	public TaskState State {
		get;
		private set;
	}

	public List<Condition> Preconditions {
		get;
		private set;
	}
	public List<Condition> Postconditions {
		get;
		private set;
	}
	/// <summary>
	/// The method to be executed by this class.
	/// </summary>
	public Method taskToExecute {
		get;
		private set;
	}

	public Task(List<Condition> preconditions,
		List<Condition> postconditions,
		Method taskToExecute) { 
		this.Preconditions = preconditions;
		this.Postconditions = postconditions;
		this.taskToExecute = taskToExecute;
	}
}
