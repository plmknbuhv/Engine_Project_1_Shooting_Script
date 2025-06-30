using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> rendererList = new List<SpriteRenderer>();
    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private GameObject shield;
    [SerializeField] private Transform targetTrm;
    [SerializeField] private Boss boss;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject shieldEffect;
    [SerializeField] private AudioSource audioSource;
    private PlayerMovement _playerMovement;
    private PlayerImpulse _playerImpulse;
    private WeaponFire _weaponFire;
    public bool isCanEffect = true;
    private bool _isHaveShield = true;
    public UnityEvent OnDeadEvent;
    private bool _isDead;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        _weaponFire = transform.Find("Weapon").GetComponent<WeaponFire>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerImpulse = GetComponent<PlayerImpulse>();
        boss.currentEnum.OnValueChanged += HandleReShield;
    }

    private void HandleReShield(BossPhase prev, BossPhase next)
    {
        if (_isHaveShield) return;
        
        _isHaveShield = true;
        shield.SetActive(true);
        Destroy(Instantiate(shieldEffect, transform)
            , 0.99f);
    }

    private void OnEnable()
    {
        _isHaveShield = true;
        shield.SetActive(true);
    }

    public void StopHit()
    {
        _isDead = true;
        isCanEffect = false;
        animator.SetBool("Idle", true);
    }

    private void Update()
    {
        if (_isDead) return;
        
        Vector2 direction = targetTrm.position - transform.position;
        
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0,0, angle);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Boss") || other.CompareTag("BossBullet"))
        {
            StartCoroutine(HitEffectCoroutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss") || other.CompareTag("BossBullet"))
        {
            StartCoroutine(HitEffectCoroutine());
        }
    }

    private IEnumerator HitEffectCoroutine()
    {
        if (isCanEffect)
        {
            isCanEffect = false;
            GameObject effect = Instantiate(effectPrefab, transform.position, quaternion.identity);
            effect.transform.localScale *= 2f;
            Destroy(effect, 0.99f);
            _playerImpulse.PlayImpulse();

            if (_isHaveShield)
            {
                shield.SetActive(false);
                StartCoroutine(ShipBlinkCoroutine());
                _isHaveShield = false;
            }
            else if(!_isHaveShield)
            {
                _playerMovement.isCanMove = false;
                transform.DOShakePosition(2.5f, 0.8f);
                transform.DOMoveY(transform.position.y - 20, 4.8f).SetEase(Ease.Linear);
                _weaponFire.isCanFire = false;
                _weaponFire.animator.SetBool("Idle", true);
                
                audioSource.Play();
                OnDeadEvent?.Invoke();
                boss.BossBattleLeave();
            }
            
            yield return new WaitForSeconds(2.8f); 
        }
        yield return null;
    }

    private IEnumerator ShipBlinkCoroutine()
    {
        for (int i = 0; i < 5; i++)
        {
            foreach (var item in rendererList)
            {
                var color = item.color;
                color.a = 0.26f;
                item.color = color;
            }
            
            yield return new WaitForSeconds(0.28f);
            
            foreach (var item in rendererList)
            {
                var color = item.color;
                color.a = 1f;
                item.color = color;
            }
            
            yield return new WaitForSeconds(0.28f);
        }

        isCanEffect = true;
    }
}