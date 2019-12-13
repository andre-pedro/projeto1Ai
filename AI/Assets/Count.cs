using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Count : MonoBehaviour
{
    private int numberOfAgents = 0;
    private List<string> nameOfAgents = new List<string>();

    private void OnTriggerExit(Collider other)
    {
        if (nameOfAgents.Contains(other.name))
        {
            numberOfAgents--;
            nameOfAgents.Remove(other.name);
        }
    }

    public void IsGoing(string nameOfAgent)
    {
        numberOfAgents++;
        //nameOfAgents.Add(nameOfAgent);
        nameOfAgents.Add(nameOfAgent);
    }

    public int GetNumberOfAgents()
    {
        return numberOfAgents;
    }
}
