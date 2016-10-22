﻿// 
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

namespace LibNoise
{
    public class FastNoise
        : FastNoiseBasis, IModule
    {
        public float Frequency { get; set; }
        public float Persistence { get; set; }
        public NoiseQuality NoiseQuality { get; set; }
        int mOctaveCount;
        public float Lacunarity { get; set; }

        const int MaxOctaves = 30;

        public FastNoise()
            : this(0)
        {

        }

        public FastNoise(int seed)
            : base(seed)
        {
            Frequency = 1f;
            Lacunarity = 2f;
            OctaveCount = 6;
            Persistence = 0.5f;
            NoiseQuality = NoiseQuality.Standard;
        }

        public float GetValue(float x, float y, float z)
        {
            var value = 0f;
			var signal = 0f;
			var curPersistence = 1f;
            long seed;

            x *= Frequency;
            y *= Frequency;
            z *= Frequency;

            for (var currentOctave = 0; currentOctave < OctaveCount; currentOctave++)
            {
                seed = (Seed + currentOctave) & 0xffffffff;
                signal = GradientCoherentNoise(x, y, z, (int)seed, NoiseQuality);
                value += signal * curPersistence;

                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
                curPersistence *= Persistence;
            }

            return value;
        }

        public int OctaveCount
        {
            get { return mOctaveCount; }
            set
            {
                if (value < 1 || value > MaxOctaves)
                    throw new ArgumentException("Octave count must be greater than zero and less than " + MaxOctaves);

                mOctaveCount = value;
            }
        }
    }
}
