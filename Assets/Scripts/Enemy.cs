using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public gameUnit unit;
	public bool aggro = true;
	public float maxAlertDistance = 0.2f;
	
	private bool	onAlert = false;
	private Vector2 alertPosition;
	private float	alertTime;

	// Use this for initialization
	void Start () {
		unit.GetComponent<SpriteRenderer> ().sprite = gameManager.instance.getEnemySprite ();
		unit.equip((Weapon)GameObject.Instantiate (gameManager.instance.getWeapon (), transform.position, transform.rotation));
		gameManager.instance.enemies.Add (this);
	}
	
	// Update is called once per frame
	void Update () {
		if (unit.dead) {
			aggro = false;
			onAlert = false;
			gameManager.instance.enemies.Remove(this);
			Destroy (this);
			return;
		}
		if (!gameManager.instance.isPaused) {
			// Calculate State
			searchForPlayer();
			if (onAlert && Vector2.Distance (transform.position, alertPosition) < unit.collider.radius + maxAlertDistance) {
				onAlert = false;
			}
			// Do state
			if (aggro) {
				unit.sprite.color = new Color(0.6f,1f,0.6f);
				if (unit.weapon.melee)
					unit.body.velocity = ((Vector2)playerHandler.position() - (Vector2)transform.position).normalized * unit.speed;
				unit.transform.rotation = Quaternion.LookRotation (Vector3.forward, (Vector2)unit.transform.position - (Vector2)playerHandler.position ());
				unit.weapon.fire (false, (Vector2)playerHandler.position ());
			} else if (onAlert) {
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

	public void alert(gameUnit player) {
		if (Vector2.Distance (transform.position, player.transform.position) <= player.weapon.alertDistance) {
			onAlert = true;
			alertPosition = player.transform.position;
			alertTime = Time.time;
		}
	}

	private void searchForPlayer() {
		RaycastHit2D hit = Physics2D.Raycast (transform.position, (playerHandler.position() - transform.position).normalized, Mathf.Infinity, LayerMask.GetMask("Player", "Wall"));
		if (hit.collider && hit.collider.tag == "Player") {
			aggro = true;
		} else {
			if (aggro == true) {
				onAlert = true;
				alertPosition = playerHandler.position();
				alertTime = Time.time;
			}
			aggro = false;
		}
	}
}
