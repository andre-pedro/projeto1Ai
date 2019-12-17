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
    /// Variable used to fetch the table this GameObject is assigned tos
    /// </summary>
    private AgentsInTable table;

    /// <summary>
    /// Start is used to assign Components
    /// </summary>
    private void Start()
    {
        //Gets a component in parent
        table = GetComponentInParent<AgentsInTable>();
    }

    /// <summary>
    /// This method checks if the agent that entered the seat is 
    /// the one assigned and if it is he let's him eat
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Checks if the agent that entered the collider is the one 
        //allowed to eat
        if (other.gameObject.name == agentName)
        {
            //Let's the agent eat
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
        //Checks if the agent that left the collider was the one allowed to eat
        if (other.gameObject.name == agentName)
        {
            //Doesn't allow the agent to eat anymore
            other.gameObject.GetComponent<AgentBehaviour>()
                .SetHungryMode(false);

            //Resets the number of agents coming to this GameObject
            numberOfAgentsGoing = 0;

            //Resets the agent name
            agentName = "";

            //Removes an agent from the table
            table.RemoveAgents();
        }
    }

    /// <summary>
    /// This agent reserves the seat with the agent name
    /// </summary>
    /// <param name="name"></param>
    public void UpdateAgentsGoing(string name)
    {
        //Updates the name of the agent coming to this GameObject
        agentName = name;

        //Updates the number of agents coming to thi GameObject
        numberOfAgentsGoing = 1;

        //Adds an agent to the table
        table.AddAgents();        
    }

    /// <summary>
    /// this method is used to see if the seat is reserved already
    /// </summary>
    /// <returns>number of agents assigned to that seat</returns>
    public int GetNumberOfAgentsGoing()
    {
        //Returns the number of agents coming to this GameObject
        return numberOfAgentsGoing;
    }
}
