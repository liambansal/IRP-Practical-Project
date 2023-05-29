// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;

/// <summary>
/// A collection of tasks to be executed in a sequence.
/// </summary>
public class CompoundTask : Task {
	private Stack<Task> subtasks = null;

	public CompoundTask(Stack<Task> subtasks,
		Condition[] preconditions,
		Condition[] postconditions,
		GoalData goalInfo) : base(preconditions,
		postconditions,
		goalInfo) {
		Preconditions = preconditions;
		Postconditions = postconditions;
		this.subtasks = subtasks;
	}

	// TODO: create a method that executes subtasks one by one.
}
