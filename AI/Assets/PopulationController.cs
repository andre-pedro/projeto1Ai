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
        //Current amount of agents in scene
        i = 0;

        //Amount of time in between each agent spawns
        timer = 0.1f;

        //Fetched all the active spawners
        spawners = GameObject.FindGameObjectsWithTag("Spawn");
    }

    /// <summary>
    /// The Update is used to populate the scene, instanciating one agent
    /// each 0.1 seconds
    /// </summary>
    private void Update()
    {
        //If it's allow to start this will begin to spawn agents until 
        //it reaches the desired number
        if (canStart)
        {
            //Decreases value according to time
            timer -= Time.deltaTime;

            //When timer reaches 0 and the max amount of agents in scene has
            //not been reached yet
            if (timer <= 0f && i != maxAgents)
            {
                //Spawns an agent
                SpawnAgent(i);

                //Increases number of active agents in scene
                i++;

                //Resets the time in between spawns to 0.1f
                timer = 0.1f;
            }
            else
            {
                //Stops Spawning
            }
        }
        
    }

    /// <summary>
    /// Event that Updates GUI
    /// </summary>
    void OnGUI()
    {
        //Displays the current amount of agents in scene
        GUI.Label(new Rect(10, 10, 200, 20), $"Current agents on scene: " +
            $"{agents.Count}");

        //Display current amount of dead agents in scene
        GUI.Label(new Rect(10, 25, 200, 20), $"Dead agents: {deadAgents}");

        //Displays current amount of agents that successfully escaped
        GUI.Label(new Rect(10, 40, 300, 20),
            $"Successfully escaped agents: {agentsThatEscaped}");
    }

    /// <summary>
    /// This method is used to spawn an agent and add it to a List of agents
    /// </summary>
    /// <param name="i"></param>
    private void SpawnAgent(int i)
    {
        //Chooses a random spawn point for the agent to be spawned
        int x = Random.Range(0, spawners.Length);

        //Spawns the agent
        GameObject agent = Instantiate(agentPrefab, spawners[x].transform);

        //Assigns the agent name
        agent.name += i;

        //Adds agent to the List of current amount of agents in scene
        agents.Add(agent);
    }

    /// <summary>
    /// This method is used to remove agents from a list
    /// </summary>
    /// <param name="agent"></param>
    public void RemoveAgent(GameObject agent)
    {
        //Removes agent of the amount of active agents on scene
        agents.Remove(agent);
    }

    /// <summary>
    /// Increases the number of dead agents
    /// </summary>
    public void UpdatedDeadAngents()
    {
        //Increases the number of dead agents
        deadAgents++;
    }

    /// <summary>
    /// Increases the number of escaped agents
    /// </summary>
    public void EscapedAgents()
    {
        //Increases number of escaped agents
        agentsThatEscaped++;
    }

    /// <summary>
    /// Sets the number of max agents that can be spawned
    /// </summary>
    /// <param name="x"></param>
    public void SetMaxAgents(int x)
    {
        //Sets the max amount agents to be spawned
        maxAgents = x;
    }

    /// <summary>
    /// Sets the number of exits that will be available on the scene
    /// </summary>
    /// <param name="x"></param>
    public void SetExits(int x)
    {
        //See the amount of exits desired to be in the scene and acts according
        //to it
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
