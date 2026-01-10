using UnityEngine;

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
    public float recoilOffset = 0f;
    public float recoilReturnSpeed = 10f;
    public void AddRecoil(float amount)
        {
            recoilOffset -= amount;
        }
    private CharacterController controller;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {        
        //move
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        playerCamera.localRotation = Quaternion.Euler(xRotation + recoilOffset, 0f, 0f);
        recoilOffset = Mathf.Lerp(recoilOffset, 0f, recoilReturnSpeed * Time.deltaTime);

        transform.Rotate(Vector3.up * mouseX);
    	
        isGrounded = controller.isGrounded;
        Debug.Log(isGrounded);

        if (isGrounded && velocityY < 0)
            velocityY = -2f;

        if (isGrounded && Input.GetButtonDown("Jump"))
            velocityY = jumpForce;

        velocityY += gravity * Time.deltaTime;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move *= speed;
        move.y = velocityY;

        controller.Move(move * Time.deltaTime);   
    }
}
