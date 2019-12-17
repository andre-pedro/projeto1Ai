using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to populate the scene with the agents
/// </summary>
public class PopulationController : MonoBehaviour
{
    /// <summary>
    /// List with all the agents alive inside scene
    /// </summary>
    [SerializeField]
    private List<GameObject> agents = new List<GameObject>();

    /// <summary>
    /// Define the max number of agents that will be spawned inside scene
    /// </summary>
    [SerializeField]
    private int maxAgents;

    /// <summary>
    /// Is the prefab that will be spawned as agents
    /// </summary>
    [SerializeField]
    private GameObject agentPrefab;

    /// <summary>
    /// GameObject that will represent the first exit
    /// </summary>
    [SerializeField]
    private GameObject exit1;

    /// <summary>
    /// GameObject that will represent the second exit
    /// </summary>
    [SerializeField]
    private GameObject exit2;

    /// <summary>
    /// GameObject that will represent the first wall
    /// </summary>
    [SerializeField]
    private GameObject wall1;

    /// <summary>
    /// GameObject that will represent the second wall
    /// </summary>
    [SerializeField]
    private GameObject wall2;

    /// <summary>
    /// GameObject that will represent the first spawner
    /// </summary>
    [SerializeField]
    private GameObject spawn1;

    /// <summary>
    /// GameObject that will represent the second spawner
    /// </summary>
    [SerializeField]
    private GameObject spawn2;

    /// <summary>
    /// This bool is used to check if the PopulationController 
    /// can start spawning agents
    /// </summary>
    public bool canStart { get; set; }

    /// <summary>
    /// Array with all the spawners inside scene
    /// </summary>
    private GameObject[] spawners;

    /// <summary>
    /// Variable with the number of agents that are dead
    /// </summary>
    private int deadAgents = 0;

    /// <summary>
    /// Variable with the number of agents that escaped
    /// </summary>
    private int agentsThatEscaped = 0;

    /// <summary>
    /// This variable is used to set the time between spawns
    /// </summary>
    private float timer;

    /// <summary>
    /// Amount of agents that have been spawned
    /// </summary>
    int i;

    /// <summary>
    /// The start is used to assign variables and GameObjects
    /// </summary>
    private void Start()
    {
        i = 0;
        timer = 0.1f;
        spawners = GameObject.FindGameObjectsWithTag("Spawn");
    }

    /// <summary>
    /// The Update is used to populate the scene, instanciating one agent
    /// each 0.1 seconds
    /// </summary>
    private void Update()
    {
        if (canStart)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f && i != maxAgents)
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
        
    }

    /// <summary>
    /// Event that Updates GUI
    /// </summary>
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"Current agents on scene: " +
            $"{agents.Count}");

        GUI.Label(new Rect(10, 25, 200, 20), $"Dead agents: {deadAgents}");
        GUI.Label(new Rect(10, 40, 300, 20), $"Successfully escaped agents: {agentsThatEscaped}");
    }

    /// <summary>
    /// This method is used to spawn an agent and add it to a List of agents
    /// </summary>
    /// <param name="i"></param>
    private void SpawnAgent(int i)
    {
        int x = Random.Range(0, spawners.Length);
        GameObject agent = Instantiate(agentPrefab, spawners[x].transform);
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

    /// <summary>
    /// Increases the number of dead agents
    /// </summary>
    public void UpdatedDeadAngents()
    {
        deadAgents++;
    }

    /// <summary>
    /// Increases the number of escaped agents
    /// </summary>
    public void EscapedAgents()
    {
        agentsThatEscaped++;
    }

    /// <summary>
    /// Sets the number of max agents that can be spawned
    /// </summary>
    /// <param name="x"></param>
    public void SetMaxAgents(int x)
    {
        maxAgents = x;
    }

    /// <summary>
    /// Sets the number of exits that will be available on the scene
    /// </summary>
    /// <param name="x"></param>
    public void SetExits(int x)
    {
        switch (x)
        {
            case 1:
                wall1.SetActive(true);
                wall2.SetActive(true);
                exit1.SetActive(false);
                exit2.SetActive(false);
                spawn1.SetActive(false);
                spawn2.SetActive(false);
                break;

            case 2:
                wall1.SetActive(true);
                wall2.SetActive(false);
                exit1.SetActive(false);
                exit2.SetActive(true);
                spawn1.SetActive(false);
                spawn2.SetActive(true);
                break;

            case 3:
                wall1.SetActive(false);
                wall2.SetActive(false);
                exit1.SetActive(true);
                exit2.SetActive(true);
                spawn1.SetActive(true);
                spawn2.SetActive(true);
                break;
        }
    }

}
