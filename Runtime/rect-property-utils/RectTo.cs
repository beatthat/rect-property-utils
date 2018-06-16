using BeatThat.TransformPathExt;
using BeatThat.GetComponentsExt;
using BeatThat.Properties;
using UnityEngine;
using BeatThat.ColorExtensions;
using BeatThat.Rects;

namespace BeatThat.Properties{
	/// <summary>
	/// Animates a rect to a target using whatever start rect encountered
	/// Exposes the IHasFloat set_value interface, so this component can be used more easily in transitions (e.g. as an element of a TransitionsElements)
	/// </summary>
	public class RectTo : RectTween, HasToRect, HasPlay
	{
		public HasRect m_end;

		[Tooltip("If set then uses a fixed offset from end as the start rect. Default is to capture start rect when 'value' is 0")]
		[SerializeField]private bool m_startFromFixedOffset;

		[Tooltip("Fixed offset from end used to calculated the start rect")]
		[SerializeField]private Vector2 m_fixedOffset;

		#region HasToRect implementation
		public HasRect toRect { get { return m_end; } }
		#endregion


		public Rect startRect { get { return m_startRect; } }

		override protected bool CalculateRect(ref Rect r) 
		{
			return CalculateRect(ref r, true);
		}

		private bool CalculateRect(ref Rect r, bool allowSideEffects)
		{
			if(m_end == null || m_drivenRect == null) {
				return false;
			}

			var endRect = m_end.rect;

			var stRect = (m_startFromFixedOffset || Mathf.Approximately(0, this.value))? 
				RecalculateStartRect(endRect, allowSideEffects): this.startRect;

			r = stRect.LerpTo(endRect, this.value);
			return true;
		}

		private Rect RecalculateStartRect(Rect endRect, bool updateProp)
		{
			Rect sRect;
			if(m_startFromFixedOffset) {
				sRect = endRect;
				sRect.center = endRect.center + m_fixedOffset;
			}
			else {
				sRect = this.driven.rect;
			}

			#if BT_DEBUG_UNSTRIP
			if(m_debug && updateProp) {
				Debug.LogError("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() 
					+ "::RecalculateStartRect startRect=" + sRect);
			}
			#endif

			if(sRect.Area() <= 0f) {
				Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] RecalculateStartRect got rect with non-positive area");
			}

			if(updateProp) {
				m_startRect = sRect;
			}

			return sRect;
		}

		public void Play()
		{
			#if BT_DEBUG_UNSTRIP
			if(m_debug) {
				Debug.Log("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + "::Play");
			}
			#endif
			RecalculateStartRect(m_end.rect, true);
		}

		#if UNITY_EDITOR
		void Reset()
		{
			var mightReplace = this.GetSiblingComponent<IDrive<HasRect>>(true);

			if(mightReplace != null) {
				m_drivenRect = mightReplace.driven;

				var toRectProp = mightReplace as HasToRect;
				if(toRectProp != null) {
					m_end = toRectProp.toRect;
				}
			}
		}

		void OnDrawGizmosSelected()
		{
			Rect r = Rect.zero;
			if(!CalculateRect(ref r, false)) {
				return;
			}

			var rt = this.transform as RectTransform;

			rt.DrawGizmoScreenRect(m_startRect, Color.red);
			rt.DrawGizmoFillScreenRect(m_startRect, Color.red.WithAlpha(0.3f));

			DrawGizmosDebugPath();

			rt.DrawGizmoScreenRect(r, Color.cyan);
			rt.DrawGizmoFillScreenRect(r, Color.cyan.WithAlpha(0.3f));

			rt.DrawGizmoScreenRect(m_end.rect, Color.green);
			rt.DrawGizmoFillScreenRect(m_end.rect, Color.green.WithAlpha(0.3f));
		}
		#endif

		private Rect m_startRect = new Rect(0,0,0,0);
	}
}



