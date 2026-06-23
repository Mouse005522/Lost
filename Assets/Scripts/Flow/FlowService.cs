namespace Kun.Tool
{
    public class FlowService : IFlowable
    {
        void IFlowable.OnSetup ()
        {
            Setup ();
        }

        /// <summary>
        /// 兩階段更新的Setup先再來是Init
        /// </summary>
        protected virtual void Setup ()
        {
            
        }

        void IFlowable.OnInit ()
        {
            Init ();
        }

        /// <summary>
        /// 兩階段更新的Setup先再來是Init
        /// </summary>
        protected virtual void Init ()
        {

        }

        void IFlowable.OnPreUpdate (float deltaTime)
        {
            DoPreUpdate (deltaTime);
        }

        protected virtual void DoPreUpdate (float deltaTime)
        {
        }

        void IFlowable.OnUpdate (float deltaTime)
        {
            DoUpdate (deltaTime);
        }

        protected virtual void DoUpdate (float deltaTime)
        {

        }

        void IFlowable.OnPhysicsRateUpdate (float physicsRate)
        {
            DoPhysicsRateUpdate (physicsRate);
        }

        void IFlowable.OnLateUpdate (float deltaTime)
        {
            DoLateUpdate (deltaTime);
        }

        protected virtual void DoLateUpdate (float deltaTime)
        {
        }

        /// <summary>
        /// 計算物理又怕每個FixedUpdate都計算一次太耗效能
        /// 由底層約定一個時間觸發一次
        /// </summary>
        protected virtual void DoPhysicsRateUpdate (float physicsRate)
        {

        }

        void IFlowable.OnFixedUpdate (float deltaTime)
        {
            DoFixedUpdate (deltaTime);
        }

        protected virtual void DoFixedUpdate (float deltaTime)
        {

        }

        void IFlowable.OnBeforeRender ()
        {
            DoBeforeRender ();
        }

        protected virtual void DoBeforeRender ()
        {

        }

        void IFlowable.OnGameQuit ()
        {
            DoGameQuit ();
        }

        protected virtual void DoDrawGizmos ()
        {

        }

        void IFlowable.OnDrawGizmos () 
        {
            DoDrawGizmos ();
        }

        /// <summary>
        /// 遊戲結束時或者進到新遊戲卸載舊遊戲時
        /// </summary>
        protected virtual void DoGameQuit ()
        {

        }
    }
}
