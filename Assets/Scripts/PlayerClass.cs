using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
   int MaxHelth;
   int currentHealth;
   
   Animator anim;

   public PlayerClass(int MaxHelth)
   {
      this.MaxHelth = MaxHelth;
   }
   private void Awake()
   {
      anim = GetComponent<Animator>();
   }

   void Start()
   {
      currentHealth = MaxHelth;
   }
   
   public void TakeDamage(int damage)
   {
      currentHealth -= damage;
      anim.SetTrigger("Hit");
   }

   public void Detroy()
   {
      Destroy(gameObject);
   }
}
