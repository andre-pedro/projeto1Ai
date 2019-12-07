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

    private NavMeshAgent agent;
    private NavMeshPath path;

    private Vector3 destination;    
    private Vector3 panicOrigin;

    private GameObject[] stages;
    private GameObject[] openAreas;

    private Transform exit;
    private Transform restauracao;

    private float hunger;
    private float tired;
    private float fun = 0f;

    private int x = default;
    

    private void Start()
    {        
        hunger = Random.Range(10f, 100f);
        tired = Random.Range(10f, 100f);

        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();

        stages = GameObject.FindGameObjectsWithTag("Fun");
        openAreas = GameObject.FindGameObjectsWithTag("Open");

        exit = GameObject.FindGameObjectWithTag("Exit").transform;
        restauracao = GameObject.FindGameObjectWithTag("Eat").transform;
        
        
        behaviour = Behaviour.Seek;

        isAlive = true;
    }

    private void Update()
    {
        //Debug.Log($"Hunger - {hunger} {isHungry} | tired {tired} {isTired} | fun {fun} \n GoingToRest {isGoingToRest} | GoingToFun {isGoingToFun} | isResting {isResting}");

        if (!inPanic && !isStunned && isAlive)
        {
            Conditions();
            ChecksStats();
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
                if (isHungry)
                {                    
                    SeekFood();

                }else if (isTired && !isGoingToRest)
                {
                    isGoingToRest = true;
                    SeekOpenZone();
                }

                if (!isHungry && !isTired && !isGoingToFun)
                {
                    isGoingToRest = false;
                    isGoingToFun = true;
                    SeekFun();
                }        
                break;

            case Behaviour.Flee:
                if (agent.remainingDistance < 0.5f) Debug.Log("DONE");
                break;
        }       
        
    }    

    private void Conditions()
    {
        if (!isEating)
        {
            if (hunger >= 0.03f)
            {
                hunger -= 0.01f;
            }
        }
        else
        {
            hunger += Random.Range(0.01f, 0.2f);
        }

        if (isHavingFun)
        {
            fun += Random.Range(0.01f, 0.04f);
            if(tired <= 0.05f)
            {
                // nao faz nada para estabilizar o valor e nao estar sempre a por tired = 0
            }
            else
            {
                tired -= Random.Range(0.02f, 0.05f);
            }           

            if (fun > Random.Range(100f, 251f))
            {
                StopAllCoroutines();
                StartCoroutine(RunToExit());
            }
        }

        if (isResting)
        {
            tired += Random.Range(0.01f, 0.2f);
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
        else
        {           
            isHungry = false;
            StopCoroutine(GoToFood());
        }
        if (tired <= 50f)
        {
            isGoingToFun = false;
            isTired = true;
        }
        else
        {
            isTired = false;
            StopCoroutine(GoToOpenZone(x));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.tag);

        if (other.gameObject.tag == "Eat")
        {
            isEating = true;
        }

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
        isEating = false;
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

    private void SeekFood()
    {
        StartCoroutine(GoToFood());
    }

    private void SeekOpenZone()
    {
        if(openAreas[0].GetComponent<Count>().GetNumberOfAgents() >
            openAreas[1].GetComponent<Count>().GetNumberOfAgents())
        {
            StartCoroutine(GoToOpenZone(1));
        }else if (openAreas[0].GetComponent<Count>().GetNumberOfAgents() <
            openAreas[1].GetComponent<Count>().GetNumberOfAgents())
        {
            StartCoroutine(GoToOpenZone(0));
        }
        else
        {
            int i = Random.Range(0, openAreas.Length);
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
        GenerateFleeDestination(panicOrigin);
        StartCoroutine(RunToExit());
    }

    public void Die()
    {
        isAlive = false;
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

    private IEnumerator GoToFood()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        if (isHungry)
        {
            agent.SetDestination(restauracao.position);
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
}
