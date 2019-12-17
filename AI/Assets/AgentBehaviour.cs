using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This Class is used to determine the agent stats and behaviour
/// </summary>
public class AgentBehaviour : MonoBehaviour
{
    /// <summary>
    /// Enum of available Behaviours
    /// </summary>
    public Behaviour behaviour;

    /// <summary>
    /// Tells if the player is alive
    /// </summary>
    public bool isAlive;

    /// <summary>
    /// Tells if the player is dead
    /// </summary>
    public bool isStunned;

    /// <summary>
    /// Tells if the player is in Panic
    /// </summary>
    public bool inPanic { get; set; }

    /// <summary>
    /// Tells if the player is Hungry
    /// </summary>
    private bool isHungry;

    /// <summary>
    /// Tells if the player is tired
    /// </summary>
    private bool isTired;

    /// <summary>
    /// Tells if the player is resting
    /// </summary>
    public bool isResting { get; set; }

    /// <summary>
    /// Tells if the player is eating
    /// </summary>
    private bool isEating; 
    
    /// <summary>
    /// Tells if the player is on the move to a stage
    /// </summary>
    private bool isGoingToFun;

    /// <summary>
    /// Tells if the player is on the move to an open Area
    /// </summary>
    private bool isGoingToRest;

    /// <summary>
    /// Tells is the player is on the move to a seat
    /// </summary>
    private bool isGoingForFood;

    /// <summary>
    /// Tells if the player has found a seat yet
    /// </summary>
    private bool hasFoundSeat = false;

    /// <summary>
    /// NavMeshAgent of this Gameobject
    /// </summary>
    private NavMeshAgent agent;

    /// <summary>
    /// NavMeshPath of this agent
    /// </summary>
    private NavMeshPath path;

    /// <summary>
    /// Vector3 that holds the destination to the agent
    /// </summary>
    private Vector3 destination; 
    
    /// <summary>
    /// Vector 3 that holds the panic origin of an event(Explosion)
    /// </summary>
    private Vector3 panicOrigin;
    
    /// <summary>
    /// Array with all the stages
    /// </summary>
    private GameObject[] stages;

    /// <summary>
    /// Array with all the Open Areas
    /// </summary>
    private GameObject[] openAreas;

    /// <summary>
    /// Array with all the exits
    /// </summary>
    private GameObject[] exits;

    /// <summary>
    /// Array with all the upperStageAreas
    /// </summary>
    private GameObject[] upperStage;

    /// <summary>
    /// Array with all the tables
    /// </summary>
    private GameObject[] tables;

    /// <summary>
    /// Array with all the seats
    /// </summary>
    private GameObject[] seats;

    /// <summary>
    /// Current amount of hunger
    /// </summary>
    public float hunger;

    /// <summary>
    /// Current tired amount
    /// </summary>
    private float tired;
    
    /// <summary>
    /// This method is used to assign all the GameObjects and 
    /// stats of the agent
    /// </summary>
    private void Start()
    {        
        hunger = Random.Range(5f, 800f);
        tired = Random.Range(5f, 500f);

        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();

        stages = GameObject.FindGameObjectsWithTag("Fun");
        openAreas = GameObject.FindGameObjectsWithTag("Open");
        upperStage = GameObject.FindGameObjectsWithTag("Stage");
        exits = GameObject.FindGameObjectsWithTag("Exit");
        tables = GameObject.FindGameObjectsWithTag("Tables");
        seats = GameObject.FindGameObjectsWithTag("Seats");

        behaviour = Behaviour.Seek;

        isAlive = true;
    }

