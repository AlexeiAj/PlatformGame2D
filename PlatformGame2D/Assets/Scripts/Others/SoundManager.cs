using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static AudioClip run, jump, land, slash, explosion, die;
    public Toggle toggleMute;
    static AudioSource audioSrc;

    void Start()
    {
        run = Resources.Load<AudioClip>("run");
        jump = Resources.Load<AudioClip>("jump");
        land = Resources.Load<AudioClip>("land");
        slash = Resources.Load<AudioClip>("slash");
        explosion = Resources.Load<AudioClip>("explosion");
        die = Resources.Load<AudioClip>("die");
        
        if(!audioSrc) audioSrc = GetComponent<AudioSource>();
        DontDestroyOnLoad(audioSrc);
        audioSrc.volume = 0.65f;
        toggleMute.isOn = audioSrc.mute;
    }
    
    public static void PlaySound(string clip)
    {
        switch(clip){
            case "run":
                audioSrc.PlayOneShot(run);
                break;
            case "jump":
                audioSrc.PlayOneShot(jump);
                break;
            case "land":
                audioSrc.PlayOneShot(land);
                break;
            case "slash":
                audioSrc.PlayOneShot(slash);
                break;
            case "explosion":
                audioSrc.PlayOneShot(explosion);
                break;
            case "die":
                audioSrc.PlayOneShot(die);
                break;
        }        
    }

    public void mute(bool mute){
        audioSrc.mute = mute;
    }
}
