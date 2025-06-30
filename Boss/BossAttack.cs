using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [SerializeField] private List<BossBullet> bulletPrefabList = new List<BossBullet>();
    [SerializeField] private Transform fireTrm;
    [SerializeField] private Boss boss;
    [SerializeField] private Transform playerTrm;
    private float _startTime = 4.1f;
    private bool _isPlayerDead;

    private int[] bulletValue = new int[]
    {
        2, // 노말어택
        4, // 파동탄
        5, // 십자탄
        2, // 물결탄
    };

    public Stack<BossBullet>[] BossBulletPoolList = new Stack<BossBullet>[]
    {
        new Stack<BossBullet>(),
        new Stack<BossBullet>(),
        new Stack<BossBullet>(),
        new Stack<BossBullet>()
    };

    private void OnEnable()
    {
        _isPlayerDead = false;
        boss.currentEnum.OnValueChanged += HandleCheckPhase;
        CreateBulletPool();
        StartCoroutine(NormalFireCoroutine());
        StartCoroutine(WaveFireCoroutine());
        StartCoroutine(CrossFireCoroutine());
        StartCoroutine(nameof(GuidedFireCoroutine));
        bulletValue = new int[]
        {
            2, // 노말어택
            3, // 파동탄
            4, // 십자탄
            2, // 물결탄
        };
    }

    public void stopAttack()
    {
        StopAllCoroutines();
    }

    public void HandlePlayerDeadEvent()
    {
        _isPlayerDead = true;
    }

    private void HandleCheckPhase(BossPhase prev, BossPhase next)
    {
        switch (boss.currentEnum.Value)
        {
            case BossPhase.Phase1:
                break;
            case BossPhase.Phase2:
                StopCoroutine(nameof(GuidedFireCoroutine));
                bulletValue[0] = 3;
                bulletValue[1] = 4;
                bulletValue[2] = 5;
                StartCoroutine(nameof(FlowFireCoroutine));
                break;
            case BossPhase.Phase3:
                bulletValue[0] = 4;
                bulletValue[1] = 5;
                bulletValue[2] = 6;
                bulletValue[3] = 3;
                break;
            case BossPhase.Phase4:
                bulletValue[0] = 4;
                bulletValue[1] = 5;
                bulletValue[2] = 6;
                bulletValue[3] = 4;
                StartCoroutine(BombFireCoroutine());
                break;
        }
    }

    #region SpecialFire

    private IEnumerator BombFireCoroutine()
    {
        while (!boss.isDead || !_isPlayerDead)
        {
            if (!boss.isCanHit) continue;

            var targetRotate = GetTargetDir();

            BossBullet bullet = GetBulletInPool(3);
            bullet.transform.SetPositionAndRotation(fireTrm.position, targetRotate);
            bullet.specialBulletEnum = SpecialBulletEnum.Replication;
            bullet.transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
            bullet.moveSpeed = 2f;
            bullet.lifeTime = 2.1f;

            yield return new WaitForSeconds(4.38f);
        }
    }

    private IEnumerator FlowFireCoroutine()
    {
        var rot = 0f;

        while (!boss.isDead || !_isPlayerDead)
        {
            if (!boss.isCanHit) continue;

            rot += 11f;

            for (int i = 1; i < bulletValue[3] + 1; i++)
            {
                float rotate = 360f / bulletValue[3] * i;

                BossBullet bullet = GetBulletInPool(3);
                bullet.transform.SetPositionAndRotation(
                    fireTrm.position, Quaternion.Euler(0f, 0f, rotate + rot));

                bullet.transform.localScale = new Vector3(0.45f, 0.45f, 0);
                bullet.moveSpeed = 3.3f;
                bullet.lifeTime = 5f;
            }

            yield return new WaitForSeconds(0.28f);
        }
    }

    private IEnumerator GuidedFireCoroutine()
    {
        yield return new WaitForSeconds(_startTime);

        while (!boss.isDead || !_isPlayerDead)
        {
            if (!boss.isCanHit) continue;

            var targetRotate = GetTargetDir();

            BossBullet bullet = GetBulletInPool(3);
            bullet.transform.SetPositionAndRotation(fireTrm.position, targetRotate);
            bullet.specialBulletEnum = SpecialBulletEnum.Guided;
            bullet.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
            bullet.moveSpeed = 2.82f;
            bullet.lifeTime = 2.8f;

            yield return new WaitForSeconds(5f);
        }
    }

    #endregion

    #region basicFire

    private IEnumerator NormalFireCoroutine()
    {
        yield return new WaitForSeconds(_startTime);

        while (!boss.isDead || !_isPlayerDead)
        {
            if (!boss.isCanHit) continue;

            var targetRotate = GetTargetDir();

            BossBullet bullet = GetBulletInPool(0);
            bullet.transform.SetPositionAndRotation(fireTrm.position, targetRotate);
            bullet.transform.localScale = Vector3.one * 1f; 

            yield return new WaitForSeconds(1.75f - (bulletValue[0] * 0.2f));
        }
    }

    private IEnumerator WaveFireCoroutine()
    {
        float delayTime = 5.75f;

        yield return new WaitForSeconds(_startTime);

        while (!boss.isDead || !_isPlayerDead)
        {
            if (!boss.isCanHit) continue;

            if (bulletValue[1] >= 6)
            {
                delayTime = 5.25f;
            }

            for (int i = -bulletValue[1]; i < bulletValue[1] + 1; i++)
            {
                var targetRotate = GetTargetDir();

                BossBullet bullet = GetBulletInPool(1);
                bullet.transform.SetPositionAndRotation(
                    fireTrm.position, targetRotate * Quaternion.Euler(0f, 0f, 11.8f * i));
            }

            yield return new WaitForSeconds(delayTime);
        }
    }

    private IEnumerator CrossFireCoroutine()
    {
        yield return new WaitForSeconds(_startTime);

        while (!boss.isDead || !_isPlayerDead)
        {
            if (!boss.isCanHit) continue;

            for (int i = 0; i < bulletValue[2]; i++)
            {
                BossBullet bullet = GetBulletInPool(2);
                bullet.transform.SetPositionAndRotation(
                    fireTrm.position, Quaternion.Euler(0f, 0f, i * (360f / bulletValue[2])));
            }

            yield return new WaitForSeconds(4.25f);
        }
    }

    #endregion

    #region TargetFindRegion

    private Quaternion GetTargetDir()
    {
        Vector2 direction = playerTrm.position - transform.position;

        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRotate = Quaternion.Euler(0, 0, angle);

        return targetRotate;
    }

    #endregion

    #region PoolLogicRegion

    public BossBullet GetBulletInPool(int value)
    {
        if (BossBulletPoolList[value].Count > 0)
        {
            BossBullet bullet = BossBulletPoolList[value].Pop();
            bullet.gameObject.SetActive(true);
            return bullet;
        }
        else
        {
            return Instantiate(bulletPrefabList[value], PoolManager.Instance.transform);
        }
    }

    private void CreateBulletPool()
    {
        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                BossBullet bullet = Instantiate(bulletPrefabList[i], PoolManager.Instance.transform);

                BossBulletPoolList[i].Push(bullet);
                bullet.gameObject.SetActive(false);
            }
        }
    }

    #endregion
}