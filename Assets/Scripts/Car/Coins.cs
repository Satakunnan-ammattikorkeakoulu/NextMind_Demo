using UnityEngine;

public class Coins : MonoBehaviour
{
    public GameObject coin;
    public GameObject parent;

    public const int CoinCount = 10;

    private const float RotationSpeed = 20;

    private readonly Vector3[] lanes =
    {
        new(-0.02f, -0.003f, 0f), new(0.02f, -0.003f, 0f), new(0.06f, -0.003f, 0f),
        new(0.09f, -0.003f, 0f)
    };

    // Start is called before the first frame update
    private void Start()
    {
        CreateCoins();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        RotateCoins();
    }

    private void RotateCoins()
    {
        foreach (Transform child in parent.transform)
        {
            // This makes no sense, but it looks nice
            child.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
            child.Rotate(Vector3.right, RotationSpeed * Time.deltaTime);
        }
    }

    private void CreateCoins()
    {
        for (var i = 0; i < CoinCount; i++)
        {
            var lane = Random.Range(0, 4);
            var position = lanes[lane];
            position.z = i + 1f;
            var newCoin = Instantiate(coin, parent.transform);
            newCoin.transform.localPosition = position;
            var rotation = Quaternion.Euler(0, i * 36, 90);
            newCoin.transform.localRotation = rotation;
        }
    }
}