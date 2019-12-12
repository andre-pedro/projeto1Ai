using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentBehaviour : MonoBehaviour
{
    public Behaviour behaviour;

    public bool isAlive;
    public bool isStunned;
    public bool inPanic;

    private bool isHungry;
    private bool isTired;

    private bool isHavingFun;
    private bool isEating;
    private bool isResting;
    private bool isGoingToFun;
    private bool isGoingToRest;
    private bool isGoingForFood;

    private bool hasFoundSeat = false;

    private NavMeshAgent agent;
    private NavMeshPath path;

    private Vector3 destination;    
    private Vector3 panicOrigin;

    private GameObject[] stages;
    private GameObject[] openAreas;
    private GameObject[] seats;

    private Transform exit;

    private float hunger;
    private float tired;
    private float fun = 0f;

    private Vector3 z;
    private int x = default;
    

    private void Start()
    {        
        hunger = Random.Range(5f, 200f);
        tired = Random.Range(5f, 200f);

        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();

        stages = GameObject.FindGameObjectsWithTag("Fun");
        openAreas = GameObject.FindGameObjectsWithTag("Open");
        seats = GameObject.FindGameObjectsWithTag("Seats");

        exit = GameObject.FindGameObjectWithTag("Exit").transform;
        
        
        behaviour = Behaviour.Seek;

        isAlive = true;
    }

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

                case Behaviour.Wander:
                    Wander();
                    break;

                case Behaviour.Seek:
                    //Prioritizes Food over Tired
                    if (isHungry && !isGoingForFood)
                    {
                        isGoingForFood = true;
                        SeekFood();

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
                    if (agent.remainingDistance < 0.5f) Debug.Log("DONE");
                    break;
            }

            Debug.DrawLine(agent.transform.position, agent.pathEndPosition, Color.black, 0.1f);
        }
        else
        {
            StopAllCoroutines();
        }
        
        
    }    

    private void Conditions()
    {
        if (isEating)
        {
            hunger += 0.08f;
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
            tired += 0.09f;
        }
        else
        {
            if (tired > 0.3f)
            {
                tired -= 0.03f;
            }
        }
        //----------------------------------------
        if (isHavingFun)
        {
            fun += Random.Range(0.01f, 0.04f);
        }        
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Open")
        {
            isResting = true;
        }

        if (other.gameObject.tag == "Fun")
        {
            isHavingFun = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isResting = false;
        isHavingFun = false;
    }

    private void SeekFun()
    {
        if (stages[0].GetComponent<Count>().GetNumberOfAgents() >
            stages[1].GetComponent<Count>().GetNumberOfAgents())
        {
            StartCoroutine(GoToFun(1));
        }
        else if (stages[0].GetComponent<Count>().GetNumberOfAgents() <
           stages[1].GetComponent<Count>().GetNumberOfAgents())
        {
            StartCoroutine(GoToFun(0));
        }
        else
        {
            int i = Random.Range(0, openAreas.Length);
            StartCoroutine(GoToFun(i));
        }
    }

    private void Idle()
    {
        agent.isStopped = true;
    }

    private bool SeekFood()
    {
        int i = 0;        

        while (i != seats.Length && !hasFoundSeat)
        {
            if (seats[i].GetComponent<PeopleGoing>().GetNumberOfAgentsGoing() == 0 && !hasFoundSeat)
            {
                seats[i].GetComponent<PeopleGoing>().UpdateAgentsGoing(agent.name);
                StopAllCoroutines();
                StartCoroutine(GoToFood(seats[i].transform.position));
                hasFoundSeat = true;
                return true;
            }
            i++;
        }

        return false;
        
    }

    private void SeekOpenZone()
    {
        if(openAreas[0].GetComponent<Count>().GetNumberOfAgents() >
            openAreas[1].GetComponent<Count>().GetNumberOfAgents())
        {
            StopAllCoroutines();
            StartCoroutine(GoToOpenZone(1));
        }else if (openAreas[0].GetComponent<Count>().GetNumberOfAgents() <
            openAreas[1].GetComponent<Count>().GetNumberOfAgents())
        {
            StopAllCoroutines();
            StartCoroutine(GoToOpenZone(0));
        }
        else
        {
            int i = Random.Range(0, openAreas.Length);
            StopAllCoroutines();
            StartCoroutine(GoToOpenZone(i));
        }            
    }

    private void Wander()
    {
        if (agent.remainingDistance < 0.5f)
        {
            GenerateWanderDestination();
        }
    }

    private void Flee()
    {
        NavMesh.SetAreaCost(3, 0);
        GenerateFleeDestination(panicOrigin);
        StartCoroutine(RunToExit());
    }

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

    public void Stun()
    {
        isStunned = true;
        behaviour = Behaviour.Idle;

        StartCoroutine(RegainConsciousness());
    }

    public void Panic(Vector3 origin)
    {
        inPanic = true;
        panicOrigin = origin;
        behaviour = Behaviour.Flee;
        StopAllCoroutines();
        Flee();
    }

    private void GenerateWanderDestination()
    {
        destination = new Vector3(
            transform.position.x + Random.Range(-15, 15),
            transform.position.y,
            transform.position.z + Random.Range(-15, 15));

        if (agent.CalculatePath(destination, path))
            agent.SetDestination(destination);
        else
            GenerateWanderDestination();
    }

    private void GenerateSeekDestination()
    {
        Vector3 linear = Vector3.zero;
    }

    private void GenerateFleeDestination(Vector3 panicOrigin)
    {
        Vector3 linear = Vector3.zero;

        linear = transform.position - panicOrigin;

        linear = linear.normalized * 50;

        destination = transform.position + linear;

        agent.SetDestination(destination);
    }

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

    private IEnumerator GoToFood(Vector3 seatPos)
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        if (isHungry)
        {
            agent.SetDestination(seatPos);
        }
    }

    private IEnumerator GoToOpenZone(int i)
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        if (isTired)
        {
            agent.SetDestination(openAreas[i].transform.position);
        }
    }

    private IEnumerator GoToFun(int i)
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        agent.SetDestination(stages[i].transform.position);
    }

    private IEnumerator RunToExit()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        if (isAlive && isStunned == false)
        {
            agent.SetDestination(exit.position);
        }
    }

    private IEnumerator DestroyAgent()
    {
        yield return new WaitForSeconds(Random.Range(8, 15));
        if (isAlive == false)
            Destroy(gameObject);
    }

    public void GoToExit()
    {
        Flee();
    }

    public void ChangePanic(bool state)
    {
        inPanic = state;
    }

    public void SetHungryMode(bool mode)
    {
        isEating = mode;
    }
}
