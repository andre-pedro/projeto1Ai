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
    private Text warning;

    /// <summary>
    /// This start assigns values to variables
    /// </summary>
    private void Start()
    {
        //Sets the minimum vale of the slider
        slider.minValue = 1;
        //Sets the maximum value to the slider
        slider.maxValue = 3;
    }

    /// <summary>
    /// The updated is used to update the text according to the slider value
    /// </summary>
    void Update()
    {
        //Text that is showing the current amount of exits selected
        text.text = $"Amount of exits: {slider.value}";
    }

    /// <summary>
    /// This method is used to start the scene with the desired parameters
    /// </summary>
    public void OnClick()
    {
        //Checks if can convert the input text to an integer
        try
        {
            //Only allows the progam to continue if the value is above or
            //equal to 100
            if(System.Convert.ToInt32(input.text) >= 100)
            {
                //Sends the slider value (Number of exits) to the
                //PopulationController script
                GameManager.Instance.GetComponent<PopulationController>()
                .SetExits(System.Convert.ToInt32(slider.value));

                ///Sends the amount of agents to be spawned to the 
                //PopulationController script
                GameManager.Instance.GetComponent<PopulationController>()
                .SetMaxAgents(System.Convert.ToInt32(input.text));

                //Allows the PopulationController to start spawning agents
                GameManager.Instance.GetComponent<PopulationController>()
                    .canStart = true;

                //Disables this Gameobject and his children
                this.gameObject.SetActive(false);
            }
            else
            {
                //Displays a warning message in case the amount of agents
                //to be spawned is inferior to 100
                warning.text = "Value must be above or equal to 100";
            }            

        }catch(System.Exception e)
        {
            //Displays a warning message in case its not possible to convert
            //string to int
            warning.text = "Please insert a number";
        }
        
    }
}
