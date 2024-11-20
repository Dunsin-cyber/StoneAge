using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class AeonPaymentManager : MonoBehaviour
{
    // public GameObject webViewGameObject;
    // private WebViewObject webViewObject;


    [Header("AEON Payment Settings")]
    public string aeonApiUrl = "https://sbx-crypto-payment-api.aeon.xyz/open/api/tg/payment/V2"; // Update with the correct URL
    public string appId = "xx";
    public string secretKey = "xx"; // Replace with your actual secret key
    // public string callbackURL = "https://yourcallback.url/v1/xxxxx";
    public string redirectURL = "https://stone-age-farm.vercel.app/resolution";
    public string userId = "ayodejiabisuwa23@gmail.com"; // User email or ID
    public string paymentExchange = "34aeb688-decb-485f-9d80-b66466783394,7f4307ea-58c6-4678-9eb2-fec205de5401";
    public TMP_Text paymentStatusText; // Optional UI text to display status

    void Start()
    {
        // webViewObject = webViewGameObject.GetComponent<WebViewObject>();
        // if (webViewObject == null)
        // {
        //     Debug.LogError("WebViewObject component is missing on the WebViewGameObject!");
        //     return;
        // }
        // webViewObject.Init(
        //     cb: (msg) => Debug.Log($"WebView Message: {msg}"),
        //     err: (msg) => Debug.LogError($"WebView Error: {msg}"),
        //     started: (msg) => Debug.Log($"WebView Started: {msg}"),
        //     ld: (msg) =>
        //     {
        //         Debug.Log($"WebView Loaded: {msg}");
        //         CheckPaymentStatus(msg); // Check for payment resolution
        //     }
        // );
        // webViewObject.SetMargins(0, 0, 0, 0);
        // webViewObject.SetVisibility(false);
        // CheckPaymentStatus();
    }
    // Method to generate the signature
    // Method to generate a signature without using Newtonsoft.Json
    private string GenerateSignature(Dictionary<string, string> parameters)
    {
        // Sort parameters alphabetically by key
        var sortedParams = new SortedDictionary<string, string>(parameters);
        StringBuilder concatenatedParams = new StringBuilder();

        // Build the signature string in the format: key1=value1&key2=value2...
        foreach (var param in sortedParams)
        {
            concatenatedParams.Append($"{param.Key}={param.Value}&");
        }

        // Append the secret key at the end
        concatenatedParams.Append($"key={secretKey}");

        // Generate the SHA-512 hash of the concatenated string
        using (SHA512 sha512 = SHA512.Create())
        {
            byte[] bytes = sha512.ComputeHash(Encoding.UTF8.GetBytes(concatenatedParams.ToString()));
            StringBuilder hash = new StringBuilder();
            foreach (byte b in bytes)
            {
                hash.Append(b.ToString("X2")); // Convert to uppercase hex string
            }
            return hash.ToString();
        }
    }


    // Helper method to convert a Dictionary to a JSON string
    private string ConvertToJson(Dictionary<string, string> dictionary)
    {
        StringBuilder jsonString = new StringBuilder("{");
        foreach (var kvp in dictionary)
        {
            jsonString.Append($"\"{kvp.Key}\":\"{kvp.Value}\",");
        }
        if (jsonString.Length > 1)
            jsonString.Remove(jsonString.Length - 1, 1); // Remove trailing comma
        jsonString.Append("}");
        return jsonString.ToString();
    }

    // Method to make the payment request
    public IEnumerator MakePayment(string orderAmount, string payCurrency = "USD", string paymentTokens = "USDT")
    {
        // Step 1: Prepare the request parameters
        string merchantOrderNo = DateTime.Now.Ticks.ToString(); // Unique order ID
        Dictionary<string, string> paymentData = new Dictionary<string, string>
        {
            { "appId", appId },
            { "merchantOrderNo", merchantOrderNo },
            { "userId", userId },
            { "orderAmount", orderAmount },
            { "payCurrency", payCurrency },
            { "paymentTokens", paymentTokens },
            { "paymentExchange", paymentExchange }
        };

        // Step 2: Generate the signature
        paymentData["sign"] = GenerateSignature(paymentData);
        paymentData["redirectURL"] = redirectURL;

        // Convert the dictionary to JSON using the custom method
        string jsonData = ConvertToJson(paymentData);

        Debug.Log("Request Payload: " + jsonData);

        // Step 3: Make the POST request using UnityWebRequest
        using (UnityWebRequest request = new UnityWebRequest(aeonApiUrl, UnityWebRequest.kHttpVerbPOST))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Send the request and wait for the response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                // Step 4: Handle success response
                Debug.Log("Payment Request Sent Successfully");
                PaymentResponse response = JsonUtility.FromJson<PaymentResponse>(request.downloadHandler.text);

                if (response != null && response.code == "0")
                {
                    Debug.Log("Payment URL: " + response.model.webUrl);
                    OpenPaymentPageInApp(response.model.webUrl);
                }
                else
                {
                    Debug.LogError($"Payment Error: {response?.msg ?? "Unknown Error"}");
                    paymentStatusText.SetText($"Error: {response?.msg ?? "Unknown Error"}");
                }
            }
            else
            {
                // Handle failure response
                Debug.LogError("Request Failed: " + request.error);
                Debug.LogError("Response Code: " + request.responseCode);
                Debug.LogError("Response Text: " + request.downloadHandler.text);
                paymentStatusText.SetText("Payment Request Failed!");
            }
        }
    }



    // This method will be called from JavaScript when payment status is received
    public void OnPaymentSuccess(string message)
    {
        Debug.Log(message);
        // You can now update the UI or trigger actions after a successful payment
        paymentStatusText.SetText("Payment Successful!");
        UIManager.Instance.BuyCoins();
    }

    public void OnPaymentFailure(string message)
    {
        Debug.Log(message);
        // You can update the UI or trigger actions after a failed payment
        paymentStatusText.SetText("Payment Request Failed!");
    }


    public void OpenPaymentPageInApp(string paymentUrl)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    Application.ExternalCall("openPaymentPageInApp", paymentUrl);
#endif
    }


    // Trigger payment request via UI button
    public void MakePaymentButton()
    {
        StartCoroutine(MakePayment("1"));
    }

    // void OpenWebView(string url)
    // {
    //     webViewObject.LoadURL(url);
    //     webViewObject.SetVisibility(true);
    // }

    // void CheckPaymentStatus(string currentUrl)
    // {
    //     if (currentUrl.Contains("resolution"))
    //     {
    //         Debug.Log("Payment Successful!");
    //         paymentStatusText.SetText("Payment Successful!");
    //         webViewObject.SetVisibility(false);
    //         UIManager.Instance.BuyCoins();
    //     }
    // }

    [Serializable]
    public class PaymentResponse
    {
        public string code;
        public PaymentModel model;
        public string msg;
        public string traceId;
    }

    [Serializable]
    public class PaymentModel
    {
        public string orderNo;
        public string webUrl;
    }
}
