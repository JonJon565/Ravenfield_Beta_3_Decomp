using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RealtimeCSG
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Surface
	{
		public Plane Plane;

		public Vector3 Tangent;

		public Vector3 BiNormal;

		public int TexGenIndex;
	}
}
