using NextMind.Examples;
using UnityEngine;

public class BackToHub : MonoBehaviour
{
    public void GoToHub()
    {
        HubManager.Instance.BackToHubScene();
    }
}