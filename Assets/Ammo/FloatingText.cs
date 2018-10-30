using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float lifetime = 0.5f;
    private TextMesh mesh;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, lifetime);
        mesh = GetComponent<TextMesh>();
        transform.position += Vector3.back;
        transform.position += Random.Range(-1f,1f) * Vector3.right;
        transform.position += Random.Range(-1f, 1f) * Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
