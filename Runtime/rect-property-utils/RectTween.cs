using System.Collections.Generic;
using BeatThat.Rects;
using BeatThat.TransformPathExt;
using UnityEngine;

namespace BeatThat.Properties
{
    /// <summary>
    /// Animates a rect to a target using whatever start rect encountered
    /// Exposes the IHasFloat set_value interface, so this component can be used more easily in transitions (e.g. as an element of a TransitionsElements)
    /// </summary>
    public abstract class RectTween : DisplaysFloat, IDrive<HasRect>
	{
		public HasRect m_drivenRect;

//		public bool m_debug;
//		// Analysis disable ConvertToAutoProperty
//		public bool debug { get { return m_debug; } set { m_debug = value; } }
//		// Analysis restore ConvertToAutoProperty

		/// <summary>
		/// When TRUE, then while playing in editor, will capture path and draw frame positions in gizmos
		/// </summary>
		[Tooltip("While playing in editor, will capture path and draw frame positions in gizmos")]
		public bool m_debugPath;


		#region IDrive implementation
		// Analysis disable ConvertToAutoProperty
		public HasRect driven { get { return m_drivenRect; } set { m_drivenRect = value; } }
		// Analysis restore ConvertToAutoProperty

		public object GetDrivenObject() { return this.driven; }
		public bool ClearDriven() { m_drivenRect = null; return true; } 
		#endregion

		override public void UpdateDisplay()
		{
			var drvn = this.driven;
			if(drvn == null) {
				Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + " driven rect is not set");
				return;
			}
			
			Rect r = Rect.zero;
			if(CalculateRect(ref r)) {

				drvn.rect = r;

				#if UNITY_EDITOR
				if(m_debugPath) {
					if(this.pathCaptured == null) {
						this.pathCaptured = new List<Rect>();
					}
					this.pathCaptured.Add(r);

				}
				else {
					this.pathCaptured = null;
				}
				#endif
			}
		}

		abstract protected bool CalculateRect(ref Rect r);

		#if UNITY_EDITOR
		protected List<Rect> pathCaptured { get; private set; }

		protected void DrawGizmosDebugPath()
		{
			if(this.pathCaptured == null) {
				return;
			}

			var rt = this.transform as RectTransform;
			foreach(var r in this.pathCaptured) {
				rt.DrawGizmoScreenRect(r, Color.magenta);
			}
		}
		#endif

	}
}


