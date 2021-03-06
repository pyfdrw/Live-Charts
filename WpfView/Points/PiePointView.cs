﻿//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System.Windows.Controls;
using System.Windows.Media.Animation;
using LiveCharts.Charts;

namespace LiveCharts.Wpf.Points
{
    internal class PiePointView : PointView, IPieSlicePointView
    {
        public double Rotation { get; set; }
        public double InnerRadius { get; set; }
        public double Radius { get; set; }
        public double Wedge { get; set; }
        public PieSlice Slice { get; set; }

        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
            if (IsNew)
            {
                Canvas.SetTop(Slice, chart.DrawMargin.Height/2);
                Canvas.SetLeft(Slice, chart.DrawMargin.Width/2);

                Slice.WedgeAngle = 0;
                Slice.RotationAngle = 0;

                if (DataLabel != null)
                {
                    Canvas.SetTop(DataLabel, 0d);
                    Canvas.SetLeft(DataLabel, 0d);
                }
            }

            if (HoverShape != null)
            {
                var hs = (PieSlice) HoverShape;

                Canvas.SetTop(hs, chart.DrawMargin.Height/2);
                Canvas.SetLeft(hs, chart.DrawMargin.Width/2);
                hs.WedgeAngle = Wedge;
                hs.RotationAngle = Rotation;
                hs.InnerRadius = InnerRadius;
                hs.Radius = Radius;
            }

            Canvas.SetTop(Slice, chart.DrawMargin.Height / 2);
            Canvas.SetLeft(Slice, chart.DrawMargin.Width / 2);
            Slice.InnerRadius = InnerRadius;
            Slice.Radius = Radius;


            if (chart.View.DisableAnimations)
            {
                Slice.WedgeAngle = Wedge;
                Slice.RotationAngle = Rotation;

                if (DataLabel != null)
                {
                    DataLabel.UpdateLayout();

                    Canvas.SetTop(DataLabel, 0d);
                    Canvas.SetLeft(DataLabel, 0d);
                }

                return;
            }

            var animSpeed = chart.View.AnimationsSpeed;

            if (DataLabel != null)
            {
                DataLabel.UpdateLayout();

                DataLabel.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(0d, animSpeed));
                DataLabel.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(0d, animSpeed));
            }

            Slice.BeginAnimation(PieSlice.WedgeAngleProperty, new DoubleAnimation(Wedge, animSpeed));
            Slice.BeginAnimation(PieSlice.RotationAngleProperty, new DoubleAnimation(Rotation, animSpeed));
        }

        public override void RemoveFromView(ChartCore chart)
        {
            chart.View.RemoveFromDrawMargin(HoverShape);
            chart.View.RemoveFromDrawMargin(Slice);
            chart.View.RemoveFromDrawMargin(DataLabel);
        }
    }
}
