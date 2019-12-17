using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This Class is used to start the simulation with the desired information
/// </summary>
public class UI : MonoBehaviour
{
    /// <summary>
    /// Is the InputField in the Panel
    /// </summary>
    [SerializeField]
    private InputField input;

    /// <summary>
    /// Is the slider in the Panel
    /// </summary>
    [SerializeField]
    private Slider slider;

    /// <summary>
    /// Is the Text above the slider to display his value
    /// </summary>
    [SerializeField]
    private Text text;

    /// <summary>
    /// Text that will be used as warning in case player doesn't insert
    /// a valid number
    /// </summary>
    [SerializeField]
    private Text warming;

    /// <summary>
    /// This start assigns values to variables
    /// </summary>
    private void Start()
    {
        slider.minValue = 1;
        slider.maxValue = 3;
    }

    /// <summary>
    /// The updated is used to update the text according to the slider value
    /// </summary>
    void Update()
    {
        text.text = $"Amount of exits: {slider.value}";
    }

    /// <summary>
    /// This method is used to start the scene with the desired parameters
    /// </summary>
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
