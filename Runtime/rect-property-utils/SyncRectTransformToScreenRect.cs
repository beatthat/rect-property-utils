using BeatThat.GameObjectUtil;
using BeatThat.Rects;
using UnityEngine;
using UnityEngine.Serialization;

namespace BeatThat.Properties
{
    /// <summary>
    /// Keeps a RectTransform's screen rect in sync with some driver screen rect
    /// </summary>
    public class SyncRectTransformToScreenRect : MonoBehaviour, Syncable
	{
		// Analysis disable ConvertToConstant.Local
		#pragma warning disable 0414
		[SerializeField][HideInInspector][FormerlySerializedAs("m_findSourceRectByTag")] private string m_findScreenRectByTag = null;
		[SerializeField][HideInInspector]private bool m_unsetFoundScreenRectOnDisable = true;
		[SerializeField][HideInInspector]private HasRect m_screenRect;
		[SerializeField][HideInInspector]private RectTransform m_rectTransform = null;
		[SerializeField][HideInInspector]private string m_findRectTransformByTag = null;
		[SerializeField][HideInInspector]private bool m_unsetFoundTransformOnDisable = true;
		[SerializeField][HideInInspector]private bool m_debug = false;
		#pragma warning restore 0414
		// Analysis restore ConvertToConstant.Local

		// can't use ??. unity sometimes treats null as not null and then missing ref exceptions ensue
		private RectTransform rectTransform 
		{ 
			get { 
				if(m_rectTransform != null) {
					return m_rectTransform;
				}

				if(!string.IsNullOrEmpty(m_findRectTransformByTag)) {
					m_rectTransform = this.gameObject.FindByTag<RectTransform>(m_findRectTransformByTag);
					this.didFindTransform |= m_rectTransform != null;
					return m_rectTransform; // even if we didn't find it, should not use local in this case
				}

				return (m_rectTransform = GetComponent<RectTransform>());
			} 
		}

		void OnDisable()
		{
			if(this.didFindTransform && m_unsetFoundTransformOnDisable) {
				m_rectTransform = null;
				this.didFindTransform = false;
			}

			if(this.didFindScreenRect && m_unsetFoundScreenRectOnDisable) {
				m_screenRect = null;
				this.didFindScreenRect = false;
			}
		}

		private bool didFindTransform { get; set; }
		private bool didFindScreenRect { get; set; }

		private HasRect screenRect 
		{
			get {
				if(m_screenRect != null) {
					return m_screenRect;
				}

				if(!string.IsNullOrEmpty(m_findScreenRectByTag)) {
					m_screenRect = this.gameObject.FindByTag<HasRect>(m_findScreenRectByTag);
					this.didFindScreenRect |= m_screenRect != null;
					return m_screenRect; // even if we didn't find it, should not use local in this case
				}

				return (m_screenRect = GetComponent<ScreenRect>());
			}
		}

		public void Sync()
		{
			var hasRect = this.screenRect;
			if(hasRect == null) { 
				#if BT_DEBUG_UNSTRIP
				if(m_debug) {
					Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + "::Sync no rect set for sync");
				}
				#endif

				return; 
			}

			var rt = this.rectTransform;
			if(rt == null) { 
				#if BT_DEBUG_UNSTRIP
				if(m_debug) {
					Debug.LogWarning("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + "::Sync no rectTransform set for sync (probably be 'find by tag' set or would use local)");
				}
				#endif

				return; 
			}

			#if BT_DEBUG_UNSTRIP
			if(m_debug) {
				Debug.Log("[" + Time.frameCount + "][" + this.Path() + "] " + GetType() + "::Sync to " + hasRect.rect);
			}
			#endif
				
			rt.SetScreenRect(hasRect.rect);
		}

		void LateUpdate() 
		{
			Sync();
		}
	}
}

