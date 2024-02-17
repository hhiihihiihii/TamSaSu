using System;
using System.Collections;
using System.Collections.Generic;
using DesignPattern;
using Manager;
using UnityEngine;
using Type = Manager.Type;

namespace ASTAR {
    public class AStar : MonoBehaviour {
        public AStarGrid grid;
        private float Heuristic(StarNode a, StarNode b, bool diagonal = false) {
            // 맨해튼 거리
            
            float dx = Mathf.Abs(a.x - b.x);
            float dy = Mathf.Abs(a.z - b.z);

            if(!diagonal) return 1 * (dx + dy) + b.cost;
            
            // 체비쇼프 거리
            return Mathf.Max(Mathf.Abs(a.x - b.x), Mathf.Abs(a.z - b.z)) + b.cost;
        }
        

        public void Move() {
            
        }


        public List<StarNode> GetPath(Transform start, Transform target) {
            StarNode startNode = AStarGrid.Instance.GetNodeFromWorld(start.position);
            StarNode endNode = AStarGrid.Instance.GetNodeFromWorld(target.position);
            if (startNode == null || endNode == null) {
                GameManager.Instance.consoleManager.ErrorPrint(Type.GAMEMANAGER, "Unknown Node");
                return null;
            }
            return CreatePath(startNode, endNode);

        }
        private List<StarNode> CreatePath(StarNode start, StarNode end, bool diagonal = false)
        {
            if (start == null || end == null) return null;
            grid.ResetNode();
            List<StarNode> open_list = new List<StarNode>();
            List<StarNode> closed_list = new List<StarNode>();

            StarNode startNode = start;
            StarNode endNode = end;

            // startNode.gCost = 0;
            startNode.gcost = 0;
            startNode.hcost = Heuristic(start, endNode);
            open_list.Add(startNode);

            while (open_list.Count > 0) {
                // Open Set 내의 노드 중 가장 거리가 짧은 노드를 찾는다.
                int shortest = 0;
                for (int i = 1; i < open_list.Count; i++) {
                    if (open_list[i].fcost < open_list[shortest].fcost) {
                        shortest = i;
                    }
                }

                StarNode currentNode = open_list[shortest];

                // 목적지 도착
                if (currentNode == endNode) {
                    // Debug.Log("PathMakingFinished");
                    // 경로만들어서 반환
                    List<StarNode> path = new List<StarNode>();
                    path.Add(endNode);
                    StarNode tempNode = endNode;
                    while (tempNode.parent != null) {
                        path.Add(tempNode.parent);
                        tempNode = tempNode.parent;
                    }

                    path.Reverse();
                    return path;
                }
                // 리스트를 업데이트한다.
                open_list.Remove(currentNode);
                closed_list.Add(currentNode);

                // 다음 노드를 방문한다.
                List<StarNode> neighbors = grid.GetNeighborNodes(currentNode, true);
                for (int i = 0; i < neighbors.Count; i++) {
                    if (closed_list.Contains(neighbors[i]) || !neighbors[i].isWalkable) continue;

                    float gCost = currentNode.gcost + Heuristic(currentNode, neighbors[i]);
                    
                    if (gCost < neighbors[i].gcost) {
                        neighbors[i].parent = currentNode;
                        neighbors[i].gcost = gCost;
                        neighbors[i].hcost = Heuristic(neighbors[i], endNode, diagonal);
                        
                        if (!open_list.Contains(neighbors[i])) open_list.Add(neighbors[i]);
                    }
                }
            }

            return null;

        }
        

        IEnumerator PathFind() {
            StarNode start;
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    public class StarNode {
        public float x, z;
        public float hcost, gcost;
        public StarNode parent;

        public bool isWalkable;


        public float cost;

        public int xi, yi;

        public float fcost {
            get {return (hcost + gcost); }
        }
        
        public StarNode(int x, int z, int cost) {
            this.x = x;
            this.z = z;
            this.gcost = cost;
            isWalkable = true;
        }

        public StarNode(int x, int z) {
            this.x = x;
            this.z = z;
            this.gcost = 0;
            isWalkable = true;
        }

        public StarNode(float x, float z) {
            this.x = (int)x;
            this.z = (int)z;
            this.gcost = 0;
            this.cost = 0;
            this.hcost = 0;
            isWalkable = true;

        }
        
        public StarNode(float x, float z, int cost) {
            this.x = (int)x;
            this.z = (int)z;
            this.cost = cost;
            this.gcost = 0;
            this.hcost = 0;
            isWalkable = true;
        }

        public StarNode()
        {
            x = -1;
            z = -1;
            xi = -1;
            yi = -1;
            hcost = 0;
            gcost = 0;
            cost = 0;
            parent = null;
        }

        public int Compare(StarNode node) {
            if (node.fcost > this.fcost) return 1;
            if (node.fcost < this.fcost) return -1;
            return 0;
        }
        public void Reset()
        {
            hcost = 0;
            gcost = int.MaxValue;
            parent = null;
        }
    }
    
    
}