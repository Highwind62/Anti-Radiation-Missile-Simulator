using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
    public Jammer jammerScript;

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

    //------------------------jammer part-----------------------------

    public TMP_InputField jammerXInputField;
    public TMP_InputField jammerYInputField;
    public TMP_InputField jammerZInputField;

    public Button addJammerButton;
    public Button removeAllJammersButton;

    private List<Jammer> jammerList = new List<Jammer>();

    private float currentJammerX;
    private float currentJammerY;
    private float currentJammerZ;

    public void InitializeJammerUI()
    {
        jammerXInputField.onEndEdit.AddListener(OnJammerXChanged);
        jammerYInputField.onEndEdit.AddListener(OnJammerYChanged);
        jammerYInputField.onEndEdit.AddListener(OnJammerZChanged);

        addJammerButton.onClick.AddListener(AddJammer);
        removeAllJammersButton.onClick.AddListener(RemoveAllJammers);
    }

    void OnJammerXChanged(string value)
    {
        float xValue;
        if (float.TryParse(value, out xValue))
        {
            currentJammerX = xValue;
        }
        else
        {
            currentJammerX = 0f;
            jammerXInputField.text = "0";
        }
    }

    void OnJammerYChanged(string value)
    {
        float yValue;
        if (float.TryParse(value, out yValue))
        {
            currentJammerY = yValue;
        }
        else
        {
            currentJammerY = 0f;
            jammerYInputField.text = "0";
        }
    }

    void OnJammerZChanged(string value)
    {
        float zValue;
        if (float.TryParse(value, out zValue))
        {
            currentJammerZ = zValue;
        }
        else
        {
            currentJammerZ = 0f;
            jammerZInputField.text = "0";
        }
    }

    void AddJammer()
    {
        GameObject jammerObj = new GameObject("Jammer");
        Jammer currentJammer = jammerObj.AddComponent<Jammer>();

        currentJammer.x = currentJammerX;
        currentJammer.y = currentJammerY;
        currentJammer.z = currentJammerZ;

        currentJammer.AddJammer();

        jammerList.Add(currentJammer);

        Debug.Log($"[UIController] Added Jammer at x={currentJammer.x}, y={currentJammer.y}, z={currentJammer.z}. Total Jammers: {jammerList.Count}");
    }

    void RemoveAllJammers()
    {
        foreach (Jammer jammer in jammerList)
        {
            jammer.RemoveJammer();
        }
        jammerList.Clear();
        Debug.Log("[UIController] All Jammers removed.");
    }

}
