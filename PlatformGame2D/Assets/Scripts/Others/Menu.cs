using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject PauseUI;
	private bool paused = false;

	void Start(){
		PauseUI.SetActive(false);
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)) paused = !paused;

		if(paused) pauseGame();
		else play();
	}

	void pauseGame(){
		PauseUI.SetActive(true);
		Time.timeScale = 0;
	}

	void play(){
		PauseUI.SetActive(false);
		Time.timeScale = 1;
	}

	public void Resume(){
		paused = false;
	}
}
