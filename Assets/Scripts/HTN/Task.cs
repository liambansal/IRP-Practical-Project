// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;
using UnityEngine;

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
		this.Preconditions = preconditions;
		this.Postconditions = postconditions;
	}
}

public class SingleTask : Task {
	public delegate TaskState Method();

	public Method Task {
		get;
		protected set;
	}

	public SingleTask(Method task,
		List<Condition> preconditions,
		List<Condition> postconditions) : base(preconditions,
		postconditions) {
		this.Preconditions = preconditions;
		this.Postconditions = postconditions;
	}
}

public class SingleTaskVector : Task {
	public delegate TaskState Method(Vector3 vector);

	public Method Task {
		get;
		protected set;
	}
	public Vector3 Vector {
		get;
		protected set;
	}

	public SingleTaskVector(Method task,
		Vector3 vector,
		List<Condition> preconditions,
		List<Condition> postconditions) : base(preconditions,
		postconditions) {
		Task = task;
		Vector = vector;
		this.Preconditions = preconditions;
		this.Postconditions = postconditions;
	}
}

public class SingleTaskInteractable : Task {
	public delegate TaskState Method(Interactable interactable);

	public Method Task {
		get;
		protected set;
	}
	public Interactable Interactable {
		get;
		protected set;
	}

	public SingleTaskInteractable(Method task,
		Interactable interactable,
		List<Condition> preconditions,
		List<Condition> postconditions) : base(preconditions,
		postconditions) {
		Task = task;
		Interactable = interactable;
		Preconditions = preconditions;
		Postconditions = postconditions;
	}
}

// TODO: The subtasks appear to be the AI's actions.
// Tasks are defined as the outcome from an unknown series of action, that's
// what is HTN needs to figure out. The Tasks' subtasks are chosen based on
// which ones satisfy the tasks preconditions, therefore a task is like a HTN's
// goal task.
/// <summary>
/// Holds a delegate to another class' method with preceonditions that need to 
/// be satisfied in order to execute the method.
/// Aimed to be used by a HTN.
/// </summary>
public class CompoundTaskClass : Task {
	private Stack<Task> subtasks = null;

	public CompoundTaskClass(List<Condition> preconditions,
		List<Condition> postconditions,
		Stack<Task> subtasks) : base(preconditions,
		postconditions) {
		this.Preconditions = preconditions;
		this.Postconditions = postconditions;
		this.subtasks = subtasks;
	}
}
