using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    // public TMP_Text buttonTitle;
    // Reference to the Button component
    // public Button myButton;
    private bool isPressed = false;

    void Start()
    {
        // // Check if the button is assigned
        // if (myButton != null)
        // {
        //     // Add a listener to the button
        //     myButton.onClick.AddListener(OnButtonClicked);
        // }
    }



    // Called when the button is pressed down
    public void OnPointerDown(PointerEventData eventData)
    {
        TimeManager.Instance.Tick();

        isPressed = true;
        Debug.Log("Button Pressed");
    }

    // Called when the button is released
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        Debug.Log("Button Released");
    }

    // Method called when the button is clicked
    public void OnButtonClicked()
    {

        TimeManager.Instance.Tick();

    }

    public void IncreaseItemQty()
    {
        InventoryManager.Instance.SetAllItemQuantitiesTo50();
        // buttonTitle.SetText("Item Updated!");
    }




}
