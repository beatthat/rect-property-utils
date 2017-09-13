using UnityEditor;
using UnityEngine;

namespace BeatThat
{
	[CustomEditor(typeof(RectTween), true)]
	[CanEditMultipleObjects]
	public class RectTweenEditor : DisplaysFloatEditor
	{
		protected void DrawDrivenRectProp()
		{
			var drivenRectProp = this.serializedObject.FindProperty("m_drivenRect");
			EditorGUILayout.PropertyField(drivenRectProp, new GUIContent("Driven Rect", "The object moved by this transition"));
		}

		protected void DrawDebugProps()
		{
			var debugProp = this.serializedObject.FindProperty("m_debug");
			EditorGUILayout.PropertyField(debugProp);

			if(debugProp.boolValue) {
				var debugPathProp = this.serializedObject.FindProperty("m_debugPath");
				EditorGUILayout.PropertyField(debugPathProp, new GUIContent("Debug Path", "While playing in editor, will capture path and draw frame positions in gizmos"));
			}

		}
	}
}