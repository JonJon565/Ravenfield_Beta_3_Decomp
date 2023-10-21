using System;

namespace RealtimeCSG
{
	[Serializable]
	public enum CSGOperationType : byte
	{
		Additive = 0,
		Subtractive = 1,
		Intersecting = 2
	}
}
