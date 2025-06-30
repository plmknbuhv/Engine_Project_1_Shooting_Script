using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public enum BossPhase
{   
    Phase1,
    Phase2,
    Phase3,
    Phase4,
}

public class Boss : MonoBehaviour
{
    [SerializeField] private List<GameObject> disableObjectList;
    [SerializeField] private List<GameObject> enableObjectList;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BossEngineAnimator animator;
    public NotifyValue<BossPhase> currentEnum;
    public GameObject bulletBombEffect;
    private BossHp _bossHp;
    public Player player;
    public bool isCanHit;
    public bool isDead;

    private void Awake()
    {
        _bossHp = GetComponent<BossHp>();
        currentEnum.Value = BossPhase.Phase1;
    }

    private void OnEnable()
    {
        transform.position = new Vector3(0, 7.38f);
        currentEnum.Value = BossPhase.Phase1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCanHit || _bossHp.currentHealth.Value <= 0) return;
        
        if (other.CompareTag("Bullet"))
        {
            _bossHp.TakeDamage(5);
            spriteRenderer.DOKill();
            spriteRenderer.color = Color.red;
            spriteRenderer.DOColor(Color.white,0.5f);
        }
    }

    #region BossLeave

    public void BossBattleLeave()
    {
        StartCoroutine(BossBattleLeaveCoroutine());
    }

    private IEnumerator BossBattleLeaveCoroutine()
    {
        yield return new WaitForSeconds(1.8f);
        Tween myTween = transform.DOMoveY(-9f, 2.7f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.8f);
        animator.SetAnimation("EngineEffect");
        yield return myTween.WaitForCompletion();
        yield return new WaitForSeconds(1);
        
        foreach (var item in enableObjectList)
        {
            item.SetActive(true);
        }
        GameManager.Instance.text.text = $"{++GameManager.Instance.playerCount} Dead";

        foreach (var item in disableObjectList)
        {
            item.SetActive(false);
        }

        GameManager.Instance.isCanGame = true;
    }

    #endregion

    #region BossStart

    public void BossBattleStart()
    {
        StartCoroutine(BossBattleStartCoroutine());
    }

    private IEnumerator BossBattleStartCoroutine()
    {
        animator.SetAnimation("EngineEffect");
        Tween myTween = transform.DOMoveY(0.3f, 1.8f);
        yield return myTween.WaitForCompletion();
        animator.SetAnimation("Idle");
        isCanHit = true;
    }

    #endregion
}