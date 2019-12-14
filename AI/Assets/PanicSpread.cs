using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class is used to spread the panic to ther agents
/// </summary>
public class PanicSpread : MonoBehaviour
{
    private bool canPass;

    /// <summary>
    /// The update checks if the agent is in panic and let's him spread the 
    /// panic or not according to it
    /// </summary>
    void Update()
    {
        if (this.GetComponentInParent<AgentBehaviour>().inPanic)
        {
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
        if (canPass && other.gameObject.GetComponent<AgentBehaviour>() != null)
        {
            if (!other.gameObject.GetComponent<AgentBehaviour>().inPanic)
            {
                other.gameObject.GetComponent<AgentBehaviour>().inPanic = true;
                other.gameObject.GetComponent<AgentBehaviour>().GoToExit();
            }
            
        }
    }
}
