using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour {

	public List<Waypoint> links = new List<Waypoint>();

	void OnDrawGizmos() {
		Gizmos.DrawIcon(transform.position, "map.png", false);
		foreach (Waypoint link in links) {
			if (link) {
				Vector3 target = link.transform.position;
				Gizmos.color = Color.green;
				Gizmos.DrawLine(transform.position, target);
			}
		}
	}
}
