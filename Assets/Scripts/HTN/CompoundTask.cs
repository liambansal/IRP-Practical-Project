// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;

/// <summary>
/// A collection of tasks to be executed in a sequence.
/// </summary>
public class CompoundTask : Task {
	public Stack<Task> Subtasks {
		get;
		private set;
	}

	public CompoundTask(Stack<Task> subtasks,
		Condition[] preconditions,
		Condition[] postconditions,
		GoalData goalInfo) : base(preconditions,
		postconditions,
		goalInfo) {
		Preconditions = preconditions;
		Postconditions = postconditions;
		Subtasks = subtasks;
	}

	// TODO: create a method that executes subtasks one by one.
	// If a subtask is a compound task then use recursion on the method above.
}
