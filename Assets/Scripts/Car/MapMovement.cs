using UnityEngine;

public class MapMovement : MonoBehaviour
{
    private const float MovementSpeed = 3f;

    private void FixedUpdate()
    {
        MoveMap();
    }

    private void MoveMap()
    {
        if (!Car.IsEndGame()) transform.position += Vector3.back * (MovementSpeed * Time.deltaTime);
    }
}