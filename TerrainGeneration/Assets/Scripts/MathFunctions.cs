using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathFunctions
{
    public static float e { get { return _e; } }
    private readonly static float _e = 2.71828f;


    public static List<Vector2Int> DistributeOverPolygon(List<Vector2Int> points, float expectedDensity)
    {
        if (points.Count < 3)
        {
            Debug.LogError("DistributeOverPolygon: No valid polygon was used as input");
            return new List<Vector2Int>();
        }

        RectInt boundingRect = CreateBoundingRect(points);

        int surface = boundingRect.width * boundingRect.height;
        float lambda = expectedDensity * surface;

        List<Vector2Int> distributedPoints = new List<Vector2Int>();

        int k = PoisonDistribution(lambda);

        for (int i = 0; i < k; ++i)
        {
            Vector2Int point = new Vector2Int
            (
                Random.Range(boundingRect.x, boundingRect.x + boundingRect.width)
                , Random.Range(boundingRect.y, boundingRect.y + boundingRect.height)
            );
            distributedPoints.Add(point);
        }
        PolygonCollider2D polygonCollider2D = new PolygonCollider2D();

        polygonCollider2D.points = points.ConvertAll<Vector2>(new System.Converter<Vector2Int, Vector2>(value => { return (Vector2)value; })).ToArray();

        distributedPoints.RemoveAll(value => { return polygonCollider2D.OverlapPoint(value); });

        return distributedPoints;
    }

    public static int PoisonDistribution(float lambda)
    {
        float l = Mathf.Pow(_e, -lambda);
        int k = 0;
        float p = 1;

        do
        {
            ++k;
            float u = Random.Range(0.0f, 1.0f);
            p *= u;
        } while (p > l);
        return k;
    }

    public static RectInt CreateBoundingRect(List<Vector2Int> points)
    {
        RectInt boundingRect = new RectInt(int.MaxValue, int.MaxValue, 0, 0);
        for (int i = 0; i < points.Count; ++i)
        {
            // Calc x min and max
            if (points[i].x < boundingRect.x)
            {
                boundingRect.x = points[i].x;
            }
            else if (points[i].x - boundingRect.x > boundingRect.width)
            {
                boundingRect.width = points[i].x - boundingRect.x;
            }

            // calc y min and max
            if (points[i].y < boundingRect.y)
            {
                boundingRect.y = points[i].y;
            }
            else if (points[i].y - boundingRect.y > boundingRect.height)
            {
                boundingRect.height = points[i].y - boundingRect.y;
            }
        }
        return boundingRect;
    }
}
