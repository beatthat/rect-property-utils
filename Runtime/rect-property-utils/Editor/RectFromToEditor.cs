using BeatThat.Properties;
using UnityEditor;
using UnityEngine;

namespace BeatThat.Properties{
	[CustomEditor(typeof(RectFromTo), true)]
	[CanEditMultipleObjects]
	public class RectFromToEditor : RectTweenEditor
	{
		override protected void OnDisplaysFloatInspectorGUI() 
		{
			base.DrawDisplaysFloatProperties();
			base.DrawDrivenRectProp();

			var startProp = this.serializedObject.FindProperty("m_start");
			var endProp = this.serializedObject.FindProperty("m_end");
			var lockStartRectProp = this.serializedObject.FindProperty("m_lockStartRect");
			var unclampedProp = this.serializedObject.FindProperty("m_unclamped");

			EditorGUILayout.PropertyField(startProp, new GUIContent("Start", "ScreenRect where this transition starts"));
			EditorGUILayout.PropertyField(endProp, new GUIContent("End", "ScreenRect where this transition ends"));

			EditorGUILayout.PropertyField(lockStartRectProp);
			EditorGUILayout.PropertyField(unclampedProp, new GUIContent("Unclamped", "Allow overshoot values"));

			base.DrawDebugProps();

			this.serializedObject.ApplyModifiedProperties();
		}
	}
}
