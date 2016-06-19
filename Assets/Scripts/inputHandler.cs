using UnityEngine;
using System.Collections;

public class inputHandler : MonoBehaviour {

	public static inputHandler instance { get; private set; }

	public Texture2D menuCursor;
	public Vector2   menuCursor_center = new Vector2 (3, 0);
	public Texture2D playCursor;
	public Vector2   playCursor_center = new Vector2 (6, 6);
	
	void Awake () {
		if (!instance)
			instance = this;
	}

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(playCursor, playCursor_center, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (gameManager.instance.isPaused) {
				gameManager.instance.pause(false);
			} else {
				gameManager.instance.pause(true);
			}
		}
	}

	public static void cameraTo (Vector3 position) {
		Camera.main.transform.position = new Vector3 (position.x, position.y, Camera.main.transform.position.z);
	}

	public static RaycastHit2D mouseCast() {
		return Physics2D.Raycast (inputHandler.mousePoint(), Vector2.zero);
	}
	public static RaycastHit2D[] mouseCastAll() {
		return Physics2D.RaycastAll (inputHandler.mousePoint(), Vector2.zero);
	}

	public static Vector2 mousePoint () {
		return Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}
}
