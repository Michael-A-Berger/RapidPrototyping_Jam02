using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Vector3 targetPos;
    public Quaternion targetDir;
    [SerializeField]
    float translateSpeed = 0.1f, rotateSpeed = 0.1f;
    // Start is called before the first frame update
    void Awake()
    {
        targetPos = transform.position;
        targetDir = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, translateSpeed / (translateSpeed + 1));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetDir, rotateSpeed / (rotateSpeed + 1));
    }
}
