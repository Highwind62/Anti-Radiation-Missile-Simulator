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

  // Start is called before the first frame update
  void Start()
  {
        
  }

  // Update is called once per frame
  void Update()
  {
    transform.position += moveDir * speed;  
    HitCheck();
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
