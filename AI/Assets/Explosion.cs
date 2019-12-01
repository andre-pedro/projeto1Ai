using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Explosion : MonoBehaviour
{
    private void Start()
    {
        KillAgentsInRadius1();
        StunAgentsInRadius2();
        ScareAgentsInRadius3();
    }

    private void KillAgentsInRadius1()
    {
        Collider[] hits = Physics.OverlapSphere(
                  transform.position,
                  10f);

        foreach (Collider hit in hits)
        {
            if (hit.GetComponent<AgentBehaviour>() != null)
            {
                hit.GetComponent<AgentBehaviour>().enabled = false;
                hit.GetComponent<NavMeshAgent>().enabled = false;
                hit.GetComponent<Rigidbody>().isKinematic = false;
                hit.GetComponent<Rigidbody>().AddExplosionForce(
                    1000f, 
                    transform.position, 
                    10f, 
                    1000f);

                hit.GetComponent<AgentBehaviour>().Die();

                Debug.Log($"Killed {hit.gameObject.name}");
            }
        }
    }

    private void StunAgentsInRadius2()
    {
        Collider[] hits = Physics.OverlapSphere(
                  transform.position,
                  15f);

        foreach (Collider hit in hits)
        {
            if (hit.GetComponent<AgentBehaviour>() != null &&
                hit.GetComponent<AgentBehaviour>().isAlive == true)
            {
                hit.GetComponent<AgentBehaviour>().Stun();

                Debug.Log($"Stunned {hit.gameObject.name}");
            }
        }
    }

    private void ScareAgentsInRadius3()
    {
        Collider[] hits = Physics.OverlapSphere(
                  transform.position,
                  100f);

        foreach (Collider hit in hits)
        {
            if (hit.GetComponent<AgentBehaviour>() != null &&
                hit.GetComponent<AgentBehaviour>().isAlive == true &&
                hit.GetComponent<AgentBehaviour>().isStunned == false)
            {
                hit.GetComponent<AgentBehaviour>().Panic(transform.position);

                Debug.Log($"Scared {hit.gameObject.name}");
            }
        }
    }
}