using TMPro;
using UnityEngine;

public class PurchaseManager : MonoBehaviour
{
    public static PurchaseManager Instance { get; private set; }

    public TMP_Text coinText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void RenderPurchaseManager()
    {
        coinText.SetText(UIManager.Instance.GetCoinBalance().ToString());
    }
}
