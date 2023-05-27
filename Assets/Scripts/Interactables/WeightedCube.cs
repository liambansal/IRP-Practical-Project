// Written by Liam Bansal
// Date Created: 27/5/2023

using UnityEngine;

public class WeightedCube : Interactable, Interactable.ICanTrigger {
	public bool Active {
		get { return active; }
		set { }
	}

	private bool active = true;

	protected override void Awake() {
		base.Awake();
	}

	protected override void Update() {
		base.Update();
	}
}
