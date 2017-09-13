using UnityEngine;

namespace BeatThat
{
	/// <summary>
	/// Animates a rect to a target using whatever start rect encountered
	/// Exposes the IHasFloat set_value interface, so this component can be used more easily in transitions (e.g. as an element of a TransitionsElements)
	/// </summary>
	public class LockRect : MonoBehaviour, IDrive<HasRect>, HasToRect, HasPlay
	{
		public HasRect m_drivenRect;
		public HasRect m_toRect;

		public bool m_lockOnEnable = true;
		public bool m_disabled; 

		#region IDrive implementation
		public HasRect driven { get { return m_drivenRect; } }

		public object GetDrivenObject() { return this.driven; }
		#endregion


		#region HasToRect implementation
		public HasRect toRect { get { return m_toRect; } }
		#endregion

		void OnEnable()
		{
			if(m_lockOnEnable) {
				Lock();
			}
		}

		#region HasPlay implementation
		public void Play()
		{
			Lock();
		}
		#endregion


		public void Lock()
		{
			if(this.toRect == null) {
				Debug.LogError("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + " a required property (toRect) is unset. Unable to lock");
				return;
			}

			this.lockRect = this.toRect.rect;
			this.isLockRectSet = true;
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

			if(!this.isLockRectSet) {
				Lock();
			}

			this.driven.rect = this.lockRect;
		}

		void OnDidApplyAnimationProperties()
		{
			UpdateDisplay();
		}

		void Update()
		{
			UpdateDisplay();
		}

		void LateUpdate()
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

			rt.DrawGizmoScreenRect(r, Color.red);
			rt.DrawGizmoFillScreenRect(r, Color.red.WithAlpha(0.3f));
		}
		#endif

		private bool isLockRectSet { get; set; }
		private Rect lockRect { get; set; }
	}
}
