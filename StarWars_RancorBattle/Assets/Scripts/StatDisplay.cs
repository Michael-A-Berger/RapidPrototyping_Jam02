using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplay : MonoBehaviour
{
    [SerializeField]
    //Transform barTransform;
    TextMesh healthText;
    [SerializeField]
    TextMesh specialText;
    new Transform camera;
    public CharacterBase fighter;

    public void SetCharacter(CharacterBase fighter)
    {
        this.fighter = fighter;
        transform.position = fighter.transform.position + 3 * Vector3.up;
    }
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = camera.forward;
        specialText.text = fighter.special > -1 ? "Specials: " + fighter.special : "";
        healthText.text = "Health: " + fighter.health;
    }
}
