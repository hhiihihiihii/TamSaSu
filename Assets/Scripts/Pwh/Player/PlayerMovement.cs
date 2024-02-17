using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerMovement
    {
        public BaseEntity _base;

        public Vector2 Move()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector2 dir = new Vector2(x, y).normalized;
            return dir * 10 * Time.deltaTime;
        }
    }
}
