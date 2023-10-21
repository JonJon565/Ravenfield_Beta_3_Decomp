using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using UnityEngine;

namespace RealtimeCSG
{
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct Plane
	{
		public float a;

		public float b;

		public float c;

		public float d;

		public Vector4 vector
		{
			get
			{
				return new Vector4(a, b, c, d);
			}
			set
			{
				a = value.x;
				b = value.y;
				c = value.z;
				d = value.w;
			}
		}

		public Vector3 normal
		{
			get
			{
				return new Vector3(a, b, c);
			}
			set
			{
				a = value.x;
				b = value.y;
				c = value.z;
			}
		}

		public Vector3 pointOnPlane
		{
			get
			{
				return normal * d;
			}
		}

		public Plane(Plane inPlane)
		{
			a = inPlane.a;
			b = inPlane.b;
			c = inPlane.c;
			d = inPlane.d;
		}

		public Plane(Vector3 inNormal, float inD)
		{
			a = inNormal.x;
			b = inNormal.y;
			c = inNormal.z;
			d = inD;
		}

		public Plane(Vector3 inNormal, Vector3 pointOnPlane)
		{
			a = inNormal.x;
			b = inNormal.y;
			c = inNormal.z;
			d = Vector3.Dot(inNormal, pointOnPlane);
		}

		public Plane(Quaternion inRotation, Vector3 pointOnPlane)
		{
			Vector3 lhs = inRotation * Vector3.up;
			a = lhs.x;
			b = lhs.y;
			c = lhs.z;
			d = Vector3.Dot(lhs, pointOnPlane);
		}

		public Plane(float inA, float inB, float inC, float inD)
		{
			a = inA;
			b = inB;
			c = inC;
			d = inD;
		}

		public Plane(Vector4 inVector)
		{
			a = inVector.x;
			b = inVector.y;
			c = inVector.z;
			d = inVector.w;
		}

		public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
		{
			Vector3 lhs = point2 - point1;
			Vector3 rhs = point3 - point1;
			Vector3 normalized = Vector3.Cross(lhs, rhs).normalized;
			a = normalized.x;
			b = normalized.y;
			c = normalized.z;
			d = Vector3.Dot(normalized, point1);
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "({0}, {1}, {2}, {3})", a, b, c, d);
		}

		public Vector3 Intersection(Ray ray)
		{
			Vector3 origin = ray.origin;
			Vector3 vector = ray.origin + ray.direction * 1000f;
			float sdist = Distance(origin);
			float edist = Distance(vector);
			return Intersection(origin, vector, sdist, edist);
		}

		public bool TryIntersection(Ray ray, out Vector3 intersection)
		{
			Vector3 origin = ray.origin;
			Vector3 vector = ray.origin + ray.direction * 1000f;
			float num = Distance(origin);
			if (float.IsInfinity(num) || float.IsNaN(num))
			{
				intersection = Vector3.zero;
				return false;
			}
			float num2 = Distance(vector);
			if (float.IsInfinity(num2) || float.IsNaN(num2))
			{
				intersection = Vector3.zero;
				return false;
			}
			intersection = Intersection(origin, vector, num, num2);
			if (float.IsInfinity(intersection.x) || float.IsNaN(intersection.x) || float.IsInfinity(intersection.y) || float.IsNaN(intersection.y) || float.IsInfinity(intersection.z) || float.IsNaN(intersection.z))
			{
				intersection = Vector3.zero;
				return false;
			}
			return true;
		}

		public static Vector3 Intersection(Vector3 start, Vector3 end, float sdist, float edist)
		{
			Vector3 vector = end - start;
			float num = edist - sdist;
			float num2 = edist / num;
			return end - num2 * vector;
		}

		public Vector3 Intersection(Vector3 start, Vector3 end)
		{
			return Intersection(start, end, Distance(start), Distance(end));
		}

