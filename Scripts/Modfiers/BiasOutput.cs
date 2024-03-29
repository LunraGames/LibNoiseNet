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

namespace LibNoise.Modifiers
{
    /// <summary>
    /// Module that biases the output of a source module by adding a given value to the output.
    /// </summary>
    public class BiasOutput
        : IModule
    {
        /// <summary>
        /// The module from which to retrieve noise.
        /// </summary>
        public IModule SourceModule { get; set; }

        /// <summary>
        /// The value to add to the output.
        /// </summary>
        public float Bias { get; set; }

        /// <summary>
        /// Initialises a new instance of the BiasOutput class.
        /// </summary>
        /// <param name="sourceModule">The module from which to retrieve noise.</param>
        /// <param name="bias">The value to add to the output.</param>
        public BiasOutput(IModule sourceModule, float bias)
        {
            if (sourceModule == null)
                throw new ArgumentNullException("A source module must be provided.");

            SourceModule = sourceModule;
            Bias = bias;
        }

        /// <summary>
        /// Returns the biased output of the source module.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public float GetValue(float x, float y, float z)
        {
            if (SourceModule == null)
                throw new NullReferenceException("A source module must be provided.");

            return SourceModule.GetValue(x, y, z) + Bias;
        }
    }
}
