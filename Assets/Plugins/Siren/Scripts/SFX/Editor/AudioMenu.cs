#if UNITY_EDITOR
using UnityEditor;

namespace Siren.Editor
{
    /// <summary>
    /// Contains menu items to open things from the audio editor
    /// </summary>
    public class AudioMenu
    {
        [MenuItem("Window/Audio/Siren %&A")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<AudioSystemWindow>();
        }
    }
}
#endif