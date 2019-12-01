using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> agents = new List<GameObject>();

    [Range(0, 10000)]
    [SerializeField]
    private int maxAgents;

    [SerializeField]
    private GameObject agentPrefab;

    [SerializeField]
    private Transform agentHolder;

    private void Start()
    {
        SpawnPopulation();
    }

    private void SpawnPopulation()
    {
        for (int i = 0; i < maxAgents; i++)
        {
            SpawnAgent(i);
        }
    }

    private void SpawnAgent(int i)
    {
        GameObject agent = Instantiate(agentPrefab, agentHolder);
        agent.name += i;
        agents.Add(agent);
    }

    public void RemoveAgent(GameObject agent)
    {
        agents.Remove(agent);
    }

    public GameObject GetRandomAgent()
    {
        return agents[Random.Range(0, agents.Count - 1)];
    }

    public int GetAgentListSize()
    {
        return agents.Count;
    }
}
