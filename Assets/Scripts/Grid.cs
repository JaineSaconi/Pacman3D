using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{

    public GameObject bottomLeft, topRight;
    //DEBUG
    public GameObject start, goal;

    Node[,]myGrid;

    public List<Node> path;

    public LayerMask unwalkable;

    //GRID INFO
    int xStart, zStart;

    int xEnd, zEnd;

    int vCell, hCell;

    int cellWidth = 1;
    int cellHight = 1;

    void Awake()
    {
        MPGridCreate();
    }

    void MPGridCreate()
    {
        xStart = (int)bottomLeft.transform.position.x;
        zStart = (int)bottomLeft.transform.position.z;

        xEnd = (int)topRight.transform.position.x;
        zEnd = (int)topRight.transform.position.z;

        hCell = (int)((xEnd - xStart) / cellWidth);
        vCell = (int)((zEnd - zStart) / cellHight);


        myGrid = new Node[hCell+1, vCell+1];      


        UpdateGrid();
    }

    public void UpdateGrid()
    {
        for (int i = 0; i <= hCell; i++)
        {
            for (int j = 0; j <= vCell; j++)
            {
                bool walkable = !(Physics.CheckSphere(new Vector3(xStart + i, 0.5f, zStart + j), 0.4f, unwalkable));

                myGrid[i,j] = new Node(i, j, 0, walkable);
            }
        }
    }

    void OnDrawGizmos()
    {
        if(myGrid != null)
        {
            foreach (Node node in myGrid)
            {
                Gizmos.color = (node.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    if (path.Contains(node))
                    {
                        Gizmos.color = Color.green;
                        //Debug.Log("fase 2");
                    }
                }

                Gizmos.DrawWireCube(new Vector3(xStart + node.posX, 0, zStart + node.posZ), new Vector3(0.8f, 0.8f, 0.8f));
            }
            
        }
    }

    public Node NodeRequest(Vector3 pos)
    {
        int gridX = (int)Vector3.Distance(new Vector3(pos.x, 0, 0), new Vector3(xStart, 0, 0));
        int gridZ = (int)Vector3.Distance(new Vector3(0, 0, pos.z), new Vector3(0, 0, zStart));

        return myGrid[gridX, gridZ];
    }

    public Vector3 NextPathPoint(Node node)
    {
        int gridX = (int)(xStart + node.posX);
        int gridZ = (int)(zStart + node.posZ);

        return new Vector3(gridX, 0, gridZ);
    }

    public List<Node> GetNeighborNodes(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <=1; x++)
        {
            for (int z = -1; z <=1; z++)
            {
                if(x == 0 && z== 0)
                {
                    continue;
                }

                if(x == -1 && z == 1)
                {
                    continue;
                }

                if(x == 1 && z ==1)
                {
                    continue;
                }

                if (x == 1 && z == -1)
                {
                    continue;
                }

                if (x == -1 && z == -1)
                {
                    continue;
                }

                int checkPosX = node.posX + x;
                int checkPosZ = node.posZ + z;

                if(checkPosX>= 0 && checkPosX < (hCell+1) && checkPosZ>= 0 && checkPosZ < (vCell + 1))
                {
                    neighbours.Add(myGrid[checkPosX, checkPosZ]);
                }
            }
        }

        return neighbours;
    }

}