    /// <summary>
    /// The Update is used to keep in check all the stats of the agent and
    /// to determine the behaviours he will do next
    /// </summary>
    private void Update()
    {
        //Debug.Log($"hunger {isEating} | tired {isResting}");
        if (isAlive)
        {
            if (!inPanic && !isStunned && isAlive)
            {
                Conditions();
                ChecksStats();
            }
            else if (inPanic)
            {
                behaviour = Behaviour.Flee;
            }
            
            switch (behaviour)
            {
                case Behaviour.Idle:
                    Idle();
                    break;

                case Behaviour.Seek:
                    //Prioritizes Food over Tired
                    if (isHungry && !isGoingForFood)
                    {
                        isGoingForFood = true;
                        if (!hasFoundSeat)
                        {
                            SeekFood();
                        }                       

                    }
                    else if (isTired && !isGoingToRest)
                    {
                        isGoingToRest = true;
                        isGoingForFood = false;
                        SeekOpenZone();
                    }

                    if (!isHungry && !isTired && !isGoingToFun)
                    {
                        isGoingToRest = false;
                        isGoingToFun = true;
                        isGoingForFood = false;
                        SeekFun();
                    }
                    break;

                case Behaviour.Flee:
                    FleeWithoutExplosion();
                    break;
            }

            Debug.DrawLine(agent.transform.position, agent.pathEndPosition,
                Color.black, 0.1f);
        }
        else
        {
            StopAllCoroutines();
        }       
    }    

    /// <summary>
    /// This method serves to update the agent stats
    /// </summary>
    private void Conditions()
    {
        if (isEating)
        {
            hunger += Random.Range(0.1f , 1f);
        }
        else
        {
            if(hunger > 0.3f)
            {
                hunger -= 0.03f;
            }            
        }
        //----------------------------------------
        if (isResting)
        {
            tired += Random.Range(0.1f, 1f);
        }
        else
        {
            if (tired > 0.3f)
            {
                tired -= 0.03f;
            }
        }    
    }

    /// <summary>
    /// This method is used to check the stats of the agent and
    /// to determine is conditions
    /// </summary>
    private void ChecksStats()
    {
        if (hunger <= 25f)
        {
            isHungry = true;
            isGoingToFun = false;
            isGoingToRest = false;
        }
        else if(hunger > 50f)
        {
            hasFoundSeat = false;
            isHungry = false;            
        }
        if (tired <= 50f)
        {
            isGoingToFun = false;
            isGoingForFood = false;
            isTired = true;
        }
        else if(tired > 50f)
        {
            isTired = false;
        }
    }

    /// <summary>
    /// This method is called for when the agent is looking for the stages.
    /// It helps determine the stage with fewer agents and
    /// sends him to that one
    /// </summary>
    private void SeekFun()
    {
        if (stages[0].GetComponent<Count>().GetNumberOfAgents() >
            stages[1].GetComponent<Count>().GetNumberOfAgents())
        {
            stages[1].GetComponent<Count>().IsGoing(this.name);
            StartCoroutine(GoTo(SpreadAlong(1)));
        }
        else if (stages[0].GetComponent<Count>().GetNumberOfAgents() <
           stages[1].GetComponent<Count>().GetNumberOfAgents())
        {
            stages[0].GetComponent<Count>().IsGoing(this.name);
            StartCoroutine(GoTo(SpreadAlong(0)));
        }
        else
        {
            int i = Random.Range(0, openAreas.Length);
            stages[i].GetComponent<Count>().IsGoing(this.name);
            StartCoroutine(GoTo(SpreadAlong(i)));
        }
    }

    /// <summary>
    /// This method is called when the user needs to seek for food
    /// It determines which seat is empty and which is taken and assigns one
    /// empty seat to the agent
    /// </summary>
    /// <returns></returns>
    private bool SeekFood()
    {
        int i = 0;
        GameObject table = LeastFullTable();

        while (i != seats.Length && !hasFoundSeat)
        {
            if (seats[i].GetComponent<PeopleGoing>().GetNumberOfAgentsGoing() == 0 && !hasFoundSeat)
            {
                if (CheckTransforms(table, seats[i]))
                {
                    seats[i].GetComponent<PeopleGoing>().UpdateAgentsGoing(agent.name);
                    StopAllCoroutines();
                    StartCoroutine(GoTo(seats[i].transform.position));
                
                    hasFoundSeat = true;
                    return true;
                }
                
            }
            i++;
        }
        
        return false;
        
    }

