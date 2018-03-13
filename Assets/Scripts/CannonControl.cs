using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonControl : MonoBehaviour {
    [SerializeField]
    private GameObject m_base;
    [SerializeField]
    private GameObject m_barrel;
    [SerializeField]
    private GameObject m_cannon;
    [SerializeField]
    private Transform m_exit;
    [SerializeField]
    private float m_shotTime = 1;//1sec
    private float m_timer = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    
        if (Input.GetKey(KeyCode.Q))
        {
            m_base.transform.Rotate(Vector3.up, -1);
        }
        if (Input.GetKey(KeyCode.E))
        {
            m_base.transform.Rotate(Vector3.up, 1);
        }
        if (Input.GetKey(KeyCode.R))
        {
            m_barrel.transform.Rotate(Vector3.right, -1);
        }
        if (Input.GetKey(KeyCode.F))
        {
            m_barrel.transform.Rotate(Vector3.right, 1);
        }
        if (Input.GetKey(KeyCode.Alpha1) && m_timer >= m_shotTime)
        {
            //Shoot big cannon
            GameObject b = Instantiate<GameObject>(m_cannon, m_exit.position, m_exit.rotation);
            b.GetComponent<BulletScript>().m_speed = 5000;
            m_timer = 0;
        }
        if (m_timer < m_shotTime)
            m_timer += Time.deltaTime;
    }
}
