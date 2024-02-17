using System;
using UnityEngine;

namespace Entity {
    public class EnemyTest1 : BaseEntity
    {
        [SerializeField] private Transform target;
        public void Start() {
            
        }


        public void Update()
        {
            Move();
        }
        public override void Move() {
            
        }
    }
}