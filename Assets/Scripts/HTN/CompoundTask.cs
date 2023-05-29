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

	private Task currentTaskToExecute = null;
	private TaskState currentTaskState = TaskState.NotStarted;

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

	public TaskState ExecuteSubtasks() {
		if (currentTaskToExecute == null ||
			currentTaskState == TaskState.Succeeded) {
			currentTaskToExecute = Subtasks.Pop();
			currentTaskState = TaskState.NotStarted;
		}

		currentTaskToExecute.ExecuteTask(ref currentTaskToExecute, ref currentTaskState);

		if (currentTaskState == TaskState.Succeeded && Subtasks.Count > 0) {
			currentTaskToExecute = null;
			currentTaskState = TaskState.NotStarted;
		}

		return currentTaskState;
	}
}
