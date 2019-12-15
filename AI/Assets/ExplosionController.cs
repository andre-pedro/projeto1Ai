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
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wordPos;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                wordPos = hit.point;
            }
            else
            {
                wordPos = Camera.main.ScreenToWorldPoint(mousePos);
            }
            Instantiate(explosion, wordPos, Quaternion.identity);
            Instantiate(fire, wordPos, Quaternion.identity);
        }        
    }
}
