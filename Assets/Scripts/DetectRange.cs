using System.Collections.Generic;
using UnityEngine;
using CustomPrimitiveColliders;
using UnityEditor;
using Voxus.Random;

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
    private List<GameObject> gridIntersects = new List<GameObject>();
    private GameObject LargestPower;
    [SerializeField] public float noisePower;


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

    public void FindLargestPower(GameObject radar) {
        Vector3 radarPos = radar.transform.position;
        Vector3 missilePos = missile.transform.position;
        Ray ray = new Ray(missilePos, radarPos);

        if (gridPlane.Raycast(ray, out float distance)) {
            Vector3 LargestPowerPos = ray.GetPoint(distance);
            LargestPower = Instantiate(gridIntersect, LargestPowerPos, Quaternion.identity);
            LargestPower.transform.SetParent(transform);
            LargestPower.GetComponent<GridIntersect>().SetPower(100);
        }
        //Debug.Log("" + LargestPower.GetComponent<GridIntersect>().GetPower());
    }

    private void SetPower(GameObject gridIntersect, double power) {
        gridIntersect.GetComponent<GridIntersect>().SetPower(power);
    }

    public void CalculatePowerBasedOnLargest(GameObject radar, GameObject jammer) {
        double radarPower = 100;
        double jammerPower = 175;
        noisePower = 0;

        Vector3 radarPos = radar.transform.position;
        Vector3 jammerPos = jammer.transform.position;

        double radarWeight = radarPower / (radarPower + jammerPower + noisePower);
        double jammerWeight = jammerPower / (radarPower + jammerPower + noisePower);

        var randGen1 = new RandomGaussian(1, 0);
        randGen1.SetSeed(Random.Range(0F, 1F));
        var randGen2 = new RandomGaussian(1, 0);
        randGen2.SetSeed(Random.Range(0F, 1F));

        double power = 0;
        foreach (GameObject gameObject in gridIntersects) {
            if (gameObject != null) {
                double d1 = Vector3.Distance(gameObject.transform.position, radarPos);
                Vector3 gOPosition = gameObject.transform.position;
                gOPosition.z = jammerPos.z;
                double d2 = Vector3.Distance(gOPosition, jammerPos);
                power = radarPower * ( 1 / (radius * radius)) * ( 1 / (d1 * d1)) * radarWeight +
                        jammerPower * ( 1 / (radius * radius)) * ( 1 / (d2 * d2)) * jammerWeight + 
                        noisePower * Mathf.Sqrt(Mathf.Pow(randGen1.Get(), 2) + Mathf.Pow(randGen2.Get(), 2));
                SetPower(gameObject, power);
            }
        }
    }
    
    public Vector3 FindNewDirection() {
        GameObject newDirection = null;
        double power = 0;
        foreach (GameObject gameObject in gridIntersects) {
            if (gameObject != null) {
                double currentPower = gameObject.GetComponent<GridIntersect>().GetPower();
                //Debug.Log("" + currentPower);
                if (currentPower > power) {
                    power = currentPower;
                    newDirection = gameObject;
                }
            }
        }

        Vector3 newDir = missile.GetComponent<Missile>().GetDirection();
        if (newDirection != null) {
            newDir = (newDirection.transform.position - missile.transform.position).normalized;
        }
        return newDir;
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
        //CalculatePowerBasedOnLargest();
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

    public void GenerateGrid(GameObject target, int resolution) {
        float distance = Vector3.Distance(transform.position, target.transform.position);
        radius = 0.5F * distance;

        for(int i = 0; i < resolution; i++){
            float current_x = (i * (2.0F * radius/resolution)) - radius;
            for(int j = 0; j < resolution; j++){
                float current_y = (j * (2.0F * radius/resolution)) - radius;
                GameObject obj = Instantiate(gridIntersect, transform.TransformPoint(new Vector3 (current_x * 10.0F, current_y * 10.0F, 10.0F*distance)), transform.rotation);
                gridIntersects.Add(obj);
                obj.transform.SetParent(transform);
                
            }
        }
    }

    public void DestroyGrids() {
        foreach(GameObject obj in gridIntersects) {
            float currentTime = Time.time;
            if (obj != null && currentTime - obj.GetComponent<GridIntersect>().creationTime >= 0.01f) {
                Destroy(obj);
            }
        }
    }
}
