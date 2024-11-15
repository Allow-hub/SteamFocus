using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingPendulum : MonoBehaviour
{
    public float swingSpeed = 2.0f;  // —h‚ê‚é‘¬‚³
    public float swingAngle = 45.0f; // Å‘åŠp“x

    private float initialRotation;

    void Start()
    {
        initialRotation = transform.localEulerAngles.z;
    }

    void Update()
    {
        // ŠÔ‚ÉŠî‚Ã‚¢‚Äƒuƒ‰ƒ“ƒR‚ğ—h‚ç‚·
        float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
        transform.localEulerAngles = new Vector3(0, 0, initialRotation + angle);
    }
}
