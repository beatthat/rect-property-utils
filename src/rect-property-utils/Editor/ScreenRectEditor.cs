using UnityEditor;
using UnityEngine;

namespace BeatThat
{
	[CustomEditor(typeof(ScreenRect), true)]
	[CanEditMultipleObjects]
	public class ScreenRectEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			var screenRect = (this.target as ScreenRect);

			var rect = screenRect.rect;
		
			var rectAfter = EditorGUILayout.RectField("Screen Rect", rect);
			if(!rectAfter.Approximately(rect)) {
				screenRect.rect = rectAfter;
			}

			base.OnInspectorGUI();
		}
	}
}