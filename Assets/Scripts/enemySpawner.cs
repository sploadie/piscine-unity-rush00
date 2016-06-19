using UnityEngine;
using System.Collections;

public class enemySpawner : MonoBehaviour {

	public Enemy	unit;
	public int		spawnMinimum = 1;
	public int		spawnPlusInterval = 100;

	private float timer = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (gameManager.instance.enemies.Count < spawnMinimum) {
			Enemy newSpawn = GameObject.Instantiate (unit, transform.position, transform.rotation) as Enemy;
			newSpawn.status = Enemy.Status.patrol;
		}
		if (timer > spawnPlusInterval) {
			timer = 0f;
			spawnMinimum++;
		}
	}
}
