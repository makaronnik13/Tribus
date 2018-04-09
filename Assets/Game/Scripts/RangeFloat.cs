using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangeFloat
{
	[SerializeField]
	private float min, max, minBorder, maxBorder;

	public float Min
	{
		get
		{
			return min;
		}
		set
		{
			min = value;
			min = Mathf.Clamp (value, MinBorder, MaxBorder);
		}
	}

	public float Max
	{
		get
		{
			return max;
		}
		set
		{
			max = value;
			max = Mathf.Clamp (value, MinBorder, MaxBorder);
		}
	}

	public float MinBorder
	{
		get
		{
			return minBorder;
		}
		set
		{
			minBorder = Mathf.Clamp (value, -Mathf.Infinity, MaxBorder);
			min = Mathf.Clamp (Min, MinBorder, MaxBorder);
			max = Mathf.Clamp (Max, MinBorder, MaxBorder);
		}
	}

	public float MaxBorder
	{
		get
		{
			return maxBorder;
		}
		set
		{
			maxBorder = Mathf.Clamp (value, MaxBorder, Mathf.Infinity);
			min = Mathf.Clamp (Min, MinBorder, MaxBorder);
			max = Mathf.Clamp (Max, MinBorder, MaxBorder);
		}
	}

	public RangeFloat (float minBorder, float maxBorder)
	{
		this.minBorder = minBorder;
		this.maxBorder = maxBorder;
	}

	public RangeFloat (float minBorder, float maxBorder, float min, float max)
	{
		this.minBorder = minBorder;
		this.maxBorder = maxBorder;
		this.min = min;
		this.max = max;
	}
}

