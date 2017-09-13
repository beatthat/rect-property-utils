using UnityEditor;
using UnityEngine;

namespace BeatThat
{
	[CustomEditor(typeof(SyncRectTransformToScreenRect), true)]
	[CanEditMultipleObjects]
	public class SyncRectTransformToScreenRectEditor : UnityEditor.Editor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var findRectTransformByTagProp = this.serializedObject.FindProperty("m_findRectTransformByTag");

			EditorGUILayout.PropertyField(findRectTransformByTagProp, 
				new GUIContent("Find RectTransform By Tag", "Find the target RectTransform by tag instead of direct configuration"));


			if(string.IsNullOrEmpty(findRectTransformByTagProp.stringValue)) {

				var rectTransformProp = this.serializedObject.FindProperty("m_rectTransform");

				EditorGUILayout.PropertyField(rectTransformProp, 
					new GUIContent("RectTransform", "Leave empty to use the local component"));
			}
			else {
				EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_unsetFoundScreenRectOnDisable"),
					new GUIContent("Unset Found ScreenRect On Disable", "When TRUE ScreenRect found by tag will be unset OnDisable"));
			}

			var findScreenRectByTagProp = this.serializedObject.FindProperty("m_findScreenRectByTag");

			EditorGUILayout.PropertyField(findScreenRectByTagProp, 
				new GUIContent("Find ScreenRect By Tag", "Find the driver ScreenRect by tag instead of direct configuration"));

			if(string.IsNullOrEmpty(findScreenRectByTagProp.stringValue)) {

				var screenRectProp = this.serializedObject.FindProperty("m_screenRect");

				EditorGUILayout.PropertyField(screenRectProp, 
					new GUIContent("ScreenRect", "Leave empty to use the local component"));
			}
			else {
				EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_unsetFoundScreenRectOnDisable"),
					new GUIContent("Unset Found ScreenRect On Disable", "When TRUE ScreenRect found by tag will be unset OnDisable"));
			}
			
			EditorGUILayout.PropertyField(this.serializedObject.FindProperty("m_debug"));

			this.serializedObject.ApplyModifiedProperties();

			if(GUILayout.Button("Sync")) {
				(this.target as SyncRectTransformToScreenRect).Sync();
			}
		}
	}
}