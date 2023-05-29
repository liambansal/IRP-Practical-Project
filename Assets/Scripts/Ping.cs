// Written by Liam Bansal
// Date Created: 9/5/2023

using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A UI element that temporarily displays an image above targeted game-object.
/// </summary>
public class Ping : MonoBehaviour {
	/// <summary>
	/// An interface containing all the relevant information about a
	/// game-object to populate it's ping UI element.
	/// </summary>
	public interface IPingInfo {
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
	private TextMeshProUGUI pingName = null;

	public void SetPing(IPingInfo pingInfo) {
		pingName.text = pingInfo.ObjectName;
	}

	public void SetPing(string pingName) {
		this.pingName.text = pingName;
	}

	private void Awake() {
		pingName = GetComponentInChildren<TextMeshProUGUI>();
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
