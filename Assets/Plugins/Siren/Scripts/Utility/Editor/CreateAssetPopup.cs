using System.IO;
using ToolBox.Editor;
using UnityEditor;
using UnityEngine;

namespace Siren.Utilities.Editor
{
    public abstract class CreateAssetPopup<T> : EditorWindow where T : ScriptableObject, new()
    {
        public System.Action<T> OnCreated;

        /// <summary>
        /// File location the asset will be stored in.
        /// </summary>
        private string _FolderPath = "Assets/";
        private const string ERR_FILE_EXISTS = "Filename already exists";

        private string _AssetName = "name";
        private bool _ShowFileExistsMessage;
        private bool _SelectField = true;

        private GUIStyle _RedLabelStyle;
        private GUIStyle _CenteredLabelStyle;

		public void OnLostFocus()
        {
            Close();
        }

        public void Awake()
        {
            titleContent = new GUIContent($"Create {typeof(T).Name}");

			_CenteredLabelStyle = new GUIStyle(GUI.skin.label)
			{
				alignment = TextAnchor.MiddleCenter,
				wordWrap = true
			};

			_RedLabelStyle = new GUIStyle(_CenteredLabelStyle)
            {
                alignment = TextAnchor.MiddleCenter,
				normal = new GUIStyleState()
				{
					textColor = Color.yellow
				}
            };

			ShowUtility();

            Vector2 mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            Vector2 size = new Vector2(200, 110);
            position = new Rect(mousePos - size / 2, size);
        }

        /// <summary>
        /// Folder leading up to the target folder
        /// </summary>
        /// <param name="folder"></param>
        /// <returns>instance for function chaining</returns>
        public CreateAssetPopup<T> SetFolder(string folder)
        {
            _FolderPath = folder;

            return this;
        }

		/// <summary>
		/// Centers inside of a rectangle
		/// </summary>
		/// <param name="rect"></param>
		/// <returns></returns>
		public CreateAssetPopup<T> CenterOnRect(Rect rect)
		{
			position = new Rect(rect.center - (position.size / 2), position.size);
			return this;
		}

		private void CreateAndClose()
        {
            OnCreated?.Invoke(AssetUtil.CreateAt<T>(_FolderPath, _AssetName));
            Close();
        }

        private bool KeyPressed(KeyCode keyCode)
        {
            return Event.current.isKey && Event.current.keyCode == keyCode;
        }

        public void OnGUI()
        {
            GUILayout.Label($"Create {typeof(T).Name}:", _CenteredLabelStyle);

            GUILayout.FlexibleSpace();

			if (AssetUtil.Exists(_FolderPath, _AssetName))
			{
				GUILayout.Label(ERR_FILE_EXISTS, _RedLabelStyle);
			}

			GUILayout.BeginHorizontal();
            GUILayout.Label("Name:");

            GUI.SetNextControlName("namefield");
            var newAssetName = EditorGUILayout.TextField(_AssetName, GUILayout.ExpandWidth(true));

			if (!newAssetName.Equals(_AssetName))
			{
				Repaint();
				_AssetName = newAssetName;
			}

			GUILayout.EndHorizontal();


			if (GUILayout.Button("Create Asset") || KeyPressed(KeyCode.Return))
            {
				if (!AssetUtil.Exists(_FolderPath, _AssetName))
					CreateAndClose();
			}

            if (KeyPressed(KeyCode.Escape))
            {
	            Close();
            }

            if (_SelectField)
            {
                EditorGUI.FocusTextInControl("namefield");
                _SelectField = false;
            }
        }
    }
}
