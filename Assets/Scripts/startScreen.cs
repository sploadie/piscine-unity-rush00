using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class startScreen : MonoBehaviour {

	public Text titleText;
	public Font font1;
	public Font font2;

	private int lastCurrent = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (titleText != null) {
			int current = (int)Time.time % titleText.text.Length;
			if (current != lastCurrent) {
				char[] tmp = titleText.text.ToLower ().ToCharArray();
				tmp [current] = char.ToUpper (tmp [current]);
				titleText.text = new string(tmp);
				lastCurrent = current;

				titleText.font = (int)Time.time % 2 == 0 ? font1 : font2;
			}
		}
	}

	public void OnClickButtonQuit () {
		Debug.Log ("Quitting game");
		Application.Quit ();
	}
	
	public void OnClickButtonPlay () {
		Debug.Log ("Lauching game");
		Application.LoadLevel ("Demo");
	}
}
