using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public gameUnit unit;
	public bool seekPlayer = false;
	public float maxAlertPositionBuffer = 0.2f;

	public float viewFront = 10f;
	public float viewAngle = 55f;
	public float viewPeripheral = 1f;
	public float stealthRatio = 0.3f;

	public enum Status{ko, bored, patrol, searching, alert, aggro};
	public Status			status;

	private Vector2			alertPosition;
	private int				alertPathIndex;
	private int				alertPathCount;
	private List<Vector2>	alertPath = null;
	
	void OnDrawGizmos () {
		if (alertPath != null) {
			int i = alertPathIndex;
			while (i < alertPathCount) {
				Gizmos.color = Color.red;
				if (i == 0)
					Gizmos.DrawLine (transform.position, alertPath[0]);
				else
					Gizmos.DrawLine (alertPath[i-1], alertPath[i]);
				i++;
			}
		}
		if (playerHandler.instance) {
			Gizmos.color = Color.blue;
			Gizmos.DrawLine (transform.position, (transform.position + ((Vector3)playerHandler.position () - transform.position).normalized * checkSight ()));
		}
	}

	void Awake() {
//		status = Status.bored;
	}

	// Use this for initialization
	void Start () {
		unit.GetComponent<SpriteRenderer> ().sprite = gameManager.instance.getRandomEnemySprite ();
		unit.equip((Weapon)GameObject.Instantiate (gameManager.instance.getRandomWeapon (), transform.position, transform.rotation));
		gameManager.instance.enemies.Add (this);
	}
	
	// Update is called once per frame
	void Update () {
		if (unit.dead) {
//			aggro = false;
//			onAlert = false;
			gameManager.instance.enemies.Remove(this);
			Destroy (this);
			return;
		}
		if (seekPlayer) {
			seekPlayer = false;
			alert(playerHandler.position());
		}
		if (status == Status.patrol) {
			status = Status.searching;
			createPath (gameManager.instance.getRandomWaypoint ().position, true);
		}
		if (!gameManager.instance.isPaused) {
			// Calculate State
			searchForPlayer();
			// Do state
			if (status == Status.aggro) {
				unit.sprite.color = new Color(0.6f,1f,0.6f);
				if (unit.weapon.melee)
					unit.body.velocity = ((Vector2)playerHandler.position() - (Vector2)transform.position).normalized * unit.speed;
				unit.transform.rotation = Quaternion.LookRotation (Vector3.forward, (Vector2)unit.transform.position - (Vector2)playerHandler.position ());
				unit.weapon.fire (false, (Vector2)playerHandler.position ());
			} else if (alertPath != null) {
				unit.sprite.color = new Color(0.6f,0.6f,1f);
				unit.transform.rotation = Quaternion.LookRotation (Vector3.forward, (Vector2)unit.transform.position - alertPosition);
				unit.body.velocity = (alertPosition - (Vector2)transform.position).normalized * unit.speed;
			} else {
				unit.sprite.color = Color.white;
				unit.body.velocity = Vector2.zero;
			}
			unit.body.angularVelocity = 0f;
		}
	}

	public void getNextPathPoint() {
		if (alertPath != null) {
			// If reached next waypoint
			if (Vector2.Distance (transform.position, alertPosition) < unit.hitbox.radius + maxAlertPositionBuffer) {
				alertPathIndex++;
				if (alertPathIndex < alertPathCount) {
					alertPosition = alertPath [alertPathIndex];
				} else {
					// If reach alert point (but player not found)
					status = Status.searching;
					createPath (gameManager.instance.getRandomWaypoint ().position, true);
				}
			}
		} else {
			status = Status.bored;
		}
	}

	public void createPath(Vector3 alertPoint, bool fast) {
		alertPath = gameManager.instance.getPath (transform.position, alertPoint, LayerMask.GetMask ("Wall"), fast);
		//		alertPath = gameManager.instance.getPath (transform.position, alertPoint, LayerMask.GetMask ("Player", "Wall"), "Player");
		if (alertPath != null) {
			alertPathIndex = 0;
			alertPathCount = alertPath.Count;
			alertPosition = alertPath [alertPathIndex];
		} else {
			status = Status.bored;
		}
	}

	public void alert(Vector3 alertPoint) {
		status = Status.alert;
		createPath (alertPoint, false);
	}

	private float checkSight() {
		float angle = viewAngle;
		if (status > Status.bored) {
			angle *= 1.5f;
		}
		if (Vector2.Angle ((playerHandler.position () - transform.position).normalized, -transform.up) < viewAngle)
			return viewFront;
		else 
			return viewPeripheral + (playerHandler.instance.character.body.velocity.magnitude * stealthRatio);
	}

	private void searchForPlayer() {
		float viewDistance = checkSight ();
		RaycastHit2D hit = Physics2D.Raycast (transform.position, (playerHandler.position() - transform.position).normalized, viewDistance, LayerMask.GetMask("Player", "Wall"));
		if (hit.collider && hit.collider.tag == "Player") {
			status = Status.aggro;
		} else {
			if (status == Status.aggro) {
				alert(playerHandler.position());
			} else {
				getNextPathPoint();
			}
		}
	}
}
