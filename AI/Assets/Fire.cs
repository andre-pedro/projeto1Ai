﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Fire : MonoBehaviour
{
    private Collider col;

    private void Start()
    {
        col = GetComponent<Collider>();
    }
    void Update()
    {
        transform.localScale += new Vector3(Time.deltaTime, 0f, Time.deltaTime);
        col.transform.localScale += new Vector3(Time.deltaTime, 0f, Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Agent")
        {
            other.GetComponent<AgentBehaviour>().Die();
            Debug.Log($"Touched {other.name}");
        }
        
    }
}