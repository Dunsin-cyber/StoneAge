using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float offsetZ = 5f;
    public float smoothing = 2f;
    // Transform playerPos;
    [SerializeField] private Transform playerPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [System.Obsolete]
    private void Start()
    {
        // playerPos = FindObjectOfType<PlayerController>().transform;
        if (playerPos == null)
        {
            Debug.LogWarning("Player position not assigned!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    void FollowPlayer()
    {
        //Position the camera should be in
        Vector3 targetPosition = new(playerPos.position.x, transform.position.y, playerPos.position.z - offsetZ);

        //Set the position accordingly
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);
    }
}
