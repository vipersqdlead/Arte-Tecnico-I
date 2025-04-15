using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    Vector3 MoveDir;
    public float moveSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Move()
    {
        MoveDir.x =  Input.GetAxis("Horizontal");
        MoveDir.z = Input.GetAxis("Vertical");
        transform.position += Time.deltaTime * moveSpeed * MoveDir;

    }
    // Update is called once per frame
    void Update()
    {
        Move();

    }
}
