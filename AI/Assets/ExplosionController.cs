using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (GameManager.Instance
                .GetComponent<PopulationController>()
                .GetAgentListSize() > 0)
            {
                Instantiate(
                    explosion, 
                    GameManager.Instance
                    .GetComponent<PopulationController>()
                    .GetRandomAgent().transform.position + Vector3.down,
                    transform.rotation
                    );
            }
        }
    }
}
