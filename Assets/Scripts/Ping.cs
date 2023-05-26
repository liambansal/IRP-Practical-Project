// Written by Liam Bansal
// Date Created: 9/5/2023

using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// A UI element that temporarily displays an image above targeted game-object.
/// </summary>
public class Ping : MonoBehaviour {
	/// <summary>
	/// An interface containing all the relevant information about a
	/// game-object to populate it's ping UI element.
	/// </summary>
	public interface PingInfo {
		public string ObjectName {
			get;
			set;
		}
		public Vector3 WorldPosition {
			get;
			set;
		}
	}

	/// <summary>
	/// How long in seconds the ping is displayed on-screen for.
	/// </summary>
	private float lifetime = 4.0f;
	private TextElement pingName = null;

	public void SetPing(PingInfo pingInfo) {
		pingName.text = pingInfo.ObjectName;
	}

	private void Awake() {
		pingName = GetComponent<TextElement>();
	}

	private void Update() {
		DestructionTimer();
	}

	/// <summary>
	/// Counts down to zero and destroys the game-object this script is 
	/// attached to when the countdown is reached.
	/// </summary>
	private void DestructionTimer() {
		lifetime -= Time.deltaTime;

		if (lifetime <= 0) {
			Destroy(gameObject);
		}
	}
}
