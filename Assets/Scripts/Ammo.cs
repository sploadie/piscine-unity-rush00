using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Ammo : MonoBehaviour {

	public float speed = 10.0f;
	public float timeOut = 5.0f;

	public Vector3 direction { get; private set; }

	void Awake () {
		direction = Vector3.zero;
	}
	
	void Update () {
		timeOut -= Time.deltaTime;
		if (timeOut < 0)
			GameObject.Destroy (this.gameObject);
//		transform.position += direction * speed * Time.deltaTime;
	}

	public void setDirection (Vector3 new_direction) {
		direction = new_direction;
//		Debug.Log ("New direction: " + direction);
		GetComponent<Rigidbody2D> ().velocity = direction * speed;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		gameUnit victim = coll.gameObject.GetComponent<gameUnit>();
		if (victim) {
			victim.dead = true;
		}
	}
}
