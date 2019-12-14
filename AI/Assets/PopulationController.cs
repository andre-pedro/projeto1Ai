using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to populate the scene with the agents
/// </summary>
public class PopulationController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> agents = new List<GameObject>();

    [Range(0, 10000)]
    [SerializeField]
    private int maxAgents;

    [SerializeField]
    private GameObject agentPrefab;

    private GameObject[] agentHolder;

    private float timer;
    int i;

    /// <summary>
    /// The start is used to assign variables and GameObjects
    /// </summary>
    private void Start()
    {
        i = 0;
        timer = 0.1f;
        agentHolder = GameObject.FindGameObjectsWithTag("Spawn");
    }

    /// <summary>
    /// The Update is used to populate the scene, instanciating one agent
    /// each 0.1 seconds
    /// </summary>
    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0f && i != maxAgents)
        {
            //Debug.Log($"{i + 1} agents on field");
            SpawnAgent(i);
            i++;
            timer = 0.1f;
            //SpawnPopulation();
        }
        else
        {

        }
    }

    /// <summary>
    /// This method is used to spawn an agent and add it to a List of agents
    /// </summary>
    /// <param name="i"></param>
    private void SpawnAgent(int i)
    {
        int x = Random.Range(0, agentHolder.Length);
        GameObject agent = Instantiate(agentPrefab, agentHolder[x].transform);
        agent.name += i;
        agents.Add(agent);
    }

    /// <summary>
    /// This method is used to remove agents from a list
    /// </summary>
    /// <param name="agent"></param>
    public void RemoveAgent(GameObject agent)
    {
        agents.Remove(agent);
    }

    /// <summary>
    /// This method is used to retrn a random agent from the list
    /// </summary>
    /// <returns>Random Agent</returns>
    public GameObject GetRandomAgent()
    {
        return agents[Random.Range(0, agents.Count - 1)];
    }

    /// <summary>
    /// This list gets the size of the agent list
    /// </summary>
    /// <returns>Size of agent list</returns>
    public int GetAgentListSize()
    {
        return agents.Count;
    }
}
