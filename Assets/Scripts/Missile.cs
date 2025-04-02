using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

public class Missile : MonoBehaviour
{
    public float speed = 0;

    //2.16
    public float acceleration = 0;
    // Moving direction of missile, set to (0, 0, -1) as default
    private Vector3 moveDir = new Vector3(0, 0, -1);

    //2.16: Store initial position
    private Vector3 startPosition;
    //2.16: Control Missile move or not
    private bool isLaunched = false;

    private Transform targetPos;

    public GameObject target;

    public GameObject jammers;
    private List<GameObject> jmInCone;

    public GameObject cube;

    public GameObject DetectRange;
    public DetectRange detectRange;
    //  Reference to `UIController` script
    public UIController uiControllerScript;


    [SerializeField] public int resolution;

    private float radius;

    private float distance;

    private float closest_cube;

    void Start() {
      detectRange = DetectRange.GetComponent<DetectRange>();
      startPosition = transform.position;
      isLaunched = false; //Ensure when start, Missile dones't move
      jmInCone = detectRange.FindJammersInCone(jammers);

    }
    void FixedUpdate()
    {
        //2.16 Stop the missile from moving until launched
        if (!isLaunched) return;

        //2.16 Calculate radar signals and determine missile movement direction
        detectRange.GenerateGrid(target, resolution);

        jmInCone = detectRange.FindJammersInCone(jammers);// Refresh jammer list in real-time
        detectRange.CalculatePowerBasedOnLargest(target, jmInCone);
        moveDir = detectRange.FindNewDirection();
        Debug.Log("New Direction: " + moveDir);
        detectRange.DestroyGrids();

        //  2.16 The missile only moves when `isLaunched = true`
        speed += acceleration * Time.deltaTime;
        transform.position += moveDir * speed;
        HitCheck();


        /*
        distance = Vector3.Distance(transform.position, target.transform.position);
        radius = 0.5F * distance;

        for(int i = 0; i < resolution; i++){
          float current_x = (i * (2.0F * radius/resolution)) - radius;
          for(int j = 0; j < resolution; j++){
            float current_y = (j * (2.0F * radius/resolution)) - radius;
            Instantiate(cube, transform.TransformPoint(new Vector3 (current_x * 10.0F, current_y * 10.0F, -10.0F*distance)), transform.rotation);
          }
        }
        */
      /*
      detectRange.GenerateGrid(target, resolution);
      detectRange.CalculatePowerBasedOnLargest(target, jammer);
      moveDir = detectRange.FindNewDirection();
      Debug.Log("New Direction: " + moveDir);
      detectRange.DestroyGrids();
      transform.position += moveDir * speed;  
      HitCheck();
      */
    }

    public Vector3 GetDirection() 
    {
      return moveDir;  
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
    }
    */

    void HitCheck() 
    {
      // If missile hit radar, exit editor game mode.
      if (Vector3.Distance(target.transform.position, transform.position) < 10)
      {
        Debug.Log("Missile hit radar.");
        speed = 0f;
        acceleration = 0f;
        //EditorApplication.isPaused = true;
        uiControllerScript.ShowUIAfterHit();
      }
    }

    //2.16:Launch missile (triggered by `LaunchButton`)
    public void Launch()
    {
        Debug.Log("Missile Launched!");
        jmInCone = detectRange.FindJammersInCone(jammers);
        isLaunched = true;
    }

    //2.16:Reset missile (triggered by `ResetButton`)
    public void ResetPosition()
    {
        Debug.Log("Missile Reset!");
        transform.position = startPosition; // Reset to initial position
        speed = 0;
        acceleration = 0;
        isLaunched = false;
    }
}
