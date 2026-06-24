using UnityEngine;

public abstract class UICollectorBase : MonoBehaviour
{
    [SerializeField]
    string key;

    public string Key => key;
}
