using BeatThat.Properties;
using BeatThat.Rects;
using UnityEditor;
using UnityEngine;

namespace BeatThat.Properties{
	[CustomEditor(typeof(RectTo), true)]
	[CanEditMultipleObjects]
	public class RectToEditor : RectTweenEditor
	{
		override protected void OnDisplaysFloatInspectorGUI() 
		{
			base.DrawDisplaysFloatProperties();
			base.DrawDrivenRectProp();

			var endProp = this.serializedObject.FindProperty("m_end");
			var startFromFixedOffsetProp = this.serializedObject.FindProperty("m_startFromFixedOffset");
			 
			EditorGUILayout.PropertyField(endProp, new GUIContent("End", "ScreenRect where this transition ends"));

			EditorGUILayout.PropertyField(startFromFixedOffsetProp, 
				new GUIContent("Start From Fixed Offset", 
					"If set then uses a fixed offset from end as the start rect. Default is to capture start rect when 'value' is 0"));

			if(startFromFixedOffsetProp.boolValue) {
				var fixedOffsetProp = this.serializedObject.FindProperty("m_fixedOffset");

				EditorGUILayout.PropertyField(fixedOffsetProp, 
					new GUIContent("Fixed Offset", 
						"The offset from 'end' that will be used to calculate the start rect"));
			}
		
			base.DrawDebugProps();

			var rectTo = this.target as RectTo;

			if(Application.isPlaying) {
				EditorGUILayout.RectField("Start Rect", rectTo.startRect);

				if(GUILayout.Button("Debug Start Rect")) {
					var go = new GameObject(target.name + "-startRect", new [] { typeof(RectTransform) });
					go.transform.SetParent(rectTo.GetComponentInParent<Canvas>().transform);
					(go.transform as RectTransform).SetScreenRect((this.target as RectTo).startRect);
				}
			}

			this.serializedObject.ApplyModifiedProperties();
		}
	}
}
