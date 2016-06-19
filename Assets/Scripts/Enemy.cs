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
		}
		if (!gameManager.instance.isPaused) {
			// Calculate State
			if (onAlert && Vector2.Distance (transform.position, alertPosition) < unit.collider.radius + maxAlertDistance) {
				onAlert = false;
			}
			// Do state
			if (aggro) {
				unit.sprite.color = new Color(0.6f,1f,0.6f);
				unit.transform.rotation = Quaternion.LookRotation (Vector3.forward, (Vector2)unit.transform.position - (Vector2)playerHandler.position ());
				unit.weapon.fire (false, (Vector2)playerHandler.position ());
			} else if (onAlert) {
				unit.sprite.color = new Color(0.6f,0.6f,1f);
				unit.transform.rotation = Quaternion.LookRotation (Vector3.forward, (Vector2)unit.transform.position - alertPosition);
				unit.body.velocity = (alertPosition - (Vector2)transform.position).normalized * unit.speed;
			} else if (!unit.dead) {
				unit.sprite.color = Color.white;
				unit.body.velocity = Vector2.zero;
			}
			if (!unit.dead)
				unit.body.angularVelocity = 0f;
		}
	}

	public void alert(gameUnit player)
	{
		if (Vector2.Distance (transform.position, player.transform.position) <= player.weapon.alertDistance) {
			onAlert = true;
			alertPosition = player.transform.position;
			alertTime = Time.time;
		}
	}
}
