using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static AudioClip run, jump, land, slash, explosion;
    static AudioSource audioSrc;
    
    void Start()
    {
        run = Resources.Load<AudioClip>("run");
        jump = Resources.Load<AudioClip>("jump");
        land = Resources.Load<AudioClip>("land");
        slash = Resources.Load<AudioClip>("slash");
        explosion = Resources.Load<AudioClip>("explosion");

        audioSrc = GetComponent<AudioSource>();
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
        }        
    }
}
