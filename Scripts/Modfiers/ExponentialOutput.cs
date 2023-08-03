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

namespace LibNoise.Modifiers
{
    public class ExponentialOutput
        : IModule
    {
        public IModule SourceModule { get; set; }

        public float Exponent { get; set; }

        public ExponentialOutput(IModule sourceModule, float exponent)
        {
            if (sourceModule == null)
                throw new ArgumentNullException("A source module must be provided.");

            SourceModule = sourceModule;
            Exponent = exponent;
        }

        public float GetValue(float x, float y, float z)
        {
            if (SourceModule == null)
                throw new NullReferenceException("A source module must be provided.");

            return (Mathf.Pow(Mathf.Abs((SourceModule.GetValue(x, y, z) + 1f) / 2f), Exponent) * 2f - 1f);
        }
    }
}
