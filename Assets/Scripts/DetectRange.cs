using System.Collections.Generic;
using UnityEngine;
using CustomPrimitiveColliders;
using UnityEditor;

public class DetectRange : MonoBehaviour
{
    public GameObject missile;
    private Vector3 moveDir;
    public int gridSize = 4;
    private float radius, length;
    private Vector3 center;
    private List<Vector3> grid;
    private Plane gridPlane;
    public GameObject cone;
    public GameObject gridIntersect;
    private List<GameObject> gridIntersects;
    private GameObject LargestPower;


    private List<Vector3> FindGridIntersect(float radius, int size) {
        List<Vector3> worldPos = new List<Vector3>();
        if (size < 4) { size = 4; }

        float spacing = (2 * radius) / size;  

        for (int row = 0; row <= size; row++) 
        {
            for (int col = 0; col <= size; col++)
            {

                float x = center.x - radius + (col * spacing);
                float y = center.y - radius + (row * spacing);

                float dx = x - center.x;
                float dy = y - center.y;

                if (dx * dx + dy * dy <= radius * radius)
                {
                    worldPos.Add(new Vector3(x, y, center.z));
                }
            }
        }

        return worldPos;
    }

    private void InstantiateIntersect(List<Vector3> grid, GameObject gridIntersect) {
        foreach (Vector3 pos in grid) {
            GameObject obj = Instantiate(gridIntersect, pos, Quaternion.identity);
            obj.transform.SetParent(transform);
        }
    }

    private void FindLargestPower(GameObject radar) {
        Vector3 radarPos = radar.transform.position;
        Vector3 missilePos = missile.transform.position;
        Ray ray = new Ray(missilePos, radarPos);

        if (gridPlane.Raycast(ray, out float distance)) {
            Vector3 LargestPowerPos = ray.GetPoint(distance);
            LargestPower = Instantiate(gridIntersect, LargestPowerPos, Quaternion.identity);
            LargestPower.transform.SetParent(transform);
        }
    }

    private void SetPower(GameObject gridIntersect, double power) {
        gridIntersect.GetComponent<GridIntersect>().SetPower(power);
    }

    public void CalculatePowerBasedOnLargest() {
        double lgpower = LargestPower.GetComponent<GridIntersect>().Pr;
        double power;
        foreach (GameObject gameObject in gridIntersects) {
            double d = Vector3.Distance(gameObject.transform.position, LargestPower.transform.position);
            power = lgpower * ( 1 / (radius * radius)) * ( 1 / (d * d));
            SetPower(gameObject, power);
        }
    }

    public void RadarDetected(GameObject radar)
    {
        moveDir = missile.GetComponent<Missile>().GetDirection();
        center = FindConeBaseCenter();

        radius = cone.GetComponent<ConeCollider>().GetRadius();
        length = cone.GetComponent<ConeCollider>().GetLength();
        float ratio = Vector3.Distance(center, transform.position) / length;
        radius = radius * ratio;

        grid = FindGridIntersect(radius, gridSize);
        InstantiateIntersect(grid, gridIntersect);
        FindPlane();
        FindLargestPower(radar);
        CalculatePowerBasedOnLargest();
        EditorApplication.isPaused = false;
    }

    private Vector3 FindConeBaseCenter() {
        Vector3 center = Vector3.zero;
        Ray ray = new Ray(transform.position + 1 * moveDir, moveDir);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            center = hit.point;
            Debug.Log("Center: " + center);
        }

        return center;
    }

    private void FindPlane() {
        Vector3 p1 = grid[0];
        Vector3 p2 = grid[1];
        Vector3 p3 = grid[2];

        Vector3 v1 = p2 - p1;
        Vector3 v2 = p3 - p1;
        
        Vector3 normal = Vector3.Cross(v1, v2).normalized;

        gridPlane = new Plane(normal, p1);
    }
}
