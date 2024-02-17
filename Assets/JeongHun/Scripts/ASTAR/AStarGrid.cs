using System.Collections.Generic;
using DesignPattern;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace ASTAR {

    public class AStarGrid : SingleTon<AStarGrid> { 

        [SerializeField] private Tilemap walkableMap; 
        [SerializeField] private GameObject mark;
    
    private StarNode[,] grid; // [y,x] 그리드

    private AStar pathfinder;

    // for TEST
    private StarNode startNode;
    private StarNode endNode;
    
    private void Awake() {
        // 초기화
        CreateGrid();
        /*pathfinder = new AStarPathfind(this);*/
        AStar.Instance.grid = this;
    }
    private void Start() {
        BoundsInt bounds = walkableMap.cellBounds;
        for (int i = 0; i < bounds.size.y; i++) {
            for (int j = 0; j < bounds.size.x; j++) {
                GameObject g = Instantiate(mark, new Vector3(grid[i, j].x, grid[i, j].z, 0), Quaternion.identity);
                print(grid[i, j].isWalkable);
                g.GetComponent<SpriteRenderer>().sortingOrder = 1000;

                switch (grid[i,j].fcost)
                {
                    case 999999: g.GetComponent<SpriteRenderer>().color = Color.red;  break;
                    case 1: g.GetComponent<SpriteRenderer>().color = Color.blue; break;
                    case 1000: g.GetComponent<SpriteRenderer>().color = Color.green; break;
                    default: break;

                }
            }
        }
    }
    
    private void CreateGrid()
    {
        walkableMap.CompressBounds();
        BoundsInt bounds = walkableMap.cellBounds;
        grid = new StarNode[bounds.size.y, bounds.size.x]; 
        for (int y = bounds.yMin, i = 0; i < bounds.size.y; y++, i++)
        {   
            for (int x = bounds.xMin, j = 0; j < bounds.size.x; x++, j++)
            {
                StarNode node = new StarNode(walkableMap.CellToWorld(new Vector3Int(x, y)).x + 0.4f, 
                                            walkableMap.CellToWorld(new Vector3Int(x, y)).y + 0.4f,
                                            int.MaxValue);
                node.yi = i;
                node.xi = j;
                node.parent = null;

                // walkable Tilemap에 타일이 있으면 이동 가능한 노드, 타일이 없으면 이동 불가능한 노드이다.
                /*                if (walkableMap.HasTile(new Vector3Int(x, y, 0)))
                                {
                                    node.isWalkable = true;
                                    grid[i, j] = node;
                                }
                                else
                                {
                                    node.isWalkable = false;
                                    grid[i, j] = node;
                                }*/
                Collider2D[] cols = Physics2D.OverlapCircleAll(new Vector2(node.x,node.z), 0.1f);
                if (cols.Length > 0)
                {
                    bool s = false;
                    for (int k = 0; k < cols.Length; k++) {
                        if (cols[k] is TilemapCollider2D) {
                            s = true;
                            break;
                        }
                    }
                    if (s) {
                        node.isWalkable = true;
                        grid[i, j] = node;

                        //정렬
                        List<GameObject> sortedArr = new List<GameObject>();
                        
                        for (int k = 0; k < cols.Length; k++)
                        {
                            int l;
                            if (cols[k] is not TilemapCollider2D || !cols[k].GetComponent<TilemapRenderer>()) continue;
                            if(sortedArr.Count == 0) sortedArr.Add(cols[k].gameObject);

                            for (l = 0; l < sortedArr.Count; l++)
                            {
                                if (cols[k].GetComponent<TilemapRenderer>().sortingOrder
                                    > sortedArr[l].GetComponent<TilemapRenderer>().sortingOrder) 
                                {
                                    sortedArr.Insert(l, cols[k].gameObject);
                                    break;
                                }
                            }

                        }

                        if (sortedArr.Count == 0) continue;
                        print(sortedArr[0]);
                        switch (sortedArr[0].name)
                        {
                            case "Obstacle": node.cost = 999999; break;
                            case "Road": node.cost = 1f; break;
                            case "Base": node.cost = 1000f; break;
                            default: break;

                        }

                        continue;
                    }

                }

                node.isWalkable = false;
                grid[i, j] = node;

            }
        }
        foreach (StarNode node in grid)
        {
            if (node.cost != 1)
            {
/*                GameObject g = Instantiate(mark, new Vector3(node.xPos, node.yPos, 0), Quaternion.identity);
                print(node.isWalkable);
                g.GetComponent<SpriteRenderer>().sortingOrder = 1000;*/
            }
        }
    }

    public void ResetNode()
    {
        foreach (StarNode node in grid)
        {
            node.Reset();
        }
    }

    public StarNode GetNodeFromWorld(Vector3 worldPosition)
    {
        // 월드 좌표로 해당 좌표의 AStarNode 인스턴스를 얻는다.
        Vector3Int cellPos = walkableMap.WorldToCell(worldPosition);
        int y = cellPos.y + Mathf.Abs(walkableMap.cellBounds.yMin);
        int x = cellPos.x + Mathf.Abs(walkableMap.cellBounds.xMin);

        if (y >= grid.GetLength(0) || x >= grid.GetLength(1)) return null;
        StarNode node = grid[y, x];
        return node;
    }
    public List<StarNode> GetNeighborNodes(StarNode node, bool diagonal = false) {
        List<StarNode> neighbors = new List<StarNode>();
        int height = grid.GetUpperBound(0);
        int width = grid.GetUpperBound(1);

        int y = node.yi;
        int x = node.xi;
        // 상하
        if (y < height)
            neighbors.Add(grid[y + 1, x]); 
        if (y > 0)
            neighbors.Add(grid[y - 1, x]); 
        // 좌우
        if (x < width)
            neighbors.Add(grid[y, x + 1]); 
        if (x > 0)
            neighbors.Add(grid[y, x - 1]);

        if (!diagonal) return neighbors;

        // 대각선
        if (x > 0 && y > 0)
            neighbors.Add(grid[y - 1, x - 1]);
        if (x < width && y > 0)
            neighbors.Add(grid[y - 1, x + 1]);
        if (x > 0 && y < height)
            neighbors.Add(grid[y + 1, x - 1]);
        if (x < width && y < height)
            neighbors.Add(grid[y + 1, x + 1]);

        return neighbors;
    }
    }
}