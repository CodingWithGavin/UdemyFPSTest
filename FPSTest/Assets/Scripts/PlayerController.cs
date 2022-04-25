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

    public float moveSpeed = 5f;
    private Vector3 moveDir, movement;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

        movement = ((transform.forward * moveDir.z) + (transform.right * moveDir.x)).normalized;

        transform.position += movement * moveSpeed * Time.deltaTime;

        
    }
}
