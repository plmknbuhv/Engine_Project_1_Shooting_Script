using Unity.Mathematics;
using UnityEngine;

public enum BossBulletEnum
{
    Bullet1,
    Bullet2,
    Bullet3,
    Bullet4,
}

public enum SpecialBulletEnum
{
    None,
    Guided,
    Replication
}

public class BossBullet : MonoBehaviour
{
    [SerializeField] private bool isDead;
    public BossBulletEnum bulletEnum;
    private BossAttack _bossAttack;
    public float moveSpeed = 4;
    public float lifeTime = 2f;
    private float _timer;
    private Boss _boss;
    public SpecialBulletEnum specialBulletEnum;

    private void Awake()
    {
        _bossAttack = FindObjectOfType<BossAttack>();
        _boss = GameObject.Find("Boss").GetComponent<Boss>();
    }

    private void OnEnable()
    {
        isDead = false;
    }

    private void Update()
    {
        if (specialBulletEnum == SpecialBulletEnum.Guided)
        {
            var targetDir = _boss.player.transform.position - transform.position;
            targetDir.Normalize();
            
            transform.position += targetDir * (moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position += transform.right * (moveSpeed * Time.deltaTime);
        }

        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            if (specialBulletEnum == SpecialBulletEnum.Guided)
            {
                var effect = Instantiate(_boss.bulletBombEffect, transform.position, quaternion.identity);
            
                Destroy(effect, 0.99f);
            }
            else if (specialBulletEnum == SpecialBulletEnum.Replication)
            {
                var effect = Instantiate(_boss.bulletBombEffect, transform.position, quaternion.identity);
                effect.transform.localScale = Vector3.one * 1.4f;
                    
                Destroy(effect, 0.99f);
            }
            
            DestroyBullet();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead || !_boss.player.isCanEffect) return;
        
        DestroyBullet();
    }

    private void DestroyBullet()
    {
        if (specialBulletEnum == SpecialBulletEnum.Replication)
        {
            for (int i = 1; i < 5; i++)
            {
                float rotate = 90 * i;
                
                BossBullet bullet = _bossAttack.GetBulletInPool(0);
                bullet.transform.SetPositionAndRotation(
                    transform.position, Quaternion.Euler(0f, 0f, rotate));
                
                bullet.transform.localScale = new Vector3(0.55f,0.55f, 0);
                bullet.moveSpeed = 3.4f;
                bullet.lifeTime = 8f;   
            }    
        }
        
        _bossAttack.BossBulletPoolList[(int)bulletEnum].Push(this);
        gameObject.SetActive(false);
        isDead = true;
        _timer = 0;

        specialBulletEnum = SpecialBulletEnum.None;
    }
}
