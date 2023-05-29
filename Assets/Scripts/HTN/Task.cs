// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

		public Condition(string condition, bool state) {
			name = condition;
			satisfied = state;
		}
	}

	public struct GoalData {
		public GoalType goalType;
		/// <summary>
		/// The object associated with the goal.
		/// </summary>
		public GameObject goalObject;
		public Transform goalPosition;

		public GoalData(GoalType goalType,
			GameObject goalObject,
			Transform goalPosition) {
			this.goalType = goalType;
			this.goalObject = goalObject;
			this.goalPosition = goalPosition;
		}
	}

	/// <summary>
	/// Used to define what type of goal has been set for the HTN to achieve.
	/// </summary>
	public enum GoalType {
		Undefined,
		Follow,
		MoveTo,
		Stay,
		Pickup,
		Drop,
		LookAround
	}

	public enum TaskState {
		NotStarted,
		Started,
		Executing,
		Succeeded,
		Failed,
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
	public GoalData Goal {
		get { return goal; }
		private set { goal = value; }
	}

	private Condition[] preconditions = new Condition[0];
	private Condition[] postconditions = new Condition[0];
	private GoalData goal = default;

	public Task(Condition[] preconditions,
		Condition[] postconditions,
		GoalData goalInfo) {
		this.preconditions = preconditions;
		this.postconditions = postconditions;
		this.Goal = goalInfo;
	}

	public bool AllConditionsSatisfied(Condition[] conditions) {
		foreach (Condition condition in conditions) {
			if (!condition.satisfied) {
				return false;
			}
		}

		return true;
	}

	public static int MatchingConditions(Condition[] firstConditions, Condition[] secondConditions) {
		int matchingConditions = 0;

		foreach (Condition firstCondition in firstConditions) {
			foreach (Condition secondCondition in secondConditions) {
				if (firstCondition.name == secondCondition.name) {
					++matchingConditions;
				}
			}
		}

		return matchingConditions;
	}

	public static bool MissingCondition(Condition[] requiredConditions, Condition[] otherConditions) {
		foreach (Condition condition in requiredConditions) {
			bool foundCondition = false;

			foreach (Condition otherCondition in otherConditions) {
				if (otherCondition.name == condition.name) {
					foundCondition = true;
					break;
				}
			}

			if (!foundCondition) {
				return false;
			}
		}

		return false;
	}

	/// <summary>
	/// Collects all the conditions of one type from among a collection of tasks.
	/// </summary>
	/// <param name="tasks"> The tasks to search through. </param>
	/// <param name="conditionListType"> The type of conditions to collect. </param>
	/// <returns> A list of all the conditions of one type that exist within 
	/// the task collection. </returns>
	public static Condition[] GatherConditions(Task[] tasks, ConditionLists conditionListType) {
		List<Condition> conditions = new List<Condition>();

		foreach (Task task in tasks) {
			switch (conditionListType) {
				case ConditionLists.Preconditions: {
					foreach (Condition condition in task.Preconditions) {
						conditions.Add(condition);
					}

					break;
				}
				case ConditionLists.Postconditions: {
					foreach (Condition condition in task.Postconditions) {
						conditions.Add(condition);
					}

					break;
				}
				default: {
					break;
				}
			}
		}

		return conditions.ToArray();
	}

	/// <summary>
	/// Allows the task's pre/postconditions to be changed.
	/// </summary>
	/// <param name="conditionList"> The type of list to search. </param>
	/// <param name="conditionToChange"> The condition to chagne. </param>
	/// <param name="newConditionName"> The new name for the condition.
	/// Set to "" to leave the name unchanged. </param>
	/// <param name="newConditionValue"> True if the condition has been met. </param>
	public void ChangeCondition(ConditionLists conditionList,
		string conditionToChange,
		string newConditionName,
		bool newConditionValue) {
		switch (conditionList) {
			case ConditionLists.Preconditions: {
				UpdateCondition(ref preconditions);
				break;
			}
			case ConditionLists.Postconditions: {
				UpdateCondition(ref postconditions);
				break;
			}
			default: {
				break;
			}
		}

		void UpdateCondition(ref Condition[] conditionList) {
			for (int i = 0; i < conditionList.Length; ++i) {
				if (conditionToChange != conditionList[i].name) {
					continue;
				}

				if (newConditionName != "") {
					conditionList[i].name = newConditionName;
				}

				conditionList[i].satisfied = newConditionValue;
			}
		}
	}

	/// <summary>
	/// Updates the type of goal this task is associated with completing.
	/// </summary>
	/// <param name="goalType"> The type of goal the task completes. </param>
	/// <param name="goalObject"> The data related to the goal. Leave empty to 
	/// prevent an update. </param>
	public void UpdateGoal(GoalType goalType,
		GameObject goalObject,
		Transform goalPosition) {
		goal.goalType = goalType;

		if (goalObject) {
			goal.goalObject = goalObject;
		}

		if (goalPosition) {
			goal.goalPosition = goalPosition;
		}
	}

	/// <summary>
	/// Executes a task by calling its associated delegate method.
	/// </summary>
	/// <param name="taskToExecute"> The task to execute. </param>
	/// <param name="taskState"> State of the tasks execution. </param>
	/// <returns> True if the task is working correctly. False if the task has 
	/// stopped working. </returns>
	public bool ExecuteTask(ref Task taskToExecute, ref TaskState taskState) {
		if (taskToExecute is PrimitiveTask &&
			!(taskToExecute is PrimitiveVectorTask) &&
			!(taskToExecute is PrimitiveInteractableTask)) {
			taskState = (taskToExecute as PrimitiveTask).Task();
		} else if (taskToExecute is PrimitiveVectorTask) {
			taskState = (taskToExecute as PrimitiveVectorTask).Task((taskToExecute as PrimitiveVectorTask).Vector);
		} else if (taskToExecute is PrimitiveInteractableTask) {
			taskState = (taskToExecute as PrimitiveInteractableTask).Task((taskToExecute as PrimitiveInteractableTask).Interactable);
		} else if (taskToExecute is CompoundTask) {
			taskState = (taskToExecute as CompoundTask).ExecuteSubtasks();
		}

		switch (taskState) {
			case TaskState.NotStarted: {
				return true;
			}
			case TaskState.Started: {
				return true;
			}
			case TaskState.Executing: {
				return true;
			}
			// Enables all of the current task's postconditions.
			case TaskState.Succeeded: {
				for (int i = 0; i < taskToExecute.Postconditions.Length; ++i) {
					taskToExecute.ChangeCondition(ConditionLists.Postconditions,
						taskToExecute.Postconditions[i].name,
						"",
						true);
				}

				taskToExecute = null;
				return true;
			}
			case TaskState.Failed: {
				return false;
			}
			case TaskState.Cancelled: {
				return false;
			}
			default: {
				return false;
			}
		}
	}
}
