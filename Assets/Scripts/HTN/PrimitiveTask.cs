// Written by Liam Bansal
// Date Created: 22/5/2023

using System.Collections.Generic;
using UnityEngine;

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
		Interactable interactable,
		List<Condition> preconditions,
		List<Condition> postconditions) : base(null,
			preconditions,
			postconditions) {
		Task = task;
		Interactable = interactable;
		Preconditions = preconditions;
		Postconditions = postconditions;
	}
}
