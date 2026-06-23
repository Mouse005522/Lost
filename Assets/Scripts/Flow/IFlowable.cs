namespace Kun.Tool
{
    public interface IFlowable
    {
        /// <summary>
        /// 兩階段更新的Setup先再來是Init
        /// </summary>
        void OnSetup ();

        /// <summary>
        /// 兩階段更新的Setup先再來是Init
        /// </summary>
        void OnInit ();

        void OnPreUpdate (float deltaTime);

        void OnUpdate (float deltaTime);
        void OnFixedUpdate (float deltaTime);
        void OnPhysicsRateUpdate (float physicsRate);

        void OnLateUpdate (float deltaTime);

        void OnBeforeRender ();

        void OnDrawGizmos ();

        void OnGameQuit ();
    }
}
