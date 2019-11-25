using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> agents = new List<GameObject>();

    [Range(100, 1000)]
    [SerializeField]
    private int maxAgents;

    [SerializeField]
    private GameObject agentPrefab;

    private void Update()
    {
        ControlPopulation();
    }

    private void ControlPopulation()
    {
        if (agents.Count < maxAgents)
        {
            SpawnAgent();
        }
    }

    private void SpawnAgent()
    {
        GameObject agent = Instantiate(agentPrefab);
        agents.Add(agent);
    }

    public void DespawnAgent(GameObject agent)
    {
        Destroy(agent);
        agents.Remove(agent);
    }
}
