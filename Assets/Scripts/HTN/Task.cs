// Written by Liam Bansal
// Date Created: 11/5/2023

using System.Collections.Generic;
using System.Linq;
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
				return false;
			}
		}

		return true;
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
}
