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
		if(Input.GetButtonDown("Cancel")){
			if(!paused) pauseGame(true);
			else play();
		}
	}

	public void pauseGame(bool exibeMenu){
		paused = true;
		PauseUI.SetActive(exibeMenu);
		Time.timeScale = 0;
	}

	public void play(){
		paused = false;
		PauseUI.SetActive(false);
		Time.timeScale = 1;
	}
}