		public static Vector3 Intersection(Plane inPlane1, Plane inPlane2, Plane inPlane3)
		{
			double num = inPlane1.b * inPlane3.c - inPlane3.b * inPlane1.c;
			double num2 = inPlane2.b * inPlane1.c - inPlane1.b * inPlane2.c;
			double num3 = inPlane3.b * inPlane2.c - inPlane2.b * inPlane3.c;
			double num4 = inPlane1.a * inPlane3.d - inPlane3.a * inPlane1.d;
			double num5 = inPlane2.a * inPlane1.d - inPlane1.a * inPlane2.d;
			double num6 = inPlane3.a * inPlane2.d - inPlane2.a * inPlane3.d;
			double num7 = 0.0 - ((double)inPlane1.d * num3 + (double)inPlane2.d * num + (double)inPlane3.d * num2);
			double num8 = 0.0 - ((double)inPlane1.c * num6 + (double)inPlane2.c * num4 + (double)inPlane3.c * num5);
			double num9 = (double)inPlane1.b * num6 + (double)inPlane2.b * num4 + (double)inPlane3.b * num5;
			double num10 = 0.0 - ((double)inPlane1.a * num3 + (double)inPlane2.a * num + (double)inPlane3.a * num2);
			if (num10 > -0.0001500000071246177 && num10 < 0.0001500000071246177)
			{
				return new Vector3(float.NaN, float.NaN, float.NaN);
			}
			return new Vector3((float)(num7 / num10), (float)(num8 / num10), (float)(num9 / num10));
		}

		public float Distance(float x, float y, float z)
		{
			return a * x + b * y + c * z - d;
		}

		public float Distance(Vector3 vertex)
		{
			return a * vertex.x + b * vertex.y + c * vertex.z - d;
		}

		public void Normalize()
		{
			float num = 1f / Mathf.Sqrt(a * a + b * b + c * c);
			a *= num;
			b *= num;
			c *= num;
			d *= num;
		}

		public Plane Negated()
		{
			return new Plane(0f - a, 0f - b, 0f - c, 0f - d);
		}

		public void Transform(Matrix4x4 transformation)
		{
			Matrix4x4 transpose = transformation.inverse.transpose;
			Vector4 vector = transpose * new Vector4(a, b, c, 0f - d);
			a = vector.x;
			b = vector.y;
			c = vector.z;
			d = 0f - vector.w;
		}

		public void InverseTransform(Matrix4x4 transformation)
		{
			Matrix4x4 transpose = transformation.transpose;
			Vector4 vector = transpose * new Vector4(a, b, c, 0f - d);
			a = vector.x;
			b = vector.y;
			c = vector.z;
			d = 0f - vector.w;
		}

		public static Plane Transformed(Plane plane, Matrix4x4 transformation)
		{
			Matrix4x4 transpose = transformation.inverse.transpose;
			Vector4 vector = transpose * new Vector4(plane.a, plane.b, plane.c, 0f - plane.d);
			return new Plane(vector.x, vector.y, vector.z, 0f - vector.w);
		}

		public static Plane InverseTransformed(Plane plane, Matrix4x4 transformation)
		{
			Matrix4x4 transpose = transformation.transpose;
			Vector4 vector = transpose * new Vector4(plane.a, plane.b, plane.c, 0f - plane.d);
			return new Plane(vector.x, vector.y, vector.z, 0f - vector.w);
		}

		public static void Transform(List<Plane> src, Matrix4x4 transformation, out Plane[] dst)
		{
			Matrix4x4 transpose = transformation.inverse.transpose;
			dst = new Plane[src.Count];
			for (int i = 0; i < src.Count; i++)
			{
				Plane plane = src[i];
				Vector4 vector = transpose * new Vector4(plane.a, plane.b, plane.c, 0f - plane.d);
				dst[i] = new Plane(vector.x, vector.y, vector.z, 0f - vector.w);
			}
		}

		public static void Transform(Plane[] srcPlanes, Matrix4x4 transformation, out Plane[] dstPlanes)
		{
			Matrix4x4 transpose = transformation.inverse.transpose;
			dstPlanes = new Plane[srcPlanes.Length];
			for (int i = 0; i < srcPlanes.Length; i++)
			{
				Plane plane = srcPlanes[i];
				Vector4 vector = transpose * new Vector4(plane.a, plane.b, plane.c, 0f - plane.d);
				dstPlanes[i] = new Plane(vector.x, vector.y, vector.z, 0f - vector.w);
			}
		}

