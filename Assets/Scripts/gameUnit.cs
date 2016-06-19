using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class gameUnit : MonoBehaviour {

	public float speed = 3.0f;
	public bool dead = false;
	
	public Weapon weapon { get; private set; }

	public Rigidbody2D body { get; private set; }
	public CircleCollider2D collider { get; private set; }
	public SpriteRenderer sprite { get; private set; }

	private float dying = 0;

	void Awake () {
		body = GetComponent<Rigidbody2D> ();
		collider = GetComponent<CircleCollider2D> ();
		sprite = GetComponent<SpriteRenderer> ();
		weapon = null;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		if (dead && !dying) {
		if (dead) {
			if (dying == 0) {
				drop ();
				Destroy (this.gameObject, 1.0f);
			}
			dying += Time.deltaTime;
			sprite.color = Color.Lerp(Color.white, new Color(1,0,0,0), dying);
		}
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
