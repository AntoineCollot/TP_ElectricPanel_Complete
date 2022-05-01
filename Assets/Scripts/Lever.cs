using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public bool isOn = false;
    bool isHovered;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("IsOn", isOn);
    }

    void OnMouseEnter()
    {
        //Note that the mouse is hovering the object
        isHovered = true;
    }

    void OnMouseExit()
    {
        //Note that the mouse stopped hovering the object
        isHovered = false;
    }

    private void Update()
    {
        //Detect a left click
        if(isHovered && Input.GetMouseButtonDown(0))
        {
            isOn = !isOn;
            anim.SetBool("IsOn", isOn);
        }
    }
}
