// Copyright (c) 2021 Emil Forslund
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System;
using SFML.System;

namespace breakout {
    
    /// <summary>
    /// Lightweight collision/intersection library for SFML. Not overly
    /// optimized, but suitable for simple 2D games. This class also adds some
    /// utility functions as extensions to the Vector2f-struct in SFML, like
    /// Length(), Normalized(), etc.
    /// </summary><remarks>
    /// Copyright (c) 2021 Emil Forslund. All rights reserved.
    /// Licensed under the MIT-License.
    /// </remarks>
    /// 
    public static class Collision {

        /// <summary>
        /// Returns the dot-product (scalar product/inner product) between this
        /// and another vector. The dot-product represents the cosine of the
        /// angle between two vectors, multiplied with the product of their
        /// lengths. It can be used to efficiently compare the direction of two
        /// vectors without using trigonometry.
        /// </summary>
        /// <param name="a">the first vector</param>
        /// <param name="b">the second vector</param>
        /// <returns>the dot-product</returns>
        public static float Dot(this Vector2f a, Vector2f b) {
            return a.X * b.X + a.Y * b.Y;
        }
        
        /// <summary>
        /// Returns the length of this vector.
        /// </summary>
        /// <param name="v">the vector to compute the length of</param>
        /// <returns>the length</returns>
        public static float Length(this Vector2f v) {
            return MathF.Sqrt(v.Dot(v));
        }
        
        /// <summary>
        /// Returns a new vector with the same direction as this but with a
        /// length = 1.
        /// </summary>
        /// <param name="v">the vector to normalize</param>
        /// <returns>the normalized vector</returns>
        public static Vector2f Normalized(this Vector2f v) {
            return v / v.Length();
        }
        
        /// <summary>
        /// Returns a new vector that is rotated 90 degrees counter-clockwise.
        /// </summary>
        /// <param name="v">the vector to rotate</param>
        /// <returns>the rotated vector</returns>
        public static Vector2f Orthogonal(this Vector2f v) {
            return new Vector2f(v.Y, -v.X);
        }
        
        /// <summary>
        /// Tests for intersection between a point and a rectangle of a
        /// particular size.
        /// </summary>
        /// <param name="p">the point to test for intersection</param>
        /// <param name="rectCenter">the center point of the rectangle</param>
        /// <param name="rectSize">the size (width and height) of the rectangle</param>
        /// <returns>true if there is an intersection, otherwise false</returns>
        public static bool PointRectangle(Vector2f p, Vector2f rectCenter, Vector2f rectSize) {
            var pInRect = p - rectCenter;
            return MathF.Abs(pInRect.X) < 0.5f * rectSize.X 
                && MathF.Abs(pInRect.Y) < 0.5f * rectSize.Y;
        }
        
        /// <summary>
        /// Test if a circle intersects a rectangle, and if so, compute the
        /// direction and the offset for the circle to move away in to resolve
        /// the intersection.
        /// </summary>
        /// <param name="circleCenter">center of the circle</param>
        /// <param name="circleRadius">radius of the circle</param>
        /// <param name="rectCenter">center of the rectangle</param>
        /// <param name="rectSize">width and height of the rectangle</param>
        /// <param name="hit">the direction and offset to move away in</param>
        /// <returns>true if they intersect, otherwise false</returns>
        public static bool CircleRectangle(
                Vector2f circleCenter, float circleRadius,
                Vector2f rectCenter, Vector2f rectSize,
                out Vector2f hit) {

            // Early-out if the circle is too far away for them to intersect
            if (!PointRectangle(circleCenter, rectCenter, 
                    rectSize + new Vector2f(2, 2) * circleRadius)) {
                
                hit = new Vector2f(0f, 0f);
                return false;
            }
            
            // If circle center is inside the rect, move it out in the direction
            // that minimizes the overlap.
            if (PointRectangle(circleCenter, rectCenter, rectSize)) {
                var d = circleCenter - rectCenter; // rectCenter to circleCenter

                rectSize *= .5f;
                var hOverlap = rectSize.X - MathF.Abs(d.X);
                var vOverlap = rectSize.Y - MathF.Abs(d.Y);

                // If hit horizontally (from positive or negative X)
                hit = hOverlap < vOverlap 
                    ? new Vector2f(MathF.Sign(d.X) * hOverlap, 0.0f) 
                    : new Vector2f(0.0f, MathF.Sign(d.Y) * vOverlap);
                
                return true;
            }
            
            // Circle center is outside the rectangle, but they could still
            // intersect. Check all the sides of the rectangle using line/circle
            // testing.
            rectSize *= .5f;
            var t0 = rectCenter + new Vector2f(rectSize.X, rectSize.Y);
            var t1 = rectCenter + new Vector2f(-rectSize.X, rectSize.Y);
            var t2 = rectCenter + new Vector2f(-rectSize.X, -rectSize.Y);
            var t3 = rectCenter + new Vector2f(rectSize.X, -rectSize.Y);
            
            if (CircleLine(circleCenter, circleRadius, t0, t1, out hit)
            ||  CircleLine(circleCenter, circleRadius, t1, t2, out hit)
            ||  CircleLine(circleCenter, circleRadius, t2, t3, out hit)
            ||  CircleLine(circleCenter, circleRadius, t3, t0, out hit)) {
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Test if a circle intersects a line, and if so, compute the direction
        /// and the offset for the circle to move away in to resolve the
        /// intersection.
        /// </summary>
        /// <param name="circleCenter">the center of the circle</param>
        /// <param name="circleRadius">the radius of the circle</param>
        /// <param name="lineStart">point where the line starts</param>
        /// <param name="lineEnd">point where the line stops</param>
        /// <param name="hit">the direction and offset to move away in</param>
        /// <returns>true if they intersect, otherwise false</returns>
        public static bool CircleLine(Vector2f circleCenter, float circleRadius, Vector2f lineStart, Vector2f lineEnd, out Vector2f hit) {

            // If circle contains the first point
            var l1ToC = circleCenter - lineStart;
            var l1ToCLength = l1ToC.Length();
            if (l1ToCLength < circleRadius) {
                hit = l1ToC * (circleRadius / l1ToCLength - 1.0f);
                return true;
            }
            
            // If circle contains the second point
            var l2ToC = circleCenter - lineEnd;
            var l2ToCLength = l2ToC.Length();
            if (l2ToCLength < circleRadius) {
                hit = l2ToC * (circleRadius / l2ToCLength - 1.0f);
                return true;
            }
            
            // If circle is either behind start point or beyond end point
            var tangent = (lineEnd - lineStart).Normalized();
            if (MathF.Sign(l1ToC.Dot(tangent)) ==
                MathF.Sign(l2ToC.Dot(tangent))) {
                hit = new Vector2f(0f, 0f);
                return false;
            }

            // If distance between circle center and line is more than radius
            var normal = tangent.Orthogonal();
            var distance = l1ToC.Dot(normal); // Distance to line from center
            if (MathF.Abs(distance) >= circleRadius) { 
                hit = new Vector2f(0f, 0f);
                return false; // c is too far away from line
            }

            // Set collision hit to nearest point on line
            hit = normal * (MathF.Sign(distance) * (circleRadius - MathF.Abs(distance)));
            return true;
        }
    }
}