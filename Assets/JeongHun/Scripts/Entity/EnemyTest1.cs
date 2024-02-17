using System;
using System.Collections;
using System.Collections.Generic;
using ASTAR;
using Manager;
using UnityEngine;
using Type = Manager.Type;

namespace Entity {
    public class EnemyTest1 : BaseEntity
    {
        // [SerializeField] private Transform target;

        private bool inAnimation;

        private List<StarNode> path;
        private Vector3 currentTarget;
        private int currentIdx;

        private SteeeringInfo info;
        public void Start()
        {
            inAnimation = false;
            MoveTo();
            StartCoroutine(MoveToTarget());
        }


        public void Update() {
            Move();
            
        }

        
        private void MoveTo() {
            path = AStarGrid.Instance.pathfinder.GetPath(this.transform, target);
            if (path == null || path.Count < 2) return;
            currentTarget = target.position;
            currentIdx = 1;
            
            info = new SteeeringInfo(path[0].x, path[1].z);
        }

        private bool ToTarget() {
            if (inAnimation || path == null) return false;

            Vector2 ePos = this.transform.position;
            if(Mathf.Abs(Vector2.Distance(ePos, info.pos)) <= 0.05) {
                currentIdx++;
                if(currentIdx < path.Count)
                    info.Set(path[currentIdx].x, path[currentIdx].z);
                else {
                    //entity.stateMachine.ChangeState(entity.states[(int)NonePlayerCharacterState.Idle]);
                    // entity.steeringInfo.targetPosition = Vector2.zero ;
                    // entity.stateMachine.RevertToPriviousState();
                    // GameManager.Instance.consoleManager.Print(Type.Unknown, "Path update fail");
                    return true;
                }
            }

            return false;

        }
        IEnumerator MoveToTarget()
        {
            bool flag = true;
            while (flag) {
                if (currentIdx > 2) MoveTo();
                ToTarget();
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        public override void Move() {
            if (info == null) return;
            Vector3 displacement = speed* (info.pos - (Vector2)transform.position).normalized * Time.deltaTime;
            transform.Translate(displacement, Space.World);
        }
    }

    class SteeeringInfo {
        private float x, y;

        public Vector2 pos {
            get { return new Vector2(x, y); }
        }
        public SteeeringInfo(float x, float y) {
            Set(x, y);
        }

        public void Set(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        
    }
}