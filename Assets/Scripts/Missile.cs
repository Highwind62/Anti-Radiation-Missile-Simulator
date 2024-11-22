using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Missile : MonoBehaviour
{
    public float speed = 0;
    // Moving direction of missile, set to (0, 0, -1) as default
    private Vector3 moveDir = new Vector3(0, 0, -1);
    private Transform targetPos;

    public GameObject target;

    public GameObject cube;


    [SerializeField] public int resolution;

    private float radius;

    private float distance;

    private float closest_cube;

    void Update()
    {
      
      distance = Vector3.Distance(transform.position, target.transform.position);
      radius = 0.5F * distance;

      for(int i = 0; i < resolution; i++){
        float current_x = (i * (2.0F * radius/resolution)) - radius;
        for(int j = 0; j < resolution; j++){
          float current_y = (j * (2.0F * radius/resolution)) - radius;
          Instantiate(cube, transform.TransformPoint(new Vector3 (current_x * 10.0F, current_y * 10.0F, -10.0F*distance)), transform.rotation);
        }
      }

      transform.position += moveDir * speed;  
      HitCheck();
    }

    public Vector3 GetDirection() 
    {
      return moveDir;  
    }

    void OnCollisionEnter(Collision collision) 
    {
      Debug.Log("Radar in range");
      string radar = "1S12 Long Track Acquisition Radar";
      if (collision.gameObject.tag == "Radar") 
      {
        Debug.Log("Radar in range");
        targetPos = collision.transform.Find(radar);
        moveDir = targetPos.position - transform.position;
        moveDir.Normalize();
      }
    }

    void HitCheck() 
    {
      if (targetPos != null) 
      {
        // If missile hit radar, exit editor game mode.
        if (Vector3.Distance(targetPos.position, transform.position) < 10)
        {
          Debug.Log("Missile hit radar.");
          EditorApplication.isPlaying = false;
        }
      }
    }
}
