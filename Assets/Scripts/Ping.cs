// Written by Liam Bansal
// Date Created: 9/5/2023

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
