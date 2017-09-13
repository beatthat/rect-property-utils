
namespace BeatThat
{
	/// <summary>
	/// Common property of RectTo, RectFromTo, SetRect, etc
	/// </summary>
	public interface HasToRect 
	{
		HasRect toRect { get; }
	}
}
