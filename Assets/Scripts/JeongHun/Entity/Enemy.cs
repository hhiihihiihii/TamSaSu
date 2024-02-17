using UnityEngine;

namespace Entity
{
    public class Enemy : MonoBehaviour
    {
        [Header("Stats")]
        public int hp;
        public int speed;
        public int attack_damage;
        [Range(0.5f, 4.5f)] public float attack_range;
        [SerializeField] protected Transform target;
        public virtual void Move() {
            
            
        }

        private void MoveTo()
        {
            
        }

        public virtual void Attack() {
            
        }
        
        
    }
    }
