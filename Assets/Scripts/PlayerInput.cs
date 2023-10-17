using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    [SerializeField] private float mouseSensitivity;
    private float horizontalInput;
    private float verticalInput;
    private bool isJumping;
    private bool isDashing;

    public float MouseX { get => mouseX; set => mouseX = value; }
    public float MouseY { get => mouseY; set => mouseY = value; }
    public float HorizontalInput { get => horizontalInput; set => horizontalInput = value; }
    public float VerticalInput { get => verticalInput; set => verticalInput = value; }
    public bool IsJumping { get => isJumping; set => isJumping = value; }
    public bool IsDashing { get => isDashing; set => isDashing = value; }

    private void Start()
    {
        MouseX = 0;
        MouseY = 0;
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isJumping = Input.GetKeyDown("space");
        isDashing = Input.GetKeyDown("left shift");
    }
}
