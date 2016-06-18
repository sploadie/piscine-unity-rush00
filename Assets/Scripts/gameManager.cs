using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(Image))]
public class gameManager : MonoBehaviour {

	public static gameManager instance { get; private set; }
	private float tmpTimeScale = 1;

	void Awake () {
		if (!instance)
			instance = this;
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
		if (paused == true) {
			tmpTimeScale = Time.timeScale;
			Time.timeScale = 0;
		}
		else
			Time.timeScale = tmpTimeScale;
	}

	// Change speed
	public void changeSpeed(float speed) {
		Time.timeScale = speed;
	}
}
