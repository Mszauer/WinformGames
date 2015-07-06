using System;
using System.Collections.Generic;

namespace Game {
    public class Easing {
        //All easing types http://easings.net/
        #region Equations
        // These methods are all public to enable reflection in GetCurrentValueCore.

        #region Linear

        /// <summary>
        /// Easing equation function for a simple linear tweening, with no easing.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float Linear(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            return c * t / d + b;
        }

        #endregion

        #region Expo

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseOut(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            return (t == d) ? b + c : c * (-(float)Math.Pow(2f, -10f * t / d) + 1f) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseIn(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            return (t == 0f) ? b : c * (float)Math.Pow(2f, 10f * (t / d - 1f)) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseInOut(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            if (t == 0f)
                return b;

            if (t == d)
                return b + c;

            if ((t /= d / 2f) < 1f)
                return c / 2f * (float)Math.Pow(2f, 10f * (t - 1f)) + b;

            return c / 2f * (-(float)Math.Pow(2f, -10f * --t) + 2f) + b;
        }

        /// <summary>
        /// Easing equation function for an exponential (2^t) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ExpoEaseOutIn(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            if (t < d / 2f)
                return ExpoEaseOut(t * 2f, b, c / 2f, d);

            return ExpoEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #region Circular

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseOut(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            return c * (float)Math.Sqrt(1f - (t = t / d - 1f) * t) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseIn(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            return -c * ((float)Math.Sqrt(1f - (t /= d) * t) - 1f) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseInOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if ((t /= d / 2f) < 1f)
                return -c / 2f * ((float)Math.Sqrt(1f - t * t) - 1f) + b;

            return c / 2f * ((float)Math.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
        }

        /// <summary>
        /// Easing equation function for a circular (sqrt(1-t^2)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CircEaseOutIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if (t < d / 2f)
                return CircEaseOut(t * 2f, b, c / 2f, d);

            return CircEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #region Quad

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return -c * (t /= d) * (t - 2f) + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return c * (t /= d) * t + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseInOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if ((t /= d / 2f) < 1f)
                return c / 2f * t * t + b;

            return -c / 2f * ((--t) * (t - 2f) - 1f) + b;
        }

        /// <summary>
        /// Easing equation function for a quadratic (t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuadEaseOutIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if (t < d / 2f)
                return QuadEaseOut(t * 2f, b, c / 2f, d);

            return QuadEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #region Sine

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseOut(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            return c * (float)Math.Sin(t / d * ((float)Math.PI / 2f)) + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return -c * (float)Math.Cos(t / d * ((float)Math.PI / 2f)) + c + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseInOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if ((t /= d / 2f) < 1f)

                return c / 2f * ((float)Math.Sin((float)Math.PI * t / 2f)) + b;

            return -c / 2f * ((float)Math.Cos((float)Math.PI * --t / 2f) - 2f) + b;
        }

        /// <summary>
        /// Easing equation function for a sinusoidal (sin(t)) easing in/out: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float SineEaseOutIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if (t < d / 2f)
                return (float)SineEaseOut(t * 2f, b, c / 2f, d);

            return (float)SineEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #region Cubic

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return c * ((t = t / d - 1f) * t * t + 1f) + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseIn(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            return c * (t /= d) * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseInOut(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            if ((t /= d / 2f) < 1f)
                return c / 2f * t * t * t + b;

            return c / 2f * ((t -= 2f) * t * t + 2f) + b;
        }

        /// <summary>
        /// Easing equation function for a cubic (t^3) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float CubicEaseOutIn(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            if (t < d / 2)
                return CubicEaseOut(t * 2f, b, c / 2f, d);

            return CubicEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #region Quartic

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseOut(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return c * (t /= d) * t * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseInOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if ((t /= d / 2f) < 1f)
                return c / 2f * t * t * t * t + b;

            return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
        }

        /// <summary>
        /// Easing equation function for a quartic (t^4) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuartEaseOutIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if (t < d / 2f)
                return QuartEaseOut(t * 2f, b, c / 2f, d);

            return QuartEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #region Quintic

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return c * (t /= d) * t * t * t * t + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseInOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if ((t /= d / 2f) < 1f)
                return c / 2f * t * t * t * t * t + b;
            return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
        }

        /// <summary>
        /// Easing equation function for a quintic (t^5) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float QuintEaseOutIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if (t < d / 2f)
                return QuintEaseOut(t * 2f, b, c / 2f, d);
            return QuintEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #region Elastic

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseOut(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            if ((t /= d) == 1f)
                return b + c;

            float p = d * .3f;
            float s = p / 4f;

            return (c * (float)Math.Pow(2f, -10f * t) * (float)Math.Sin((t * d - s) * (2f * (float)Math.PI) / p) + c + b);
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseIn(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            if ((t /= d) == 1f)
                return b + c;

            double p = d * .3f;
            double s = p / 4f;

            return -(c * (float)Math.Pow(2f, 10f * (t -= 1f)) * (float)Math.Sin((t * d - s) * (2f * (float)Math.PI) / p)) + b;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseInOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if ((t /= d / 2f) == 2f)
                return b + c;

            double p = d * (.3f * 1.5f);
            double s = p / 4f;

            if (t < 1)
                return -.5f * (c * (float)Math.Pow(2f, 10f * (t -= 1f)) * (float)Math.Sin((t * d - s) * (2f * (float)Math.PI) / p)) + b;
            return c * (float)Math.Pow(2f, -10f * (t -= 1f)) * (float)Math.Sin((t * d - s) * (2f * (float)Math.PI) / p) * .5f + c + b;
        }

        /// <summary>
        /// Easing equation function for an elastic (exponentially decaying sine wave) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float ElasticEaseOutIn(float t, float b, float c, float d = 1.0f) {
            c = c - b;
            if (t < d / 2f)
                return ElasticEaseOut(t * 2f, b, c / 2f, d);
            return ElasticEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #region Bounce

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseOut(float t, float b, float c, float d) {
            c = c - b;
            if ((t /= d) < (1f / 2.75f))
                return c * (7.5625f * t * t) + b;
            else if (t < (2f / 2.75f))
                return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + .75f) + b;
            else if (t < (2.5f / 2.75f))
                return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + .9375f) + b;
            else
                return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + .984375f) + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return c - BounceEaseOut(d - t, 0f, c, d) + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseInOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if (t < d / 2f)
                return BounceEaseIn(t * 2f, 0f, c, d) * .5f + b;
            else
                return BounceEaseOut(t * 2f - d, 0f, c, d) * .5f + c * .5f + b;
        }

        /// <summary>
        /// Easing equation function for a bounce (exponentially decaying parabolic bounce) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BounceEaseOutIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if (t < d / 2f)
                return BounceEaseOut(t * 2f, b, c / 2f, d);
            return BounceEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #region Back

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out: 
        /// decelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return c * ((t = t / d - 1f) * t * ((1.70158f + 1f) * t + 1.70158f) + 1f) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in: 
        /// accelerating from zero velocity.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            return c * (t /= d) * t * ((1.70158f + 1f) * t - 1.70158f) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing in/out: 
        /// acceleration until halfway, then deceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseInOut(float t, float b, float c, float d=1.0f) {
            c = c - b;
            float s = 1.70158f;
            if ((t /= d / 2f) < 1f)
                return c / 2f * (t * t * (((s *= (1.525f)) + 1f) * t - s)) + b;
            return c / 2f * ((t -= 2f) * t * (((s *= (1.525f)) + 1) * t + s) + 2f) + b;
        }

        /// <summary>
        /// Easing equation function for a back (overshooting cubic easing: (s+1)*t^3 - s*t^2) easing out/in: 
        /// deceleration until halfway, then acceleration.
        /// </summary>
        /// <param name="t">Current time in seconds.</param>
        /// <param name="b">Starting value.</param>
        /// <param name="c">Final value.</param>
        /// <param name="d">Duration of animation.</param>
        /// <returns>The correct value.</returns>
        public static float BackEaseOutIn(float t, float b, float c, float d=1.0f) {
            c = c - b;
            if (t < d / 2f)
                return BackEaseOut(t * 2f, b, c / 2f, d);
            return BackEaseIn((t * 2f) - d, b + c / 2f, c / 2f, d);
        }

        #endregion

        #endregion
    }
}