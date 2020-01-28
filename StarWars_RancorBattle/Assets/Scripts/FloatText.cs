using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatText : MonoBehaviour
{
    // Attributes
    public Camera mainCamera;
    public string displayText = "Sample Text";
    public float fontSize = 12f;
    public Color displayColor = Color.black;
    public float floatTime = 2f;
    public float floatSpeed = 1f;
    public Vector3 floatDirection = new Vector3(0f, 1f, 0f);
    [Range(0, 1)] public float disappearPercentage = 0.5f;

    // Properties
    private TextMeshPro textPro;
    private float startTime;
    private float startFadeTime;

    // Start is called before the first frame update
    void Start()
    {
        // mainCamera = Camera.main;
        textPro = GetComponent<TextMeshPro>();
        startTime = Time.time;
        startFadeTime = floatTime * disappearPercentage;

        // Hiding the text initially so that it doesn't "Snap" into place for the player
        textPro.color = new Color(0f, 0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        // Getting the current time to live
        float currentTime = Time.time;
        float timeLeft = (startTime + floatTime) - currentTime;

        // Changing the text properties
        textPro.text = displayText;
        textPro.fontSize = fontSize;
        textPro.color = displayColor;

        // IF the time left is less than the fade time, make the text opaque
        if (timeLeft < startFadeTime)
        {
            textPro.color = new Color(displayColor.r, displayColor.g, displayColor.b, timeLeft / startFadeTime); ;
        }

        // IF the main camera is not set, then do that
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        // Rotating the object towards the camera
        transform.forward = mainCamera.transform.forward;

        // Translating in the movement direction
        transform.Translate(floatDirection * floatSpeed * Time.deltaTime, Space.World);

        // IF the time limit has been exceeded, delete this object
        if (currentTime > startTime + floatTime)
        {
            Destroy(gameObject);
        }
    }

    static GameObject prefab = null;

    public static void CreateFloatText(string text, Color colour, Vector3 position, 
        Vector3 floatDirection, float fontSize = 8, float floatTime = 1, 
        float floatSpeed = 1, float disappearPercentage = 0.5f)
    {
        if (!prefab)
        {
            prefab = Resources.Load<GameObject>("Float text");

            FloatText inst = Instantiate(prefab, position, Quaternion.identity).GetComponent<FloatText>();
            inst.displayColor = colour;
            inst.displayText = text;
            inst.floatDirection = floatDirection;
            inst.fontSize = fontSize;
            inst.floatTime = floatTime;
            inst.floatSpeed = floatSpeed;
            inst.disappearPercentage = disappearPercentage;
        }
    }

    public static void CreateFloatText(string text, Color colour, 
        Vector3 position, float fontSize = 8, float floatTime = 1, 
        float floatSpeed = 1, float disappearPercentage = 0.5f)
    {
        if (!prefab)
        {
            prefab = Resources.Load<GameObject>("Float text");
        }


        FloatText inst = Instantiate(prefab, position, Quaternion.identity).GetComponent<FloatText>();
        inst.displayColor = colour;
        inst.displayText = text;
        inst.floatDirection = Vector3.up;
        inst.fontSize = fontSize;
        inst.floatTime = floatTime;
        inst.floatSpeed = floatSpeed;
        inst.disappearPercentage = disappearPercentage;
    }
}
