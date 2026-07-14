using Kun.Tool;
using UnityEngine;

public class CameraContainerManager : MonoBehaviour
{
    [SerializeField]
    Transform cameraContainer;

    public void MoveTo (Transform target)
    {
        if (cameraContainer == null)
        {
            Debug.LogError ($"{name} 缺少 cameraContainer", this);
        }
        else if (target != null)
        {
            cameraContainer.position = target.position;
        }
        else
        {
            Debug.LogError ($"{name} 收到空的攝影機目標", this);
        }
    }
}
