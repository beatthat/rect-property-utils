using BeatThat.Rects;
using UnityEngine;

namespace BeatThat.Properties
{
    /// <summary>
    /// Configure a rect transform in terms of viewport coordinates relative to its parent.
    /// This is similar to using anchors, except replacing right and top anchors with width and height
    /// </summary>
    public class ViewportRect : HasRect 
	{
		#region implemented abstract members of HasRect
		public override Rect rect 
		{
			get {
				return new Rect(m_x, m_y, m_width, m_height);
			}
			set {
				m_x = value.x;
				m_y = value.y;
				m_width = value.width;
				m_height = value.height;

				if(m_changeDetection == ChangeDetection.NONE || !this.enabled) {
					UpdateIfRectPropertyChanged();
				}
			}
		}
		#endregion

		public float m_x = 0;
		public float m_y = 0;
		public float m_width = 1;
		public float m_height = 1;

		public enum ChangeDetection { NONE = 0, VIEWPORT_PROPERTIES = 1, SCREEN_RECT = 2 }
		public ChangeDetection m_changeDetection;

		private void UpdateIfRectPropertyChanged()
		{
			var curViewport = new Vector4(m_x, m_y, m_width, m_height);
			if(this.lastViewport == curViewport) {
				return;
			}

			var pRect = (this.rectTransform.parent as RectTransform).rect;

			if(pRect.width <= 0f) {
				return;
			}

			this.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, m_x * pRect.width, m_width * pRect.width);

			this.lastViewport = curViewport;
		}

		void LateUpdate ()
		{
			switch(m_changeDetection) {
			case ChangeDetection.VIEWPORT_PROPERTIES:
				UpdateIfRectPropertyChanged();
				break;
			case ChangeDetection.SCREEN_RECT:
				var parRect = (this.rectTransform.parent as RectTransform).GetScreenRect();

				if(parRect.width <= 0f || parRect.height <= 0f) {
					return;
				}
					
				var tgtRect = new Rect(parRect.x + m_x * parRect.width, parRect.y + m_y * parRect.height, parRect.width * m_width, parRect.height * m_height);

				if(tgtRect == this.lastScreenRect) {
					return;
				}

				this.lastScreenRect = tgtRect;

				this.rectTransform.SetScreenRect(tgtRect);

				break;
			}



		}

		private RectTransform rectTransform { get { return m_rectTransform?? (m_rectTransform = this.transform as RectTransform); } }
		private RectTransform m_rectTransform;
			
		private Vector4 lastViewport { get; set; }
		private Rect lastScreenRect { get; set; }
	}
}

