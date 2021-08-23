using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PCTetse : MonoBehaviour
{
    public GameObject[] waypoints = new GameObject[3];
    public GameObject[] homepoinst = new GameObject[2];
    private int index = 0;
    public float maxDist = 0.5f;
    public float maxDistPC = 20f;
    public float vel = 2;

    public GameObject PC;
    private NavMeshAgent agente;
    public Vector3 destination;

    public LayerMask unwalkable;

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

    public enum Ghosts
    {
        GHOST,
        GHOST1,
        GHOST2,
        GHOST3
    }
    public Ghosts ghosts;

    //appearence
    int activeAppearence; //0-normal, 1-frightened, 2-eyes
    public GameObject[] appearence;

    //RESET state
    Vector3 initPosition;
    GhostStates initState;

    //Home timer
    float timer = 5f;
    float curTime = 0;

    //frightend timer
    float fTimer = 5f;
    float curFTimer = 0f;

    //chase timer
    float cTimer = 20f;
    float curCTimer = 0;

    //scatter timer
    float sTimer = 7f;
    float curSTimer = 0;

    //release info
    public int pointsToCollect;
    public bool released = false;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;
        initState = state;

        agente = GetComponent<NavMeshAgent>();
        DisabledCubesAtStart();
        CheckSate();
    }

    public void DisabledCubesAtStart()
    {
        for (int i = 0; i < homepoinst.Length; i++)
        {
            homepoinst[i].GetComponent<MeshRenderer>().enabled = false;
        }

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i].GetComponent<MeshRenderer>().enabled = false;
        }
    }

    private void Runaway()
    {
        agente.SetDestination(waypoints[index].transform.position);
        destination = agente.destination;

        if ( Vector3.Distance(agente.destination, transform.position) < 1 ) {
            index++;
            if (index >= waypoints.Length)
            {
                index = 0;
            }
        }            
    }

    public void HomePoints()
    {
        agente.SetDestination(homepoinst[index].transform.position);
        destination = agente.destination;

        if (Vector3.Distance(agente.destination, transform.position) < 1)
        {
            index++;
            if (index >= homepoinst.Length)
            {
                index = 0;
            }

            if (state != GhostStates.HOME)
            {
                state = GhostStates.HOME;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
       CheckSate();
       Timing();
    }

    void Follow()
    {
        agente.SetDestination(PC.transform.position);
    }

    void CheckSate()
    {
        switch (state)
        {
            case GhostStates.HOME:
                activeAppearence = 0;
                SetAppearence();

                vel = 1.5f;
                HomePoints();
                break;

            case GhostStates.LEAVING_HOME:
                activeAppearence = 0;
                SetAppearence();
                break;

            //chase pacman
            case GhostStates.CHASE:
                activeAppearence = 0;
                SetAppearence();

                vel = 2f;
                Follow();
                break;

            case GhostStates.SCATTER:
                activeAppearence = 0;
                SetAppearence();

                vel = 3f;
                Runaway();              
                break;

            case GhostStates.FRIGHTEND:
                activeAppearence = 2;
                SetAppearence();
                vel = 1.5f;
                Runaway();
                break;

            case GhostStates.GHOST_EATEN:
                activeAppearence = 1;
                SetAppearence();
                vel = 7f;
                HomePoints();
                //state = GhostStates.HOME;
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

    void Timing()
    {
        if(state == GhostStates.HOME && released)
        {
            curTime = curTime + Time.deltaTime;
            if(curTime >= timer)
            {
                curTime = 0;
                state = GhostStates.CHASE;
            }
        }

        if(state == GhostStates.FRIGHTEND)
        {
            curFTimer = curFTimer + Time.deltaTime;
            if(curFTimer >= fTimer)
            {
                if(state != GhostStates.GHOST_EATEN)
                {
                    curFTimer = 0;
                    state = GhostStates.CHASE;
                }
            }
        }

        if(state == GhostStates.CHASE)
        {
            curCTimer = curCTimer + Time.deltaTime;
            if(curCTimer >= cTimer)
            {
                curCTimer = 0;
                state = GhostStates.SCATTER;
            }
        }

        if (state == GhostStates.SCATTER)
        {
            curSTimer = curSTimer + Time.deltaTime;
            if (curSTimer >= sTimer)
            {
                curSTimer = 0;
                state = GhostStates.CHASE;
            }
        }
    }
  

    public void Reset()
    {
        transform.position =initPosition ;
        state = initState;
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.name == "Chomp")
        {
            if(state != GhostStates.GHOST_EATEN && state != GhostStates.FRIGHTEND)
            {
                GameManager.intance.LoseLife();
                Reset();
            } else
            {
                GameManager.intance.AddScore(20);
                state = GhostStates.GHOST_EATEN;
            }
        }
    }
}


