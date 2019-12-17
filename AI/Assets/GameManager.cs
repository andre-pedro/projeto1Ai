using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that starts the GameManager
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Instance of GameManger
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Creates only one instance of GameManager
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
