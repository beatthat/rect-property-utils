using UnityEngine;

namespace BeatThat
{
	/// <summary>
	/// Useful when a rect is passed in to a component as a param at runtime a
	/// and multiple child components of the main component need to use the HasRect.
	/// Instead of having to configure each of those children, have a single proxy component.
	/// In that way, all the children can be configured in advance with the proxy.
	/// </summary>
	public class HasRectProxy : HasRect 
	{
		public HasRect m_hasRect;

		public HasRect hasRect { get { return m_hasRect; } set { m_hasRect = value; } }

		#region implemented abstract members of HasRect
		public override Rect rect 
		{
			get {
				return m_hasRect.rect;
			}
			set {
				m_hasRect.rect = value;
			}
		}
		#endregion
	}
}