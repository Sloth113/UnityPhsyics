using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SoftBodyScript : MonoBehaviour {
    enum Type {
        Hinge
        //Spring,
    }
    [SerializeField]
    private Type m_type;
    [SerializeField]
    private GameObject m_physicsBall;
    [SerializeField]
    private int m_links;
    [SerializeField]
    private float m_ballSize = 0.5f;
    [SerializeField]
    private float m_gap = 1.0f;

	// Use this for initialization
	void Start () {
        if(m_physicsBall != null)
        {
            Vector3 pos = transform.position;

            GameObject anchor = Instantiate<GameObject>(m_physicsBall, pos, transform.rotation);
            anchor.transform.parent = gameObject.transform;
            anchor.name = "Anchor";
            anchor.GetComponent<Rigidbody>().isKinematic = true;
            anchor.transform.localScale = new Vector3(m_ballSize, m_ballSize, m_ballSize);

            GameObject[] links = new GameObject[m_links];
            for(int i =0; i < m_links; i++)
            {
                links[i] = Instantiate<GameObject>(m_physicsBall, pos + Vector3.down * (i + 1) * m_gap, transform.rotation);
                links[i].transform.parent = gameObject.transform;
                links[i].name = "Link";
                links[i].GetComponent<Rigidbody>().isKinematic = false;
                links[i].transform.localScale = new Vector3(m_ballSize, m_ballSize, m_ballSize);

                HingeJoint joint = links[i].AddComponent<HingeJoint>();
                joint.connectedBody = (i == 0 ? anchor : links[i - 1]).GetComponent<Rigidbody>();

                joint.anchor = new Vector3(0, 0, 0);
                //joint.connectedAnchor = new Vector3(0,-1.0f,0);

            }
        }    		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
