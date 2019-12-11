using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanicSpread : MonoBehaviour
{
    private bool canPass;
    void Update()
    {
        if (this.GetComponentInParent<AgentBehaviour>().inPanic)
        {
            canPass = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canPass && other.gameObject.GetComponent<AgentBehaviour>() != null)
        {
            if (!other.gameObject.GetComponent<AgentBehaviour>().inPanic)
            {
                other.gameObject.GetComponent<AgentBehaviour>().ChangePanic(true);
                other.gameObject.GetComponent<AgentBehaviour>().GoToExit();
            }
            
        }
    }
}
