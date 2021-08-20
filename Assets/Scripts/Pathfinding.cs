using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    //pathfinding
   List<Node> path = new List<Node>();
   int D = 10;
   Node lastVisitedNode;
   public Grid grid;

    //targets
    public Transform currentTarget;
    public Transform pacmanTarget;
    public List<Transform> homeTarget = new List<Transform>();
    public Transform frightendTarget;

    public List<Transform> scatterTarget = new List<Transform>();
    public Transform leavingHomeTarget;

    //movements
   float speed = 3f;
   Vector3 nextPos, destination;
    int i =0;

    //directions
   Vector3 up = Vector3.zero,
   right = new Vector3(0, 90, 0),
   down = new Vector3(0, 180, 0),
   left = new Vector3(0, 170, 0),
   currentDirection = Vector3.zero;

    //statemachine
    public enum GhostStates
    {
        HOME,
        LEAVING_HOME,
        CHASE,
        SCATTER,
        FRIGHTEND,
        GHOST_EATEN,
    }
    public GhostStates state;

    //appearence
    int activeAppearence; //0-normal, 1-frightened, 2-eyes
    public GameObject[] appearence;


    void Start()
    {
        destination = transform.position;
    }

    void Update()
    {
        CheckSate();
    }

    void FindThePath()
    {
        Node startNode = grid.NodeRequest(transform.position); //ghost position
        Node goalNode = grid.NodeRequest(currentTarget.position); // pacman position

        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if(openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[1];
                }
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //goal found
            if(currentNode == goalNode)
            {
                //get path before exit
                PathTracer(startNode, goalNode);
                return;
            }

            foreach (Node neighbour in grid.GetNeighborNodes(currentNode))
            {
                if (!neighbour.walkable || closedList.Contains(neighbour) ||neighbour == lastVisitedNode)
                {
                    continue;
                }

                int calcMoveCost = currentNode.gCost + GetDistance(currentNode, neighbour);
                if(calcMoveCost < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = calcMoveCost;
                    neighbour.hCost = GetDistance(neighbour, goalNode);

                    neighbour.parentNode = currentNode;
                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }

            lastVisitedNode = null;
        }
    }

    void PathTracer(Node startNode, Node goalNode)
    {
        lastVisitedNode = startNode;
        //List<Node> path = new List<Node>();
        path.Clear();
        Node currentNode = goalNode;

        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parentNode;
        }

        //reverse path to get is sorted right
        path.Reverse();
        grid.path = path;
    }

    int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.posX - b.posX);
        int distZ = Mathf.Abs(a.posZ - b.posZ);

        return D * (distX + distZ);
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, destination) < 0.001f)
        {
            FindThePath();

            if(path.Count > 0)
            {
                //destination
                nextPos = grid.NextPathPoint(path[0]);
                destination = nextPos;

                //rotation
                SetDirection();
                transform.localEulerAngles = currentDirection;

            }

        }
    }

    void SetDirection()
    {
        int dirX = (int)(nextPos.x - transform.position.x);
        int dirZ = (int)(nextPos.z - transform.position.z);

        //up
        if (dirX == 0 && dirZ > 0)
        {
            currentDirection = up;
        }

        //right
        else if (dirX > 0 && dirZ == 0)
        {
            currentDirection = right;
        }

        //left
        else if (dirX < 0 && dirZ == 0)
        {
            currentDirection = left;
        }

        else if (dirX == 0 && dirZ < 0)
        {
            currentDirection = down;
        }
    }

    void CheckSate()
    {
        switch(state)
        {
            case GhostStates.HOME:
                activeAppearence = 0;
                SetAppearence();

                speed = 1.5f;

                if (!homeTarget.Contains(currentTarget))
                {
                    Debug.Log("fase 5");
                    currentTarget = homeTarget[0];
                }

                    if (Vector3.Distance(transform.position, homeTarget[i].position) < 0.2)
                    {
                      i++;

                      if (i >= homeTarget.Count)
                       {
                          i = 0;
                       }
                       currentTarget = homeTarget[i];
                    }
                Move();
                break;

            case GhostStates.LEAVING_HOME:
                activeAppearence = 0;
                SetAppearence();
                break;

            //chase pacman
            case GhostStates.CHASE:
                activeAppearence = 0;
                SetAppearence();

                speed = 3f;

                currentTarget = pacmanTarget;
                Move();
                break;

            case GhostStates.SCATTER:
                activeAppearence = 0;
                SetAppearence();

                speed = 3f;

                if (!scatterTarget.Contains(currentTarget))
                {
                    currentTarget = scatterTarget[0];
                }


                    if (Vector3.Distance(transform.position, scatterTarget[i].position) < 2)
                    {
                        Debug.Log("fase 3");

                        i++;

                        if(i >= scatterTarget.Count)
                        {
                            i = 0;
                        }
                        currentTarget = scatterTarget[i];
                    }

                Move();
                break;

            case GhostStates.FRIGHTEND:
                currentTarget = frightendTarget;
                activeAppearence = 1;
                SetAppearence();

                speed = 3f;

                Move();
                break;

            case GhostStates.GHOST_EATEN:
                activeAppearence = 2;
                SetAppearence();

                speed = 7f;
                break;
        }
    }

    void SetAppearence()
    {
        for (int i = 0; i < appearence.Length; i++)
        {
            appearence[i].SetActive(i == activeAppearence);
        }
    }
}
