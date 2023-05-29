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
	}

	public struct GoalData {
		public GoalType goalType;
		/// <summary>
		/// The object associated with the goal.
		/// </summary>
		public GameObject goalObject;

		public GoalData(GoalType goalType, GameObject goalObject) {
			this.goalType = goalType;
			this.goalObject = goalObject;
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

		foreach (Condition condition in firstConditions) {
			if (secondConditions.Contains(condition)) {
				++matchingConditions;
			}
		}

		return matchingConditions;
	}

	public static bool MissingCondition(Condition[] requiredConditions, Condition[] otherConditions) {
		foreach (Condition condition in requiredConditions) {
			if (!otherConditions.Contains(condition)) {
				return true;
			}
		}

		return false;
	}

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
		GameObject goalObject) {
		goal.goalType = goalType;

		if (goalObject) {
			goal.goalObject = goalObject;
		}
	}
}
