using System;
using System.Collections.Generic;

namespace Kun.Tool
{
    public class FlowManager : FlowComponent
    {
        [EasyInject]
        protected ServiceCollection serviceCollection;

        protected readonly List<IFlowable> subFlowables = new List<IFlowable> ();

        protected override void Init ()
        {
            base.Init ();
            PrepareSubFlowables ();

            foreach (var flowable in subFlowables)
            {
                try
                {
                    serviceCollection.BindingService (flowable);
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnSetup ();
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnInit ();
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        protected virtual void PrepareSubFlowables ()
        {
            subFlowables.Clear ();
        }

        protected override void DoPreUpdate (float deltaTime)
        {
            base.DoPreUpdate (deltaTime);

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnPreUpdate (deltaTime);
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        protected override void DoUpdate (float deltaTime)
        {
            base.DoUpdate (deltaTime);

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnUpdate (deltaTime);
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        protected override void DoPhysicsRateUpdate (float physicsRate)
        {
            base.DoPhysicsRateUpdate (physicsRate);

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnPhysicsRateUpdate (physicsRate);
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        protected override void DoLateUpdate (float deltaTime)
        {
            base.DoLateUpdate (deltaTime);

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnLateUpdate (deltaTime);
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        protected override void DoFixedUpdate (float deltaTime)
        {
            base.DoFixedUpdate (deltaTime);

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnFixedUpdate (deltaTime);
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        protected override void DoBeforeRender ()
        {
            base.DoBeforeRender ();

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnBeforeRender ();
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        protected override void DoDrawGizmos ()
        {
            base.DoDrawGizmos ();

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnDrawGizmos ();
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        protected override void DoGameQuit ()
        {
            base.DoGameQuit ();

            foreach (var flowable in subFlowables)
            {
                try
                {
                    flowable.OnGameQuit ();
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }

            subFlowables.Clear ();
        }
    }
}
