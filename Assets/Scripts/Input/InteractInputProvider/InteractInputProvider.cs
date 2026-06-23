using Cysharp.Threading.Tasks;
using Kun.XR;
using System.Collections.Generic;
using UnityEngine;

namespace Kun.Tool
{
    public class KeyboardMouseInteractInput : InteractInputProvider
    {
        protected override Vector2Int GetMoveInputInternal ()
        {
            Vector2Int dir = Vector2Int.zero;

            if (Input.GetKey (KeyCode.D))
            {
                dir += Vector2Int.right;
            }
            else
            {
                if (Input.GetKey (KeyCode.A))
                {
                    dir -= Vector2Int.right;
                }
            }

            if (Input.GetKey (KeyCode.W))
            {
                dir += Vector2Int.up;
            }
            else
            {
                if (Input.GetKey (KeyCode.S))
                {
                    dir -= Vector2Int.up;
                }
            }

            return dir;
        }

        protected override Vector2Int GetMoveInputDownInternal ()
        {
            Vector2Int dir = Vector2Int.zero;

            if (Input.GetKeyDown (KeyCode.D))
            {
                dir += Vector2Int.right;
            }
            else
            {
                if (Input.GetKeyDown (KeyCode.A))
                {
                    dir -= Vector2Int.right;
                }
            }

            if (Input.GetKeyDown (KeyCode.W))
            {
                dir += Vector2Int.up;
            }
            else
            {
                if (Input.GetKeyDown (KeyCode.S))
                {
                    dir -= Vector2Int.up;
                }
            }

            return dir;
        }

        protected override bool PrimaryUseDown () => Input.GetKeyDown (KeyCode.RightShift);
        protected override bool PrimaryUseUp () => Input.GetKeyUp (KeyCode.RightShift);

        protected override bool MainUseDown () => Input.GetKeyDown (KeyCode.Return);
        protected override bool MainUseUp () => Input.GetKeyUp (KeyCode.Return);
    }

    public abstract class InteractInputProvider : InputProvider
    {
        /// <summary>
        /// 子類別實作的原始持續輸入方向。
        /// </summary>
        protected abstract Vector2Int GetMoveInputInternal ();

        /// <summary>
        /// 子類別實作的原始單下點擊方向。
        /// </summary>
        protected abstract Vector2Int GetMoveInputDownInternal ();

        /// <summary>
        /// 對外取得的持續輸入方向。
        /// </summary>
        public Vector2Int GetMoveInput ()
        {
            Vector2Int result = GetMoveInputInternal ();

            return result;
        }

        /// <summary>
        /// 對外取得的單下點擊方向。
        /// </summary>
        public Vector2Int GetMoveInputDown ()
        {
            Vector2Int result = GetMoveInputDownInternal ();

            return result;
        }

        // 各子類提供原始信號，僅供內部使用
        protected abstract bool MainUseDown ();
        protected abstract bool MainUseUp ();
        protected abstract bool PrimaryUseDown ();
        protected abstract bool PrimaryUseUp ();

        // 對外公開的模組化按鈕狀態
        public ButtonCache MainUse { get; private set; }
        public ButtonCache PrimaryUse { get; private set; }

        // 所有 ButtonCache 統一管理
        private List<ButtonCache> buttonCaches;

        protected InteractInputProvider ()
        {
            MainUse = ButtonCache.Create (MainUseDown, MainUseUp);
            PrimaryUse = ButtonCache.Create (PrimaryUseDown, PrimaryUseUp);

            buttonCaches = new List<ButtonCache>
            {
                MainUse,
                PrimaryUse
            };
        }

        public override void Update (float deltaTime)
        {
            base.Update (deltaTime);

            float currentTime = Time.time;

            foreach (var cache in buttonCaches)
            {
                cache.Update (currentTime);
            }
        }

        public virtual void Dispose ()
        {
        }
    }
}
