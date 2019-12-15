using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Class is used to check how many agents are in a table
/// </summary>
public class AgentsInTable : MonoBehaviour
{
    public int AgentsIn;

    /// <summary>
    /// This method is used to Remove agents from the table
    /// </summary>
    public void RemoveAgents()
    {
        AgentsIn--;
    }

    /// <summary>
    /// This method is used to add agents to the table
    /// </summary>
    public void AddAgents()
    {
        AgentsIn++;
    }

    /// <summary>
    /// This method returns the number of agents in the table
    /// </summary>
    /// <returns>Number of agents inside table</returns>
    public int GetAgents()
    {
        return AgentsIn;
    }
}
