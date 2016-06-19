using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class Weapon : MonoBehaviour {

	public Ammo ammo;
	public int ammo_count = 20;
	public float fireRate = 0.2f;
	public bool auto = false;
	public bool melee = false;
	public float alertDistance = 5f;

	private float lastFired;

	public gameUnit held { get; private set; }
	public Vector3 held_position = new Vector3 (-0.235f, 0, 0);
	public Quaternion held_rotation = Quaternion.Euler (0, 0, -90);

	void Awake () {
		lastFired = fireRate;
		held = null;
	}
	
	void Update () {
		if (!gameManager.instance.isPaused) {
			lastFired += Time.deltaTime;
			if (held && held == playerHandler.instance.character) {
//			transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector2)transform.position - inputHandler.mousePoint()) * Quaternion.Euler (0, 0, -90);
				transform.rotation = Quaternion.Slerp (
					transform.rotation,
					Quaternion.LookRotation (Vector3.forward, (Vector2)transform.position - inputHandler.mousePoint ()) * Quaternion.Euler (0, 0, -90),
					0.1f
				);
			} else if (held && held.GetComponent<Enemy> ().status == Enemy.Status.aggro) {
				transform.rotation = Quaternion.Slerp (
					transform.rotation,
					Quaternion.LookRotation (Vector3.forward, (Vector2)transform.position - (Vector2)playerHandler.position()) * Quaternion.Euler (0, 0, -90),
					0.1f
				);
			}
		}
	}

	public void fire (bool isPlayer, Vector2 target) {
		if (ammo && ammo_count > 0 && lastFired >= fireRate) {
			lastFired = 0.0f;
			if (isPlayer) {
				if (!melee) {
					ammo_count -= 1;
					gameManager.instance.alertEnemies(this);
				}
				if (ammo_count == 0) {
					GetComponent<SpriteRenderer> ().color = Color.red;
				}
				ammo.gameObject.layer = LayerMask.NameToLayer("Player Ammo");
			} else {
				ammo.gameObject.layer = LayerMask.NameToLayer("Enemy Ammo");
			}
			Ammo shell = GameObject.Instantiate (ammo, transform.position, transform.rotation) as Ammo;
			shell.setDirection((target - (Vector2)transform.position).normalized);
		}
	}

	public Weapon equip (gameUnit unit) {
		if (!held) {
			GetComponent<BoxCollider2D> ().enabled = false;
			transform.parent = unit.transform;
			transform.localPosition = held_position;
			transform.localRotation = held_rotation;
			held = unit;
			return this;
		}
		Debug.Log ("Weapon "+this+" already held!");
		return null;
	}

	public void drop () {
		if (held) {
			GetComponent<BoxCollider2D> ().enabled = true;
			transform.position = transform.parent.transform.position;
			transform.rotation = transform.parent.transform.rotation;
			transform.parent = null;
			held = null;
		} else {
			Debug.Log ("Weapon " + this + " already dropped!");
		}
	}
}
