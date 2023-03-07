using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    private const float MovementSpeed = 5f;

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.back * (MovementSpeed * Time.deltaTime);
    }
}