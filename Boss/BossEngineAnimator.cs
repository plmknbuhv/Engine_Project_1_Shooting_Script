using System;
using UnityEngine;

public class BossEngineAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public SpriteRenderer spriteRenderer;
    private int animHash;

    private void Awake()
    {
        animHash = Animator.StringToHash("Idle");
    }

    private void OnEnable()
    {
        SetAnimation("Idle");
    }

    public void SetAnimation(string animName)
    {
        animator.SetBool(animHash, false);
        animHash = Animator.StringToHash(animName);
        animator.SetBool(animHash, true);
    }
}
