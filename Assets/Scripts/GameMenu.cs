using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {

	public Button	buttonResume;
	public Button	buttonRetry;
	public Button	buttonExit;
	public Text		textResume;
	public Text		textRetry;
	public Text		textExit;

	public Text		Status;

	public Font		font1;
	public Font		font2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gameManager.instance.gameIsOver) {
			//disable resume on win or loose
			buttonResume.gameObject.SetActive (false);
			//update win/loose/pause status text
			Status.text = "GAME OVER";
			Status.color = Color.red;
		} else {
			Status.text = "PAUSED";
			Status.color = Color.green;
		}
	}

	public void OnMouseEnterResume()
	{
		textResume.font = font2;
	}

	public void OnMouseExitResume()
	{
		textResume.font = font1;
	}

	public void OnClickResume()
	{
		if (!gameManager.instance.gameIsOver)
			gameManager.instance.pause (false);
	}

	public void OnMouseEnterRetry()
	{
		textRetry.font = font2;
	}
	
	public void OnMouseExitRetry()
	{
		textRetry.font = font1;
	}
	
	public void OnClickRetry()
	{
		Application.LoadLevel (Application.loadedLevel);
	}

	public void OnMouseEnterExit()
	{
		textExit.font = font2;
	}
	
	public void OnMouseExitExit()
	{
		textExit.font = font1;
	}
	
	public void OnClickExit()
	{
		Application.LoadLevel ("StartScreen");
	}
}
