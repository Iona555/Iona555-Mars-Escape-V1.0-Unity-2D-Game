using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseScript : MonoBehaviour
{	
	public bool gamePaused;
	public bool gameOver;
	public GameObject pauseMenuCanvas;
	public GameObject ResumeButton;
	public GameObject GameOverText;
	private Scene currentScene;

	// Comunicate with the GameStateMachine
	public GameStateMachine GSM;

	// Comunicate with the ActionMenu
	public actionMenuScript AM;

	void Start () {
		gameOver = false;
		GSM = GameObject.Find ("GameManager").GetComponent<GameStateMachine>();
		AM = GameObject.Find ("ActionMenu").GetComponent<actionMenuScript>();
	}

	// Update is called once per frame
	void Update ()
	{
		if (GSM.GameState == GameStateMachine.GameStateEnum.GAME_OVER && !gameOver) {
			gameOver = true;
			gamePaused = true;
			ResumeButton.SetActive (false);
			GameOverText.SetActive (true);
			if (GSM.winners == "The Cosmonauts")
				GameOverText.GetComponent<Text> ().color = new Color (0.0f/255.0f, 84.0f/255.0f, 251.0f/255.0f);
			else
				GameOverText.GetComponent<Text> ().color = new Color (255.0f/255.0f, 12.0f/255.0f, 12.0f/255.0f);
			GameOverText.GetComponent<Text> ().text = GSM.winners + " have won!";
		}

		if (Input.GetKeyDown (KeyCode.Escape))
		{
			gamePaused = !gamePaused;
		}
		
		if (gamePaused)
		{
			pauseMenuCanvas.SetActive (true);
			if (Time.timeScale == 1) // if the game is running
				Time.timeScale = 0; // pause the game
		}
		else
		{
			pauseMenuCanvas.SetActive (false);
			if(!AM.panelVisible)
				Time.timeScale = 1; // resume
		}
	}

	public void Resume()
	{
		if(gamePaused)
			gamePaused = !gamePaused;
		if (Time.timeScale == 0) // if the game is paused
			if(!AM.panelVisible)
				Time.timeScale = 1; // resume the game
		// Time.timeScale is the speed of the whole game
		// 1 = normal speed
		// 0.5 = 2x slower
		// 0 = stop/pause0
	}

	public void Restart()
	{
		if(gamePaused)
			gamePaused = !gamePaused;
		if (Time.timeScale == 0) // if the game is paused
			Time.timeScale = 1; // resume the game
		currentScene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (currentScene.name);
	}

	public void Exit()
	{
		Application.Quit ();
	}
}
