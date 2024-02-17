using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Entity
{
    public class PlayerAtt
    {
        public bool Attack()
        {
            Debug.Log("ht");
            return Input.GetMouseButtonDown(0);
        }
    }
}