using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMove : MonoBehaviour
{
    public GameObject targetPosition;
    public GameObject firstPosition;
    public float speed;
    public float timer;
    public float moveTime;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= moveTime)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition.transform.position, speed);

            if (gameObject.transform.position == targetPosition.transform.position)
            {
                transform.position = firstPosition.transform.position;
                timer = 0;
            }
        }
    }
}
