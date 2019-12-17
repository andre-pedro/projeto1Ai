using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to instanciate the explosion
/// </summary>
public class ExplosionController : MonoBehaviour
{
    /// <summary>
    /// Prefab of the explosion
    /// </summary>
    [SerializeField]
    private GameObject explosion;

    /// <summary>
    /// Prefab of the fire
    /// </summary>
    [SerializeField]
    private GameObject fire;

    /// <summary>
    /// The Update checks if the mouse was click and instanciate an explosion
    /// and fire in the location
    /// </summary>
    private void Update()
    {
        if (GameManager.Instance.GetComponent<PopulationController>().canStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    if (hit.collider.gameObject.tag != "Stage" &&
                        hit.collider.gameObject.tag != "Walls" &&
                        hit.collider.gameObject.tag != "Agent" &&
                        hit.collider.gameObject.tag != "Panel")
                    {
                        Vector3 pos = new Vector3(hit.point.x, 0, hit.point.z);
                        Instantiate(explosion, pos, Quaternion.identity);
                        Instantiate(fire, pos, Quaternion.identity);
                    }

                }

            }
        }        
    }
}
