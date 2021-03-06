﻿using UnityEngine;
using System.Collections;

public class playerHandler : MonoBehaviour {

	public static playerHandler instance { get; private set; }

	public gameUnit character;

	private float eHeldTime = 0;

	void Awake () {
		if (!instance)
			instance = this;
	}

	// Use this for initialization
	void Start () {
		inputHandler.cameraTo(character.transform.position);
		character.Death.AddListener (onDeath);
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameManager.instance.isPaused && !character.dead) {
			Vector3 cameraPosition = character.transform.position;
			if (Input.GetKeyDown(KeyCode.E)) {
				eHeldTime = 0f;
			}
			if (Input.GetKey (KeyCode.E)) {
				cameraPosition = Vector3.Lerp(character.transform.position, character.transform.position + new Vector3 ((Input.mousePosition.x - (Screen.width * 0.5f)) / Screen.width, (Input.mousePosition.y - (Screen.height * 0.5f)) / Screen.height, character.transform.position.z) * 5f, eHeldTime);
//				cameraPosition += new Vector3 ((Input.mousePosition.x - (Screen.width * 0.5f)) / Screen.width, (Input.mousePosition.y - (Screen.height * 0.5f)) / Screen.height, 0f) * 5f;
				eHeldTime += Time.deltaTime * 3;
			}
			inputHandler.cameraTo(cameraPosition);
			character.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector2)character.transform.position - inputHandler.mousePoint());
			character.body.velocity = new Vector2 (Input.GetAxis ("Horizontal") * character.speed, Input.GetAxis ("Vertical") * character.speed);
			if (Input.GetMouseButtonDown (0)) {
				characterFire();
			} else if (Input.GetMouseButtonDown (1)) {
				if (character.weapon) {
					Debug.Log ("Dropping");
					character.drop ();
				} else {
					Debug.Log ("Picking up");
					foreach (RaycastHit2D hit in Physics2D.RaycastAll (character.transform.position, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Weapon"))) {
						if (hit.collider && hit.collider.tag == "weapon") {
							Weapon weapon = hit.collider.gameObject.GetComponent<Weapon> ();
							if (weapon && !weapon.held) {
								Debug.Log ("Equipping Weapon: " + weapon);
								character.equip(weapon);
								break;
							}
						}
					}
				}
			} else if (Input.GetMouseButton (0) && character.weapon && character.weapon.auto) {
				characterFire();
			}
		}
		character.body.angularVelocity = 0.0f;
	}

	private void onDeath() {
		gameManager.instance.Invoke ("gameOver", 0.9f);
	}

	private void characterFire() {
		if (character.weapon) {
			character.weapon.fire (true, inputHandler.mousePoint());
		}
	}

	public static Vector3 position() {
		return instance.character.transform.position;
	}
}
