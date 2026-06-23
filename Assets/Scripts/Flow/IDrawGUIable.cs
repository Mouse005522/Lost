using UnityEngine;

namespace Kun.Tool
{
    public interface IDrawGUIable : IDrawable
    {
        void DrawGUI ();
    }

    public interface IDrawable
    {
        string TitleName { get; }
    }

    public static class DrawableUtility
    {
        /// <summary>
        /// 繪製出folder以及GUIContent
        /// </summary>
        /// <param name="folderCache"></param>
        /// <param name="drawable"></param>
        public static void DrawGUI (FolderCache folderCache, IDrawGUIable drawGUIable)
        {
            bool showEditor = folderCache.GetFolderToggle (drawGUIable);

            var newShowEditor = GUITool.FolderOut (showEditor, drawGUIable.TitleName);

            if (newShowEditor != showEditor)
            {
                folderCache.SetFolderToggle (drawGUIable, newShowEditor);
            }

            if (showEditor)
            {
                GUITool.DrawInIndent (() =>
                {
                    drawGUIable.DrawGUI ();
                });
            }
        }
    }
}

