#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace Siren.Utilities.Editor
{
    /// <summary>
    /// Displays a custom question confirmation popup to give the user a moment to reconsider what he's doing. Default: "Are you sure?"
    /// </summary>
    public class ConfirmActionPopup : EditorWindow
    {
        private GUIStyle _WordWrapStyle;

        private string _QuestionText = "Are you sure?";

        public event System.Action OnConfirm;
        public event System.Action<bool> OnButton;

        public void Awake()
        {
            ShowUtility();
            Focus();

            _WordWrapStyle = new GUIStyle(GUI.skin.label)
            {
				alignment = TextAnchor.MiddleCenter,
                wordWrap = true,
            };

            titleContent = new GUIContent("Please Confirm");
            Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Vector2 size = new Vector2(220, 100);
            position = new Rect(mousePos - size / 2, size);
        }

        public ConfirmActionPopup SetQuestion(string question)
        {
            _QuestionText = question;
            return this;
        }

		public ConfirmActionPopup CenterOnRect(Rect rect)
		{
			position = new Rect(rect.center - (position.size / 2), position.size);
			return this;
		}

		/// <summary>
		/// Set a description. Note: Try to keep it a small comprehensive sentence without too much jargon. 
		/// </summary>
		/// <param name="context"></param>
		public void SetActionContext(string context)
        {
            titleContent = new GUIContent(context);
        }

        public void OnLostFocus()
        {
            Close();
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Label(_QuestionText, _WordWrapStyle, GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
			GUILayout.Space(10);
            if (GUILayout.Button("Yes"))
            {
                OnConfirm?.Invoke();
                OnButton?.Invoke(true);
                Close();
            }
			GUILayout.Space(10);

			if (GUILayout.Button("No"))
            {
                OnButton?.Invoke(false);
                Close();
            }
			GUILayout.Space(10);

			GUILayout.EndHorizontal();
			GUILayout.Space(10);
			EditorGUILayout.EndVertical();
        }
    }
}
#endif