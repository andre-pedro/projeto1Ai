using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Class is used to start the simulation with the desired information
/// </summary>
public class UI : MonoBehaviour
{
    [SerializeField]
    private InputField input;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text text;

    [SerializeField]
    private Text warming;

    private void Start()
    {
        slider.minValue = 1;
        slider.maxValue = 3;
    }

    void Update()
    {
        text.text = $"Amount of exits - {slider.value}";
    }

    public void OnClick()
    {
        try
        {
            GameManager.Instance.GetComponent<PopulationController>()
                .SetExits(System.Convert.ToInt32(slider.value));

            GameManager.Instance.GetComponent<PopulationController>()
            .SetMaxAgents(System.Convert.ToInt32(input.text));

            GameManager.Instance.GetComponent<PopulationController>()
                .canStart = true;

            this.gameObject.SetActive(false);

        }catch(System.Exception e)
        {
            warming.text = "Please insert a number";
        }
        
    }
}
