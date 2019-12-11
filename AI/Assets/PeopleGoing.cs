using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleGoing : MonoBehaviour
{
    private int numberOfAgentsGoing = 0;
    private string agentName;
    private bool canEat;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == agentName)
        {
            //Debug.Log("he entered");
            other.gameObject.GetComponent<AgentBehaviour>().SetHungryMode(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == agentName)
        {
            other.gameObject.GetComponent<AgentBehaviour>().SetHungryMode(false);
            numberOfAgentsGoing = 0;
            agentName = "";
        }
    }

    public void UpdateAgentsGoing(string name)
    {
        agentName = name;
        numberOfAgentsGoing = 1;
    }

    public int GetNumberOfAgentsGoing()
    {
        return numberOfAgentsGoing;
    }
}
