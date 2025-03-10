using UnityEngine;

public class Jammer : MonoBehaviour
{
    public float x;
    public float y;
    public float z;

    private GameObject jammerObject; // 存储生成的 Jammer 实体

    // 在地图上添加 Jammer
    public void AddJammer()
    {
        // 创建一个新的空 GameObject 作为 Jammer 实体
        jammerObject = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // 设置 Jammer 的位置
        jammerObject.transform.position = new Vector3(x, y, z);

        jammerObject.SetActive(true);

        jammerObject.transform.localScale = new Vector3(10, 10, 10);

        // 在新 Jammer 上添加 DetectRange 组件
        DetectRange detectRange = jammerObject.AddComponent<DetectRange>();

        // 确保 detectRange 组件正确挂载
        if (detectRange != null)
        {
            Debug.Log($"DetectRange component added to Jammer at ({x}, {y}, {z})");
        }
        else
        {
            Debug.LogError("Failed to add DetectRange component!");
        }

        Debug.Log($"Jammer added at ({x}, {y}, {z})");
    }

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
}
