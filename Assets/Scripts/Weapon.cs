using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Weapon : MonoBehaviour {

	public Ammo ammo;
	public float fireRate = 0.2f;

	private float lastFired;

	public bool held { get; private set; }
	public Vector3 held_position = new Vector3 (-0.235f, 0, 0);
	public Quaternion held_rotation = Quaternion.Euler (0, 0, -90);

	void Start () {
		lastFired = fireRate;
		held = false;
	}
	
	void Update () {
		if (!inputHandler.instance.pause) {
			lastFired += Time.deltaTime;
			if (held) {
//			transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector2)transform.position - inputHandler.mousePoint()) * Quaternion.Euler (0, 0, -90);
				transform.rotation = Quaternion.Slerp (
				transform.rotation,
				Quaternion.LookRotation (Vector3.forward, (Vector2)transform.position - inputHandler.mousePoint ()) * Quaternion.Euler (0, 0, -90),
				0.1f
				);
			}
		}
	}

	public void fire (bool player) {
		if (ammo && lastFired >= fireRate) {
			lastFired = 0.0f;
			if (player)
				ammo.gameObject.layer = LayerMask.NameToLayer("Player Ammo");
			else
				ammo.gameObject.layer = LayerMask.NameToLayer("Enemy Ammo");
			Ammo shell = GameObject.Instantiate (ammo, transform.position, transform.rotation) as Ammo;
			shell.setDirection((inputHandler.mousePoint() - (Vector2)transform.position).normalized);
		}
	}

	public Weapon equip (gameUnit unit) {
		if (!held) {
			transform.parent = unit.transform;
			transform.localPosition = held_position;
			transform.localRotation = held_rotation;
			held = true;
			return this;
		}
		Debug.Log ("Weapon "+this+" already held!");
		return null;
	}

	public void drop () {
		if (held) {
			transform.position = transform.parent.transform.position;
			transform.rotation = transform.parent.transform.rotation;
			transform.parent = null;
			held = false;
		} else {
			Debug.Log ("Weapon " + this + " already dropped!");
		}
	}
}
