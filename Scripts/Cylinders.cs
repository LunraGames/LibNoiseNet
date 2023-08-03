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
    public class Cylinders
        : IModule
    {
        public float Frequency { get; set; }

        public Cylinders()
        {
            Frequency = 1f;
        }

        public float GetValue(float x, float y, float z)
        {
            x *= Frequency;
            z *= Frequency;

			var distFromCenter = Mathf.Sqrt(x * x + z * z);
			var distFromCenter0 = Mathf.Floor(distFromCenter > 0f ? (int)distFromCenter : (int)distFromCenter - 1f);
            var distFromSmallerSphere = distFromCenter - distFromCenter0;
            var distFromLargerSphere = 1f - distFromSmallerSphere;
            var nearestDist = Math.GetSmaller(distFromSmallerSphere, distFromLargerSphere);
            return 1f - (nearestDist * 4f);
        }
    }
}
