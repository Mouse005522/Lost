using UnityEngine;
using Kun.Tool;

public class MainCore : MonoBehaviour
{
    ServiceCollection collection = null;

    [SerializeField]
    PlayerController player;

    [SerializeField]
    OverlayCanvasController overlayCanvasController;

    [SerializeField]
    ItemManager itemManager;

    [SerializeField]
    BlockManager blockManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        collection = new ServiceCollection ();

        collection.AddService<InputController> ();

        collection.AddService (player);
        collection.AddService (overlayCanvasController);
        collection.AddService (itemManager);
        collection.AddService (blockManager);

        collection.BindingService ();
        collection.SetupServices ();
        collection.InitServices ();

        Application.onBeforeRender += OnBeforeRender;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        collection.OnUpdate (deltaTime);
    }

    void FixedUpdate ()
    {
        float fixedDeltaTime = Time.fixedDeltaTime;
        collection.OnFixedUpdate (fixedDeltaTime);
    }

    void OnBeforeRender () 
    {
        collection.OnBeforeRender ();
    }

    void OnDrawGizmos ()
    {
        collection?.OnDrawGizmos ();
    }

    void OnGUI ()
    {
        collection?.DrawGUI ();
    }

    void OnApplicationQuit ()
    {
        collection.OnGameQuit ();

        Application.onBeforeRender -= OnBeforeRender;
    }
}
