using UnityEngine;
using System.Collections;
using System;

public class DiamondSqare
{
	public static float[][] DiamondSquareGrid(int size, int seed = 0, float rMin = 0, float rMax = 255, float noise = 0.0f)
	{
		size = (int)Mathf.Pow (2, size)+1;
		// Fail if grid size is not of the form (2 ^ n) - 1 or if min/max values are invalid
		int s = size - 1;
		if (!pow2(s) || rMin >= rMax)
			return null;

		float modNoise = 0.0f;

		// init the grid
		float[][] grid = new float[size][];
		for (int i = 0; i < size; i++)
			grid[i] = new float[size];

		// Seed the first four corners
		System.Random rand = (seed == 0 ? new System.Random() : new System.Random(seed));
		grid[0][0] = RandRange(rand, rMin, rMax);
			grid[s][0] = RandRange(rand, rMin, rMax);
			grid[0][s] = RandRange(rand, rMin, rMax);
			grid[s][s] = RandRange(rand, rMin, rMax);

		/*
			 * Use temporary named variables to simplify equations
			 * 
			 * s0 . d0. s1
			 *  . . . . . 
			 * d1 . cn. d2
			 *  . . . . . 
			 * s2 . d3. s3
			 * 
			 * */
		float s0, s1, s2, s3, d0, d1, d2, d3, cn;

		for (int i = s; i > 1; i /= 2)
		{
			// reduce the random range at each step
			modNoise = (rMax - rMin) * noise * ((float)i / s);

			// diamonds
			for (int y = 0; y < s; y += i)
			{
				for (int x = 0; x < s; x += i)
				{
					s0 = grid[x][y];
					s1 = grid[x + i][y];
					s2 = grid[x][y + i];
					s3 = grid[x + i][y + i];

					// cn
					grid[x + (i / 2)][y + (i / 2)] = ((s0 + s1 + s2 + s3) / 4.0f)
						+ RandRange(rand, -modNoise, modNoise);
				}
			}

			// squares
			for (int y = 0; y < s; y += i)
			{
				for (int x = 0; x < s; x += i)
				{
					s0 = grid[x][y];
					s1 = grid[x + i][y];
					s2 = grid[x][y + i];
					s3 = grid[x + i][y + i];
					cn = grid[x + (i / 2)][y + (i / 2)];

					d0 = y <= 0 ? (s0 + s1 + cn) / 3.0f : (s0 + s1 + cn + grid[x + (i / 2)][y - (i / 2)]) / 4.0f;
					d1 = x <= 0 ? (s0 + cn + s2) / 3.0f : (s0 + cn + s2 + grid[x - (i / 2)][y + (i / 2)]) / 4.0f;
					d2 = x >= s - i ? (s1 + cn + s3) / 3.0f :
						(s1 + cn + s3 + grid[x + i + (i / 2)][y + (i / 2)]) / 4.0f;
					d3 = y >= s - i ? (cn + s2 + s3) / 3.0f :
						(cn + s2 + s3 + grid[x + (i / 2)][y + i + (i / 2)]) / 4.0f;

					grid[x + (i / 2)][y] = d0 + RandRange(rand, -modNoise, modNoise);
					grid[x][y + (i / 2)] = d1 + RandRange(rand, -modNoise, modNoise);
					grid[x + i][y + (i / 2)] = d2 + RandRange(rand, -modNoise, modNoise);
					grid[x + (i / 2)][y + i] = d3 + RandRange(rand, -modNoise, modNoise);
				}
			}
		}


		float min = 900;
		float max = -900;
		for (int y = 0; y < s; y++) 
		{
			for (int x = 0; x < s; x++) 
			{
				if(grid[x][y]>max)
				{
					max = grid [x] [y];
				}
				if(grid[x][y]<min)
				{
					min = grid [x] [y];
				}
			}
		}

		for (int y = 0; y < s; y++) 
		{
			for (int x = 0; x < s; x++) 
			{
				grid [x] [y] = Remap (grid [x] [y], min, max, 0, 1);
			}
		}

		return grid;
	}

	private static int RandRange(System.Random r, int rMin, int rMax)
	{
		return rMin + r.Next() * (rMax - rMin);
	}

	private static double RandRange(System.Random r, double rMin, double rMax)
	{
		return rMin + r.NextDouble() * (rMax - rMin);
	}

	private static float RandRange(System.Random r, float rMin, float rMax)
	{
		return rMin + (float)r.NextDouble() * (rMax - rMin);
	}

	// Returns true if a is a power of 2, else false
	private static bool pow2(int a)
	{
		return (a & (a - 1)) == 0;
	}

	private static float Remap(float value, float low1, float high1,float low2, float high2)
	{
		return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
	}
}
	