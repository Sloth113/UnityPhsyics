using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {
    public float m_speed = 1000;
    public float m_lifeTime = 2.0f;
    private float m_timer = 0;

    private Rigidbody m_rigidbody;
	// Use this for initialization
	void Start () {
        //Rigid body move forward
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.AddForce(transform.forward * m_speed);
	}
	
	// Update is called once per frame
	void Update () {
        m_timer += Time.deltaTime;
        //Static move forward
        //transform.position = transform.position + transform.forward * m_speed;
        //Destroy itself 
        if (m_timer > m_lifeTime) Destroy(gameObject);
	}

    private void OnCollisionEnter(Collision collision)
    {
        //Add force of the collider it hits
        Rigidbody body = collision.gameObject.GetComponent<Rigidbody>();
        if (body != null)
            body.AddForce(transform.forward * m_speed);
    }
    
}
