using UnityEngine;

public class PortalCollector : MonoBehaviour
{
    [HideInInspector]
    public PortalDirection Direction;

    [SerializeField]
    public string blockKey;

    [SerializeField]
    public string entryKey;
}

