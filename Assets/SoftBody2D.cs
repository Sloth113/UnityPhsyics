using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBody2D : MonoBehaviour {
    enum Type
    {
        Spring
    }
    [SerializeField]
    private Type m_type;
    [SerializeField]
    private GameObject m_physicsBall;
    [SerializeField]
    private int m_numCols = 10;
    [SerializeField]
    private int m_numRows = 10;
    [SerializeField]
    private float m_ballSize = 0.5f;
    [SerializeField]
    private Vector3 m_xAxis = new Vector3(1, 0, 0);
    [SerializeField]
    private Vector3 m_yAxis = new Vector3(0, 0, 1);
    [SerializeField]
    private float m_spring = 1000;
    [SerializeField]
    private float m_damping = 1;
    [SerializeField]
    private MeshFilter m_cloth;
    [SerializeField]
    private bool showBalls;

    GameObject[][] m_links;
    // Use this for initialization
    void Start()
    {
        if (m_physicsBall != null)
        {
            Vector3 pos = transform.position;

            m_links = new GameObject[m_numCols][];

            for (int i = 0; i <m_numCols; i++)
            {
                m_links[i] = new GameObject[m_numRows];
                for (int j =0; j < m_numRows; j++)
                {
                    Vector3 pos0 = pos + m_xAxis * i + m_yAxis * j;
                    m_links[i][j] = Instantiate<GameObject>(m_physicsBall, pos0, transform.rotation) ;
                    m_links[i][j].transform.parent = gameObject.transform;
                    m_links[i][j].name = "Link_" + i + "_" + j;
                    m_links[i][j].transform.localScale = new Vector3(m_ballSize, m_ballSize, m_ballSize);
                    m_links[i][j].GetComponent<Rigidbody>().isKinematic = false;
                    //Vert/Horizon springs
                    if (i != 0)
                    {
                        SpringJoint joint = m_links[i][j].AddComponent<SpringJoint>();
                        joint.connectedBody = m_links[i - 1][j].GetComponent<Rigidbody>();
                        joint.anchor = new Vector3(0, 0, 0);
                        joint.connectedAnchor = -m_xAxis;
                        joint.spring = m_spring;
                        joint.damper = m_damping;
                    }
                    if (j != 0)
                    {
                        SpringJoint joint = m_links[i][j].AddComponent<SpringJoint>();
                        joint.connectedBody = m_links[i][j-1].GetComponent<Rigidbody>();
                        joint.anchor = new Vector3(0, 0, 0);
                        joint.connectedAnchor = -m_xAxis;
                        joint.spring = m_spring;
                        joint.damper = m_damping;
                    }
                    //Bend springs
                    if (i > 1)
                    {
                        SpringJoint joint = m_links[i][j].AddComponent<SpringJoint>();
                        joint.connectedBody = m_links[i - 2][j].GetComponent<Rigidbody>();
                        joint.anchor = new Vector3(0, 0, 0);
                        joint.connectedAnchor = -m_xAxis;
                        joint.spring = m_spring;
                        joint.damper = m_damping;
                    }
                    if (j > 1)
                    {
                        SpringJoint joint = m_links[i][j].AddComponent<SpringJoint>();
                        joint.connectedBody = m_links[i][j - 2].GetComponent<Rigidbody>();
                        joint.anchor = new Vector3(0, 0, 0);
                        joint.connectedAnchor = -m_xAxis;
                        joint.spring = m_spring;
                        joint.damper = m_damping;
                    }
                    //Diag
                    if(i > 0 && j > 0)
                    {
                        SpringJoint joint = m_links[i][j].AddComponent<SpringJoint>();
                        joint.connectedBody = m_links[i - 1][j - 1].GetComponent<Rigidbody>();
                        joint.anchor = new Vector3(0, 0, 0);
                        joint.connectedAnchor = -m_xAxis;
                        joint.spring = m_spring;
                        joint.damper = m_damping;
                    }
                    //Diag
                    if (i > 0 && j < m_numRows -1)
                    {
                        SpringJoint joint = m_links[i][j].AddComponent<SpringJoint>();
                        joint.connectedBody = m_links[i - 1][j + 1].GetComponent<Rigidbody>();
                        joint.anchor = new Vector3(0, 0, 0);
                        joint.connectedAnchor = -m_xAxis;
                        joint.spring = m_spring;
                        joint.damper = m_damping;
                    }
                    m_links[i][j].GetComponent<MeshRenderer>().enabled = showBalls;
                }
            }
        }
        m_cloth = GetComponent<MeshFilter>();
        if (m_cloth)
        {
            SetUpCloth();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCloth();
    }
    void SetUpCloth()
    {
        m_cloth.mesh = new Mesh();
        UpdateCloth();
        //UVs
        Vector2[] uvs = new Vector2[m_numRows * m_numCols];
        for(int i = 0; i < m_numCols; i++)
        {
            for(int j = 0; j < m_numRows; j++)
            {
               uvs[j + i] = new Vector2(i / m_numCols, j / m_numRows);
            }
        }
        m_cloth.mesh.uv = uvs;
        //topo
        int[] triangles = new int[(m_numRows - 1) * (m_numCols - 1) * 12];
        int k = 0;
        for(int i =0; i < m_numCols - 1; i++)
        {
            for(int j = 0; j < m_numRows - 1; j++)
            {
                triangles[k] = i + j * m_numCols; k++;
                triangles[k] = (i + 1) + j * m_numCols; k++;
                triangles[k] = i + (j + 1) * m_numCols; k++;

                triangles[k] = i + (j + 1) * m_numCols; k++;
                triangles[k] = (i + 1) + j * m_numCols; k++;
                triangles[k] = (i + 1) + (j + 1) * m_numCols; k++;
                //Both sides triangled?
                triangles[k] = i + j * m_numCols; k++;
                triangles[k] = i + (j + 1) * m_numCols; k++;
                triangles[k] = (i + 1) + j * m_numCols; k++;

                triangles[k] = (i + 1) + j * m_numCols; k++;
                triangles[k] = i + (j + 1) * m_numCols; k++;
                triangles[k] = (i + 1) + (j + 1) * m_numCols; k++;
            }
        }
        m_cloth.mesh.triangles = triangles;
    }
    void UpdateCloth()
    {
        Vector3[] vertices = new Vector3[m_numRows * m_numCols];
        Vector3[] normals = new Vector3[m_numRows * m_numCols];

        for(int i = 0; i < m_numCols; i++)
        {
            for(int j = 0; j < m_numRows; j++)
            {
                vertices[i + j * m_numCols] = m_links[i][j].transform.position - transform.position;
                m_links[i][j].GetComponent<MeshRenderer>().enabled = showBalls;
            }
        }
        for(int i = 0; i < m_numCols; i++)
        {
            for(int j = 0; j < m_numRows; j++)
            {
                Vector3 left = i == 0 ? vertices[i + j * m_numCols] : vertices[i - 1 + j * m_numCols];
                Vector3 right = i == m_numCols - 1 ? vertices[i + j * m_numCols] : vertices[i + 1 + j * m_numCols];
                Vector3 down = j == 0 ? vertices[i + j * m_numCols] : vertices[i + (j - 1) * m_numCols];
                Vector3 up = j == m_numRows - 1 ?  vertices[i + j * m_numCols] : vertices[i + (j + 1) * m_numCols];
                normals[i + j * m_numCols] = Vector3.Cross(right - left, up - down);
                normals[i + j * m_numCols].Normalize();
            }
        }
        m_cloth.mesh.vertices = vertices;
        m_cloth.mesh.normals = normals;
        m_cloth.mesh.RecalculateBounds();
    }

}

