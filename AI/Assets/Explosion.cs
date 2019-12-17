using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This Class is used to generate an explosion
/// </summary>
public class Explosion : MonoBehaviour
{
    /// <summary>
    /// The start is used to start the explosion by calling 3 methods that
    /// apply the effects of the 3 radius
    /// </summary>
    private void Start()
    {
        //Calls the first Radius of explosion
        KillAgentsInRadius1();

        //Calls second Radius of explosion
        StunAgentsInRadius2();

        //Calls third radius of explosion
        ScareAgentsInRadius3();
    }

    /// <summary>
    /// This method applies the effect of the first Radius(Kills the agents)
    /// </summary>
    private void KillAgentsInRadius1()
    {
        //Creates a colliders that are "Fired" in a designated range
        Collider[] hits = Physics.OverlapSphere(
                  transform.position,
                  10f);

        //Checks every collider hit
        foreach (Collider hit in hits)
        {
            //Checks if collider has hit an agent
            if (hit.GetComponent<AgentBehaviour>() != null)
            {
                //Kills the agent
                hit.GetComponent<AgentBehaviour>().Die();

                //Spawns a an explosion force
                hit.GetComponent<Rigidbody>().AddExplosionForce(
                    1000f,
                    transform.position,
                    10f,
                    1000f);

                //Updates number of dead agents
                GameManager.Instance.GetComponent<PopulationController>()
                    .UpdatedDeadAngents();
            }
        }
    }

    /// <summary>
    /// This method applies the effect of the second Radius(Stuns the agents)
    /// </summary>
    private void StunAgentsInRadius2()
    {
        //Creates a colliders that are "Fired" in a designated range
        Collider[] hits = Physics.OverlapSphere(
                  transform.position,
                  15f);

        //Checks every collider hit
        foreach (Collider hit in hits)
        {
            //Checks if collider has hit an agents and if he is alive
            if (hit.GetComponent<AgentBehaviour>() != null &&
                hit.GetComponent<AgentBehaviour>().isAlive == true)
            {
                //Decreases speed value to half of the original value
                hit.GetComponent<NavMeshAgent>().speed = 2.5f;

                //Stuns the agent
                hit.GetComponent<AgentBehaviour>().Stun();
            }
        }
    }

    /// <summary>
    /// This method applies the effect of the third Radius(Scares the agents)
    /// </summary>
    private void ScareAgentsInRadius3()
    {
        //Creates a colliders that are "Fired" in a designated range
        Collider[] hits = Physics.OverlapSphere(
                  transform.position,
                  50);

        //Checks every collider hits
        foreach (Collider hit in hits)
        {
            //Checks if collider has hit an agents and if he is alive
            if (hit.GetComponent<AgentBehaviour>() != null &&
                hit.GetComponent<AgentBehaviour>().isAlive == true &&
                hit.GetComponent<AgentBehaviour>().isStunned == false)
            {
                //Increases speed value to double of his original speed 
                hit.GetComponent<NavMeshAgent>().speed = 10f;

                //Makes agent panic
                hit.GetComponent<AgentBehaviour>().Panic(transform.position);
            }
        }
    }
}