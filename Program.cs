using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Point p1 = new Point() { X = 0, Y = 1 };
            //Point p2 = new Point() { X = 1, Y = 0 };
            Point p3 = new Point() { X = 0, Y = -1 };
            Point p4 = new Point() { X = 1, Y = -1 };
            Point p5 = new Point() { X = 2, Y = -1 };
            //Point p6 = new Point() { X = 2, Y = 0 };
            //Point p7 = new Point() { X = 2, Y = 1 };


            //Point p1 = new Point() { X = 0, Y = 1 };
            //Point p2 = new Point() { X = 1, Y = 0 };
            //Point p3 = new Point() { X = 0, Y = -1 };
            //Point p4 = new Point() { X = -1, Y = -1 };
            //Point p5 = new Point() { X = -2, Y = -1 };
            //Point p6 = new Point() { X = -2, Y = 0 };
            //Point p7 = new Point() { X = -2, Y = 1 };
            List<Point> points = new List<Point>();

            //double result = cal(p3, p2, p1);
            //points.Add(p1);
            //points.Add(p2);
            points.Add(p3);
            points.Add(p4);
            points.Add(p5);
            //points.Add(p6);
            //points.Add(p7);
            ClockDirection vDirection = Polygon.CalculateClockDirection(points, false);
            PolygonType type = Polygon.CalculatePolygonType(points, false);
            bool flag = Polygon.IsPolyClockwise(points);

        }
        static double cal(Point p1, Point p2, Point p3)
        {
            // (x2-x1)*(y3-y2)-(y2-y1)*(x3-x2) 
            return (p2.X - p1.X) * (p3.Y - p2.Y) - (p2.Y - p1.Y) * (p3.X - p2.X);
        }
    }

    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
    public class Polygon
    {
        static double cal(Point p1, Point p2, Point p3)
        {
            // (x2-x1)*(y3-y2)-(y2-y1)*(x3-x2) 
            return (p2.X - p1.X) * (p3.Y - p2.Y) - (p2.Y - p1.Y) * (p3.X - p2.X);
        }
        #region CalculateClockDirection
        /// <summary>
        /// 判断多边形是顺时针还是逆时针.
        /// </summary>
        /// <param name="points">所有的点</param>
        /// <param name="isYAxixToDown">true:Y轴向下为正(屏幕坐标系),false:Y轴向上为正(一般的坐标系)</param>
        /// <returns></returns>
        public static ClockDirection CalculateClockDirection(List<Point> points, bool isYAxixToDown)
        {
            int i, j, k;
            int count = 0;
            double z;
            int yTrans = isYAxixToDown ? (-1) : (1);
            if (points == null || points.Count < 3)
            {
                return (0);
            }
            int n = points.Count;
            for (i = 0; i < n; i++)
            {
                j = (i + 1) % n;
                k = (i + 2) % n;
                //z = (points[j].X - points[i].X) * (points[k].Y * yTrans - points[j].Y * yTrans);
                //z -= (points[j].Y * yTrans - points[i].Y * yTrans) * (points[k].X - points[j].X);
                z = cal(points[i], points[j], points[k]);
                if (z < 0)
                {
                    count--;
                }
                else if (z > 0)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                return (ClockDirection.Counterclockwise);
            }
            else if (count < 0)
            {
                return (ClockDirection.Clockwise);
            }
            else
            {
                return (ClockDirection.None);
            }
        }
        #endregion


        public static bool IsPolyClockwise(List<Point> points)
        {
            //沿着多边形的边求曲线积分,若积分为正,则是沿着边界曲线正方向(逆时针),反之为顺时针
            double d = 0;
            int nSize = points.Count;
            //(p2.X - p1.X) * (p3.Y - p2.Y) - (p2.Y - p1.Y) * (p3.X - p2.X);
            for (int i = 0; i < nSize - 1; ++i)
            {
                d += -0.5 * (points[i + 1].Y + points[i].Y) * (points[i + 1].X - points[i].X);
            }

            //这条边不能忘记
            d += -0.5 * (points[0].Y + points[nSize - 1].Y) * (points[0].X - points[nSize - 1].X);

            //小于零为顺时针，大于零为逆时针
            return d < 0.0; ;
        }


        #region CalculatePolygonType
        /// <summary>      
        ///判断多边形是凸多边形还是凹多边形.
        ///假定该多边形是简单的多边形，即没有横穿也没有洞的多边形。
        /// </summary>
        /// <param name="points"></param>
        /// <param name="isYAxixToDown">true:Y轴向下为正(屏幕坐标系),false:Y轴向上为正(一般的坐标系)</param>
        /// <returns></returns>
        public static PolygonType CalculatePolygonType(List<Point> points, bool isYAxixToDown)
        {
            int i, j, k;
            int flag = 0;
            double z;

            if (points == null || points.Count < 3)
            {
                return (0);
            }
            int n = points.Count;
            int yTrans = isYAxixToDown ? (-1) : (1);
            for (i = 0; i < n; i++)
            {
                j = (i + 1) % n;
                k = (i + 2) % n;
                z = (points[j].X - points[i].X) * (points[k].Y * yTrans - points[j].Y * yTrans);
                z -= (points[j].Y * yTrans - points[i].Y * yTrans) * (points[k].X - points[j].X);
                if (z < 0)
                {
                    flag |= 1;
                }
                else if (z > 0)
                {
                    flag |= 2;
                }
                if (flag == 3)
                {
                    return (PolygonType.Concave);
                }
            }
            if (flag != 0)
            {
                return (PolygonType.Convex);
            }
            else
            {
                return (PolygonType.None);
            }
        }
        #endregion
    }
}
