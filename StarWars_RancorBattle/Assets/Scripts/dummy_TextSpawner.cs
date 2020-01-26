using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dummy_TextSpawner : MonoBehaviour
{
    // Attributes
    public GameObject floatTextPrefab;

    // Properties
    private GameObject spawned;
    private FloatText spawnedScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            float x = Random.Range(-5f, 5f);
            float y = Random.Range(1f, 5f);
            float z = Random.Range(-5f, 5f);
            spawned = Instantiate(floatTextPrefab, new Vector3(x, y, z), Quaternion.identity);
            spawnedScript = spawned.GetComponent<FloatText>();
            spawnedScript.fontSize = Random.Range(8f, 24f);
            spawnedScript.displayText = "Instance #" + Random.Range(1, 1000);
            spawnedScript.displayColor = new Color(Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f), Random.Range(0.2f, 0.8f));
        }
    }
}
