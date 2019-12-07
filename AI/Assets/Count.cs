using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Count : MonoBehaviour
{
    private int numberOfAgents = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        numberOfAgents++;
    }

    private void OnTriggerExit(Collider other)
    {
        numberOfAgents--;
    }

    public int GetNumberOfAgents()
    {
        return numberOfAgents;
    }
}
