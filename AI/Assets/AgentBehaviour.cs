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
        //Sets value of hunger
        hunger = Random.Range(5f, 800f);

        //Sets value of tired
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
        //Checks is agent is alive
        if (isAlive)
        {
            //Checks if agent is in panic, stunned and if he is alive
            if (!inPanic && !isStunned && isAlive)
            {
                Conditions();
                ChecksStats();
            }
            else if (inPanic) //Checks if agent is in panic
            {
                //Tells agent to flee
                behaviour = Behaviour.Flee;
            }
            
            //Switch to tell the agent how to act according to his behaviour
            switch (behaviour)
            {
                //Case agent behaviour is being Idle
                case Behaviour.Idle:
                    Idle();
                    break;

                //Case agent behaviour is seek
                case Behaviour.Seek:
                    //Prioritizes Food over Tired.
                    //Checks if player is hungry and is not already going
                    //for food
                    if (isHungry && !isGoingForFood)
                    {
                        //Says agent is already going for food
                        isGoingForFood = true;

                        //Checks if player has already found a seat
                        if (!hasFoundSeat)
                        {
                            SeekFood();
                        }                       

                    }
                    //Checks if player is tired and
                    //is not going for food already
                    else if (isTired && !isGoingToRest)
                    {
                        //Says the agent is going to rest
                        isGoingToRest = true;
                        //Says the agent is not going for food
                        isGoingForFood = false;

                        SeekOpenZone();
                    }

                    //Checks if player is not hungry, not tires and not going
                    //for a fun spot
                    if (!isHungry && !isTired && !isGoingToFun)
                    {
                        //Says the player is not going to rest
                        isGoingToRest = false;

                        //Says the player is going for a fun spot
                        isGoingToFun = true;

                        //Says player is not going for food
                        isGoingForFood = false;

                        SeekFun();
                    }
                    break;

                //Case agent behaviour is to flee
                case Behaviour.Flee:
                    FleeWithoutExplosion();
                    break;
            }

            //Draws lines in gizmos
            Debug.DrawLine(agent.transform.position, agent.pathEndPosition,
                Color.black, 0.1f);
        }
        else
        {
            //Stops all courotines
            StopAllCoroutines();
        }       
    }    

    /// <summary>
    /// This method serves to update the agent stats
    /// </summary>
    private void Conditions()
    {
        //Checks if player is eating
        if (isEating)
        {
            //Increases hunger
            hunger += Random.Range(0.1f , 1f);
        }
        else
        {
            //Checks if hungry is bellow 0.3f
            if(hunger > 0.3f)
            {
                //Decreases hunger
                hunger -= 0.03f;
            }            
        }
        //Checks if player is resting
        if (isResting)
        {
            //Increses tired
            tired += Random.Range(0.1f, 1f);
        }
        else
        {
            //Checks if tired is bellow 0.3f
            if (tired > 0.3f)
            {
                //Decreases tired
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
        //Checks if hunger is bellow or equal to 25f
        if (hunger <= 25f)
        {
            //Agent is hungry
            isHungry = true;

            //Agent is not going to fun
            isGoingToFun = false;

            //Agent is not going to rest
            isGoingToRest = false;
        }
        else if(hunger > 50f) //Checks if hunger is above 50f
        {
            //Agent has no seat assigned
            hasFoundSeat = false;
            
            //Agent is not hungry
            isHungry = false;            
        }

        //Checks if tired is bellow or equal 30f
        if (tired <= 30f)
        {
            //Agent is not going for fun zone
            isGoingToFun = false;

            //Agent is not going for food
            isGoingForFood = false;

            //Agent is tired
            isTired = true;
        }
        else if(tired > 50f)//Checks if tired is above 50f
        {
            //Agent is not tired
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
        //Checks if stage 1 has fewer people than stage 0
        if (stages[0].GetComponent<Count>().GetNumberOfAgents() >
            stages[1].GetComponent<Count>().GetNumberOfAgents())
        {
            //Stops all courotines
            StopAllCoroutines();

            //Adds agent to the list of agents goin to the stage 1
            stages[1].GetComponent<Count>().IsGoing(this.name);

            //Makes agent go to stage 1
            StartCoroutine(GoTo(SpreadAlong(1)));
        }
        //Checks if stage 0 has fewer people than stage 1
        else if (stages[0].GetComponent<Count>().GetNumberOfAgents() <
           stages[1].GetComponent<Count>().GetNumberOfAgents())
        {
            //Stops all courotines
            StopAllCoroutines();

            //Adds agent to the list of agents goin to the stage 0
            stages[0].GetComponent<Count>().IsGoing(this.name);

            //Makes agent go to stage 0
            StartCoroutine(GoTo(SpreadAlong(0)));
        }
        else
        {
            //Creates a random number
            int i = Random.Range(0, openAreas.Length);

            //Stops all courotines
            StopAllCoroutines();

            //Adds player to the list of agents going to that random stage
            stages[i].GetComponent<Count>().IsGoing(this.name);

            //Sends agent to a random stage
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
        //Seat number
        int i = 0;

        //Creates an object with the least full table
        GameObject table = LeastFullTable();

        //Executes while the seat number has not reached the maximum 
        //number of seats and agent has not found a valid seat
        while (i != seats.Length && !hasFoundSeat)
        {
            //Checks if the seat has been already assign to other agent and if
            //agent has already found a seat
            if (seats[i].GetComponent<PeopleGoing>()
                .GetNumberOfAgentsGoing() == 0 && !hasFoundSeat)
            {
                //Checks if the seat is assign to the least full table
                if (CheckTransforms(table, seats[i]))
                {
                    //Reserves seat to this agent
                    seats[i].GetComponent<PeopleGoing>()
                        .UpdateAgentsGoing(agent.name);

                    //Stops all courotunes
                    StopAllCoroutines();

                    //Sends player to seat
                    StartCoroutine(GoTo(seats[i].transform.position));
                
                    //Says player has seat assigned
                    hasFoundSeat = true;

                    //returns true
                    return true;
                }
                
            }

            //Advances to next seat
            i++;
        }
        
        //Return false
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
        //Gets the transforms of all the seats assign to the table
        Transform[] transforms = table.GetComponentsInChildren<Transform>();

        //Checks every transform of every seat
        foreach(Transform transform in transforms)
        {
            //Checks if transform is the same transform of seat
            if(transform.position == seat.transform.position)
            {
                //Returns true
                return true;
            }
        }
        //Return false
        return false;

    }

    /// <summary>
    /// Method thar finds The table with least people
    /// </summary>
    /// <returns>The least full table</returns>
    private GameObject LeastFullTable()
    {
        //GameObject used to store the best table
        GameObject best = default;

        //Current Cycle
        int cycle = 0;

        //Number of Agents
        int numberOfAgents = 0;

        //Best number of agents(the fewer the better)
        int bestNumber = 0;

        //Checks every table inside scene
        foreach (GameObject table in tables)
        {
            //Gets number of agents going to that table
            numberOfAgents = table.GetComponent<AgentsInTable>().GetAgents();

            //Checks if it's the first cycle
            if (cycle == 0)
            {
                //The best number is equal to the number of agents going to
                //that table
                bestNumber = numberOfAgents;

                //The best table is equal to this table
                best = table;
            }
            //Checks if Number of Agents going to that table is inferior
            //to the best number of agents
            else if (bestNumber > numberOfAgents)
            {
                //Best number is equal to the number of agents
                //going to that table
                bestNumber = numberOfAgents;

                //The best table is equal to this table
                best = table;
            }

            //Next cycle
            cycle++;
        }
        //Returns best table
        return best;
    }

    /// <summary>
    /// This method is used when the agent is looking for a open zone to rest
    /// Sending him to the most empty one
    /// </summary>
    private void SeekOpenZone()
    {
        //Checks if open area 1 has fewer people than open area 0
        if (openAreas[0].GetComponent<Count>().GetNumberOfAgents() >
            openAreas[1].GetComponent<Count>().GetNumberOfAgents())
        {
            //Stops all courotines
            StopAllCoroutines();

            //Adds agent to the list of agents going to this area
            openAreas[1].GetComponent<Count>().IsGoing(this.name);

            //Sends agent to open area 1
            StartCoroutine(GoTo(RandomPointInsideCollider(1)));
        }
        //Checks if open area 0 has fewer people than open area 1
        else if (openAreas[0].GetComponent<Count>().GetNumberOfAgents() <
            openAreas[1].GetComponent<Count>().GetNumberOfAgents())
        {
            //Stops all courotines
            StopAllCoroutines();

            //Adds agent to the list of agents going to this area
            openAreas[0].GetComponent<Count>().IsGoing(this.name);

            //Sends agent to open area 0
            StartCoroutine(GoTo(RandomPointInsideCollider(0)));
        }
        else
        {
            //Creates a random number
            int i = Random.Range(0, openAreas.Length);

            //Stops all Courotines
            StopAllCoroutines();

            //Adds player to the list of agents going to that random open area
            openAreas[i].GetComponent<Count>().IsGoing(this.name);

            //Sends agent to random open area
            StartCoroutine(GoTo(RandomPointInsideCollider(i)));
        }            
    }

    /// <summary>
    /// This method is used to make the agent panic and react
    /// to the explosion and run to the nearest exit
    /// </summary>
    private void Flee()
    {
        //Reduces the price of walking in the area number 3
        NavMesh.SetAreaCost(3, 0);

        //Generates a Flee destination based on a point of origin(explosion)
        GenerateFleeDestination(panicOrigin);

        //Sends player to the nearest exit
        StartCoroutine(RunToExit());
    }

    /// <summary>
    /// This method is used to make the agen run to the nearest exit
    /// </summary>
    private void FleeWithoutExplosion()
    {
        //Sends player to flee to the nearest exit
        StartCoroutine(RunToExit());
    }

    /// <summary>
    /// This method is called when the agent needs to die
    /// </summary>
    public void Die()
    {
        //Says player is dead(not alive)
        isAlive = false;

        //Deactivated agent Components
        GetComponent<AgentBehaviour>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = false;

        //Removes agents from the active agents list
        GameManager.Instance.GetComponent<PopulationController>()
            .RemoveAgent(gameObject);

        //Starts courotine to destroy the agent
        StartCoroutine(DestroyAgent());
    }

    /// <summary>
    /// Metho called to make agent stop when called
    /// </summary>
    private void Idle()
    {
        //Stops agent
        agent.isStopped = true;
    }

    /// <summary>
    /// This method makes the agent get stunned and its used when the agent
    /// is caught in the explosion but not in the deadly range
    /// </summary>
    public void Stun()
    {
        //Says agent is stunned
        isStunned = true;

        //Agent behaviour is idle
        behaviour = Behaviour.Idle;

        //Starts courotine to regain consciouness
        StartCoroutine(RegainConsciousness());
    }

    /// <summary>
    /// This metho makes de agent panic and flee
    /// </summary>
    /// <param name="origin"></param>
    public void Panic(Vector3 origin)
    {
        //Says agent is in panic
        inPanic = true;

        //Assigns the origin to panicOrigin
        panicOrigin = origin;

        //Changes the behaviour of the agent to flee
        behaviour = Behaviour.Flee;

        //Stops all courotines
        StopAllCoroutines();

        //Calls flee method
        Flee();
    }

    /// <summary>
    /// This method makes the agent react to an explosion running to the oppost
    /// direction of the explosion
    /// </summary>
    /// <param name="panicOrigin"></param>
    private void GenerateFleeDestination(Vector3 panicOrigin)
    {
        //Assigns a vector.zero the linear
        Vector3 linear = Vector3.zero;

        //Calculates linear
        linear = transform.position - panicOrigin;

        //Calculates linear
        linear = linear.normalized * 50;

        //Calculates destination
        destination = transform.position + linear;

        //Sends agent to the direction opost of the panic origin
        agent.SetDestination(destination);
    }

    /// <summary>
    /// This method is used to create a random point inside a gameObject
    /// </summary>
    /// <param name="i"></param>
    /// <returns>Random Point</returns>
    private Vector3 RandomPointInsideCollider(int i)
    {
        //Gets collider of desired openarea
        Collider col = openAreas[i].GetComponent<Collider>();

        //Creates random point inside the collider
        Vector3 RandomPoint = new Vector3(
            Random.Range(col.bounds.min.x, col.bounds.max.x),
            0f,
            Random.Range(col.bounds.min.z, col.bounds.max.z));

        //Returns that random point
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
        //Create a vector 3
        Vector3 along;

        //Fetches the collider of the desired stage
        Collider col = stages[i].GetComponent<Collider>();

        //Creates the closest point to the uppersatge of the stage area
        along = col.ClosestPoint(upperStage[i].transform.position);

        //Creates a random point along the the area nearest to the upper stage
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
        //Stores the best exit
        Transform best = default;

        //Stores distance
        float dist;

        //Stores bestDistance
        float bestDist = 0;

        //Cycle
        int cycle = 0;

        //Checks every exit
        foreach (GameObject exit in exits)
        {
            //Stores current distance
            dist = Vector3.Distance(this.gameObject.transform.position,
                exit.transform.position);

            //Checks if its the first cycle
            if (cycle == 0)
            {
                //BestExit is this exit
                best = exit.transform;

                //BestDist is this dist
                bestDist = dist;
            }
            //If dist is inferior to best distance
            else if (dist < bestDist)
            {
                //Best exit is this exit
                best = exit.transform;

                //Best dist is this dist
                bestDist = dist;
            }

            //Next cycel
            cycle++;
        }     

        //Returns best Exit
        return best;
    }

    /// <summary>
    /// This Courotine is used for the agent regain concience after some time
    /// </summary>
    /// <returns></returns>
    private IEnumerator RegainConsciousness()
    {
        yield return new WaitForSeconds(Random.Range(2, 10));
        //Checks if agent is alive
        if (isAlive)
        {
            //Agent is no longer stunned
            isStunned = false;

            //Agent is no longer stopped
            agent.isStopped = false;

            //Panics
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
        //Checks if player is alive
        if (isAlive)
        {
            //Sends player to a destination
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
        //Changes the avoidance prediciton time
        NavMesh.avoidancePredictionTime = 5f;

        //Checks if agent is alive and not stunned
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
        //Checks if player is not alive
        if (!isAlive)
        {
            //Destroys agent
            Destroy(gameObject);
        }
            
    }

    /// <summary>
    /// This method is used to make the agent go to the exit
    /// </summary>
    public void GoToExit()
    {
        //Flees
        Flee();
    }

    /// <summary>
    /// This method sets the hungry mode of the agent
    /// </summary>
    /// <param name="mode"></param>
    public void SetHungryMode(bool mode)
    {
        //Changes eating mode
        isEating = mode;
    }
}