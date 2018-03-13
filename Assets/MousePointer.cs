using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour {
    Light m_light;
	// Use this for initialization
	void Start () {
        m_light = GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        }
        if(m_light  != null && Input.GetMouseButton(0))
        {
            m_light.enabled = true;
        }
        else
        {
            m_light.enabled = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetMouseButton(0))
            Destroy(other.gameObject);
    }

}
