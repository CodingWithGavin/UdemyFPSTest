using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform viewPoint;
    public float mouseSensitivity = 1f;
    private float verticalRotScore;
    private Vector2 mouseInput;

    public bool invertlook;

    public float moveSpeed = 5f, runSpeed  = 8f;
    private float activeMoveSpeed;
    private Vector3 moveDir, movement;

    public CharacterController charController;
    
    private Camera cam;

    public float jumpForce = 5f, gravityMod = 2.5f;

    public Transform groundCheckPoint;
    private bool isGrounded;
    public LayerMask groundLayers;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mouseInput = new Vector2( Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x , transform.rotation.eulerAngles.y + mouseInput.x ,transform.rotation.eulerAngles.z );

        verticalRotScore += mouseInput.y;

        verticalRotScore = Mathf.Clamp( verticalRotScore, -60f, 60f );

        if(invertlook)
        {
            viewPoint.rotation = Quaternion.Euler(verticalRotScore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
        }
        else
        {
            viewPoint.rotation = Quaternion.Euler(-verticalRotScore, viewPoint.rotation.eulerAngles.y, viewPoint.rotation.eulerAngles.z);
        }

        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if(Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runSpeed;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }

        float yVel = movement.y;

        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized * activeMoveSpeed;

        movement.y = yVel;

        if (charController.isGrounded)
        {
            movement.y = 0f;
        }

        isGrounded = Physics.Raycast(groundCheckPoint.position, Vector3.down, .25f, groundLayers);

        if(Input.GetButtonDown("Jump")  && isGrounded)
        {
            movement.y = jumpForce;
        }
        
        movement.y += Physics.gravity.y * Time.deltaTime * gravityMod;
        
        charController.Move(movement * Time.deltaTime);


        if(Input.GetMouseButtonDown(0))
        {
            Shoot();
        }



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Cursor.lockState == CursorLockMode.None)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }

    private void Shoot()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        ray.origin = cam.transform.position;

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("We hit " + hit.collider.gameObject.name);
        }
        else
        {
            Debug.Log("Miss");
        }
    }
    private void LateUpdate()
    {
        cam.transform.position = viewPoint.position;
        cam.transform.rotation = viewPoint.rotation;
    }
}
