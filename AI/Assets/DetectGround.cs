using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This Class is used to detect what kind of ground is bellow the agent and 
/// make adjustments to his behaviour based on that
/// </summary>
public class DetectGround : MonoBehaviour
{
    /// <summary>
    /// This is the method that detects the type of ground the agent is in
    /// using tags
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Open")
        {
            GetComponentInParent<AgentBehaviour>().isResting = true;
        }
    }

    /// <summary>
    /// This method is used to detect the type of ground the agent left and 
    /// make adjustments based in that information
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Open")
        {
            GetComponentInParent<AgentBehaviour>().isResting = false;
            //GetComponentInParent<NavMeshAgent>().radius = 0.6f;
        }
    }
}
