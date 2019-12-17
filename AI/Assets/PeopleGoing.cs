using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to determine the number 
/// of agents that are going to one place
/// </summary>
public class PeopleGoing : MonoBehaviour
{
    /// <summary>
    /// Number of agents that are currently going to ths GameObject
    /// </summary>
    public int numberOfAgentsGoing = 0;

    /// <summary>
    /// Name of the Agent coming to this GameObject
    /// </summary>
    public string agentName;

    /// <summary>
    /// Variable to see if an agent is permited to eat in this GameObject
    /// </summary>
    private bool canEat;

    /// <summary>
    /// Variable used to fetch the table this GameObject is assigned tos
    /// </summary>
    private AgentsInTable table;

    /// <summary>
    /// Start is used to assign Components
    /// </summary>
    private void Start()
    {
        table = GetComponentInParent<AgentsInTable>();
    }

    /// <summary>
    /// This method checks if the agent that entered the seat is 
    /// the one assigned and if it is he let's him eat
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == agentName)
        {
            //Debug.Log("he entered");
            other.gameObject.GetComponent<AgentBehaviour>()
                .SetHungryMode(true);
        }
    }

    /// <summary>
    /// This method checks if the agent leaving was the one with the 
    /// assign seat and unreserves the seat
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == agentName)
        {
            other.gameObject.GetComponent<AgentBehaviour>()
                .SetHungryMode(false);
            numberOfAgentsGoing = 0;
            agentName = "";
            table.RemoveAgents();
        }
    }

    /// <summary>
    /// This agent reserves the seat with the agent name
    /// </summary>
    /// <param name="name"></param>
    public void UpdateAgentsGoing(string name)
    {
        agentName = name;
        numberOfAgentsGoing = 1;
        table.AddAgents();
        
    }

    /// <summary>
    /// this method is used to see if the seat is reserved already
    /// </summary>
    /// <returns>number of agents assigned to that seat</returns>
    public int GetNumberOfAgentsGoing()
    {
        return numberOfAgentsGoing;
    }
}
