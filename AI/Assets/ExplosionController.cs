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
        //Checks if the program is allowed to start
        if (GameManager.Instance.GetComponent<PopulationController>().canStart)
        {
            //Checks if left mouse click has been clicked
            if (Input.GetMouseButtonDown(0))
            {
                //Creates a ray that goes to the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //Creates a raycast hit
                RaycastHit hit;

                //Checks if the raycast has hit anything in the desired range
                if (Physics.Raycast(ray, out hit, 1000f))
                {
                    //Checks if the hit does not have any of the following tags
                    //Stage
                    //Walls
                    //Agent
                    //Panel
                    if (hit.collider.gameObject.tag != "Stage" &&
                        hit.collider.gameObject.tag != "Walls" &&
                        hit.collider.gameObject.tag != "Agent" &&
                        hit.collider.gameObject.tag != "Panel")
                    {
                        //Creates a position based on the raycast hit
                        Vector3 pos = new Vector3(hit.point.x, 0, hit.point.z);

                        //Instanciates an explosion
                        Instantiate(explosion, pos, Quaternion.identity);

                        //Instanciates a fire
                        Instantiate(fire, pos, Quaternion.identity);
                    }

                }

            }
        }        
    }
}
