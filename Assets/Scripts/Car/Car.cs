using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Car : MonoBehaviour
{
    private const float RotateDuration = 1f;
    private const float MoveDuration = 5f;

    private readonly Vector3[] lanes =
    {
        new(-1.05f, 0f, 0.15f), new(-0.33f, 0f, 0.15f), new(0.33f, 0f, 0.15f),
        new(1.1f, 0f, 0.15f)
    };

    private Quaternion originalRotation;
    private int currentLane = 2;
    private bool isSwitchingLane = false;

    // Start is called before the first frame update
    private void Start()
    {
        originalRotation = transform.rotation;
        transform.position = lanes[2];
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) SwitchLane(-1); // -1 == left
        if (Input.GetKeyDown(KeyCode.U)) SwitchLane(1); // 1 == right
    }

    public void SwitchLane(int direction)
    {
        if ((currentLane == 0 && direction == -1) || (currentLane == 3 && direction == 1) || isSwitchingLane) return;
        StartCoroutine(Rotate(direction));
        StartCoroutine(ChangeLane(direction));
        StartCoroutine(RotateBack());
    }

    private IEnumerator Rotate(int direction)
    {
        isSwitchingLane = true;
        var targetRotation = originalRotation;
        var t = 0f;
        switch (direction)
        {
            case -1:
                targetRotation = Quaternion.Euler(0, -10, 0);
                break;
            case 1:
                targetRotation = Quaternion.Euler(0, 10, 0);
                break;
        }

        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.rotation =
                Quaternion.Slerp(transform.rotation, targetRotation, t / RotateDuration);
            yield return null;
        }
    }

    private IEnumerator ChangeLane(int direction)
    {
        yield return new WaitForSeconds(0.5f);
        var t = 0f;
        currentLane += direction;
        var targetPosition = lanes[currentLane];
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.position =
                Vector3.Slerp(transform.position, targetPosition, t / MoveDuration);
            yield return null;
        }
    }

    private IEnumerator RotateBack()
    {
        yield return new WaitForSeconds(1);
        var t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            transform.rotation =
                Quaternion.Slerp(transform.rotation, originalRotation, t / RotateDuration);
            yield return null;
        }

        isSwitchingLane = false;
    }
}