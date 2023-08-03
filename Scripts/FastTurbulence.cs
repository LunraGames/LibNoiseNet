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

namespace LibNoise
{
    public class FastTurbulence
        : IModule
    {
        public IModule SourceModule { get; set; }

        public float Power { get; set; }

        FastNoise XDistort;
        FastNoise YDistort;
        FastNoise ZDistort;

        public FastTurbulence(IModule sourceModule)
        {
            if (sourceModule == null)
                throw new ArgumentNullException();

            SourceModule = sourceModule;

            XDistort = new FastNoise();
            YDistort = new FastNoise();
            ZDistort = new FastNoise();

            Frequency = 1f;
            Power = 1f;
            Roughness = 3;
            Seed = 0;
        }

        public float Frequency
        {
            get { return XDistort.Frequency; }
            set
            {
                XDistort.Frequency = YDistort.Frequency = ZDistort.Frequency = value;
            }
        }

        public float GetValue(float x, float y, float z)
        {
            if (SourceModule == null)
                throw new NullReferenceException();

            // Get the values from the three noise::module::Perlin noise modules and
            // add each value to each coordinate of the input value.  There are also
            // some offsets added to the coordinates of the input values.  This prevents
            // the distortion modules from returning zero if the (x, y, z) coordinates,
            // when multiplied by the frequency, are near an integer boundary.  This is
            // due to a property of gradient coherent noise, which returns zero at
            // integer boundaries.
            float x0, y0, z0;
            float x1, y1, z1;
            float x2, y2, z2;
            x0 = x + (12414.0f / 65536.0f);
            y0 = y + (65124.0f / 65536.0f);
            z0 = z + (31337.0f / 65536.0f);
            x1 = x + (26519.0f / 65536.0f);
            y1 = y + (18128.0f / 65536.0f);
            z1 = z + (60493.0f / 65536.0f);
            x2 = x + (53820.0f / 65536.0f);
            y2 = y + (11213.0f / 65536.0f);
            z2 = z + (44845.0f / 65536.0f);
            var xDistort = x + (XDistort.GetValue(x0, y0, z0) * Power);
            var yDistort = y + (YDistort.GetValue(x1, y1, z1) * Power);
            var zDistort = z + (ZDistort.GetValue(x2, y2, z2) * Power);

            // Retrieve the output value at the offsetted input value instead of the
            // original input value.
            return SourceModule.GetValue(xDistort, yDistort, zDistort);
        }

        public int Roughness
        {
            get { return XDistort.OctaveCount; }
            set
            {
                XDistort.OctaveCount = YDistort.OctaveCount = ZDistort.OctaveCount = value;
            }
        }

        public int Seed
        {
            get { return XDistort.Seed; }
            set
            {
                XDistort.Seed = value;
                YDistort.Seed = value + 1;
                ZDistort.Seed = value + 2;
            }
        }
    }
}
