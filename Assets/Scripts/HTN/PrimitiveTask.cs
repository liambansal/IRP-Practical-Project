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
		Condition[] preconditions,
		Condition[] postconditions) : base(preconditions,
			postconditions) {
		Task = task;
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
		Condition[] preconditions,
		Condition[] postconditions) : base(null,
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
public class PrimitiveInteractableTask : PrimitiveTask {
	public delegate TaskState InteractableMethod(Interactable interactable);

	public InteractableMethod Task {
		get;
		protected set;
	}
	public Interactable Interactable {
		get;
		protected set;
	}

	public PrimitiveInteractableTask(InteractableMethod task,
		Condition[] preconditions,
		Condition[] postconditions) : base(null,
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
