using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication2
{
    /// <summary>
    /// 多边形工具,判断多边形是顺时针还是逆时针
    /// </summary>
    public class PolygonUtils
    {
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
                z = (points[j].X - points[i].X) * (points[k].Y * yTrans - points[j].Y * yTrans);
                z -= (points[j].Y * yTrans - points[i].Y * yTrans) * (points[k].X - points[j].X);
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
    }
    /// <summary>
    /// 时钟方向
    /// </summary>
    public enum ClockDirection
    {
        /// <summary>
        /// 无.可能是不可计算的图形，比如多点共线
        /// </summary>
        None,

        /// <summary>
        /// 顺时针方向
        /// </summary>
        Clockwise,

        /// <summary>
        /// 逆时针方向
        /// </summary>
        Counterclockwise
    }
    public enum PolygonType
    {
        /// <summary>
        /// 无.不可计算的多边形(比如多点共线)
        /// </summary>
        None,

        /// <summary>
        /// 凸多边形
        /// </summary>
        Convex,

        /// <summary>
        /// 凹多边形
        /// </summary>
        Concave

    }
}
