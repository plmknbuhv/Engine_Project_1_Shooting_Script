using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BossHp : MonoBehaviour
{
    [SerializeField] public NotifyValue<int> currentHealth;
    [SerializeField] public int maxHealth;
    [SerializeField] public int maxHealth2;
    [SerializeField] private Boss boss;
    [SerializeField] private BossAnime animator;
    [SerializeField] private PlayerImpulse playerImpulse;
    public UnityEvent OnDeadEvent;

    private void OnEnable()
    {
        maxHealth = maxHealth2;
        currentHealth.Value = maxHealth;
    }

    private void Awake()
    {
        currentHealth = new NotifyValue<int>(maxHealth);
        
        currentHealth.OnValueChanged += HandleHpCheck;
    }

    private void HandleHpCheck(int prev, int next)
    {
        if (next != 0) return;
        
        if (boss.currentEnum.Value == BossPhase.Phase4)
        {
            OnDeadEvent?.Invoke();
            GetComponent<AudioSource>().Play();
            StartCoroutine(DeadCoroutine());
        }
        else
        {
            currentHealth.Value = maxHealth += 100;
        }
    }

    private IEnumerator DeadCoroutine()
    {
        yield return new WaitForSeconds(1f);
        
        transform.DOShakePosition(2.5f, 2f);
        playerImpulse.PlayImpulse();
        GameManager.Instance.GameOver();
        animator.SetAnimation("Dead");
        transform.DOKill();
    }

    public void TakeDamage(int damage)
    {
        currentHealth.Value -= damage;
    }
}