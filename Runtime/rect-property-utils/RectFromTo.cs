using BeatThat.ColorExtensions;
using BeatThat.GetComponentsExt;
using BeatThat.Rects;
using BeatThat.TransformPathExt;
using UnityEngine;

namespace BeatThat.Properties
{
    /// <summary>
    /// Animates a rect 
    /// Exposes the IHasFloat set_value interface, so this component can be used more easily in transitions (e.g. as an element of a TransitionsElements)
    /// </summary>
    public class RectFromTo : RectTween, HasToRect
	{
		public HasRect m_start;
		public HasRect m_end;
	
		public bool m_lockStartRect;

		/// <summary>
		/// If set to true, transition can overshoot
		/// </summary>
		[Tooltip("set true to allow overshoot")]
		public bool m_unclamped;

		#region HasToRect implementation
		public HasRect toRect { get { return m_end; } }
		#endregion

		private Rect m_startRect = Rect.zero;

		private bool hasCapturedStartRect { get; set; }

		private void CaptureStartRect()
		{
			#if UNITY_EDITOR || DEBUG_UNSTRIP
			if(m_debug) {
				Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + "::CaptureStartRect as " + m_start.rect);
			}
			#endif

			m_startRect = m_start.rect;
			this.hasCapturedStartRect = true;
		}

		override protected void Start()
		{
			base.Start ();
			if(m_lockStartRect) {
				CaptureStartRect();
			}
			this.didStart = true;
		}

		private bool didStart { get; set; }

		override protected void OnEnable()
		{
			base.OnEnable ();
			if(!this.didStart) {
				return;
			}
			if(m_lockStartRect) {
				CaptureStartRect();
			}
		}

		override protected void OnDisable()
		{
			base.OnDisable ();
			this.hasCapturedStartRect = false;
		}

		private bool GetStartRect(ref Rect start) 
		{
			
			if(m_start == null) {
				return false;
			}

			if(!(Application.isPlaying && m_lockStartRect)) {
				start = m_start.rect;
				return true;
			}

			if(!this.hasCapturedStartRect) {
				CaptureStartRect();
			}

			start = m_startRect;
			return true;
		}

		override protected bool CalculateRect(ref Rect r) 
		{
			Rect start = Rect.zero;
			if(!GetStartRect(ref start)) {
				Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] start rect is undefined");
				return false;
			}

			if(m_end == null) {
				Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] start or end rect is null");
				return false;
			}

			if(m_debug) {
				Debug.Log("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + " CalculateRect r=" 
					+ _CalculateRect(start, m_end.rect) + ", last=" + this.driven.rect
					+ ", start=" + start + ", end=" + m_end.rect);
			}

			r = _CalculateRect(start, m_end.rect);
			
			return true;
		}

		protected Rect _CalculateRect(Rect start, Rect end) 
		{
			return (m_unclamped)?
				start.LerpToUnclamped(end, this.value):
				start.LerpTo(end, this.value);
		} 


		#if UNITY_EDITOR
		void Reset()
		{
			var mightReplace = this.GetSiblingComponent<IDrive<HasRect>>(true);

			if(mightReplace != null) {
				m_drivenRect = mightReplace.driven;

				var hasTR = mightReplace as HasToRect;
				if(hasTR != null) {
					m_end = hasTR.toRect;
				}
			}
		}

		void OnDrawGizmosSelected()
		{
			if(m_start == null || m_end == null) {
				return;
			}

			var r = _CalculateRect((Application.isPlaying && this.hasCapturedStartRect)? m_startRect: m_start.rect, m_end.rect);

			var rt = this.transform as RectTransform;

			rt.DrawGizmoScreenRect(r, Color.cyan);
			rt.DrawGizmoFillScreenRect(r, Color.cyan.WithAlpha(0.3f));

			if(Application.isPlaying && m_lockStartRect && (m_startRect.width > 0f || m_startRect.height > 0f)) {
				rt.DrawGizmoScreenRect(m_startRect, Color.magenta);
				rt.DrawGizmoFillScreenRect(m_start.rect, Color.magenta.WithAlpha(0.3f));
			}

			rt.DrawGizmoScreenRect(m_start.rect, Color.red);
			rt.DrawGizmoFillScreenRect(m_start.rect, Color.red.WithAlpha(0.3f));

			DrawGizmosDebugPath();

			rt.DrawGizmoScreenRect(m_end.rect, Color.green);
			rt.DrawGizmoFillScreenRect(m_end.rect, Color.green.WithAlpha(0.3f));
		}
		#endif


	}
}



