using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput input; //input

    private Transform camTransform; //player camera info
    private float rotationX;
    private float rotationY;

    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float deaccelorationSpeed;

    [Header("Jumping")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float jumpForce;
    private bool grounded;

    [Header("Dashing")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashLength;
    [SerializeField] private float maxStamina;
    [SerializeField] private float staminaRegen;
    private float currentDashTime;
    private float currentStamina;
    private bool dashing;

    public float CurrentStamina { get => currentStamina; set => currentStamina = value; }

    private void Awake()
    {
        camTransform = Camera.main.transform;
        grounded = false;
        dashing = false;
    }

    private void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rotationX = camTransform.eulerAngles.x;
        rotationY = camTransform.eulerAngles.y;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        PlayerCamera();
        IsGrounded();
        Jump();
        RegenStamina();
        isDashing();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        Dash();
    }

    private void PlayerCamera()
    {
        if(input.MouseX != 0)
        {
            rotationY += input.MouseX;
            rotationY %= 360;
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, rotationY, transform.eulerAngles.z);
        }

        if (input.MouseY != 0)
        {
            rotationX -= input.MouseY;
            rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        }

        if(input.MouseX != 0 || input.MouseY != 0)
        {
            camTransform.rotation = Quaternion.Euler(rotationX, rotationY, camTransform.eulerAngles.z);
        }
    }

    private void IsGrounded()
    {
        grounded = Physics.CheckSphere(groundCheck.position, .1f, whatIsGround);
    }

    private void Jump()
    {
        if(input.IsJumping && grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void RegenStamina()
    {
        float nextStamina = currentStamina + staminaRegen * Time.deltaTime;
        currentStamina = nextStamina > maxStamina ? maxStamina : nextStamina;
    }

    private void isDashing()
    {
        if (input.IsDashing && currentStamina >= 1)
        {
            if(dashing)
            {
                currentDashTime = 0;
            }

            else
            {
                dashing = true;
            }

            currentStamina -= 1;
        }
    }

    private void MovePlayer()
    {
        if((input.VerticalInput != 0 || input.HorizontalInput != 0) && !dashing)
        {
            Vector3 movement = input.VerticalInput * transform.forward + input.HorizontalInput * transform.right;

            if(movement.magnitude > 1f)
            {
                movement = movement.normalized;
            }

            movement = movement * movementSpeed;
            movement.y = rb.velocity.y;

            rb.velocity = movement;
        }

        else if(grounded)
        {
            Vector3 stop = rb.velocity;

            if(stop.x > 0)
            {
                if(stop.x - deaccelorationSpeed < 0)
                {
                    stop.x = 0;
                }

                else
                {
                    stop.x = stop.x - deaccelorationSpeed;
                }
            }
            else if(stop.x < 0)
            {
                if (stop.x + deaccelorationSpeed > 0)
                {
                    stop.x = 0;
                }

                else
                {
                    stop.x = stop.x + deaccelorationSpeed;
                }
            }

            if (stop.z > 0)
            {
                if (stop.z - deaccelorationSpeed < 0)
                {
                    stop.z = 0;
                }

                else
                {
                    stop.z = stop.z - deaccelorationSpeed;
                }
            }
            else if (stop.z < 0)
            {
                if (stop.z + deaccelorationSpeed > 0)
                {
                    stop.z = 0;
                }

                else
                {
                    stop.z = stop.z + deaccelorationSpeed;
                }
            }

            rb.velocity = stop;
        }
    }

    private void Dash()
    {
        if(dashing)
        {
            Vector3 dash = rb.velocity;
            dash.y = 0;

            if(dash.magnitude == 0)
            {
                dash = transform.forward;
            }

            dash = dash.normalized;
            dash *= dashSpeed;

            rb.velocity = dash;
            currentDashTime += Time.fixedDeltaTime;
        }

        if(currentDashTime >= dashLength)
        {
            currentDashTime = 0;
            dashing = false;
        }
    }
}
