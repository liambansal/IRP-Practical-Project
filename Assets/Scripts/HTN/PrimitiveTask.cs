// Written by Liam Bansal
// Date Created: 22/5/2023

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A task class that holds a delegate for executing a single action.
/// </summary>
public class PrimitiveTask : Task {
	public delegate TaskState Method();

	public Method Task {
		get;
		protected set;
	}

	public PrimitiveTask(Method task,
		List<Condition> preconditions,
		List<Condition> postconditions) : base(preconditions,
			postconditions) {
		Preconditions = preconditions;
		Postconditions = postconditions;
	}
}

/// <summary>
/// A task class that holds a delegate for executing a single action with a 
/// vector parameter.
/// </summary>
public class PrimitiveVectorTask : PrimitiveTask {
	public delegate TaskState VectorMethod(Vector3 vector);

	public VectorMethod Task {
		get;
		protected set;
	}
	public Vector3 Vector {
		get;
		protected set;
	}

	public PrimitiveVectorTask(VectorMethod task,
		Vector3 vector,
		List<Condition> preconditions,
		List<Condition> postconditions) : base(null,
			preconditions,
			postconditions) {
		Task = task;
		Vector = vector;
		Preconditions = preconditions;
		Postconditions = postconditions;
	}
}

/// <summary>
/// A task class that holds a delegate for executing a single action with an
/// interactable parameter.
/// </summary>
public class PrimitiveTaskInteractable : PrimitiveTask {
	public delegate TaskState InteractableMethod(Interactable interactable);

	public InteractableMethod Task {
		get;
		protected set;
	}
	public Interactable Interactable {
		get;
		protected set;
	}

	public PrimitiveTaskInteractable(InteractableMethod task,
		List<Condition> preconditions,
		List<Condition> postconditions) : base(null,
			preconditions,
			postconditions) {
		Task = task;
		Interactable = null;
		Preconditions = preconditions;
		Postconditions = postconditions;
	}

	public void SetInteractable(Interactable interactable) {
		if (!interactable) {
			return;
		}

		Interactable = interactable;
	}
}
