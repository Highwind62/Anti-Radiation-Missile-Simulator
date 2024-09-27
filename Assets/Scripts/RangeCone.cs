using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomPrimitiveColliders;


public class RangeCone : MonoBehaviour
{
    public int gridSize = 4;
    private float radius, length;
    private List<Vector3> grid;
    public GameObject cone;
    public GameObject gridIntersect;

    private void Awake() {
        radius = cone.GetComponent<ConeCollider>().GetRadius();
        length = cone.GetComponent<ConeCollider>().GetLength();

        grid = findGridIntersect(radius, length, gridSize);
        InstantiateIntersect(grid, gridIntersect);
    }

    private List<Vector3> findGridIntersect(float radius, float length, int size) {

        List<Vector3> list = new List<Vector3>();
        Vector3 center = transform.position;

        if (size < 4) { size = 4; }

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                // Calculate the angle for this point in the grid
                float angle = (float)(Math.PI * 2 * (row * size + col) / (size * size));
                
                // Calculate the x and y position based on the angle and radius
                float x = center.x + radius * (float)Math.Cos(angle);
                float y = center.y + radius * (float)Math.Sin(angle);

                // Store the position
                list.Add(new Vector3(x, y, length));
            }
        }

        // Calculate the spacing between grid points
        float spacing = (2 * radius) / (size - 1);

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                // Calculate the x and y position for each grid point
                float x = center.x - radius + (col * spacing);
                float y = center.y - radius + (row * spacing);
                
                // Check if the point is within the circle
                if (Math.Sqrt(x * x + y * y) <= radius)
                {
                    list.Add(new Vector3(x, y, length));
                }
            }
        }

        return list;
    }

    private void InstantiateIntersect(List<Vector3> grid, GameObject gridIntersect) {
        foreach (Vector3 pos in grid) {
            Instantiate(gridIntersect, pos, Quaternion.identity, transform);
        }
    }
}
