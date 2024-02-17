using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject Door;
    public GameObject NowCamera;
    public GameObject NextCamera;
    public int type;
    public Vector3[] offset = new Vector3[4];
    public Vector3[] ControlCenter = new Vector3[4];

    // 이동 대상 문 좌표
    private Vector3 Npos;
    private PlayerMoving PS;
    
    void Start()
    {
        Npos = Door.transform.position;
        PS = Player.GetComponent<PlayerMoving>();
    }

    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("Player")){
            if(Input.GetKeyDown("f")){
                PS.Center += ControlCenter[type];
                other.transform.position = Npos+offset[type];
                NowCamera.SetActive(false);
                NextCamera.SetActive(true);
            }
        }
    }
}
