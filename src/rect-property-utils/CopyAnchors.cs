using UnityEngine;

namespace BeatThat
{
	/// <summary>
	/// OnEnable will copy RectTransform anchors from source to target.
	/// </summary>
	public class CopyAnchors : MonoBehaviour
	{
		public RectTransform m_from;
		public RectTransform m_to;

		void OnEnable()
		{
			UpdateTarget();
		}

		public void UpdateTarget()
		{
			var screenRect = m_to.GetScreenRect();

			m_to.anchorMax = m_from.anchorMax;
			m_to.anchorMin = m_from.anchorMin;

			m_to.SetScreenRect(screenRect);
		}

	}
}
