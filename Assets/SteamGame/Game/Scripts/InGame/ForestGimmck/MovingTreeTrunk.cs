using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTreeTrunk : MonoBehaviour
{
    public float extendDistance = 5.0f;   // Š²‚ªL‚Ñ‚é‹——£
    public float moveSpeed = 2.0f;        // Š²‚ªL‚Ñk‚İ‚·‚é‘¬“x
    public float pauseTime = 1.0f;        // Š²‚ªL‚ÑØ‚Á‚Ä‚©‚ç–ß‚é‚Ü‚Å‚ÌŠÔ

    private Vector3 initialPosition;
    private bool isExtending = true;

    void Start()
    {
        // ‰ŠúˆÊ’u‚ğ‹L˜^
        initialPosition = transform.position;
    }

    void Update()
    {
        // Š²‚ğL‚Î‚µ‚½‚èk‚ß‚½‚è‚·‚é“®‚«
        if (isExtending)
        {
            // L‚Ñ‚é“®‚«
            transform.position = Vector3.MoveTowards(transform.position, initialPosition + transform.forward * extendDistance, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, initialPosition + transform.forward * extendDistance) < 0.1f)
            {
                isExtending = false;
                Invoke(nameof(SwitchDirection), pauseTime);
            }
        }
        else
        {
            // Œ³‚ÌˆÊ’u‚É–ß‚é“®‚«
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, initialPosition) < 0.1f)
            {
                isExtending = true;
                Invoke(nameof(SwitchDirection), pauseTime);
            }
        }
    }

    void SwitchDirection()
    {
        isExtending = !isExtending;
    }
}
