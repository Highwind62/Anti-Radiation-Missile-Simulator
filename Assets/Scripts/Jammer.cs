using UnityEngine;

public class Jammer : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public double power;

    private GameObject jammerObject;

    // 移除 Jammer
    public void RemoveJammer()
    {
        if (jammerObject != null)
        {
            Destroy(jammerObject); // 删除 Jammer 物体
            jammerObject = null;
            Debug.Log("Jammer removed.");
        }
        else
        {
            Debug.LogWarning("Jammer object is already null.");
        }
    }

    public void Initialize()
    {
        transform.position = new Vector3(x, y, z);

        DetectRange detectRange = gameObject.AddComponent<DetectRange>();
        if (detectRange != null)
        {
            Debug.Log($"[Jammer] DetectRange component added at ({x}, {y}, {z})");
        }
        else
        {
            Debug.LogError("[Jammer] Failed to add DetectRange component!");
        }
    }


    public double GetJammerPower() {
        return power;
    }
}
