using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DisgendPattern;

public class PlayerManager : SingleTon<PlayerManager>
{
    [SerializeField] private BaseEntity _entity;
    
    PlayerMain _pMain;
    Rigidbody2D _rb;

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
        _pMain.Update();

        _rb.velocity = _pMain._direction;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_pMain._isAtt) return;

        //적 hp 함수 사용
    }
}