    /// <summary>
    /// Merthod that checks if the seat is assignt to the least full table
    /// </summary>
    /// <param name="table"></param>
    /// <param name="seat"></param>
    /// <returns>If the Seat is in the least full table</returns>
    private bool CheckTransforms (GameObject table, GameObject seat)
    {
        Transform[] transforms = table.GetComponentsInChildren<Transform>();
        foreach(Transform transform in transforms)
        {
            if(transform.position == seat.transform.position)
            {
                return true;
            }
        }
        return false;

    }

    /// <summary>
    /// Method thar finds The table with least people
    /// </summary>
    /// <returns>The least full table</returns>
    private GameObject LeastFullTable()
    {
        GameObject best = default;
        int cycle = 0;
        int numberOfAgents = 0;
        int bestNumber = 0;
        foreach (GameObject table in tables)
        {
            numberOfAgents = table.GetComponent<AgentsInTable>().GetAgents();

            if (cycle == 0)
            {
                bestNumber = numberOfAgents;
                best = table;
            }
            else if (bestNumber > numberOfAgents)
            {
                bestNumber = numberOfAgents;
                best = table;
            }

            cycle++;
        }
        return best;
    }

    /// <summary>
    /// This method is used when the agent is looking for a open zone to rest
    /// Sending him to the most empty one
    /// </summary>
    private void SeekOpenZone()
    {
        if(openAreas[0].GetComponent<Count>().GetNumberOfAgents() >
            openAreas[1].GetComponent<Count>().GetNumberOfAgents())
        {
            StopAllCoroutines();
            openAreas[1].GetComponent<Count>().IsGoing(this.name);
            StartCoroutine(GoTo(RandomPointInsideCollider(1)));
        }else if (openAreas[0].GetComponent<Count>().GetNumberOfAgents() <
            openAreas[1].GetComponent<Count>().GetNumberOfAgents())
        {
            StopAllCoroutines();
            openAreas[0].GetComponent<Count>().IsGoing(this.name);
            StartCoroutine(GoTo(RandomPointInsideCollider(0)));
        }
        else
        {
            int i = Random.Range(0, openAreas.Length);
            StopAllCoroutines();
            openAreas[i].GetComponent<Count>().IsGoing(this.name);
            StartCoroutine(GoTo(RandomPointInsideCollider(i)));
        }            
    }

    /// <summary>
    /// This method is used to make the agent panic and react
    /// to the explosion and run to the nearest exit
    /// </summary>
    private void Flee()
    {
        NavMesh.SetAreaCost(3, 0);
        GenerateFleeDestination(panicOrigin);
        StartCoroutine(RunToExit());
    }

    /// <summary>
    /// This method is used to make the agen run to the nearest exit
    /// </summary>
    private void FleeWithoutExplosion()
    {
        StartCoroutine(RunToExit());
    }

    /// <summary>
    /// This method is called when the agent needs to die
    /// </summary>
    public void Die()
    {
        isAlive = false;

        GetComponent<AgentBehaviour>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;

        GameManager.Instance.GetComponent<PopulationController>()
            .RemoveAgent(gameObject);

        StartCoroutine(DestroyAgent());

    }

    /// <summary>
    /// Metho called to make agent stop when called
    /// </summary>
    private void Idle()
    {
        agent.isStopped = true;
    }

    /// <summary>
    /// This method makes the agent get stunned and its used when the agent
    /// is caught in the explosion but not in the deadly range
    /// </summary>
    public void Stun()
    {
        isStunned = true;
        behaviour = Behaviour.Idle;

        StartCoroutine(RegainConsciousness());
    }

    /// <summary>
    /// This metho makes de agent panic and flee
    /// </summary>
    /// <param name="origin"></param>
    public void Panic(Vector3 origin)
    {
        inPanic = true;
        panicOrigin = origin;
        behaviour = Behaviour.Flee;
        StopAllCoroutines();
        Flee();
    }

