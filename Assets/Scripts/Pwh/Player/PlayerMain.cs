using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

public class PlayerMain :PlayerMainValue
{
    private int _hp;

    public void Start()
    {
        _hp = _base.hp;
    }

    public void Update()
    {
        _direction = Move();
        _isAtt = Attack();
    }

    public bool Attack()
    {
        Debug.Log("ht");
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(x, y).normalized;
        return dir * _base.speed * Time.deltaTime;
    }

    public void DecHp(int value)
    {
        _hp -= value;
    }

    public void Die()
    {
        if (_hp > 0) return;

        //죽는 애니메이션
    }
}
