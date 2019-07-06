using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{
    public Text textDisplay;
    public GameObject continueButton;
    public GameObject background;
    private List<string> sentences = new List<string>();
    private int i;
    private float typingSpeed = 0.02f;

    void Start(){
        Invoke("tutorialDialog", 2f);
    }

    public void tutorialDialog(){
        sentences.Add("Heyyy, [A e D] para andar");
        sentences.Add("[Space] para pular");
        sentences.Add("[E] para pegar itens");

        doDialog(sentences);
    }

    public void doDialog(List<string> sentences){
        if(background.activeSelf) return;

        i = 0;
        this.sentences = sentences;
        background.SetActive(true);
        StartCoroutine(type());
    }

    private IEnumerator type(){
        foreach(char letter in sentences[i].ToCharArray()){
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        continueButton.SetActive(true);
    }

    private void nextSentence(){
        continueButton.SetActive(false);
        background.SetActive(false);
        textDisplay.text = "";

        if(i < sentences.Count -1){
            i++;
            background.SetActive(true);
            StartCoroutine(type());
        } 
    }
}
