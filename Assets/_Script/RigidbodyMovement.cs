
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class RigidbodyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float rotatespeed = 10f;

    [SerializeField] float jumpForce = 1500f;
    [SerializeField] float gravityscale = 2f;

    [SerializeField] Rigidbody rb;
    private Transform camTransform;

    private float horz, vert;
    Vector3 camForward, camRight;
    private Vector3 movement;
    
    bool jump;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camTransform = Camera.main.transform;
    }

    void Update()
    {
        CustomGravity();
        InputKeyBoard();

        Jump();
        Rotate();
        Movement();
    }

    void InputKeyBoard()
    {
        horz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
        jump = Input.GetButtonDown("Jump");

        camForward = camTransform.transform.forward;
        camRight = camTransform.transform.right;
        
        camForward.y = 0f;
        camRight.y = 0f;
        
        camForward.Normalize();
        camRight.Normalize();

        movement = (camForward * vert + camRight * horz ).normalized;
    }

    void Movement()
    {
         //rb.linearVelocity = new Vector3(horz, 0f, vert)* moveSpeed * Time.deltaTime;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, movement * moveSpeed, Time.deltaTime * 10);
    }

    Quaternion targetRotation = Quaternion.identity;
    void Rotate()
    {
        if (movement != Vector3.zero)
            targetRotation = Quaternion.LookRotation(movement);

        rb.rotation = Quaternion.Slerp(rb.rotation,targetRotation, rotatespeed * Time.deltaTime);
    }

    void Jump()
    {
        if (jump && isGrounded)
            rb.AddExplosionForce(jumpForce, transform.position, 5f);
    }



    void CustomGravity()
    {
        rb.useGravity = isGrounded;

        if(!isGrounded)
            rb.AddForce(Physics.gravity * 2f,ForceMode.Acceleration);
    }
}
