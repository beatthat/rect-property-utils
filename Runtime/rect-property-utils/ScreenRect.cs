using BeatThat.Rects;
using BeatThat.TransformPathExt;
using UnityEngine;

namespace BeatThat.Properties
{
    /// <summary>
    /// Provides a screen rect for a RectTransform
    /// </summary>
    public class ScreenRect : HasRect
	{
		public bool m_debug;

		#region implemented abstract members of HasRect
		public override Rect rect 
		{
			get { 
				var rt = this.rectTransform;
				if(rt == null) {
					Debug.LogError("WTF???");
				}
				return this.rectTransform.GetScreenRect();
			}
			set { 
				if(m_debug) {
					Debug.Log("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + " set_rect from " + this.rect + " to " + value);
				}
				this.rectTransform.SetScreenRect(value); 
			}
		}
		#endregion

		// can't use ??. unity sometimes treats null as not null and then missing ref exceptions ensue
		private RectTransform rectTransform { get { return m_rectTransform != null? m_rectTransform: (m_rectTransform = GetComponent<RectTransform>()); } }
		private RectTransform m_rectTransform;
	}
}

