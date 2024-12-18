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
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        Vector3 dir = new Vector3(horizontal, 0f, vertical).normalized;

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


}
