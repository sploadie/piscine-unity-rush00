using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(unitSounds))]
public class gameManager : MonoBehaviour {

	public static gameManager instance { get; private set; }
	private float tmpTimeScale = 1;
	public bool debug = true;
	public bool isPaused { get; private set; }

	public unitSounds sounds;
	public Weapon[] weapon_list;
	public Sprite[] enemy_list;

	public List<Enemy> enemies;
	public List<Waypoint> waypoints;

	private bool gameIsOver = false;

	void Awake () {
		if (!instance)
			instance = this;
		isPaused = false;
		sounds = GetComponent<unitSounds> ();
		enemies = new List<Enemy> ();
		waypoints = new List<Waypoint> ();
	}

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	// Pause
	public void pause(bool paused = true) {
		if (gameIsOver)
			return;
		isPaused = paused;
		if (paused == true) {
			tmpTimeScale = Time.timeScale;
			Time.timeScale = 0;
			Cursor.SetCursor(inputHandler.instance.menuCursor, inputHandler.instance.menuCursor_center, CursorMode.Auto);
		} else {
			Time.timeScale = tmpTimeScale;
			Cursor.SetCursor(inputHandler.instance.playCursor, inputHandler.instance.playCursor_center, CursorMode.Auto);
		}
	}

	public void gameOver() {
		sounds.Play("Lose");
		Debug.Log ("GAME OVER");
		pause (true);
		gameIsOver = true;
	}

	// Change speed
	public void changeSpeed(float speed) {
		Time.timeScale = speed;
	}
	
	public Weapon getRandomWeapon() {
		if (weapon_list.Length > 0)
			return weapon_list[Random.Range (0, weapon_list.Length)];
		return null;
	}
	
	public Sprite getRandomEnemySprite() {
		if (enemy_list.Length > 0)
			return enemy_list[Random.Range (0, enemy_list.Length)];
		return null;
	}
	
	public Waypoint getRandomWaypoint() {
		if (waypoints.Count > 0)
			return waypoints[Random.Range (0, waypoints.Count)];
		return null;
	}

	public List<Vector2> getPath(Vector2 startPoint, Vector2 endPoint, LayerMask mask, bool fast) {
		Waypoint start = null;
		float startDistance = 500f;
		Waypoint end = null;
		float endDistance = 500f;
		float distance = Vector2.Distance(startPoint, endPoint);
		RaycastHit2D hit = Physics2D.Raycast (startPoint, (endPoint - startPoint).normalized, distance, mask);
		if (hitTarget(hit, null)) {
			Debug.Log("Found straight path to player!");
			List<Vector2> path = new List<Vector2> ();
			path.Add (endPoint);
			return path;
		}
		foreach (Waypoint wp in waypoints) {
			distance = Vector2.Distance(wp.position, startPoint);
			if (distance < startDistance) {
				hit = Physics2D.Raycast (wp.position, (startPoint - wp.position).normalized, distance, mask);
				if (hitTarget(hit, null)) {
					start = wp;
					startDistance = distance;
				}
			}
			distance = Vector2.Distance(wp.position, endPoint);
			if (distance < endDistance) {
				hit = Physics2D.Raycast (wp.position, (endPoint - wp.position).normalized, distance, mask);
				if (hitTarget(hit, null)) {
					end = wp;
					endDistance = distance;
				}
			}
		}
		Debug.Log("Start: " + start + "\nEnd: " + end);
		if (start && end) {
			List<Vector2> path = end.getPathFrom(start, fast, new List<Waypoint> ());
//			List<Vector2> path = new List<Vector2> (); // DEBUG
//			Debug.Log("Path: " + path);
			Debug.Log("Path: " + path);
			if (path != null) {
				path.Add (endPoint);
				return path;
			}
		}
		Debug.Log("NO PATH FOUND");
		return null;
	}

	private bool hitTarget(RaycastHit2D hit, string targetTag) {
		if ((targetTag == null && hit.collider == null) || (targetTag != null && hit.collider != null && hit.collider.tag == targetTag))
			return true;
		return false;
	}

	// RoxTeddy
	public void alertEnemies(Weapon weapon) {
		Vector3 playerPostion = playerHandler.position ();
		float alertDistance = weapon.alertDistance;
		foreach (Enemy e in enemies) {
			if (Vector2.Distance (e.transform.position, playerPostion) <= alertDistance) {
				Debug.Log ("Alerting enemy!");
				e.alert (playerPostion);
			}
		}
	}
}
