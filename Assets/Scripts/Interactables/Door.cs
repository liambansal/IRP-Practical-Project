// Written by Liam Bansal
// Date Created: 27/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Interactable;

public class Door : MonoBehaviour, ITrigger {
	public bool Active {
		get;
		set;
	}
	public ITrigger Trigger {
		get;
		set;
	}

	private bool active = false;
	private ITrigger trigger = null;
}
