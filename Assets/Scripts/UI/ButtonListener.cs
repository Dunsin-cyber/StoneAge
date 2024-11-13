using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    // Reference to the Button component
    // public Button myButton;

    void Start()
    {
        // // Check if the button is assigned
        // if (myButton != null)
        // {
        //     // Add a listener to the button
        //     myButton.onClick.AddListener(OnButtonClicked);
        // }
    }

    // Method called when the button is clicked
    public void OnButtonClicked()
    {

        TimeManager.Instance.Tick();

    }
}
