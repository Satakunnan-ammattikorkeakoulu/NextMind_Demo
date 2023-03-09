using System.Collections;
using NextMind.Examples;
using UnityEngine;
using TMPro;

public class Car : MonoBehaviour
{
    private const float RotateDuration = 1f;
    private const float MoveDuration = 3f;
    private const float WheelRotateSpeed = 300f;

    private readonly Vector3[] lanes =
    {
        new(-1.05f, 0f, 0.15f), new(-0.33f, 0f, 0.15f), new(0.33f, 0f, 0.15f),
        new(1.1f, 0f, 0.15f)
    };

    private Quaternion originalRotation;

    private int currentLane = 2;

    private bool isSwitchingLane;

    private int score;

    private static bool _isEndGame;

    public TMP_Text scoreText;
    public GameObject endGameScreen;
    public GameObject uiButtons;
    public GameObject frontLeftWheel;
    public GameObject frontRightWheel;
    public GameObject rearLeftWheel;
    public GameObject rearRightWheel;
    public AudioSource coinCollected;

    // Start is called before the first frame update
    private void Start()
    {
        originalRotation = transform.rotation;
        transform.position = lanes[2];
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z)) SwitchLane(-1); // -1 == left
        if (Input.GetKeyDown(KeyCode.X)) SwitchLane(1); // 1 == right
        RotateWheels();
    }

    public void SwitchLane(int direction)
    {
        if ((currentLane == 0 && direction == -1) || (currentLane == 3 && direction == 1) || isSwitchingLane ||
            _isEndGame) return;
        StartCoroutine(Rotate(direction));
        StartCoroutine(ChangeLane(direction));
        StartCoroutine(RotateBack());
    }

    private void RotateWheels()
    {
        frontLeftWheel.transform.Rotate(Vector3.right * (WheelRotateSpeed * Time.deltaTime));
        frontRightWheel.transform.Rotate(Vector3.right * (WheelRotateSpeed * Time.deltaTime));
        rearLeftWheel.transform.Rotate(Vector3.right * (WheelRotateSpeed * Time.deltaTime));
        rearRightWheel.transform.Rotate(Vector3.right * (WheelRotateSpeed * Time.deltaTime));
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
        yield return new WaitForSeconds(0.1f);
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
        yield return new WaitForSeconds(0.1f);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coinCollected.Play();
            Destroy(other.gameObject);
            score++;
        }
        else if (other.gameObject.CompareTag("Finish"))
        {
            _isEndGame = true;
            uiButtons.SetActive(false);
            scoreText.text = $"Your score\n {score} / {Coins.CoinCount}";
            endGameScreen.SetActive(true);
        }
    }

    public static bool IsEndGame()
    {
        return _isEndGame;
    }
}