using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDialog : MonoBehaviour
{
    private Dialog dialog;
    public List<string> sentences;
    public float checkPlayerRadius = 8f;
    public GameObject keyT;
    private GameObject KeyTInstance;

    void Start(){
        dialog = GameObject.Find("DialogManager").GetComponent<Dialog>();
    }

    private void Update() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, checkPlayerRadius, LayerMask.GetMask("Player"));
        bool show = false;
        for (int i = 0; i < colliders.Length; i++) if(colliders[i].gameObject.tag == "Player") show = true;

        addKeyT(show);

        if(show && Input.GetButton("Fire4")) dialog.doDialog(sentences);
    }

    private void addKeyT(bool show){
        var pos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2.5f, gameObject.transform.position.z);
        if(show && !KeyTInstance) KeyTInstance = Instantiate(keyT, pos, Quaternion.identity);
        if(!show && KeyTInstance) Destroy(KeyTInstance);
        if(KeyTInstance) KeyTInstance.transform.position = pos;
    }
}
