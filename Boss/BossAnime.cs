using UnityEngine;

public class BossAnime : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public int animBoolHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animBoolHash = Animator.StringToHash("Idle");
        animator.SetBool(animBoolHash, true);
    }

    public void SetAnimation(string hash)
    {
        animator.SetBool(animBoolHash, false);
        animBoolHash = Animator.StringToHash(hash);
        animator.SetBool(animBoolHash, true);
    }

    public void BossDead()
    {
        Destroy(transform.parent.gameObject);
    }
}