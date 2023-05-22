// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;

/// <summary>
/// The base task class that provides the types and properties used by all 
/// other task classes.
/// </summary>
public class Task {
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

	/// <summary>
	/// The current progress made towards completing the task.
	/// </summary>
	public TaskState State {
		get;
		protected set;
	}

	public List<Condition> Preconditions {
		get;
		protected set;
	}
	public List<Condition> Postconditions {
		get;
		protected set;
	}

	public Task(List<Condition> preconditions,
		List<Condition> postconditions) {
		Preconditions = preconditions;
		Postconditions = postconditions;
	}
}
