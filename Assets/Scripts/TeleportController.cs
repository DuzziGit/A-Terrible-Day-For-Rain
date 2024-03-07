using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [SerializeField]
    private Transform destination;

    public Transform GetDestination()
    {
        return destination;
    }
}
