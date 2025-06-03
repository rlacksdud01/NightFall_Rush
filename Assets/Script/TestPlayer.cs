using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public float moveSpeed;

    void Start()
    {
        //transform.localPosition = new Vector3(0, 0, 0);
    }
    void Update()
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        Vector2 moveDirection = new Vector2(horizontalInput, verticalInput).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
