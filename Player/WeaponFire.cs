using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
   [SerializeField] private List<Transform> firePosition;
   [SerializeField] private Transform poolTrm;
   [SerializeField] private Bullet bulletPrefab;
   public Animator animator;
   public GameObject effect;
   private bool _isFireLeft;
   public bool isCanFire = true;
   
   public Stack<Bullet> bulletPool = new Stack<Bullet>();

   private void OnEnable()
   {
      isCanFire = true;
   }

   public void StopAttack()
   {
      isCanFire = false;
   }

   private void Awake()
   {
      CreateBullet();
   }

   private void FireProcess(int index)
   {
      if (!isCanFire) return;
      
      if (bulletPool.Count < 1)
      {
         bulletPool.Push(Instantiate(bulletPrefab, poolTrm));
      } 
      var bullet = bulletPool.Pop();
      
      bullet.transform
         .SetPositionAndRotation(firePosition[index].position, transform.rotation);
      
      bullet.gameObject.SetActive(true);
      bullet.isDead = false;
   }

   private void CreateBullet()
   {
      for (var i = 0; i < 15; i++)
      {
         var bullet = Instantiate(bulletPrefab, poolTrm);
         bullet.gameObject.SetActive(false);
         bulletPool.Push(bullet);
      }
   }
}