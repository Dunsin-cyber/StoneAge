using UnityEngine;
using TMPro;

public class Flow : MonoBehaviour
{
    [Header("FLOW Chain Communication")]
    public TMP_Text successMessage;
    public TMP_Text errorMessage;

    [Header("User Details")]
    private string userAddress; // Store user address
    private int userBalance;    // Store user balance

    [Header("UI Elements")]
    public TMP_Text addressText; // UI element for address
    public TMP_Text balanceText; // UI element for balance

    void Start()
    {
        // Initialize UI
        UpdateUI();
    }

    // Function to create an account on Flow
    public void CreateAccount()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Application.ExternalCall("createAccount");
        }
#endif
    }

    // Function called when account creation succeeds
    public void OnCreateAccountSuccessful(string address)
    {
        Debug.Log("Account Created! Address: " + address);
        userAddress = address; // Store address
        UpdateUI(); // Update UI

        // Automatically fetch balance after account creation
        GetUserAccount();
    }

    // Function to fetch user account details
    public void GetUserAccount()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (Application.platform == RuntimePlatform.WebGLPlayer && !string.IsNullOrEmpty(userAddress))
        {
            Application.ExternalCall("getUserAccount", userAddress);
        }
#endif
    }

    // Function to handle received account details
    public void OnAccountData(string jsonData)
    {
        Debug.Log("Received Account Data: " + jsonData);

        // Parse JSON response (assuming a format like { "balance": "100", "id": "xyz" })
        AccountDetail data = JsonUtility.FromJson<AccountDetail>(jsonData);
        if (data != null)
        {
            userBalance = int.Parse(data.balance); // Store balance
            UpdateUI(); // Update UI with new balance
            UIManager.Instance.ToggleLoginPanel();
        }
    }

    // Function to update UI
    private void UpdateUI()
    {
        if (addressText != null)
        {
            addressText.text = string.IsNullOrEmpty(userAddress) ? "No Address" : userAddress;
        }

        if (balanceText != null)
        {
            balanceText.text = "Balance: " + userBalance.ToString();
        }
    }

    // Function to handle successful transactions
    public void OnTransactionSuccess(string message)
    {
        Debug.Log("Transaction Successful: " + message);
    }

    // Function to handle transaction failures
    public void OnTransactionFailure(string error)
    {
        Debug.LogError("Transaction Failed: " + error);
    }

    // Class to store account details received from frontend
    [System.Serializable]
    public class AccountDetail
    {
        public string balance;
        public string id;
    }
}
