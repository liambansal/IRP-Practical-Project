// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;

/// <summary>
/// A collection of tasks to be executed in a sequence.
/// Aimed to be used by a HTN.
/// </summary>
public class CompoundTask : Task {
	private Stack<Task> subtasks = null;

	public CompoundTask(List<Condition> preconditions,
		List<Condition> postconditions,
		Stack<Task> subtasks) : base(preconditions,
		postconditions) {
		Preconditions = preconditions;
		Postconditions = postconditions;
		this.subtasks = subtasks;
	}
}
