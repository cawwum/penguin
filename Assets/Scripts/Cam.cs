using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    private GameObject katy;

    // Use this for initialization
    void Start()
    {
        katy = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(katy.transform.position.x, katy.transform.position.y, transform.position.z);
    }
}
