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
        col = GetComponent<Collider>();
        navObs = GetComponent<NavMeshObstacle>();
    }

    /// <summary>
    /// The update is used to grow the fire(+ some of its components)
    /// according to time
    /// </summary>
    void Update()
    {
        transform.localScale += new Vector3(Time.deltaTime, 0f,
            Time.deltaTime);
        col.transform.localScale += new Vector3(Time.deltaTime, 0f,
            Time.deltaTime);
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
        if(other.tag == "Agent")
        {
            GameManager.Instance.GetComponent<PopulationController>()
                    .UpdatedDeadAngents();
            other.GetComponent<AgentBehaviour>().Die();
            Debug.Log($"Touched {other.name}");
        }
        
    }
}
