﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadLevel(string scene){
        SceneManager.LoadScene(scene);
    }

    public void QuitLevel(){
        Application.Quit();
    }
}