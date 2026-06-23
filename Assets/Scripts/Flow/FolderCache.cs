using System.Collections.Generic;

namespace Kun.Tool
{
    /// <summary>
    /// 需要紀錄是否展開folder時
    /// 提供一個簡易的cache緩存
    /// </summary>
    public class FolderCache
    {
        Dictionary<object, bool> folderToggle = new Dictionary<object, bool> ();

        /// <summary>
        /// 用data本身當key決定顯示的開關
        /// 傳入的data都要是class
        /// </summary>
        /// <param name="key"></param>
        /// <param name="isFolder"></param>
        public void SetFolderToggle (object key, bool isFolder)
        {
            folderToggle[key] = isFolder;
        }

        /// <summary>
        /// 用data本身當key決定顯示的開關
        /// 傳入的data都要是class
        /// </summary>
        public bool GetFolderToggle (object key, bool defaultValue = true)
        {
            if (folderToggle.ContainsKey (key) == false)
            {
                folderToggle.Add (key, defaultValue);
            }

            return folderToggle[key];
        }

        /// <summary>
        /// 物件被收回時傳入以清除
        /// </summary>
        /// <param name="key"></param>
        public void RemoveCache (object key)
        {
            if (folderToggle.ContainsKey (key))
            {
                folderToggle.Remove (key);
            }
        }
    }
}