    /// <summary>
    /// This method makes the agent react to an explosion running to the oppost
    /// direction of the explosion
    /// </summary>
    /// <param name="panicOrigin"></param>
    private void GenerateFleeDestination(Vector3 panicOrigin)
    {
        Vector3 linear = Vector3.zero;

        linear = transform.position - panicOrigin;

        linear = linear.normalized * 50;

        destination = transform.position + linear;

        agent.SetDestination(destination);
    }

    /// <summary>
    /// This method is used to create a random point inside a gameObject
    /// </summary>
    /// <param name="i"></param>
    /// <returns>Random Point</returns>
    private Vector3 RandomPointInsideCollider(int i)
    {
        Collider col = openAreas[i].GetComponent<Collider>();
        Vector3 RandomPoint = new Vector3(
            Random.Range(col.bounds.min.x, col.bounds.max.x),
            0f,
            Random.Range(col.bounds.min.z, col.bounds.max.z));
        return RandomPoint;
    }    

    /// <summary>
    /// This method is called to ake the agents spred along 
    /// the front row of the stage
    /// </summary>
    /// <param name="i"></param>
    /// <returns>Point on front row</returns>
    private Vector3 SpreadAlong(int i)
    {
        Vector3 along;
        Collider col = stages[i].GetComponent<Collider>();
        along = col.ClosestPoint(upperStage[i].transform.position);
        along = new Vector3(
            along.x + Random.Range(-15f, 15f),
            along.y,
            along.z);

        return along;
    }

    /// <summary>
    /// This method is used to determine the nearest exit and returns it
    /// </summary>
    /// <returns>The nearest exit</returns>
    private Transform ClosestExit()
    {
        Transform best = default;
        float dist;
        float bestDist = 0;
        int cycle = 0;

        foreach (GameObject exit in exits)
        {
            dist = Vector3.Distance(this.gameObject.transform.position,
                exit.transform.position);

            if (cycle == 0)
            {
                best = exit.transform;
                bestDist = dist;
            }
            else if (dist < bestDist)
            {
                best = exit.transform;
                bestDist = dist;
            }
            cycle++;
        }     

        return best;
    }

    /// <summary>
    /// This Courotine is used for the agent regain concience after some time
    /// </summary>
    /// <returns></returns>
    private IEnumerator RegainConsciousness()
    {
        yield return new WaitForSeconds(Random.Range(2, 10));
        if (isAlive)
        {
            isStunned = false;
            agent.isStopped = false;
            Panic(panicOrigin);
        }
    }

    /// <summary>
    /// Courotine used to make the agent go to some positions
    /// </summary>
    /// <param name="place"></param>
    /// <returns></returns>
    private IEnumerator GoTo(Vector3 place)
    {
        yield return new WaitForSeconds(Random.Range(1, 5));
        if (isAlive)
        {
            agent.SetDestination(place);
        }
    }

    /// <summary>
    /// This Courotine is used to make the agent run to the exit
    /// </summary>
    /// <returns></returns>
    private IEnumerator RunToExit()
    {
        yield return new WaitForSeconds(Random.Range(1, 3));
        NavMesh.avoidancePredictionTime = 5f;
        if (isAlive && isStunned == false)
        {
            agent.SetDestination(ClosestExit().position);
        }
    }

    /// <summary>
    /// This Courotine is used to destroy the agent
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyAgent()
    {
        yield return new WaitForSeconds(Random.Range(8, 15));
        if (isAlive == false)
            Destroy(gameObject);
    }

    /// <summary>
    /// This method is used to make the agent go to the exit
    /// </summary>
    public void GoToExit()
    {
        Flee();
    }

    /// <summary>
    /// This method sets the hungry mode of the agent
    /// </summary>
    /// <param name="mode"></param>
    public void SetHungryMode(bool mode)
    {
        isEating = mode;
    }
}