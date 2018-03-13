using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    [SerializeField]
    private GameObject m_target;
    private Vector3 m_offSet;
	// Use this for initialization
	void Start () {
        if (m_target != null)
            m_offSet = m_target.transform.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_target !=null )
            transform.position = m_target.transform.position - m_offSet;
	}
}
