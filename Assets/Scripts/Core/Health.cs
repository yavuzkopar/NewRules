using System.Collections;
using System.Collections.Generic;
using UnityEngine;


  namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float health = 100f;

        [SerializeField] bool isDead;

        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(float damage)
        {
            health = Mathf.Max(health - damage, 0);
            if (health == 0)
            {
                Die();
            }
        }
        void Die()
        {
            if (isDead)
            {
                return;
            }
            GetComponent<Animator>().SetTrigger("die");
            isDead = true;
        }
    }


}