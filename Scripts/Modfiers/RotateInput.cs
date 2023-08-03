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
    public class RotateInput
        : IModule
    {
        public IModule SourceModule { get; set; }
#pragma warning disable 414
        float XAngle;
        float YAngle;
        float ZAngle;
#pragma warning restore 414
        /// An entry within the 3x3 rotation matrix used for rotating the
        /// input value.
        float m_x1Matrix;

        /// An entry within the 3x3 rotation matrix used for rotating the
        /// input value.
        float m_x2Matrix;

        /// An entry within the 3x3 rotation matrix used for rotating the
        /// input value.
        float m_x3Matrix;


        /// An entry within the 3x3 rotation matrix used for rotating the
        /// input value.
        float m_y1Matrix;

        /// An entry within the 3x3 rotation matrix used for rotating the
        /// input value.
        float m_y2Matrix;

        /// An entry within the 3x3 rotation matrix used for rotating the
        /// input value.
        float m_y3Matrix;

        /// An entry within the 3x3 rotation matrix used for rotating the
        /// input value.
        float m_z1Matrix;

        /// An entry within the 3x3 rotation matrix used for rotating the
        /// input value.
        float m_z2Matrix;

        /// An entry within the 3x3 rotation matrix used for rotating the
        /// input value.
		float m_z3Matrix;

        public RotateInput(IModule sourceModule, float xAngle, float yAngle, float zAngle)
        {
            if (sourceModule == null)
                throw new ArgumentNullException("A source module must be provided.");

            SourceModule = sourceModule;
            SetAngles(xAngle, yAngle, zAngle);
        }

		public void SetAngles(float xAngle, float yAngle, float zAngle)
        {
            XAngle = xAngle;
            YAngle = yAngle;
            ZAngle = zAngle;

            float xCos, yCos, zCos, xSin, ySin, zSin;
            xCos = Mathf.Cos(xAngle);
			yCos = Mathf.Cos(yAngle);
			zCos = Mathf.Cos(zAngle);
			xSin = Mathf.Sin(xAngle);
			ySin = Mathf.Sin(yAngle);
			zSin = Mathf.Sin(zAngle);

            m_x1Matrix = ySin * xSin * zSin + yCos * zCos;
            m_y1Matrix = xCos * zSin;
            m_z1Matrix = ySin * zCos - yCos * xSin * zSin;
            m_x2Matrix = ySin * xSin * zCos - yCos * zSin;
            m_y2Matrix = xCos * zCos;
            m_z2Matrix = -yCos * xSin * zCos - ySin * zSin;
            m_x3Matrix = -ySin * xCos;
            m_y3Matrix = xSin;
            m_z3Matrix = yCos * xCos;
        }

        public float GetValue(float x, float y, float z)
        {
            if (SourceModule == null)
                throw new NullReferenceException("A source module must be provided.");

            var nx = (m_x1Matrix * x) + (m_y1Matrix * y) + (m_z1Matrix * z);
            var ny = (m_x2Matrix * x) + (m_y2Matrix * y) + (m_z2Matrix * z);
            var nz = (m_x3Matrix * x) + (m_y3Matrix * y) + (m_z3Matrix * z);
            return SourceModule.GetValue(nx, ny, nz);
        }
    }
}
