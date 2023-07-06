using System;
using UnityEngine;

public sealed class GPSEncoder
{

    /////////////////////////////////////////////////
    //////-------------公共API--------------//////
    /////////////////////////////////////////////////

    /// <summary>
    /// 将UCS（X,Y,Z）坐标转换为GPS（Lat,Lon）坐标
    /// </summary>
    /// <returns>
    /// 返回包含纬度和经度的Vector2
    /// </returns>
    /// <param name='position'>
    /// (X,Y,Z) 位置参数
    /// </param>
    public static DoubleVector2 USCToGPS(DoubleVector3 position)
    {
        return GetInstance().ConvertUCStoGPS(position);
    }

    /// <summary>
    /// 将GPS（Lat,Lon）坐标转换为UCS（X,Y,Z）坐标
    /// </summary>
    /// <returns>
    /// 返回包含（X、Y、Z）的Vector3
    /// </returns>
    /// <param name='gps'>
    /// (Lat, Lon) 转 Vector2
    /// </param>
    public static DoubleVector3 GPSToUCS(DoubleVector2 gps)
    {
        return GetInstance().ConvertGPStoUCS(gps);
    }

    /// <summary>
    /// 将GPS（Lat，Lon）坐标转换为UCS（X，Y，Z）坐标
    /// </summary>
    /// <returns>
    /// 返回包含（X、Y、Z）的Vector3
    /// </returns>
    public static DoubleVector3 GPSToUCS(double latitude, double longitude)
    {
        return GetInstance().ConvertGPStoUCS(new DoubleVector2(latitude, longitude));
    }

    /// <summary>
    /// 更改相对GPS偏移量(Lat、Lon)、默认（0,0）,
    /// 用于在UCS坐标系中将一个局部区域带到（0,0,0）
    /// </summary>
    /// <param name='localOrigin'>
    /// 耐受点
    /// </param>
    public static void SetLocalOrigin(DoubleVector2 localOrigin)
    {
        GetInstance()._localOrigin = localOrigin;
    }

    /////////////////////////////////////////////////
    //////---------实例成员------------//////
    /////////////////////////////////////////////////

    #region 单例
    private static GPSEncoder _singleton;

    private GPSEncoder()
    {

    }

    private static GPSEncoder GetInstance()
    {
        if (_singleton == null)
        {
            _singleton = new GPSEncoder();
        }
        return _singleton;
    }
    #endregion

    #region 实例变量
    private DoubleVector2 _localOrigin = DoubleVector2.zero;
    private double _LatOrigin { get { return _localOrigin.x; } }
    private double _LonOrigin { get { return _localOrigin.y; } }

    private double metersPerLat;
    private double metersPerLon;
    #endregion

    #region 实例函数
    /// <summary>
    /// 计算度长度
    /// </summary>
    /// <param name="lat"></param>
    private void FindMetersPerLat(double lat)
    {
        // Set up "Constants"
        double m1 = 111132.92d;    // 纬度计算术语1
        double m2 = -559.82d;        // 纬度计算术语2
        double m3 = 1.175d;      // 纬度计算术语3
        double m4 = -0.0023d;        // 纬度计算术语4
        double p1 = 111412.84d;    // 经度计算术语1
        double p2 = -93.5d;      // 经度度计算术语2
        double p3 = 0.118d;      // 经度度计算术语3

        lat *= Mathf.Deg2Rad;

        // 计算一个经纬度的长度，以米为单位
        metersPerLat = m1 + (m2 * Convert.ToDouble(Mathf.Cos(2 * (float)lat))) + (m3 * Convert.ToDouble(Mathf.Cos(4 * (float)lat))) + (m4 * Convert.ToDouble(Mathf.Cos(6 * (float)lat)));
        metersPerLon = (p1 * Convert.ToDouble(Mathf.Cos((float)lat))) + (p2 * Convert.ToDouble(Mathf.Cos(3 * (float)lat))) + (p3 * Convert.ToDouble(Mathf.Cos(5 * (float)lat)));
    }

    /// <summary>
    /// 经纬度转世界坐标
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private DoubleVector3 ConvertGPStoUCS(DoubleVector2 gps)
    {
        FindMetersPerLat(_LatOrigin);
        double zPosition = metersPerLat * (gps.x - _LatOrigin); //计算当前纬度
        double xPosition = metersPerLon * (gps.y - _LonOrigin); //计算当前经度
        return new DoubleVector3(xPosition, 0, zPosition);
    }

    /// <summary>
    /// 世界坐标转经纬度
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    private DoubleVector2 ConvertUCStoGPS(DoubleVector3 position)
    {
        FindMetersPerLat(_LatOrigin);
        DoubleVector2 geoLocation = new DoubleVector2(0, 0);
        geoLocation.x = (_LatOrigin + (position.z) / metersPerLat); //计算当前纬度
        geoLocation.y = (_LonOrigin + (position.x) / metersPerLon); //计算当前经度
        return geoLocation;
    }
    #endregion
}

public struct DoubleVector3
{
    public double x;
    public double y;
    public double z;

    private static readonly DoubleVector3 zeroVector = new DoubleVector3(0d, 0d, 0d);
    public static DoubleVector3 zero => zeroVector;

    public DoubleVector3(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return string.Format("({0},{1},{2})", x, y, z);
    }
}

public struct DoubleVector2
{
    public double x;
    public double y;

    private static readonly DoubleVector2 zeroVector = new DoubleVector2(0d, 0d);
    public static DoubleVector2 zero => zeroVector;

    public DoubleVector2(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }
}

