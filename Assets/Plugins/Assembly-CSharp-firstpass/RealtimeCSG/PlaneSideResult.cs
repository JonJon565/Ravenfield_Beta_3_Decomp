using System;

namespace RealtimeCSG
{
	[Serializable]
	public enum PlaneSideResult : byte
	{
		Intersects = 0,
		Inside = 1,
		Outside = 2
	}
}
