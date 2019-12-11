using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private GameObject fire;

    private Vector3 agent;

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
