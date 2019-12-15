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
    /// The Update checks if the mouse was click and instanciate an explosion
    /// and fire in the location
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f))
            {
                Debug.Log(hit.collider.gameObject.tag);
                if (hit.collider.gameObject.tag != "Stage" &&
                    hit.collider.gameObject.tag != "Walls")
                {
                    Instantiate(explosion, hit.point, Quaternion.identity);
                    Instantiate(fire, hit.point, Quaternion.identity);
                }

            }

        }
    }
}
