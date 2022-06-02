using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MultiSelect
{
    static Texture2D texture;
    public static Texture2D Texture
    {
        get
        {
            if (texture == null)
            {
                texture = new Texture2D(1, 1);
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
            }
            return texture;
        }
    }

    public static void DrawRectangle(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture);
    }

    public static void DrawRectangleBorder(Rect rect, float thickness, Color color)
    {
        //Top
        DrawRectangle(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        //Bottom
        DrawRectangle(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
        //Left
        DrawRectangle(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        //Right
        DrawRectangle(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
    }

    public static Rect GetRectangle(Vector3 pos1, Vector3 pos2)
    {
        //from bottom right to top left
        pos1.y = Screen.height - pos1.y;
        pos2.y = Screen.height - pos2.y;

        Vector3 bottomRight = Vector3.Max(pos1, pos2);
        Vector3 topLeft = Vector3.Min(pos1, pos2);

        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 pos1, Vector3 pos2)
    {
        Vector3 p1 = camera.ScreenToViewportPoint(pos1);
        Vector3 p2 = camera.ScreenToViewportPoint(pos2);

        Vector3 min = Vector3.Min(p1, p2);
        Vector3 max = Vector3.Max(p1, p2);

        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);

        return bounds;
    }
}
