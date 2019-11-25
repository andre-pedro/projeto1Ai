using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "KillPlane")
        {
            GameManager.Instance.GetComponent<PopulationController>()
                .DespawnAgent(gameObject);
        }
    }
}
