using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncing : MonoBehaviour
{
    // Attributes
    public Rigidbody rb;
    public SphereCollider sc;

    // Properties
    private Vector3 prev;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        prev = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (prev != transform.position)
        {
            Debug.Log("(X,Y,Z):\t(" + transform.position + ")");
            prev = transform.position;
        }
    }

    // On collision with "Ground" tagged objects
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision");
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("Added force");
            transform.Translate(0f, 0.01f, 0f);
            rb.AddForce(new Vector3(0f, 200f, 0f));
        }
    }
}
