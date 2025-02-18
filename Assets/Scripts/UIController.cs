using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    //  UI component references
    public Slider speedSlider;       
    public TextMeshProUGUI speedText;           
    public Slider accelerationSlider; 
    public TextMeshProUGUI accelerationText;     
    public Button launchButton;       
    public Button resetButton;
    public GameObject startPanel;

    //  Reference to `Missile` script, so UI can control the missile
    public Missile missileScript;

    void Start()
    {
        //  Listen for UI events, trigger methods when sliders change
        speedSlider.onValueChanged.AddListener(UpdateSpeed);
        accelerationSlider.onValueChanged.AddListener(UpdateAcceleration);
        launchButton.onClick.AddListener(LaunchMissile);
        resetButton.onClick.AddListener(ResetSimulation);

        startPanel.SetActive(true);

        // Initialize UI display values
        speedText.text = "Speed: " + speedSlider.value.ToString("F1");
        accelerationText.text = "Acceleration: " + accelerationSlider.value.ToString("F1");
    }

    void UpdateSpeed(float value)
    {
        //  Update speed value in UI and modify `speed` in `Missile.cs`
        speedText.text = "Speed: " + value.ToString("F1");
        missileScript.speed = value;
        CheckLaunchReady();
    }

    void UpdateAcceleration(float value)
    {
        //  Update acceleration value in UI and modify `acceleration` in `Missile.cs`
        accelerationText.text = "Acceleration: " + value.ToString("F1");
        missileScript.acceleration = value;
        CheckLaunchReady();
    }


    void CheckLaunchReady()
    {
        // When player adjust speed or acceleration, `Launch` button starts work
        launchButton.interactable = (missileScript.speed > 0 || missileScript.acceleration > 0);
    }
    void LaunchMissile()
    {
        startPanel.SetActive(false);
        //  Call `Launch()` in `Missile.cs` to launch the missile
        missileScript.Launch();
    }

    void ResetSimulation()
    {
        //  Call `ResetPosition()` in `Missile.cs` to reset the missile
        missileScript.ResetPosition();

        startPanel.SetActive(true);

    }
}
