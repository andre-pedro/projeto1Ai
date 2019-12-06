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
    public bool isHungry;
    public bool isTired;

    private NavMeshAgent agent;
    private NavMeshPath path;

    private Vector3 destination;    
    private Vector3 panicOrigin;

    private Transform exit;
    private Transform restauracao;
    private Transform openArea;

    private float hunger;
    private float tired;
    

    private void Start()
    {
        hunger = Random.Range(50f, 101f);
        tired = Random.Range(50f, 101f);

        path = new NavMeshPath();
        agent = GetComponent<NavMeshAgent>();

        exit = GameObject.FindGameObjectWithTag("Exit").transform;
        restauracao = GameObject.FindGameObjectWithTag("Eat").transform;
        openArea = GameObject.FindGameObjectWithTag("Open").transform;

        behaviour = Behaviour.Wander;

        isAlive = true;
    }

    private void Update()
    {

        if(hunger >= 0f)
        {
            hunger -= Random.Range(0f, 0.03f);

        }else if (hunger < 0f)
        {
            hunger = 0f;
        }
        if (tired >= 0f)
        {
            tired -= Random.Range(0f, 0.04f);

        }else if(tired < 0f)
        {
            tired = 0f;
        }                   
        
        switch (behaviour)
        {
            case Behaviour.Idle:
                Idle();
                break;

            case Behaviour.Wander:
                if (hunger <= 25f)
                {
                    isHungry = true;
                    behaviour = Behaviour.Seek;                        
                }
                if (tired <= 25f)
                {
                    isTired = true;
                    behaviour = Behaviour.Seek;
                }
                
                Wander();
                                   
                break;

            case Behaviour.Seek:
                //Prioritizes Food over Tired
                if (isHungry)
                {
                    SeekFood();

                }else if (isTired)
                {

                }
                
                break;

            case Behaviour.Flee:
                if (agent.remainingDistance < 0.5f) Debug.Log("DONE");
                break;
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
