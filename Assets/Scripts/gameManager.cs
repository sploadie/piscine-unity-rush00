using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Image))]
public class gameManager : MonoBehaviour {

	public static gameManager instance { get; private set; }
	private float tmpTimeScale = 1;

	public bool isPaused { get; private set; }

	public Weapon[] weapon_list;
	public Sprite[] enemy_list;

	void Awake () {
		if (!instance)
			instance = this;
		isPaused = false;
	}

	// Use this for initialization
	void Start () {
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	// Pause
	public void pause(bool paused) {
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

	// Change speed
	public void changeSpeed(float speed) {
		Time.timeScale = speed;
	}
	
	public Weapon getWeapon() {
		if (weapon_list.Length > 0)
			return weapon_list[Random.Range (0, weapon_list.Length)];
		return null;
	}

	public Sprite getEnemySprite() {
		if (enemy_list.Length > 0)
			return enemy_list[Random.Range (0, enemy_list.Length)];
		return null;
	}
}