		public static void Transform(Plane[] srcPlanes, Vector3[] srcTangents, Matrix4x4 transformation, out Plane[] dstPlanes, out Vector3[] dstTangents)
		{
			Matrix4x4 transpose = transformation.inverse.transpose;
			dstPlanes = new Plane[srcPlanes.Length];
			for (int i = 0; i < srcPlanes.Length; i++)
			{
				Plane plane = srcPlanes[i];
				Vector4 vector = transpose * new Vector4(plane.a, plane.b, plane.c, 0f - plane.d);
				dstPlanes[i] = new Plane(vector.x, vector.y, vector.z, 0f - vector.w);
			}
			dstTangents = new Vector3[srcTangents.Length];
			for (int j = 0; j < srcTangents.Length; j++)
			{
				Vector3 vector2 = srcTangents[j];
				Vector4 vector3 = transpose * new Vector4(vector2.x, vector2.y, vector2.z, 1f);
				dstTangents[j] = new Vector3(vector3.x / vector3.w, vector3.y / vector3.w, vector3.z / vector3.w).normalized;
			}
		}

		public static Plane InverseTransform(Plane srcPlane, Matrix4x4 transformation)
		{
			Matrix4x4 transpose = transformation.transpose;
			Vector4 vector = transpose * new Vector4(srcPlane.a, srcPlane.b, srcPlane.c, 0f - srcPlane.d);
			Plane result = new Plane(vector.x, vector.y, vector.z, 0f - vector.w);
			result.Normalize();
			return result;
		}

		public static void InverseTransform(Plane[] srcPlanes, Matrix4x4 transformation, out Plane[] dstPlanes)
		{
			Matrix4x4 transpose = transformation.transpose;
			dstPlanes = new Plane[srcPlanes.Length];
			for (int i = 0; i < srcPlanes.Length; i++)
			{
				Plane plane = srcPlanes[i];
				Vector4 vector = transpose * new Vector4(plane.a, plane.b, plane.c, 0f - plane.d);
				dstPlanes[i] = new Plane(vector.x, vector.y, vector.z, 0f - vector.w);
				dstPlanes[i].Normalize();
			}
		}

		public static void InverseTransform(List<Plane> srcPlanes, Matrix4x4 transformation, out Plane[] dstPlanes)
		{
			Matrix4x4 transpose = transformation.transpose;
			dstPlanes = new Plane[srcPlanes.Count];
			for (int i = 0; i < srcPlanes.Count; i++)
			{
				Plane plane = srcPlanes[i];
				Vector4 vector = transpose * new Vector4(plane.a, plane.b, plane.c, 0f - plane.d);
				dstPlanes[i] = new Plane(vector.x, vector.y, vector.z, 0f - vector.w);
				dstPlanes[i].Normalize();
			}
		}

		public static void InverseTransform(Plane[] srcPlanes, Vector3[] srcTangents, Matrix4x4 transformation, out Plane[] dstPlanes, out Vector3[] dstTangents)
		{
			Matrix4x4 transpose = transformation.transpose;
			dstPlanes = new Plane[srcPlanes.Length];
			for (int i = 0; i < srcPlanes.Length; i++)
			{
				Plane plane = srcPlanes[i];
				Vector4 vector = transpose * new Vector4(plane.a, plane.b, plane.c, 0f - plane.d);
				dstPlanes[i] = new Plane(vector.x, vector.y, vector.z, 0f - vector.w);
			}
			dstTangents = new Vector3[srcTangents.Length];
			for (int j = 0; j < srcTangents.Length; j++)
			{
				Vector3 vector2 = srcTangents[j];
				Vector4 vector3 = transpose * new Vector4(vector2.x, vector2.y, vector2.z, 1f);
				dstTangents[j] = new Vector3(vector3.x / vector3.w, vector3.y / vector3.w, vector3.z / vector3.w).normalized;
			}
		}

		public static Plane Translated(Plane plane, Vector3 translation)
		{
			return new Plane(plane.a, plane.b, plane.c, plane.d + plane.a * translation.x + plane.b * translation.y + plane.c * translation.z);
		}

