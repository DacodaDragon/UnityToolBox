using Siren.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace Siren.Editor
{
	/// <summary>
	/// Defines and manages editors for the audio system.
	/// </summary>
	public class AudioSystemWindow : EditorWindow
	{
		private const int VIEW_COUNT = 3;
		private const float TOP_PADDING = 2;

		private float ViewWidth => position.width / VIEW_COUNT;
		private float ViewHeight => position.height;

		private Rect LibraryListView => new Rect(0, TOP_PADDING, ViewWidth, ViewHeight);
		private Rect LibraryEditorView => new Rect(ViewWidth, TOP_PADDING, ViewWidth, ViewHeight);
		private Rect AudioAssetEditorView => new Rect(ViewWidth * 2, TOP_PADDING, ViewWidth, ViewHeight);

		private AudioLibraryList _AudioLibraryList;
		private AudioLibraryEditor _AudioLibraryEditor;
		private AudioAssetEditor _AudioAssetEditor;

		private readonly AudioPreviewer _AudioPreviewer = new AudioPreviewer();


		private void Awake()
		{
			minSize = new Vector2(900, 400);
			titleContent = new GUIContent("Siren", "Siren audio management system");
		}

		public void OnFocus()
		{
			_AudioLibraryList.OnFocusAndEnable();
		}

		private void OnEnable()
		{
			_AudioLibraryList = new AudioLibraryList(this);
			_AudioLibraryEditor = new AudioLibraryEditor(this);
			_AudioAssetEditor = new AudioAssetEditor(this);

			_AudioPreviewer.Create();
			_AudioLibraryList.OnFocusAndEnable();
			_AudioLibraryList.OnSelected += _AudioLibraryEditor.SetTarget;
			_AudioLibraryEditor.OnSelected += _AudioAssetEditor.SetTarget;
			_AudioLibraryEditor.OnPreview += _AudioPreviewer.Play;
			_AudioLibraryEditor.OnRequestRepaint += Repaint;
			_AudioLibraryList.OnRequestRepaint += Repaint;
		}

		private void OnDisable()
		{
			_AudioPreviewer.Remove();
			_AudioLibraryList.OnSelected -= _AudioLibraryEditor.SetTarget;
			_AudioLibraryEditor.OnSelected -= _AudioAssetEditor.SetTarget;
			_AudioLibraryEditor.OnPreview -= _AudioPreviewer.Play;
			_AudioLibraryEditor.OnRequestRepaint -= Repaint;
			_AudioLibraryList.OnRequestRepaint -= Repaint;
		}

		private void DrawInViewArea(Rect viewRect, System.Action drawFunc)
		{
			GUILayout.BeginArea(viewRect);
			drawFunc.Invoke();
			GUILayout.EndArea();
		}

		private void OnGUI()
		{
			if (Event.current.isMouse && Event.current.type == EventType.MouseDown)
			{
				// This makes the editor window more responsive in this use case
				// since it uses custom selectable elements
				Repaint();
			}

			DrawInViewArea(AudioAssetEditorView, _AudioAssetEditor.DoAssetEditor);
			DrawInViewArea(LibraryEditorView, _AudioLibraryEditor.DoLibraryEditor);
			DrawInViewArea(LibraryListView, _AudioLibraryList.DoList);

			DrawDividerLine(ViewWidth);
			DrawDividerLine(ViewWidth * 2);
		}

		private void DrawDividerLine(float x)
		{
			Vector2 p1 = new Vector2(x, position.height);
			Vector2 p2 = new Vector3(x, 0);
			EditorScriptUtil.DrawLine(p1, p2, Color.black);
		}
	}
}
