using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This Class is used to spread the panic to ther agents
/// </summary>
public class PanicSpread : MonoBehaviour
{
    /// <summary>
    /// Variable to check if this agent can spread fear
    /// </summary>
    private bool canPass;

    /// <summary>
    /// The update checks if the agent is in panic and let's him spread the 
    /// panic or not according to it
    /// </summary>
    void Update()
    {
        //Checks if this agent is allowed to sprea panic by seeing if he is
        //in panic
        if (this.GetComponentInParent<AgentBehaviour>().inPanic)
        {
            //Allows this agent to spread panic
            canPass = true;
        }
    }

    /// <summary>
    /// This method detects if other agents enters in the range of the spread 
    /// area and if he is not alredy in panic the panic is spread to him
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Checks if the "thing" that entered this collider in an agent
        if (canPass && other.gameObject.GetComponent<AgentBehaviour>() != null)
        {
            //Checks if the agent is in Panic already
            if (!other.gameObject.GetComponent<AgentBehaviour>().inPanic)
            {
                //Increases the speed to the double of his original speed
                other.gameObject.GetComponent<NavMeshAgent>().speed = 10;

                //Sets the agent to be inPanic
                other.gameObject.GetComponent<AgentBehaviour>().inPanic = true;

                //Makes the agent to go to the neares exit
                other.gameObject.GetComponent<AgentBehaviour>().GoToExit();
            }
            
        }
        
        //Sees if the "Thing" entering this collider is Fire
        if(other.tag == "Fire" && !canPass)
        {
            //Sets this agent in panic
            this.gameObject.GetComponentInParent<AgentBehaviour>().inPanic = true;

            //Makes the agent go to the nearest exit
            this.gameObject.GetComponent<AgentBehaviour>().GoToExit();

            //Increases the speed to the double of the original
            this.gameObject.GetComponent<NavMeshAgent>().speed = 10;
        }
        
    }
}