		public static Plane Translated(Plane plane, float translateX, float translateY, float translateZ)
		{
			return new Plane(plane.a, plane.b, plane.c, plane.d + plane.a * translateX + plane.b * translateY + plane.c * translateZ);
		}

		public Plane Translated(Vector3 translation)
		{
			return new Plane(a, b, c, d + a * translation.x + b * translation.y + c * translation.z);
		}

		public void Translate(Vector3 translation)
		{
			d += a * translation.x + b * translation.y + c * translation.z;
		}

		public static void Translate(List<Plane> src, Vector3 translation, out Plane[] dst)
		{
			dst = new Plane[src.Count];
			for (int i = 0; i < src.Count; i++)
			{
				Plane plane = src[i];
				dst[i] = new Plane(plane.a, plane.b, plane.c, plane.d + plane.a * translation.x + plane.b * translation.y + plane.c * translation.z);
			}
		}

		public static void Translate(Plane[] src, Vector3 translation, out Plane[] dst)
		{
			dst = new Plane[src.Length];
			for (int i = 0; i < src.Length; i++)
			{
				Plane plane = src[i];
				dst[i] = new Plane(plane.a, plane.b, plane.c, plane.d + plane.a * translation.x + plane.b * translation.y + plane.c * translation.z);
			}
		}

		public override int GetHashCode()
		{
			return a.GetHashCode() ^ b.GetHashCode() ^ c.GetHashCode() ^ d.GetHashCode();
		}

		public bool Equals(Plane other)
		{
			if (object.ReferenceEquals(this, other))
			{
				return true;
			}
			if (object.ReferenceEquals(other, null))
			{
				return false;
			}
			return Mathf.Abs(Distance(other.pointOnPlane)) <= 0.00015f && Mathf.Abs(other.Distance(pointOnPlane)) <= 0.00015f && Mathf.Abs(a - other.a) <= 0.0001f && Mathf.Abs(b - other.b) <= 0.0001f && Mathf.Abs(c - other.c) <= 0.0001f;
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (!(obj is Plane))
			{
				return false;
			}
			Plane plane = (Plane)obj;
			if (object.ReferenceEquals(plane, null))
			{
				return false;
			}
			return Mathf.Abs(Distance(plane.pointOnPlane)) <= 0.00015f && Mathf.Abs(plane.Distance(pointOnPlane)) <= 0.00015f && Mathf.Abs(a - plane.a) <= 0.0001f && Mathf.Abs(b - plane.b) <= 0.0001f && Mathf.Abs(c - plane.c) <= 0.0001f;
		}

		public Vector3 Project(Vector3 point)
		{
			float x = point.x;
			float y = point.y;
			float z = point.z;
			float x2 = normal.x;
			float y2 = normal.y;
			float z2 = normal.z;
			float num = (x - x2 * d) * x2;
			float num2 = (y - y2 * d) * y2;
			float num3 = (z - z2 * d) * z2;
			float num4 = num + num2 + num3;
			float x3 = x - num4 * x2;
			float y3 = y - num4 * y2;
			float z3 = z - num4 * z2;
			return new Vector3(x3, y3, z3);
		}

		public static bool operator ==(Plane left, Plane right)
		{
			if (object.ReferenceEquals(left, right))
			{
				return true;
			}
			if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
			{
				return false;
			}
			return Mathf.Abs(left.Distance(right.pointOnPlane)) <= 0.00015f && Mathf.Abs(right.Distance(left.pointOnPlane)) <= 0.00015f && Mathf.Abs(left.a - right.a) <= 0.0001f && Mathf.Abs(left.b - right.b) <= 0.0001f && Mathf.Abs(left.c - right.c) <= 0.0001f;
		}

		public static bool operator !=(Plane left, Plane right)
		{
			if (object.ReferenceEquals(left, right))
			{
				return false;
			}
			if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
			{
				return true;
			}
			return (Mathf.Abs(left.Distance(right.pointOnPlane)) > 0.00015f && Mathf.Abs(right.Distance(left.pointOnPlane)) > 0.00015f && Mathf.Abs(left.a - right.a) > 0.0001f) || Mathf.Abs(left.b - right.b) > 0.0001f || Mathf.Abs(left.c - right.c) > 0.0001f;
		}
	}
}
