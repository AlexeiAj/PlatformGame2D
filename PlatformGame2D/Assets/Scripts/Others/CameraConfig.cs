using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraConfig : MonoBehaviour
{
    static private Animator animCam;
    private float dampTime = 0.001f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;

    void Start(){
        animCam = GetComponent<Animator>();
    }
    
	void Update () 
	{
		if (target)
		{
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.4f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}else{
			Invoke("LoadLevel", 0.7f);
		}
	}

    public static void shake(){
        animCam.SetTrigger("shake");
    }

	private void LoadLevel(){
        SceneManager.LoadScene("Game");
    }
}
