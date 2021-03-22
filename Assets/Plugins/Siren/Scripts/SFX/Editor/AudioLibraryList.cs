#if UNITY_EDITOR
using Siren.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using ToolBox.Editor;

namespace Siren.Editor
{
	/// <summary>
    /// Used to render and control a selectable list of all audio libraries in the project
    /// /// </summary>
    public class AudioLibraryList
    {
        public event System.Action<AudioAssetLibrary> OnSelected;
        public event System.Action OnRequestRepaint;

		private readonly EditorWindow _CurrentWindow;
        private readonly SelectableList _SelectionList;
        private Vector2 _ScrollPosition = Vector2.zero;

        private string[] _AssetGuids;
        private string[] _AssetPaths;
        private string[] _AssetLabels;

        public AudioLibraryList(EditorWindow windowInstance)
        {
            _SelectionList = new SelectableList(DrawElement);
            _SelectionList.OnSelect += OnSelect;
			_CurrentWindow = windowInstance;
		}

        public void DoList()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{nameof(AudioLibraryList)}");

            if (GUILayout.Button("Create New Library"))
            {
				EditorWindow.CreateInstance<CreateAudioLibraryAssetPopup>()
					.SetFolder("Assets/Audio/Resources/")
					.CenterOnRect(_CurrentWindow.position)
                    .OnCreated += OnNewElementcreated;
            }

            if (GUILayout.Button("Delete"))
            {
                RemoveSelectedElement();
            }

            GUILayout.EndHorizontal();
            _ScrollPosition = EditorGUILayout.BeginScrollView(_ScrollPosition);
            _SelectionList.DoList(_AssetLabels.Length, _ScrollPosition);
            EditorGUILayout.EndScrollView();
        }

		internal void OnFocusAndEnable()
		{
			FetchResources();
		}

		private void OnSelect(int index)
        {
			GUI.FocusControl(null);

			if (index == -1)
			{
				OnSelected?.Invoke(null);
				return;
			}

            OnSelected?.Invoke(AssetDatabase.LoadAssetAtPath<AudioAssetLibrary>(_AssetPaths[index]));
        }

        private void DrawElement(int index)
        {
            GUILayout.Label(_AssetLabels[index]);
        }

        private void FetchResources()
        {
			_AssetGuids = AssetUtil.GetAssetGuids<AudioAssetLibrary>();
			_AssetPaths = AssetUtil.GUIDSToAssetPaths(_AssetGuids);
			_AssetLabels = AssetUtil.PathsToLabels(_AssetPaths);
        }

        private void OnNewElementcreated(AudioAssetLibrary element)
        {
			FetchResources();
			OnRequestRepaint?.Invoke();
        }

        private void RemoveSelectedElement()
        {
            int index = _SelectionList.SelectedElementIndex;

            if (index == -1) // nothing selected delete
                return;

            string name = _AssetLabels[index];
            EditorWindow.CreateInstance<ConfirmActionPopup>()
                .SetQuestion($"Are you sure you want to delete {name}?")
				.CenterOnRect(_CurrentWindow.position)
                .OnConfirm += () => RemoveAsset(_AssetPaths[index]);
        }

		private void RemoveAsset(string name)
		{
			AssetDatabase.DeleteAsset(name);
			FetchResources();
			_SelectionList.ResetSelection();
			OnRequestRepaint?.Invoke();
		}
    }
}
#endif