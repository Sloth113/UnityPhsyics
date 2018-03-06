using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
    CharacterController controller = null;
    Animator animator = null;

    public float speed = 80.0f;
    public float pushPower = 2.0f;
    public Vector3 velocity = new Vector3(0, 0, 0);
    public bool isGrounded = false;
    public bool crouched = false;
    float height;
    Vector3 controlCenter;
    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        height = controller.height;
        controlCenter = controller.center;

    }
	
	// Update is called once per frame
	void Update () {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        //Make 5 check
        isGrounded = Physics.Raycast(transform.position + transform.up * 0.2f, -Vector3.up, 0.3f);
        isGrounded = isGrounded || Physics.Raycast(transform.position + transform.forward * controller.radius + transform.up * 0.2f, -Vector3.up, 0.3f);
        isGrounded = isGrounded || Physics.Raycast(transform.position + -transform.forward * controller.radius + transform.up * 0.2f, -Vector3.up, 0.3f);
        isGrounded = isGrounded || Physics.Raycast(transform.position + -transform.right * controller.radius + transform.up * 0.2f, -Vector3.up, 0.3f);
        isGrounded = isGrounded || Physics.Raycast(transform.position + -transform.right * controller.radius + transform.up * 0.2f, -Vector3.up, 0.3f);



        velocity += Physics.gravity * Time.deltaTime;

        if(Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            crouched = true;
        }
        else
        {
            crouched = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = 5;
        }
        if (!isGrounded)
        {
            velocity.x = (transform.forward  * vertical * speed* Time.deltaTime).x;
            velocity.z = (transform.forward * vertical * speed * Time.deltaTime).z;
        }
        else
        {
         //   velocity = Vector3.zero;
        }

        if (isGrounded && velocity.y <= 0)
        {
            velocity.y = 0;
            velocity = Vector3.zero;
        }
        if (!isGrounded)
        {
            //controller.height = height * 0.5f;
            //controller.center = controlCenter + Vector3.up * 0.5f;
        }
        else if (crouched)
        {
            controller.height = height * 0.6f;
            controller.center = controlCenter - Vector3.up * 0.3f;
        }
        else
        {
            controller.height = height;
            controller.center = controlCenter;
        }
        controller.Move(velocity * Time.deltaTime);
        transform.Rotate(transform.up, horizontal * speed * Time.deltaTime);
        //animator.SetFloat("Speed", vertical * speed * Time.deltaTime);
        animator.SetFloat("Forward", vertical);
        animator.SetFloat("Turn", horizontal);
        animator.SetBool("OnGround", isGrounded);
        animator.SetBool("Crouch", crouched);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;
        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.velocity = pushDir * pushPower;

    }
}
