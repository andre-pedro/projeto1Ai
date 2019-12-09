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

    private float timer;
    int i;

    private void Start()
    {
        i = 0;
        timer = 0.1f;
        //SpawnPopulation();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0f && i != maxAgents)
        {
            Debug.Log($"{i + 1} agents on field");
            SpawnAgent(i);
            i++;
            timer = 0.1f;
            //SpawnPopulation();
        }
        else
        {

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
