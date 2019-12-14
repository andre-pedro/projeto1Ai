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
        if (other.name == "KillPlane")
        {
            GameManager.Instance.GetComponent<PopulationController>()
                .RemoveAgent(gameObject);
            Destroy(gameObject);
        }

        if (other.tag == "Exit")
        {
            GameManager.Instance.GetComponent<PopulationController>()
                .RemoveAgent(gameObject);
            Destroy(gameObject);
        }
    }
}
