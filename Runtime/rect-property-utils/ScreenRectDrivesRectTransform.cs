using BeatThat.Properties;
using BeatThat.Rects;
using UnityEngine;

namespace BeatThat.Properties{
	/// <summary>
	/// Keeps a RectTransform's screen rect in sync with some driver screen rect
	/// </summary>
	public class ScreenRectDrivesRectTransform : HasRect
	{
		public Rect m_rect = new Rect(0, 0, 1, 1);
		public bool m_updateOnEnable = false;
	
		#region implemented abstract members of HasRect
		public override Rect rect 
		{
			get { return m_rect; }
			set { m_rect = value; UpdateDisplay(); }
		}
		#endregion

		void OnEnable()
		{
			if(m_updateOnEnable) {
				UpdateDisplay();
			}
		}

		public void UpdateDisplay()
		{
			this.rectTransform.SetScreenRect(this.rect);
		}

		void OnDidApplyAnimationProperties()
		{
			UpdateDisplay();
		}

		private RectTransform rectTransform { get { return m_rectTransform != null? m_rectTransform: (m_rectTransform = this.transform as RectTransform); } }
		private RectTransform m_rectTransform;
	}
}
