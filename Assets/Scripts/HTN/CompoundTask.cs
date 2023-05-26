// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;

/// <summary>
/// A collection of tasks to be executed in a sequence.
/// </summary>
public class CompoundTask : Task {
	private Stack<Task> subtasks = null;

	public CompoundTask(Condition[] preconditions,
		Condition[] postconditions,
		Stack<Task> subtasks) : base(preconditions,
		postconditions) {
		Preconditions = preconditions;
		Postconditions = postconditions;
		this.subtasks = subtasks;
	}
}
