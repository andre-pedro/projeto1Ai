using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to populate the scene with the agents
/// </summary>
public class PopulationController : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> agents = new List<GameObject>();

    [SerializeField]
    private int maxAgents;

    [SerializeField]
    private GameObject agentPrefab;

    [SerializeField]
    private Text currentAgents;

    [SerializeField]
    private Text dead;

    [SerializeField]
    private GameObject exit1;

    [SerializeField]
    private GameObject exit2;

    [SerializeField]
    private GameObject wall1;

    [SerializeField]
    private GameObject wall2;

    [SerializeField]
    private GameObject spawn1;

    [SerializeField]
    private GameObject spawn2;

    public bool canStart { get; set; }

    private GameObject[] agentHolder;

    private int deadAgents = 0;
    private int agentsThatEscaped = 0;

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

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"Current agents on scene - " +
            $"{agents.Count}");

        GUI.Label(new Rect(10, 25, 200, 20), $"Dead agents - {deadAgents}");
        GUI.Label(new Rect(10, 40, 300, 20), $"Sucessefully escaped agents - {agentsThatEscaped}");
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

    public void UpdatedDeadAngents()
    {
        deadAgents++;
    }

    public void EscapedAgents()
    {
        agentsThatEscaped++;
    }

    public void SetMaxAgents(int x)
    {
        maxAgents = x;
    }

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
