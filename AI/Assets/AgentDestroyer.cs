using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class destroys the agent
/// </summary>
public class AgentDestroyer : MonoBehaviour
{
    /// <summary>
    /// This method detects if the agent reaches a KillPlane or an exit
    /// and destroys them
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Checks if "other" has KillPlane tag
        if (other.name == "KillPlane")
        {
            //Removes agent from the list of agents
            GameManager.Instance.GetComponent<PopulationController>()
                .RemoveAgent(gameObject);

            //Destroys agent
            Destroy(gameObject);
        }

        //Checks if "other" has Exit tag
        if (other.tag == "Exit")
        {
            //Removes agent from agents list
            GameManager.Instance.GetComponent<PopulationController>()
                .RemoveAgent(gameObject);

            //Increases number of escaped agents
            GameManager.Instance.GetComponent<PopulationController>()
                .EscapedAgents();

            //Destroys Agent
            Destroy(gameObject);
        }
    }
}
