using System;
using System.Collections.Generic;
using System.Linq;

namespace Kun.Tool
{
    /// <summary>
    /// 服務的集合群組 
    /// </summary>
    public class ServiceCollection : IServiceCollection
    {
        FolderCache folderCache = new FolderCache ();

        public ServiceCollection (float physicsRate = 0.1f, EasyInject defaultInject = null)
        {
            this.physicsRate = physicsRate;

            if (defaultInject != null)
            {
                ServiceInject = defaultInject.CreateClone ();
                ServiceInject.RemoveService<IServiceCollection> ();
                ServiceInject.RemoveService<ServiceCollection> ();
            }
            else
            {
                ServiceInject = new EasyInject ();
            }

            ServiceInject.AddService (this).With<IServiceCollection> ();
        }

        /// <summary>
        /// 管理器是否準備完成
        /// </summary>
        bool prepareFinish = false;

        public void BindingService ()
        {
            foreach (var mgr in CacheServices)
            {
                try
                {
                    BindingService (mgr);
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        public void SetupServices ()
        {
            foreach (var mgr in flowables)
            {
                try
                {
                    mgr.OnSetup ();
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }

            prepareFinish = true;
        }

        public void InitServices ()
        {
            foreach (var mgr in flowables)
            {
                try
                {
                    mgr.OnInit ();
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        void IServiceCollection.BindingService (object obj)
        {
            BindingService (obj);
        }

        /// <summary>
        /// 檢查是否需要透過attribute綁訂DI
        /// </summary>
        /// <param name="obj"></param>
        public void BindingService (object obj)
        {
            ServiceInject.BindingService<EasyInjectAttribute> (obj);
        }

        /// <summary>
        /// 透過DI產生物件
        /// 自動觸發無參數建構式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateRequest<T> () where T : class, new()
        {
            T req = new T ();
            BindingService (req);
            return req;
        }

        T IServiceCollection.CreateRequest<T> ()
        {
            return this.CreateRequest<T> ();
        }

        public EasyInject ServiceInject { get; private set; } = new EasyInject ();

        public List<object> CacheServices { get; private set; } = new List<object> ();

        /// <summary>
        /// 加入需要更新的物件
        /// </summary>
        /// <param name="flowable"></param>
        void FetchInterface (object obj)
        {
            if (obj is IFlowable flowable)
            {
                if (flowables.Contains (flowable) == false)
                {
                    flowables.Add (flowable);
                }
            }

            if (obj is IDrawGUIable drawGUIable) 
            {
                if (drawGUIables.Contains (drawGUIable) == false)
                {
                    drawGUIables.Add (drawGUIable);
                }
            }
        }

        List<IDrawGUIable> drawGUIables = new List<IDrawGUIable> ();

        List<IFlowable> flowables = new List<IFlowable> ();

        #region UnityLifeFlow

        const float PhysicsRate = 0.1f;

        public void OnUpdate (float deltaTime)
        {
            if (prepareFinish == false)
            {
                return;
            }

            foreach (var flowable in flowables)
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

            foreach (var flowable in flowables)
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

            UpdatePhysics (deltaTime);
        }

        public void OnLateUpdate (float deltaTime)
        {
            if (prepareFinish == false)
            {
                return;
            }

            foreach (var flowable in flowables)
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

        float physicsRate = 0.1f;
        float physicsTimer = 0f;

        void UpdatePhysics (float deltaTime) 
        {
            physicsTimer += deltaTime;
            if (physicsTimer >= physicsRate)
            {
                physicsTimer = 0;

                foreach (IFlowable flowable in flowables)
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
        }


        public void OnDrawGizmos ()
        {
            foreach (var flowable in flowables)
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

        public void OnFixedUpdate (float deltaTime)
        {
            if (prepareFinish == false)
            {
                return;
            }

            foreach (var flowable in flowables)
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

        public void OnBeforeRender ()
        {
            if (prepareFinish == false)
            {
                return;
            }

            foreach (var flowable in flowables)
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

        public void DrawGUI ()
        {
            foreach (var drawGUIable in drawGUIables)
            {
                try
                {
                    DrawableUtility.DrawGUI (folderCache, drawGUIable);
                }
                catch (Exception ex)
                {
                    LoggerRouter.Exception (ex);
                }
            }
        }

        /// <summary>
        /// 遊戲結束時或者進到新遊戲卸載舊遊戲時
        /// </summary>
        public void OnGameQuit ()
        {
            foreach (var flowable in flowables)
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

            flowables.Clear ();
        }

        #endregion

        #region service

        /// <summary>
        /// 如果希望被以基底型別譬如interface之類的
        /// T要傳入想被記住的Type
        /// 透過無參數建構式觸發
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public EasyInjectAddToken<ServiceType> AddService<RequestT, ServiceType> (RequestT requestT) where RequestT : ServiceType
        {
            var token = ServiceInject.AddService<ServiceType> (requestT);

            FetchInterface (requestT);

            if (CacheServices.Contains (requestT) == false)
            {
                CacheServices.Add (requestT);
            }

            return token;
        }

        /// <summary>
        /// 如果希望被以基底型別譬如interface之類的
        /// T要傳入想被記住的Type
        /// 透過無參數建構式觸發
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public EasyInjectAddToken<RequestT> AddService<RequestT, ServiceType> () where RequestT : ServiceType, new()
        {
            var token = ServiceInject.AddService<RequestT, ServiceType> (out RequestT service);

            FetchInterface (service);

            if (CacheServices.Contains (service) == false)
            {
                CacheServices.Add (service);
            }

            return token;
        }

        /// <summary>
        /// 如果希望被以基底型別譬如interface之類的
        /// T要傳入想被記住的Type
        /// 透過無參數建構式觸發
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public EasyInjectAddToken<RequestT> AddService<RequestT> () where RequestT : new()
        {
            var token = ServiceInject.AddService (out RequestT service);

            FetchInterface (service);

            if (CacheServices.Contains (service) == false)
            {
                CacheServices.Add (service);
            }

            return token;
        }

        /// <summary>
        /// 如果希望被以基底型別譬如interface之類的
        /// T要傳入想被記住的Type
        /// 某些Service在多個地方被註冊needCache傳false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public EasyInjectAddToken<T> AddService<T> (T service, bool needCache = true)
        {
            var token = ServiceInject.AddService (service);

            if (needCache)
            {
                FetchInterface (service);

                if (CacheServices.Contains (service) == false)
                {
                    CacheServices.Add (service);
                }
            }

            return token;
        }

        /// <summary>
        /// 會以實體型別被記錄
        /// 用以讓底層替衍生者進行注入的場合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        public void AddServiceWithRealType (object service)
        {
            ServiceInject.AddServiceWithRealType (service);
        }

        #endregion

        /// <summary>
        /// 加入服務的外部套件對於Service工廠的處理
        /// </summary>
        /// <param name="serviceHandle"></param>
        public void AddServiceHandle (InjectServiceHandle serviceHandle)
        {
            ServiceInject.AddInjectServiceHandle (serviceHandle);
        }
    }

    public interface IServiceCollection
    {
        /// <summary>
        /// 透過依賴注入產生
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T CreateRequest<T> () where T : class, new();

        /// <summary>
        /// 檢查是否需要透過attribute綁訂DI
        /// </summary>
        /// <param name="obj"></param>
        void BindingService (object obj);
    }
}
