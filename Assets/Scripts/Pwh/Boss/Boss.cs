using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sr;
    [SerializeField] private Animator ani;
    [SerializeField] private int _dieCount;

    [SerializeField] private GameObject BossHpCount;

    private Sprite sprite;

    private readonly int _dissolve = Shader.PropertyToID("_Dissolve");
    private readonly string _isDissolve = "_IsDissolve";
    private readonly string _isHit = "_IsSolidColor";

    [SerializeField] private GameObject _dashImage;

    private int dieCount = 5;

    private GameObject saveTypeCount = null;

    BossSkill bossSkill;

    private CapsuleCollider2D _hitAble;

    private void RenderControl()
    {
        //쉐이더 값 초기화
        _sr.material.SetInt(_isHit, 0);
        _sr.material.SetInt(_isDissolve, 0);
        _sr.material.SetFloat(_dissolve, 1f);
    }

    private void Awake()
    {
        _hitAble = GetComponent<CapsuleCollider2D>();
        bossSkill = GetComponent<BossSkill>();
        _sr = GetComponentInChildren<SpriteRenderer>();
        ani = GetComponentInChildren<Animator>();
        bossSkill.Attack();
    }

    private void Update()
    {
        StartCoroutine(Hit());
    }

    private IEnumerator Hit()
    {
        _sr.material.SetInt("_IsSolidColor", 1);
        yield return new WaitForSeconds(.1f);
        _sr.material.SetInt("_IsSolidColor", 0);
    }
    public void Die()
    {
        _hitAble.enabled = false;
        ObjectActive();

        //SoundManager.Instance.PlayBossDie(); //사운드

        //EnemySpawner.Instance.isBossDead = true; // 죽는 변수

        StartCoroutine(DieDissolve(1));
    }

    private void ObjectActive()
    {
        _dashImage.SetActive(false);
    }

    private IEnumerator DieDissolve(float time)
    {
        _sr.material.SetInt(_isDissolve, 1);
        float currentRate;
        float percent = 0;
        float currentTime = 0;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / time;
            currentRate = Mathf.Lerp(1, -1, percent);
            _sr.material.SetFloat(_dissolve, currentRate);

            yield return null;
        }

        Destroy(gameObject); // 죽으면 풀링 넣기
    }
}
