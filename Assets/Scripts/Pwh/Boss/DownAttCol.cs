using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAttCol : BossMain
{
    [SerializeField] private BossSkill bossSkill;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            bossSkill._isAtt = true;

        bossSkill._colObj = collision.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            bossSkill._isAtt = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            bossSkill._isAtt = false;
    }
}
