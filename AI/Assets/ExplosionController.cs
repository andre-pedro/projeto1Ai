using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to instanciate the explosion
/// </summary>
public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject fire;

    private Vector3 agent;

    /// <summary>
    /// The Update checks if the DownArrow has been pressed and
    /// after it has been pressed instanciates an explosion on a random agent
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (GameManager.Instance
                .GetComponent<PopulationController>()
                .GetAgentListSize() > 0)
            {
                agent = GameManager.Instance
                    .GetComponent<PopulationController>()
                    .GetRandomAgent().transform.position + Vector3.down;
                Instantiate(
                    explosion, agent,
                    transform.rotation
                    );
                Instantiate(
                    fire, agent,
                    transform.rotation
                    );
            }
        }
    }
}
