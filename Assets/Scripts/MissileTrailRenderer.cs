using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class MissileTrailRenderer : MonoBehaviour
{
    [Header("Trail Settings")]
    public float updateFrequency = 0.1f;  // How often to update the trail
    public int maxPoints = 50;           // Maximum trail points
    public float trailThickness = 0.1f;  // Width of the trail
    
    private LineRenderer trailRenderer;
    private Vector3[] positionHistory;
    private int currentPositionIndex = 0;
    private float updateTimer = 0f;

    void Start()
    {
        trailRenderer = GetComponent<LineRenderer>();
        trailRenderer.startWidth = trailThickness;
        trailRenderer.endWidth = trailThickness;
        
        positionHistory = new Vector3[maxPoints];
        trailRenderer.positionCount = maxPoints;
        
        // Initialize all points at current position
        for(int i = 0; i < maxPoints; i++)
        {
            positionHistory[i] = transform.position;
            trailRenderer.SetPosition(i, positionHistory[i]);
        }
    }

    void Update()
    {
        updateTimer += Time.deltaTime;
        
        if(updateTimer >= updateFrequency)
        {
            updateTimer = 0f;
            
            // Update position buffer
            positionHistory[currentPositionIndex] = transform.position;
            
            // Circular buffer
            currentPositionIndex = (currentPositionIndex + 1) % maxPoints;
            
            // Update Line Renderer
            for(int i = 0; i < maxPoints; i++)
            {
                int bufferIndex = (currentPositionIndex + i) % maxPoints;
                trailRenderer.SetPosition(i, positionHistory[bufferIndex]);
            }
        }
    }
    
    // Clear the trail
    public void ResetTrail()
    {
        currentPositionIndex = 0;
        for(int i = 0; i < maxPoints; i++)
        {
            positionHistory[i] = transform.position;
            trailRenderer.SetPosition(i, positionHistory[i]);
        }
    }
}