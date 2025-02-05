using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement Components
    private CharacterController controller;
    private Animator animator;
    public Joystick joystick;
    // public Buttton actionButton;

    [Header("Movement System")]
    private float moveSpeed = 4f;

    [Header("Collision Dectector")]
    public LayerMask obstacleLayer;  // Layer for obstacles
    public float groundCheckDistance = 0.1f;  // Distance to check for the ground
    public float descentSpeed = 5f;  // Speed of descent
    private bool isClimbing = false;  // Is the character climbing
    private bool isAwayFromObstacle = false;  // Is the character away from the obstacle






    //Interaction components
    PlayerInteraction playerInteraction;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        //Get interaction component
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (joystick != null)
        {
            CameraController.Instance.RecordPlayerMovement(joystick.Horizontal);
        }
        else
        {
            Debug.LogError("cameraController or joystick is null!");
        }

        //Runs the function that handles all interaction
        Interact();
        //Debugging purposes only
        //Skip the time when the right square bracket is pressed
        if (Input.GetKey(KeyCode.RightBracket))
        {
            TimeManager.Instance.Tick();
        }

        //collison decector and behaviour depending on obsactcle
        if (isClimbing && isAwayFromObstacle)
        {
            // Check if the character is no longer near the obstacle
            float distanceToGround = CheckGroundDistance();

            if (distanceToGround > groundCheckDistance)
            {
                Descend();
            }
            else
            {
                // Character has returned to the ground
                isClimbing = false;
                isAwayFromObstacle = false;
            }
        }


    }

    public void Harvest()
    {
        playerInteraction.ItemInteract();

    }
    public void Interact()
    {
        //Tool interaction
        if (Input.GetButtonDown("Fire1"))
        {
            //Interact
            playerInteraction.Interact();
        }

        //item interaction
        if (Input.GetButtonDown("Fire2"))
        {
            //Interact
            playerInteraction.ItemInteract();
        }

    }

    public void Move()
    {
        // get the horizontal and vertical inputs as a number 
        //FOR KEYBOARD CONTROLLER ETC
        // float horizontal = Input.GetAxisRaw("Horizontal");
        // float vertical = Input.GetAxisRaw("Vertical");

        // get the horizontal and vertical inputs as a number
        float vertical = joystick.Horizontal;
        float horizontal = joystick.Vertical;

        Vector3 dir = new Vector3(horizontal, 0f, vertical * -1f).normalized;

        Vector3 velocity = moveSpeed * Time.deltaTime * dir;

        //check if there is movement
        if (dir.magnitude >= 0.1f)
        {
            //look towards that direction
            transform.rotation = Quaternion.LookRotation(dir);

            //move
            controller.Move(velocity);
            //to make the player run
            // if (dir.magnitude >= 0.5f)
            // {
            //     animator.SetBool("Running", true);
            // }
            // else
            // {
            //     animator.SetBool("Running", false);
            // }

        }
        animator.SetFloat("Speed", velocity.magnitude);
    }



    private void OnCollisionEnter(Collision collision)
    {
        // Check if the character has collided with an obstacle
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            isClimbing = true;
            isAwayFromObstacle = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // When the character leaves the obstacle, start checking if it's away
        if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
        {
            isAwayFromObstacle = true;
        }
    }

    private float CheckGroundDistance()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            return hit.distance;
        }
        return Mathf.Infinity;
    }

    private void Descend()
    {
        // Smoothly move the character down
        transform.position -= new Vector3(0, descentSpeed * Time.deltaTime, 0);
    }



}
