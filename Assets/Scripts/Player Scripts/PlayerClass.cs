using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : MonoBehaviour
{
   public int MaxHelth;
   int currentHelth;
   Animator anim;

   public void Init(int MaxHelth)
   {
      currentHelth = this.MaxHelth = MaxHelth;
      anim = GetComponent<Animator>();
   }

   public void TakeDamage(int damage, bool isDefending)
   {
      Debug.Log("You recived damage!");
      if (currentHelth > 0)
      {
         if (!isDefending)
         {
            currentHelth -= damage;
            anim.SetTrigger("Hit");
         }
         else
         {
            anim.SetTrigger("Perry");
         }
      }
      else
      {
         anim.SetTrigger("Death");
      }
      
   }

   public void DestroyPlayer()
   {  
      Destroy(gameObject);
   }
}
