using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TonConnect : MonoBehaviour
{
    private const string walletUniversalUrl = "https://app.tonkeeper.com/ton-connect";
    private const string walletUniversalUrl2 = "https://app.tonsafe.net/ton-connect";
    private const string qrCodeApiUrl = "https://api.qrserver.com/v1/create-qr-code";

    private const string manifestUrl = "https://metaverse-course.ams3.cdn.digitaloceanspaces.com/tonconnect-manifest.json";

    public RawImage image1;
    public RawImage image2;

    public static readonly UTF8Encoding Encoder = new UTF8Encoding();
    public static readonly byte[] Buffer = new byte[2048];

    public TransactionSender sender;
    public string SenderId;
    public Button sendTransactionButton;

    public GameObject QRCodes;
    public GameObject connectionSuccess;

    private void Start()
    {
        connectionSuccess.SetActive(false);
        sendTransactionButton.onClick.AddListener(OnSendTransactionButtonClick);

        // Create connect request
        ConnectRequest connectRequest = new ConnectRequest
        {
            manifestUrl = manifestUrl,
            items = new ConnectItem[]
            {
                new ConnectItem
                {
                    type = "string",
                    name = "message",
                    value = "Hello, Tonkeeper!"
                }
            }
        };

        // Convert connect request to JSON using Unity's JsonUtility
        string connectRequestJson = JsonUtility.ToJson(connectRequest);
        Debug.Log(connectRequestJson);

        SenderId = GenerateRandomId();

        // Tonkeeper URL
        string url = $"{walletUniversalUrl}?v=2&id={SenderId}&r={UnityWebRequest.EscapeURL(connectRequestJson)}&ret=none";
        string url2 = $"{walletUniversalUrl2}?v=2&id={SenderId}&r={UnityWebRequest.EscapeURL(connectRequestJson)}&ret=none";

        Debug.Log(url);
        Debug.Log(url2);

        // Display QR codes
        StartCoroutine(LoadQrCode(url, image1));
        StartCoroutine(LoadQrCode(url2, image2));

        // Start listening to server-sent events
        StartCoroutine(OpenSSEStream($"https://bridge.tonapi.io/bridge/events?client_id={SenderId}"));
    }

    void OnSendTransactionButtonClick()
    {
        Debug.Log("Send transaction button clicked!");
        sender.SendTransaction(SenderId);
    }

    private string GenerateRandomId()
    {
        byte[] buffer = new byte[16]; // 128-bit ID
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(buffer);
        }
        return BitConverter.ToString(buffer).Replace("-", "");
    }

    private IEnumerator LoadQrCode(string url, RawImage image)
    {
        string qrUrl = $"{qrCodeApiUrl}/?size=512&data={UnityWebRequest.EscapeURL(url)}";
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(qrUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            image.texture = texture;
        }
        else
        {
            Debug.LogError($"Failed to load QR code: {request.error}");
        }
    }

    private IEnumerator OpenSSEStream(string url)
    {
        UnityWebRequest request = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Accept", "text/event-stream");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(request.downloadHandler.text);
            string responseText = Encoding.UTF8.GetString(buffer);
            Debug.Log(responseText);

            if (responseText.Contains("data:"))
            {
                Debug.Log("Connection successful!");
                QRCodes.SetActive(false);
                connectionSuccess.SetActive(true);
            }
        }
        else
        {
            Debug.LogError($"Failed to connect: {request.error}");
        }
    }
}

// Unity-compatible JSON classes
[Serializable]
public class ConnectRequest
{
    public string manifestUrl;
    public ConnectItem[] items;
}

[Serializable]
public class ConnectItem
{
    public string type;
    public string name;
    public string value;
}
