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
    public class RidgedMultifractal
        : GradientNoiseBasis, IModule
    {
        public float Frequency { get; set; }
        public NoiseQuality NoiseQuality { get; set; }
        public int Seed { get; set; }
        int mOctaveCount;
        float mLacunarity;

        const int MaxOctaves = 30;

        float[] SpectralWeights = new float[MaxOctaves];

        public RidgedMultifractal()
        {
            Frequency = 1f;
            Lacunarity = 2f;
            OctaveCount = 6;
            NoiseQuality = NoiseQuality.Standard;
            Seed = 0;
        }

        public float GetValue(float x, float y, float z)
        {
            x *= Frequency;
            y *= Frequency;
            z *= Frequency;

            var signal = 0f;
            var value = 0f;
            var weight = 1f;

            // These parameters should be user-defined; they may be exposed in a
            // future version of libnoise.
            var offset = 1f;
            var gain = 2f;

            for (var currentOctave = 0; currentOctave < OctaveCount; currentOctave++)
            {
				long seed = (Seed + currentOctave) & 0x7fffffff;
                signal = GradientCoherentNoise(x, y, z, (int)seed, NoiseQuality);

                // Make the ridges.
                signal = System.Math.Abs(signal);
                signal = offset - signal;

                // Square the signal to increase the sharpness of the ridges.
                signal *= signal;

                // The weighting from the previous octave is applied to the signal.
                // Larger values have higher weights, producing sharp points along the
                // ridges.
                signal *= weight;

                // Weight successive contributions by the previous signal.
                weight = signal * gain;
				// todo: isn't this just a clamp?
                if (weight > 1f) weight = 1f;
                if (weight < 0f) weight = 0f;

                // Add the signal to the output value.
                value += (signal * SpectralWeights[currentOctave]);

                // Go to the next octave.
                x *= Lacunarity;
                y *= Lacunarity;
                z *= Lacunarity;
            }

            return (value * 1.25f) - 1f;
        }

        public float Lacunarity
        {
            get { return mLacunarity; }
            set
            {
                mLacunarity = value;
                CalculateSpectralWeights();
            }
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

        void CalculateSpectralWeights()
        {
            var h = 1f;

            var frequency = 1f;
            for (var i = 0; i < MaxOctaves; i++)
            {
                // Compute weight for each frequency.
                SpectralWeights[i] = Mathf.Pow(frequency, -h);
                frequency *= mLacunarity;
            }
        }
    }
}
