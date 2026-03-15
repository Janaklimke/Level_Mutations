using UnityEngine;

public class FPSPlayerTest : MonoBehaviour
{
    public class FPSPlayer : MonoBehaviour
{
    public Transform playerCamera;
    float speed = 7;
    public float mouseSensitivity = 100f;
    float xRotation = 0f;
    float velocityY;
    public float gravity = -9.81f;
    public float jumpForce = 5f;

    bool isGrounded;
    float coyoteTime = 0.15f;
    float coyoteTimeCounter;

    public float recoilOffset = 0f;
    public float recoilReturnSpeed = 10f;

    public void AddRecoil(float amount)
    {
        recoilOffset -= amount;
    }
void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // BESSERE Ground Check - kombiniert beide Methoden
        CharacterController controller = GetComponent<CharacterController>();

        // Methode 1: CharacterController.isGrounded
        bool controllerGrounded = controller.isGrounded;

        // Methode 2: Manueller Raycast nach unten
        bool raycastGrounded = Physics.Raycast(transform.position, Vector3.down, 
                                               controller.height / 2 + 0.1f);

        // Wenn EINE der beiden Methoden sagt "am Boden" → isGrounded = true
        isGrounded = controllerGrounded || raycastGrounded;

        Debug.Log("Controller: " + controllerGrounded + " | Raycast: " + raycastGrounded + " | Final: " + isGrounded);

        // Coyote Time
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;
// Move
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation + recoilOffset, 0f, 0f);
        recoilOffset = Mathf.Lerp(recoilOffset, 0f, recoilReturnSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up * mouseX);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // Jump mit Coyote Time
        if (coyoteTimeCounter > 0f && Input.GetButtonDown("Jump"))
        {
            velocityY = jumpForce;
            coyoteTimeCounter = 0f;
        }

        if (isGrounded && velocityY < 0)
            velocityY = -2f;

        velocityY += gravity * Time.deltaTime;
        controller.Move(Vector3.up * velocityY * Time.deltaTime);
    }
}
}
