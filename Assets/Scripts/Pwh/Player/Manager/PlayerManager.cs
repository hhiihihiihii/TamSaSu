using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DisgendPattern;

public class PlayerManager : SingleTon<PlayerManager>
{
    [SerializeField] private BaseEntity _entity;

    PlayerMain _pMain;
    Rigidbody2D _rb;

    private Transform _nearEnemy;

    void Awake()
    {
        _pMain = new PlayerMain();
        _pMain._base = _entity;

        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _pMain.Start();
    }

    void Update()
    {
        Attack();
        _pMain.Update();

        _rb.velocity = _pMain._direction;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            float distance = Vector2.Distance(transform.position, collision.transform.position);

            // 가장 가까운 적 업데이트
            if (_nearEnemy == null || distance < Vector2.Distance(transform.position, _nearEnemy.position))
            {
                _nearEnemy = collision.transform;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _nearEnemy = null;
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && _nearEnemy != null)
        {
            //적 hp감소 함수
            print("sss");
        }
    }
}
