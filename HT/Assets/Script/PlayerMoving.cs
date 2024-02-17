using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float Speed;
    public float x, y;
    public Vector3 Center;

    private float xdif = 26.788f, ydif = 17.18f;

    void Start()
    {
        Speed = 30f;
        x = 0; y = 0;
        Center = new Vector3(0, 0, 0);
        StartCoroutine("ColtrolOffset");
    }

    void Update()
    {
        Vector3 pos = transform.position;
        x = pos.x; y = pos.y;
        GetKeyMoving(pos);
    }

    public void GetKeyMoving(Vector3 pos)
    {
        if (Input.GetKey("w"))
        {
            if (y < Center.y + ydif)
                y += Speed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            if (y > Center.y - ydif)
                y -= Speed * Time.deltaTime;
        }
        if (Input.GetKey("d"))
        {
            if (x < Center.x + xdif)
                x += Speed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            if (x > Center.x - xdif)
                x -= Speed * Time.deltaTime;
        }

        transform.position = new Vector3(x, y, pos.z);
    }

    IEnumerator ColtrolOffset()
    {
        while (true) // 계속 반복
        {
            Vector3 pos = transform.position;
            x = pos.x; y = pos.y;
            if (y > Center.y + ydif) y = Center.y + ydif;
            else if (y < Center.y - ydif) y = Center.y - ydif;
            if (x > Center.x + xdif) x = Center.x + xdif;
            else if (x < Center.x - xdif) x = Center.x - xdif;

            yield return null; // 한 프레임 대기
        }
    }
}
