using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectGround : MonoBehaviour
{
    public string currentTagAgentIsIn;
    public string LastTagAgentWasIn;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Open")
        {
            GetComponentInParent<AgentBehaviour>().isResting = true;
            GetComponentInParent<NavMeshAgent>().radius = Random.Range(0.6f, 2.5f);
        }
        if(other.tag == "Fun")
        {
            GetComponentInParent<AgentBehaviour>().isHavingFun = true;
        }

        currentTagAgentIsIn = other.tag;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Open")
        {
            GetComponentInParent<AgentBehaviour>().isResting = false;
            GetComponentInParent<NavMeshAgent>().radius = 0.6f;
        }
        if (other.tag == "Fun")
        {
            GetComponentInParent<AgentBehaviour>().isHavingFun = false;
        }
        LastTagAgentWasIn = other.tag;
    }
}
