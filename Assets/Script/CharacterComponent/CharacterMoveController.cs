using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{

    GameObject m_TargetObject = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        m_TargetObject.transform.position += m_TargetObject.transform.forward * vert * Time.deltaTime;
        m_TargetObject.transform.position += m_TargetObject.transform.right * hori * Time.deltaTime;


    }
}
