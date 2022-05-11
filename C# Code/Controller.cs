// 5/6/2022: Work on Jump mechanic, continue tweaking camera as well maybe?
// 5/10/2022: Jump mechanic & player movement/camera movement complete, needs animations.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public CharacterController controller;

    public Transform cam;
    public Transform groundCheck;
    public LayerMask groundMask;
    private Vector3 Velocity;
    public float speed = 20.0f;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private float gravity = -9.81f;
    private float groundDistance = 0.2f;

    private Vector3 velocity;
    bool isGrounded;

    public float jump = 3f;
       
    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        
        float horiMov = Input.GetAxis("Horizontal");
        float vertMov = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horiMov, 0f, vertMov).normalized;

        if(direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);

        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jump * -2.0f * gravity);
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
