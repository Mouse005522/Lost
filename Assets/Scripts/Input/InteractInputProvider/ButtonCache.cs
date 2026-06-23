using System;

namespace Kun.Tool
{
    /// <summary>
    /// 封裝單一按鈕的所有輸入狀態
    /// </summary>
    public class ButtonCache
    {
        // 外部注入的原始訊號取得方法
        private readonly Func<bool> getRawDown;
        private readonly Func<bool> getRawUp;

        // 雙擊偵測閾值（秒）
        private const float DoubleClickThreshold = 0.5f;

        // 使用 single-field：< 0 表示沒有可用的前次按下（已消費）
        private float lastDownTime = -1f;
        private bool isDouble = false;

        public bool IsDown { get; private set; }
        public bool IsUp { get; private set; }

        /// <summary>
        /// 一般模式：Down 之後保持為 true，直到 Up 為止。
        /// </summary>
        public bool IsPress { get; private set; }

        /// <summary>
        /// 雙擊偵測結果，當幀為 true
        /// </summary>
        public bool IsDouble => isDouble;

        /// <summary>
        /// 上一次 Down 的時間
        /// </summary>
        public float DownTime => lastDownTime;

        /// <summary>
        /// 上一次 Down 是否已被消費（true 表示已消費 / 無可用前次按下）
        /// </summary>
        public bool DownApplied => lastDownTime < 0f;

        /// <summary>
        /// 一般按鈕：IsPress 持續到 Up 信號為止
        /// </summary>
        public static ButtonCache Create (Func<bool> getRawDown, Func<bool> getRawUp)
        {
            return new ButtonCache (getRawDown, getRawUp);
        }

        ButtonCache (Func<bool> getRawDown, Func<bool> getRawUp)
        {
            this.getRawDown = getRawDown;
            this.getRawUp = getRawUp;
        }

        /// <param name="currentTime">由上層傳入的當前時間（累積秒數）</param>
        public void Update (float currentTime)
        {
            IsDown = getRawDown ();
            IsUp = getRawUp ();

            if (IsDown)
            {
                IsPress = true;
            }

            if (IsUp)
            {
                IsPress = false;
            }

            // 雙擊邏輯（以 lastDownTime 單一欄位管理）
            if (IsDown)
            {
                if (lastDownTime >= 0f && currentTime - lastDownTime <= DoubleClickThreshold)
                {
                    // 第二次按下，且在閾值內 -> 雙擊
                    isDouble = true;
                    // 將 lastDownTime 設為 -1 表示已消費
                    // 這樣下一次按下就不會再與這個時間比較，避免連續多次按下都被誤判為雙擊
                    // 連按四下 的情況：第一次按下（記錄時間），第二次按下（雙擊，消費時間），第三次按下（新時間），第四次按下（雙擊，消費時間）
                    lastDownTime = -1f;
                }
                else
                {
                    // 非雙擊：記錄為新的 lastDownTime，等待下一次判斷
                    isDouble = false;
                    lastDownTime = currentTime;
                }
            }
            else
            {
                isDouble = false;
            }
        }
    }
}
