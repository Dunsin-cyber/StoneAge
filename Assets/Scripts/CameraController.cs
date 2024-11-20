using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [SerializeField]
    private Transform playerPos;

    public Joystick joystick;
    public float offsetZ = 5f;
    public float offsetY = 2.0f; // Height above the player

    private float cameraYaw; // Rotation around the Y-axis
    public float smoothing = 2f;
    public float rotationSpeed = 150f;

    private Vector3 targetPosition;
    private float rotationAngle = 0f;

    private Camera mainCamera;



    private float horizontalMov;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keeps it between scenes
        }
        else
        {
            Destroy(gameObject); // Ensures only one instance exists
        }
    }
    void Start()
    {
        if (playerPos == null)
        {
            Debug.LogWarning("Player position not assigned!");
        }
        mainCamera = Camera.main;
    }

    void Update()
    {
        // FollowJoystick();
        FollowPlayer();
        // Rotation();
    }

    void FollowPlayer()
    {
        // Calculate the desired camera position
        Vector3 targetPosition = playerPos.position - transform.forward * offsetZ;
        targetPosition.y = transform.position.y;  // Keep the camera at the same height

        // Move the camera smoothly to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

        // Ensure the camera always looks at the player from behind
        // Vector3 directionToPlayer = playerPos.position - transform.position;
        // Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Rotate the camera smoothly
        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothing * Time.deltaTime);
    }

    void FollowJoystick()
    {
        // Rotate camera based on joystick input
        float horizontalInput = joystick.Horizontal; // Replace this with joystick's horizontal input
        float verticalInput = joystick.Vertical; // Replace this with joystick's vertical input


        cameraYaw += horizontalInput * rotationSpeed;
        // Calculate camera's rotation around the player
        Quaternion rotation = Quaternion.Euler(0f, cameraYaw, 0f);
        Vector3 offset = new Vector3(0, offsetY, -offsetZ);

        // Update camera position based on the player's position
        transform.position = playerPos.position + rotation * offset;

        // Make the camera look at the player
        transform.LookAt(playerPos);
    }
    void Rotation()
    {


        // Check if the player turns right or left, for example based on player position or input
        // You can replace playerPos.position.x with actual input or player direction.
        float rotationDelta = horizontalMov * rotationSpeed * Time.deltaTime;
        rotationAngle += rotationDelta;

        // Update the camera rotation
        mainCamera.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);


    }

    public void RecordPlayerMovement(float record)
    {
        horizontalMov = record;
        return;
    }

    void HandleTouchInput()
    {
        // For mobile touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                // Calculate the rotation based on touch delta
                float rotationDelta = touch.deltaPosition.x * rotationSpeed * Time.deltaTime;
                rotationAngle += rotationDelta;
            }
        }

        // For mouse input (useful for testing in the Editor)
        if (Input.GetMouseButton(0))
        {
            float rotationDelta = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            rotationAngle += rotationDelta;
        }
    }
}
