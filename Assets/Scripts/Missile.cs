using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Missile : MonoBehaviour
{
  public GameObject cube1;
  
  public GameObject cube2;

  public GameObject cube3;

  public GameObject target;
  public float speed = 0;
  // Moving direction of missile, set to (0, 0, -1) as default
  private Vector3 moveDir = new Vector3(0, 0, -1);
  private Transform targetPos;

  public float distance1;

  public float distance2;

  public float distance3;

 



  // Start is called before the first frame update
  void Start()
  {


    distance1 = Vector3.Distance(cube1.transform.position, target.transform.position);
    distance2 = Vector3.Distance(cube2.transform.position, target.transform.position);
    distance3 = Vector3.Distance(cube3.transform.position, target.transform.position);

  }

  // Update is called once per frame
  void Update()
  {
    //cube1.transform.position = new Vector3(cube1.transform.position.x, cube1.transform.position.y, target.transform.position.z);
    //cube2.transform.position = new Vector3(cube2.transform.position.x, cube2.transform.position.y, target.transform.position.z);
    //cube3.transform.position = new Vector3(cube3.transform.position.x, cube3.transform.position.y, target.transform.position.z);

    cube1.transform.position = new Vector3(cube1.transform.position.x, cube1.transform.position.y, target.transform.position.z);
    cube2.transform.position = new Vector3( (target.transform.position.z - transform.position.z) * Mathf.Sin((45.0F/180.0F) * Mathf.PI), cube2.transform.position.y, target.transform.position.z);
    cube3.transform.position = new Vector3( (target.transform.position.z - transform.position.z) * Mathf.Sin((-45.0F/180.0F) * Mathf.PI), cube3.transform.position.y, target.transform.position.z);

    distance1 = Vector3.Distance(cube1.transform.position, target.transform.position);
    distance2 = Vector3.Distance(cube2.transform.position, target.transform.position);
    distance3 = Vector3.Distance(cube3.transform.position, target.transform.position);



    if (distance1 < distance2 && distance1 < distance3) {
      transform.position = Vector3.MoveTowards(transform.position, cube1.transform.position, speed);
      

    } else if(distance2 < distance1 && distance2 < distance3){
      transform.position = Vector3.MoveTowards(transform.position, cube2.transform.position, speed);


    } else if(distance3 < distance1 && distance3 < distance2){
      transform.position = Vector3.MoveTowards(transform.position, cube3.transform.position, speed);


    } else {
      transform.position = Vector3.MoveTowards(transform.position, cube1.transform.position, speed);
      

    }





    /*
    transform.position += moveDir * speed;  
    */
    HitCheck();
  }
/*
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
  }*/

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
