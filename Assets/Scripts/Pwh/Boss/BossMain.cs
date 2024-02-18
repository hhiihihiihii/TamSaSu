using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossMain : MonoBehaviour
{
    protected BossValue _bossValue;
    protected Rigidbody2D _rb;
    protected Animator _animator;

    protected virtual void Awake()
    {
        _bossValue = transform.GetComponent<BossValue>();
        _rb = transform.GetComponent<Rigidbody2D>();
        _animator = transform.GetComponentInChildren<Animator>();
    }
}
