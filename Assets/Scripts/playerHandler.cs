using UnityEngine;
using System.Collections;

public class playerHandler : MonoBehaviour {

	public static playerHandler instance { get; private set; }

	public gameUnit character;

	void Awake () {
		if (!instance)
			instance = this;
	}

	// Use this for initialization
	void Start () {
		inputHandler.cameraTo(character.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
		if (!inputHandler.instance.pause) {
			inputHandler.cameraTo(character.transform.position);
			character.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector2)character.transform.position - inputHandler.mousePoint());
			character.body.velocity = new Vector2 (Input.GetAxis ("Horizontal") * character.speed, Input.GetAxis ("Vertical") * character.speed);
			if (Input.GetMouseButtonDown (0)) {
				characterFire();
			} else if (Input.GetMouseButtonDown (1)) {
				Debug.Log ("Right Click:");
				if (character.weapon) {
					Debug.Log ("Dropping");
					character.drop ();
				} else {
					Debug.Log ("Picking up");
					foreach (RaycastHit2D hit in Physics2D.RaycastAll (character.transform.position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Weapon"))) {
						if (hit.collider && hit.collider.tag == "weapon") {
							Debug.Log ("Found "+hit.collider.gameObject);
							Weapon weapon = hit.collider.gameObject.GetComponent<Weapon> ();
							Debug.Log ("Weapon: "+weapon);
							if (weapon && !weapon.held) {
								Debug.Log ("Equipping Weapon");
								character.equip(weapon);
								break;
							}
						}
					}
				}
			} else if (Input.GetMouseButton (0)) {
				characterFire();
			}
		}
	}

	private void characterFire() {
		if (character.weapon) {
			character.weapon.fire (true);
		}
	}
}
