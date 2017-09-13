using UnityEngine;

namespace BeatThat
{
	/// <summary>
	/// Animates a rect to a target using whatever start rect encountered
	/// Exposes the IHasFloat set_value interface, so this component can be used more easily in transitions (e.g. as an element of a TransitionsElements)
	/// </summary>
	public class SetRect : MonoBehaviour, IDrive<HasRect>, HasToRect, IAppliesLayout
	{
		public HasRect m_drivenRect;
		public HasRect m_toRect;

		/// <summary>
		/// If true, the will copy the anchors from toRect to drivenRect (after driven rect has been updated).
		/// 
		/// NOTE: this ONLY works if drivenRect and toRect are attached to the GameObjects 
		/// with the desired from/to RectTransforms
		/// </summary>
		[Tooltip("If true, the will copy the anchors from toRect to drivenRect (after driven rect has been updated)")]
		public bool m_copyAnchors;

		public string m_findDrivenRectByTag;
		public string m_findToRectByTag;

		public bool m_disabled;

		#region IDrive implementation
		public HasRect driven 
		{ 
			get { 
				if(m_drivenRect != null) {
					return m_drivenRect;
				}

				if(!string.IsNullOrEmpty(m_findDrivenRectByTag)) {

					Debug.LogWarning("HOW WHEN TO CLEAR TAG-FOUND ITEM (ACTIVE TAGGED ITEM MIGHT CHANGE)");

					m_drivenRect = this.gameObject.FindByTag<HasRect>(m_findDrivenRectByTag);
					return m_drivenRect; // even if we didn't find it, should not use local in this case
				}

				return m_drivenRect; 
			} 
		}

		public object GetDrivenObject() { return this.driven; }
		#endregion


		#region HasToRect implementation
		public HasRect toRect 
		{ 
			get { 
				if(m_toRect != null) {
					return m_toRect;
				}

				if(!string.IsNullOrEmpty(m_findToRectByTag)) {

					Debug.LogWarning("HOW WHEN TO CLEAR TAG-FOUND ITEM (ACTIVE TAGGED ITEM MIGHT CHANGE)");

					m_toRect = this.gameObject.FindByTag<HasRect>(m_findToRectByTag);
					return m_toRect; // even if we didn't find it, should not use local in this case
				}

				return m_toRect; 
			}
		}
		#endregion

		#region IAppliesLayout implementation
		public void ApplyLayout ()
		{
			UpdateDisplay();
		}
		#endregion

		void OnEnable()
		{
			UpdateDisplay();
		}

		public void UpdateDisplay()
		{
			if(m_disabled) {
				return;
			}

			if(this.driven == null || this.toRect == null) {
				Debug.LogError("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + " a required property (drivenRect or toRect) is unset");
				return;
			}

			this.driven.rect = this.toRect.rect;
			if(m_copyAnchors) {
				var fromRT = (this.toRect.transform as RectTransform);
				(this.driven.transform as RectTransform).SetAnchorsWithCurrentScreenRect(fromRT.anchorMin, fromRT.anchorMax);
			}

			// TODO: maybe clear anything found by tag here?
		}

		void OnDidApplyAnimationProperties()
		{
			UpdateDisplay();
		}

		#if UNITY_EDITOR
		void Reset()
		{
			var mightReplace = this.GetSiblingComponent<IDrive<HasRect>>(true);

			if(mightReplace != null) {
				m_drivenRect = mightReplace.driven;

				var toRectProp = mightReplace as HasToRect;
				if(toRectProp != null) {
					m_toRect = toRectProp.toRect;
				}
			}
		}

		void OnDrawGizmosSelected()
		{
			if(m_toRect == null) {
				return;
			}

			Rect r = m_toRect.rect;


			var rt = this.transform as RectTransform;

			rt.DrawGizmoScreenRect(r, Color.cyan);
			rt.DrawGizmoFillScreenRect(r, Color.cyan.WithAlpha(0.3f));
		}
		#endif

	}
}
