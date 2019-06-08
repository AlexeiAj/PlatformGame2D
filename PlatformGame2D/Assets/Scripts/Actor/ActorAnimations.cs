using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimations
{
    private Animator anim;

    public ActorAnimations(Animator anim){
        this.anim =  anim;
    }

    public void play(string name, bool value){
        switch(name){
            case "isRunning":{
                anim.SetBool(name, value);
                break;
            }
            case "isJumping":{
                anim.SetBool(name, value);
                break;
            }
            case "takeOf":{
                anim.SetTrigger(name);
                break;
            }
            default:{
                break;
            }
        }
    }
}
