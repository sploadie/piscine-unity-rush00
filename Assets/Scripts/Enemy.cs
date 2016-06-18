using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public gameUnit unit;
	public bool aggro = true;

	// Use this for initialization
	void Start () {
		unit.GetComponent<SpriteRenderer> ().sprite = gameManager.instance.getEnemySprite ();
		unit.equip((Weapon)GameObject.Instantiate (gameManager.instance.getWeapon (), transform.position, transform.rotation));
	}
	
	// Update is called once per frame
	void Update () {
		if (unit.dead)
			aggro = false;
		if (!gameManager.instance.isPaused) {
			if (aggro) {
				unit.transform.rotation = Quaternion.LookRotation (Vector3.forward, (Vector2)unit.transform.position - (Vector2)playerHandler.position ());
				unit.weapon.fire (false, (Vector2)playerHandler.position ());
			}
			if (!unit.dead)
				unit.body.velocity = Vector2.zero;
		}
	}
}
