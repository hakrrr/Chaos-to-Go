using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DraggableObject : MonoBehaviour
{
    public IDragBehavior behavior;

    private static bool dragging = false;

    [SerializeField]
    private float screenDistance = 5.0f;

    private bool dragged = false;
    private InputStates btnMouseL = new InputStates(KeyCode.Mouse0);
    private InputStates btnMouseR = new InputStates(KeyCode.Mouse1);
    private Vector3 initialPosition;

    public interface IDragBehavior
    {
        void OnDrag();
        void OnPickup();
        void OnAbort();
        bool OnRelease();
    }


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (dragged)
        {
            DragPosition();
            behavior.OnDrag();
            CheckDraggingInput();
        }
    }


    public static bool IsPlayerDraggingObject()
    {
        return dragging;
    }


    private void OnMouseOver()
    {
        if (dragging)
        {
            return;
        }

        if(btnMouseL.Check() == InputStates.InputState.JustPressed)
        {
            dragging = dragged = true;
            behavior.OnPickup();
        }
    }

    private void DragPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 rayPoint = ray.GetPoint(screenDistance);
        transform.position = rayPoint;
        //transform.position = new Vector3(rayPoint.x, height, rayPoint.z);
    }


    private void CheckDraggingInput()
    {
        if(btnMouseL.Check() == InputStates.InputState.JustPressed)
        {
            if (behavior.OnRelease())
            {
                dragged = dragging = false;
            }
        }
        else if(btnMouseR.Check() == InputStates.InputState.JustPressed)
        {
            behavior.OnAbort();
            transform.position = initialPosition;
            dragged = dragging = false;
        }
    }
}
