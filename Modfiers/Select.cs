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

namespace LibNoise.Modifiers
{
    public class Select
        : Math, IModule
    {
        public IModule ControlModule { get; set; }
        public IModule SourceModule1 { get; set; }
        public IModule SourceModule2 { get; set; }

        float mEdgeFalloff;
        public float UpperBound { get; private set; }
        public float LowerBound { get; private set; }

        public Select(IModule control, IModule source1, IModule source2)
        {
            if (control == null || source1 == null || source2 == null)
                throw new ArgumentNullException("Control and source modules must be provided.");

            ControlModule = control;
            SourceModule1 = source1;
            SourceModule2 = source2;

            EdgeFalloff = 0f;
            LowerBound = -1f;
            UpperBound = 1f;
        }

        public float GetValue(float x, float y, float z)
        {
            if (ControlModule == null || SourceModule1 == null || SourceModule2 == null)
                throw new NullReferenceException("Control and source modules must be provided.");

            var controlValue = ControlModule.GetValue(x, y, z);
            float alpha;

            if (EdgeFalloff > 0.0)
            {
                if (controlValue < (LowerBound - EdgeFalloff))
                {
                    // The output value from the control module is below the selector
                    // threshold; return the output value from the first source module.
                    return SourceModule1.GetValue(x, y, z);
                }
                
				if (controlValue < (LowerBound + EdgeFalloff))
                {
                    // The output value from the control module is near the lower end of the
                    // selector threshold and within the smooth curve. Interpolate between
                    // the output values from the first and second source modules.
                    var lowerCurve = (LowerBound - EdgeFalloff);
                    var upperCurve = (LowerBound + EdgeFalloff);
                    alpha = SCurve3((controlValue - lowerCurve) / (upperCurve - lowerCurve));
                    return LinearInterpolate(SourceModule1.GetValue(x, y, z), SourceModule2.GetValue(x, y, z), alpha);
                }
                
				if (controlValue < (UpperBound - EdgeFalloff))
                {
                    // The output value from the control module is within the selector
                    // threshold; return the output value from the second source module.
                    return SourceModule2.GetValue(x, y, z);
                }
                
				if (controlValue < (UpperBound + EdgeFalloff))
                {
                    // The output value from the control module is near the upper end of the
                    // selector threshold and within the smooth curve. Interpolate between
                    // the output values from the first and second source modules.
                    var lowerCurve = (UpperBound - EdgeFalloff);
                    var upperCurve = (UpperBound + EdgeFalloff);
                    alpha = SCurve3((controlValue - lowerCurve) / (upperCurve - lowerCurve));
                    return LinearInterpolate(SourceModule2.GetValue(x, y, z), SourceModule1.GetValue(x, y, z), alpha);
                }
                
                // Output value from the control module is above the selector threshold;
                // return the output value from the first source module.
                return SourceModule1.GetValue(x, y, z);
            }
            
            if (controlValue < LowerBound || controlValue > UpperBound) return SourceModule1.GetValue(x, y, z);

			return SourceModule2.GetValue(x, y, z);
        }

        public void SetBounds(float lower, float upper)
        {
            if (lower > upper)
                throw new ArgumentException("The lower bounds must be lower than the upper bounds.");

            LowerBound = lower;
            UpperBound = upper;

            // Make sure that the edge falloff curves do not overlap.
            EdgeFalloff = mEdgeFalloff;
        }

        public float EdgeFalloff
        {
            get { return mEdgeFalloff; }
            set
            {
                var boundSize = UpperBound - LowerBound;
                mEdgeFalloff = (value > boundSize / 2f) ? boundSize / 2f : value;
            }
        }
    }
}
