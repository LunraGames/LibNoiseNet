// 
// Copyright (c) 2013 Jason Bell
// 
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included 
// in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS 
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
// 

using System;
using UnityEngine;

namespace LibNoise
{
    public class Voronoi : ValueNoiseBasis, IModule
    {
        public float Frequency { get; set; }
        public float Displacement { get; set; }
        public bool DistanceEnabled { get; set; }
        public int Seed { get; set; }

        public Voronoi()
        {
            Frequency = 1f;
            Displacement = 1f;
            Seed = 0;
            DistanceEnabled = false;
        }

        public float GetValue(float x, float y, float z)
        {
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;

            int xInt = (x > 0f ? (int)x : (int)x - 1);
            int yInt = (y > 0f ? (int)y : (int)y - 1);
            int zInt = (z > 0f ? (int)z : (int)z - 1);

			// todo: can this be a constant? is it a max value?
            var minDist = 2147483647.0f;
            var xCandidate = 0f;
            var yCandidate = 0f;
            var zCandidate = 0f;

            // Inside each unit cube, there is a seed point at a random position.  Go
            // through each of the nearby cubes until we find a cube with a seed point
            // that is closest to the specified position.
            for (var zCur = zInt - 2; zCur <= zInt + 2; zCur++)
            {
                for (var yCur = yInt - 2; yCur <= yInt + 2; yCur++)
                {
                    for (var xCur = xInt - 2; xCur <= xInt + 2; xCur++)
                    {

                        // Calculate the position and distance to the seed point inside of
                        // this unit cube.
                        var xPos = xCur + ValueNoise(xCur, yCur, zCur, Seed);
                        var yPos = yCur + ValueNoise(xCur, yCur, zCur, Seed + 1);
                        var zPos = zCur + ValueNoise(xCur, yCur, zCur, Seed + 2);
                        var xDist = xPos - x;
                        var yDist = yPos - y;
                        var zDist = zPos - z;
                        var dist = xDist * xDist + yDist * yDist + zDist * zDist;

                        if (dist < minDist)
                        {
                            // This seed point is closer to any others found so far, so record
                            // this seed point.
                            minDist = dist;
                            xCandidate = xPos;
                            yCandidate = yPos;
                            zCandidate = zPos;
                        }
                    }
                }
            }

            float value;
            if (DistanceEnabled)
            {
                // Determine the distance to the nearest seed point.
                var xDist = xCandidate - x;
                var yDist = yCandidate - y;
                var zDist = zCandidate - z;
                value = (Mathf.Sqrt(xDist * xDist + yDist * yDist + zDist * zDist)) * Math.Sqrt3 - 1f;
            }
            else
            {
                value = 0f;
            }

            int x0 = (xCandidate > 0f ? (int)xCandidate : (int)xCandidate - 1);
            int y0 = (yCandidate > 0f ? (int)yCandidate : (int)yCandidate - 1);
            int z0 = (zCandidate > 0f ? (int)zCandidate : (int)zCandidate - 1);

            // Return the calculated distance with the displacement value applied.
            return value + (Displacement * ValueNoise(x0, y0, z0));
        }
    }
}
