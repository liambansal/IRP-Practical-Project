// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;

/// <summary>
/// The base task class that provides the types and properties used by all 
/// other task classes.
/// </summary>
public class Task {
	public struct Condition {
		public string name;
		public bool satisfied;

		public Condition(string condition) {
			name = condition;
			satisfied = false;
		}
	}

	public enum TaskState {
		NotStarted,
		Started,
		Executing,
		Finished,
		Cancelled
	}

	public enum ConditionLists {
		Preconditions,
		Postconditions
	}

	public enum ConditionTypes {
		HasDestination,
		InPosition,
		InRange,
		SeeObject,
		HoldingObject,
		NotHoldingObject,
	}

	/// <summary>
	/// The current progress made towards completing the task.
	/// </summary>
	public TaskState State {
		get;
		protected set;
	}

	public Condition[] Preconditions {
		get { return preconditions; }
		protected set { preconditions = value; }
	}
	public Condition[] Postconditions {
		get { return postconditions; }
		protected set { postconditions = value; }
	}

	private Condition[] preconditions = new Condition[0];
	private Condition[] postconditions = new Condition[0];

	public Task(Condition[] preconditions,
		Condition[] postconditions) {
		this.preconditions = preconditions;
		this.postconditions = postconditions;
	}

	}
}
