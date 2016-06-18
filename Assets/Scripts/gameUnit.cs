using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class gameUnit : MonoBehaviour {

	public float speed = 3.0f;
	public bool dead = false;
	
	public Weapon weapon { get; private set; }

	public Rigidbody2D body { get; private set; }
	
	void Awake () {
		body = GetComponent<Rigidbody2D> ();
		weapon = null;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void equip (Weapon wpn) {
		if (!weapon) {
			weapon = wpn.equip(this);
		} else {
			Debug.Log ("Unit "+this+" not holding a weapon!");
		}
	}

	public void drop () {
		if (weapon) {
			weapon.drop ();
			weapon = null;
		} else {
			Debug.Log ("Unit "+this+" not holding a weapon!");
		}
	}
}
