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

        InitializeJammerUI();
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
    public TMP_InputField jammerPowerInputField;

    public Button addJammerButton;
    public Button removeAllJammersButton;
    public Button removeSelectedJammerButton;
    public TMP_Dropdown jammerDropdown;

    private List<Jammer> jammerList = new List<Jammer>();
    private List<string> jammerNames = new List<string>();

    private float currentJammerX;
    private float currentJammerY;
    private float currentJammerZ;
    private float currentJammerPower;

    public void InitializeJammerUI()
    {
        jammerXInputField.onEndEdit.AddListener(OnJammerXChanged);
        jammerYInputField.onEndEdit.AddListener(OnJammerYChanged);
        jammerYInputField.onEndEdit.AddListener(OnJammerZChanged);
        jammerPowerInputField.onEndEdit.AddListener(OnJammerPowerChanged);

        addJammerButton.onClick.AddListener(AddJammer);
        removeAllJammersButton.onClick.AddListener(RemoveAllJammers);
        removeSelectedJammerButton.onClick.AddListener(RemoveSelectedJammer);
    }

    void OnJammerXChanged(string value)
    {
        currentJammerX = float.TryParse(value, out var xValue) ? xValue : 0f;
        jammerXInputField.text = currentJammerX.ToString();
    }

    void OnJammerYChanged(string value)
    {
        currentJammerY = float.TryParse(value, out var yValue) ? yValue : 0f;
        jammerYInputField.text = currentJammerY.ToString();
    }

    void OnJammerZChanged(string value)
    {
        currentJammerZ = float.TryParse(value, out var zValue) ? zValue : 0f;
        jammerZInputField.text = currentJammerZ.ToString();
    }

    void OnJammerPowerChanged(string value)
    {
        float powerValue;
        if (float.TryParse(value, out powerValue))
        {
            currentJammerPower = powerValue;
        }
        else
        {
            currentJammerPower = 100f;
            jammerPowerInputField.text = "100";
        }
    }

    void RemoveSelectedJammer()
    {
        int selectedIndex = jammerDropdown.value;
        if (selectedIndex < jammerList.Count)
        {
            jammerList[selectedIndex].RemoveJammer();
            Destroy(jammerList[selectedIndex].gameObject);

            jammerList.RemoveAt(selectedIndex);
            jammerNames.RemoveAt(selectedIndex);

            UpdateDropdown();
            Debug.Log($"[UIController] Removed Jammer at index {selectedIndex}");
        }
        else
        {
            Debug.LogWarning("No valid Jammer selected to remove.");
        }
    }

    void AddJammer()
    {
        GameObject jammerObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        jammerObj.name = "Jammer";

        jammerObj.transform.position = new Vector3(currentJammerX, currentJammerY, currentJammerZ);
        jammerObj.transform.localScale = new Vector3(10, 10, 10);

        jammerObj.transform.parent = GameObject.Find("Jammers").transform;

        Jammer currentJammer = jammerObj.AddComponent<Jammer>();

        currentJammer.x = currentJammerX;
        currentJammer.y = currentJammerY;
        currentJammer.z = currentJammerZ;
        currentJammer.power = currentJammerPower;

        currentJammer.Initialize();

        jammerList.Add(currentJammer);

        string jammerName = $"Jammer_{jammerList.Count}: {currentJammerX}, {currentJammerY}, {currentJammerZ}";
        jammerNames.Add(jammerName);

        UpdateDropdown();

        Debug.Log($"[UIController] Added {jammerName}. Total Jammers: {jammerList.Count}");
    }

    void RemoveAllJammers()
    {
        foreach (Jammer jammer in jammerList)
        {
            jammer.RemoveJammer();
            Destroy(jammer.gameObject);
        }
        jammerList.Clear();
        jammerNames.Clear();

        UpdateDropdown();
        Debug.Log("[UIController] All Jammers removed.");
    }

    void UpdateDropdown()
    {
        jammerDropdown.ClearOptions();
        jammerDropdown.AddOptions(jammerNames);
    }

}
