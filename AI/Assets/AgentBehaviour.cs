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

    private bool isEating;
    private bool isResting;

    private NavMeshAgent agent;
    private NavMeshPath path;

    private Vector3 destination;    
    private Vector3 panicOrigin;

    private Transform exit;
    private Transform restauracao;
    private Transform openArea;

    public float hunger = 100f;
    public float tired = 100f;
    

    private void Start()
    {
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

        if (!isEating)
        {
            if (hunger >= 0.03f)
            {
                hunger -= Random.Range(0f, 0.03f);
            }
        }
        else
        {
            hunger += Random.Range(0.3f, 0.8f);

            if (hunger > Random.Range(75f, 101f))
            {
                isHungry = false;
                StopCoroutine(GoToFood());
            }
        }

        if (!isResting)
        {
            if (tired >= 0.05f)
            {
                tired -= Random.Range(0f, 0.05f);
            }
        }
        else
        {
            tired += Random.Range(0.3f, 0.8f);

            if(tired > Random.Range(75f, 101f))
            {
                isTired = false;
                StopCoroutine(GoToOpenZone());
            }
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
                    SeekOpenZone();
                }

                if (!isHungry && !isTired)
                {
                    behaviour = Behaviour.Wander;
                }
                
                break;

            case Behaviour.Flee:
                if (agent.remainingDistance < 0.5f) Debug.Log("DONE");
                break;
        }       
        
    }



    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"TouchedTag {other.gameObject.tag}");

        if (other.gameObject.tag == "Eat")
        {
            isEating = true;
        }
        else
        {
            isEating = false;
        }

        if (other.gameObject.tag == "Open")
        {
            isResting = true;
        }
        else
        {
            isResting = false;
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
        StartCoroutine(GoToOpenZone());
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

    private IEnumerator GoToOpenZone()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        if (isTired)
        {
            agent.SetDestination(openArea.position);
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
