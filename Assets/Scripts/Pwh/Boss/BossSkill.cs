using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkill : BossMain
{
    [Header("쿨타임")]
    [SerializeField] private float _shootCool;
    [SerializeField] private float _dashCool;

    [Header("총알")]
    [SerializeField] private float _bulletSpeed;

    [Header("대쉬")]
    [SerializeField] protected float _waitDashTime;
    [SerializeField] protected float _dashingTime;
    [SerializeField] protected float _knockPower;

    [Header("점프 공격")]
    [SerializeField] private float _waitDownAttack;

    [Header("오브젝트")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _downAttackImg;
    [SerializeField] private GameObject _dashImage;

    [Header("콜라이더")]
    [SerializeField] private CapsuleCollider2D JumpCol;

    [Header("파티클")]
    [SerializeField] private GameObject _jumpAttEffect;

    public bool _isAtt = false;
    [HideInInspector] public GameObject _colObj;

    protected bool _isAiming = false;

    private bool _isJumpStart = false;
    private bool _isKnock = false;

    private float _stunCool = 2f;
    private float dTime;

    private Vector3 viewDir = Vector3.zero;

    private List<Bullet> saveBulletList = new List<Bullet>();
    private List<float> BulletAngleList = new List<float>();

    private Bullet bullet = null;

    int angleCount = 12;
    int defaultAngle;

    protected override void Awake()
    {
        base.Awake();

        defaultAngle = 360 / angleCount;

        _bossValue._isDash = false;

        _bossValue._playerTr = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        //대쉬 이미지 에이밍
        if (_isAiming)
            DashAiming();

        //넉백 되었을 때 플레이어 밀려남
        if (_isKnock)
        {
            _bossValue._playerTr.gameObject.GetComponent<Rigidbody2D>().AddForce(viewDir * _knockPower, ForceMode2D.Impulse);
            _isKnock = false;
        }
    }

    public void Attack()
    {
        _isJumpStart = false;

        StopAllCoroutines();
        StartCoroutine(ShootRoutine());
        StartCoroutine(DashRoutine());

        StartCoroutine(JumpRoutine());

        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }

    IEnumerator DashRoutine()
    {
        dTime = _dashCool - _waitDashTime - _dashingTime;
        while (true)
        {

            yield return new WaitWhile(() => _isJumpStart);
            yield return new WaitForSeconds(dTime);

            ReadyDash();

            yield return new WaitForSeconds(_waitDashTime);

            _isAiming = false;

            yield return new WaitForSeconds(0.5f);

            Dashing();

            yield return new WaitForSeconds(_dashingTime);

            _bossValue._isDash = false;
            _isJumpStart = true;
        }
    }
    #region 대쉬 요소
    private void ReadyDash()
    {
        _bossValue._isDash = true;
        _isAiming = true;
    }

    private void DashAiming()
    {
        viewDir = _bossValue._playerTr.position - transform.position;

        float angle = Mathf.Atan2(viewDir.y, viewDir.x) * Mathf.Rad2Deg;

        _dashImage.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90));

        //if (!EnemySpawner.Instance.isBossDead) //보스가 죽었을 때

        _dashImage.SetActive(true);
    }

    private void Dashing()
    {
        _dashImage.SetActive(false);

        _rb.velocity = viewDir.normalized * _bossValue._DashSpeed;
        //SoundManager.Instance.PlayBossDashAtk(); //사운드
    }
    #endregion
    IEnumerator ShootRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(_shootCool - _stunCool);

            _bossValue._isShoot = true;
            saveBulletList.Clear();

            _animator.SetTrigger("attack");

            for (int i = 0; i < angleCount; i++)
            {
                ShootBullet(defaultAngle * i, transform.position);
                BulletAngleList.Add(defaultAngle * i);
            }
            StartCoroutine(LastBossShoot());

            yield return new WaitForSeconds(_stunCool);

            //다시 움직임
            _bossValue._isShoot = false;
        }
    }

    IEnumerator LastBossShoot()
    {
        yield return new WaitForSeconds(1);
        foreach (var b in saveBulletList)
        {
            //Bullet bullet = b;
            b.BulletPool();
            for (int i = 0; i < 12; i++)
            {
                if (i % 2 == 0)
                    Shoot(BulletAngleList[i], b.transform.position);
            }
        }

        BulletAngleList.Clear();
    }

    IEnumerator JumpRoutine()
    {
        while (true)
        {
            yield return new WaitWhile(() => !_isJumpStart);
            yield return new WaitForSeconds(2.5f);

            JumpAttSet();

            yield return new WaitWhile(() => _bossValue._isJump);

            ShowJumpAtt();

            yield return new WaitForSeconds(_waitDownAttack);

            _downAttackImg.SetActive(false);

            _bossValue._isJump = true;

            Instantiate(_jumpAttEffect, transform);

            if (_isAtt)
            {
                //player hp 깎는 함수 넣기
                if (_colObj.TryGetComponent<PlayerMain>(out PlayerMain pMain))
                    pMain.DecHp(10);
            }

            yield return new WaitWhile(() => _bossValue._isJump);

            JumpAttEnd();
        }
    }
    #region 점프 요소
    private void JumpAttSet()
    {
        JumpCol.enabled = true;

        _bossValue._isDownAttack = true;
        _bossValue.saveTr = transform.position;
        _bossValue.saveTr.y = transform.position.y + 5;
        _bossValue._isJump = true;
    }

    private void ShowJumpAtt()
    {
        _downAttackImg.SetActive(true);
        _bossValue.saveTr = _bossValue._playerTr.position;
        _downAttackImg.transform.position = _bossValue.saveTr;
    }

    private void JumpAttEnd()
    {
        _bossValue._isDownAttack = false;
        _isJumpStart = false;
        JumpCol.enabled = false;
    }
    #endregion 점프 요소

    private void ShootBullet(float angle, Vector2 pos)
    {
        Shoot(angle, pos);
        //SoundManager.Instance.PlayBossShootAtk(); //사운드
        saveBulletList.Add(bullet);
    }

    private void Shoot(float angle, Vector2 pos)
    {
        // 총알을 발사할 각도를 라디안으로 변환
        float radianAngle = angle * Mathf.Deg2Rad;

        // 총알의 방향 벡터 계산
        Vector2 direction = new Vector2(Mathf.Cos(radianAngle), Mathf.Sin(radianAngle));

        // 총알 생성 및 설정
        bullet = Instantiate(_bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = pos;

        //GameObject bullet = Instantiate(_bulletPrefab);
        //bullet.transform.position = pos;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * _bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (_bossValue._isDash)
                Knockback();
        }
    }

    private void Knockback()
    {
        _isKnock = true;

        _rb.velocity = Vector2.zero;
    }
}
