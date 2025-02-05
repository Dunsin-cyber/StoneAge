using UnityEngine;
using UnityEngine.UI;  // To access UI components like Button

public class ButtonHoldController : MonoBehaviour
{
    private bool isButtonHeld = false;
    void Start()
    {
        // Ensure the button is not null and hook up the events
        // if (yourButton != null)
        // {
        //     // Button press starts the continuous function
        //     yourButton.onClick.AddListener(ButtonPress);
        // }
    }

    void Update()
    {
        // Continuously call Tick() while the button is being held
        if (isButtonHeld)
        {
            TimeManager.Instance.Tick();
        }
    }

    // This is called when the button is pressed
    public void ButtonPress()
    {

        isButtonHeld = true;  // Start holding the button
        UIManager.Instance.RemoveFromCoins();
    }

    // You can also detect when the button is released
    public void ButtonRelease()
    {
        isButtonHeld = false;  // Stop holding the button
    }

}
