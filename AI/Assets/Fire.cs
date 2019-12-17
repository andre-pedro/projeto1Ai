using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is used to spread the fire
/// </summary>
public class Fire : MonoBehaviour
{
    /// <summary>
    /// Collider of this GameObject
    /// </summary>
    private Collider col;

    /// <summary>
    /// NavMeshObstacle of this GameObject
    /// </summary>
    private NavMeshObstacle navObs;

    /// <summary>
    /// The start is used to assign GameObjects
    /// </summary>
    private void Start()
    {
        //Assign the collider
        col = GetComponent<Collider>();

        //Assigns the NavMeshCollider
        navObs = GetComponent<NavMeshObstacle>();
    }

    /// <summary>
    /// The update is used to grow the fire(+ some of its components)
    /// according to time
    /// </summary>
    void Update()
    {
        //Increases the size of the GameObject
        transform.localScale += new Vector3(Time.deltaTime, 0f,
            Time.deltaTime);

        //Increses the size of the collider
        col.transform.localScale += new Vector3(Time.deltaTime, 0f,
            Time.deltaTime);

        //Increases the size of the NavMeshCollider
        navObs.transform.localScale += new Vector3(Time.deltaTime, 0f,
            Time.deltaTime);
    }

    /// <summary>
    /// This method checks if any agents was hit by the fire and
    /// if they did kills them
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Checks if the "thing" entering the collider is an agent
        if(other.tag == "Agent")
        {
            //Updated the number of dead agents
            GameManager.Instance.GetComponent<PopulationController>()
                    .UpdatedDeadAngents();

            //Kills the agent
            other.GetComponent<AgentBehaviour>().Die();
        }
        
    }
}
