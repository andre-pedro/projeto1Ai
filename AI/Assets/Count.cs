using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class checks the number of agents going and currently are in a place
/// </summary>
public class Count : MonoBehaviour
{
    /// <summary>
    /// Number of agents in this GameObject
    /// </summary>
    private int numberOfAgents = 0;

    /// <summary>
    /// List of agent names in this GameObject
    /// </summary>
    private List<string> nameOfAgents = new List<string>();

    /// <summary>
    /// This method takes the agent of the list of agents goin/are in the place
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        //Checks if the name of the "thing" that entered the collider is inside
        //the list of agents that are assigned this GameObject
        if (nameOfAgents.Contains(other.name))
        {
            //Decreases number of agents in this GameObject
            numberOfAgents--;

            //Removes name of agent from the list
            nameOfAgents.Remove(other.name);
        }
    }

    /// <summary>
    /// This method adds an agent to the list of agents going/are
    /// </summary>
    /// <param name="nameOfAgent"></param>
    public void IsGoing(string nameOfAgent)
    {
        //Increses number of agents in this GameObject
        numberOfAgents++;

        //Adds Name of agent to the list of agents in this GameObject
        nameOfAgents.Add(nameOfAgent);
    }

    /// <summary>
    /// This method is used to see the number of agents goin/are in a place
    /// </summary>
    /// <returns>Number of agents going/are</returns>
    public int GetNumberOfAgents()
    {
        //Returns number of agents inside this GameObject
        return numberOfAgents;
    }
}
