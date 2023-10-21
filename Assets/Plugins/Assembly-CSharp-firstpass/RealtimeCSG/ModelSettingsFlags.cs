using System;

namespace RealtimeCSG
{
	[Serializable]
	[Flags]
	public enum ModelSettingsFlags
	{
		ShadowCastingModeFlags = 7,
		ReceiveShadows = 8,
		DoNotRender = 0x10,
		NoCollider = 0x20,
		IsTrigger = 0x40
	}
}
