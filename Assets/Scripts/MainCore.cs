using UnityEngine;
using Kun.Tool;

public class MainCore : MonoBehaviour
{
    ServiceCollection collection = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start ()
    {
        collection = new ServiceCollection ();

        collection.SetupServices ();
        collection.InitServices ();

        collection.AddService<InputController> ();

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
