using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //  UI component references
    public Slider speedSlider;       
    public Text speedText;           
    public Slider accelerationSlider; 
    public Text accelerationText;     
    public Button launchButton;       
    public Button resetButton;        

    //  Reference to `Missile` script, so UI can control the missile
    public Missile missileScript;

    void Start()
    {
        //  Listen for UI events, trigger methods when sliders change
        speedSlider.onValueChanged.AddListener(UpdateSpeed);
        accelerationSlider.onValueChanged.AddListener(UpdateAcceleration);
        launchButton.onClick.AddListener(LaunchMissile);
        resetButton.onClick.AddListener(ResetSimulation);

        // Initialize UI display values
        speedText.text = "Speed: " + speedSlider.value.ToString("F1");
        accelerationText.text = "Acceleration: " + accelerationSlider.value.ToString("F1");
    }

    void UpdateSpeed(float value)
    {
        //  Update speed value in UI and modify `speed` in `Missile.cs`
        speedText.text = "Speed: " + value.ToString("F1");
        missileScript.speed = value;
    }

    void UpdateAcceleration(float value)
    {
        //  Update acceleration value in UI and modify `acceleration` in `Missile.cs`
        accelerationText.text = "Acceleration: " + value.ToString("F1");
        missileScript.acceleration = value;
    }

    void LaunchMissile()
    {
        //  Call `Launch()` in `Missile.cs` to launch the missile
        missileScript.Launch();
    }

    void ResetSimulation()
    {
        //  Call `ResetPosition()` in `Missile.cs` to reset the missile
        missileScript.ResetPosition();
    }
}
