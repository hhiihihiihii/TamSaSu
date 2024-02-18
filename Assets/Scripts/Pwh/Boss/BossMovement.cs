using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : BossMain
{
    [SerializeField] private float _distance;
    [SerializeField] private CapsuleCollider2D _crashCol;

    private SpriteRenderer _sr;

    private bool isStop = false;

    void Start()
    {
        _sr = GetComponentInChildren<SpriteRenderer>();

        _bossValue._playerTr = GameObject.Find("Player").GetComponent<Transform>();

        _bossValue._isShoot = false;
        _bossValue._isDownAttack = false;
    }

    void Update()
    {
        OnMove();
        //print(_fence);
    }

    public void OnMove()
    {
        //float dis = Vector2.Distance(transform.position, _bossValue._playerTr.position);

        //보스 점프시 위로 올라가는
        if (_bossValue._isJump)
        {

            transform.position = Vector2.Lerp(transform.position, _bossValue.saveTr, 8f * Time.deltaTime);
            float dis = Vector3.Distance(transform.position, _bossValue.saveTr);

            if (dis <= 1)
                _bossValue._isJump = false;

        }
        else if (!isStop && !_bossValue._isShoot && !_bossValue._isDash && !_bossValue._isDownAttack)
        {

            _rb.velocity = Vector2.zero;
            transform.position = Vector2.MoveTowards(transform.position, _bossValue._playerTr.position, _bossValue._speed * Time.deltaTime);

        }

        if (_bossValue._isDownAttack)
            _crashCol.enabled = false;
        else
            _crashCol.enabled = true;

        if (_bossValue._playerTr.position.x < transform.position.x)
            _sr.flipX = true;
        else
            _sr.flipX = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isStop = true;
            _animator.SetTrigger("attack");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isStop = false;
        }
    }
}
