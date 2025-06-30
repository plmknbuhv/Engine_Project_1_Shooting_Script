using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private List<GameObject> hpBarList = new List<GameObject>();
    [SerializeField] private BossHp bossHealth;
    [SerializeField] private Boss boss;
    private bool _isDelay;

    private void OnEnable()
    {
        bossHealth.currentHealth.OnValueChanged += HandleChangeHpBar;
        foreach (var item in hpBarList)
        {
            item.transform.localScale = Vector3.one;
        }
    }

    private void HandleChangeHpBar(int prev, int next)
    {
        if (boss.currentEnum.Value == BossPhase.Phase4 && next <= -5) return;
        {
            hpBarList[(int)boss.currentEnum.Value].transform.localScale
                = new Vector3((float)next / (float)bossHealth.maxHealth, 0);
        }
        
        hpBarList[(int)boss.currentEnum.Value].transform.localScale
            = new Vector3((float)next / (float)bossHealth.maxHealth, 1);

        if (next == 0)
        {
            if (_isDelay) return;

            _isDelay = true;
            boss.currentEnum.Value++;
            StartCoroutine(DelayCoroutine());
        }
    }

    private IEnumerator DelayCoroutine()
    {
        yield return new WaitForSeconds(3);
        _isDelay = false;
    }
}