using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    //  UI component references
    public TMP_InputField speedInputField;
    public TMP_InputField accelerationInputField;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI accelerationText;
    public Button launchButton;
    public Button resetButton;
    public GameObject startPanel;
    //  Reference to `Missile` script, so UI can control the missile
    public Missile missileScript;

    void Start()
    {
        speedInputField.text = "5";
        accelerationInputField.text = "1";

        missileScript.speed = 5f;
        missileScript.acceleration = 1f;
        //  Listen for UI events, trigger methods when sliders change
        speedInputField.onEndEdit.AddListener(OnSpeedInputChanged);
        accelerationInputField.onEndEdit.AddListener(OnAccelerationInputChanged);

        launchButton.onClick.AddListener(LaunchMissile);
        resetButton.onClick.AddListener(ResetSimulation);

        startPanel.SetActive(true);

        CheckLaunchReady();
    }

    void OnSpeedInputChanged(string value)
    {
        float newSpeed;
        if (float.TryParse(value, out newSpeed))
        {
            missileScript.speed = newSpeed;
        }
        else
        {
            missileScript.speed = 5f;
            speedInputField.text = "5";
        }
        CheckLaunchReady();
    }

    void OnAccelerationInputChanged(string value)
    {
        float newAcc;
        if (float.TryParse(value, out newAcc))
        {
            missileScript.acceleration = newAcc;
        }
        else
        {
            missileScript.acceleration = 1f;
            accelerationInputField.text = "1";
        }
        CheckLaunchReady();
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

    public void ShowUIAfterHit()
    {
    startPanel.SetActive(true);
    
    }

    void ResetSimulation()
    {
        //  Call `ResetPosition()` in `Missile.cs` to reset the missile
        missileScript.ResetPosition();

        speedInputField.text = "5";
        accelerationInputField.text = "1";
        missileScript.speed = 5f;
        missileScript.acceleration = 1f;

        startPanel.SetActive(true);

        CheckLaunchReady();

    }
}
