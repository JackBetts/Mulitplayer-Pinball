using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    public enum MoveAxis { X, Y, Z, NegX, NegY, NegZ }
    public MoveAxis currentMoveAxis = MoveAxis.X; 
    
    public bool useMouse;
    public bool useTouch;

    [Space] public PlayerSpawnPoint spawnPointRef; 
    public RectTransform touchRect;
    public float width;
    public float height;


    private float _targetPlayerPos = 0;
    public float TargetPlayerPos
    {
        get => _targetPlayerPos;
        set => _targetPlayerPos = value;
    }

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        width = Screen.width;
        height = Screen.height;

        Debug.Log("Screen size is " + width + " by " + height); 
    }

    private void Update()
    {
        //Check if the input is at the bottom of the screen ish, and then check touch position and move
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            Vector2 screenPoint = GetTouchPos(); 
            bool isInMovePos = RectTransformUtility.RectangleContainsScreenPoint(touchRect, screenPoint, null);

            if (isInMovePos)
            {
                Vector2 normalizedInput = screenPoint / width;
                float targetPosition = (normalizedInput.x * 10) - 5;
                _targetPlayerPos = targetPosition; 
            }
        }
    }

    public void CalculateMoveAxis(int playerIndex)
    {
        switch (playerIndex)
        {
            case 0:
                currentMoveAxis = MoveAxis.X;
                break;
            case 1:
                currentMoveAxis = MoveAxis.NegX;
                break;
            case 2:
                currentMoveAxis = MoveAxis.Z;
                break;
            case 3:
                currentMoveAxis = MoveAxis.NegZ;
                break;
        }
    }

    private Vector2 GetTouchPos()
    {
        if (useMouse)
        {
            return Input.mousePosition;
        }

        if (useTouch)
        {
            Touch touch = Input.GetTouch(0);
            return touch.position; 
        }
        
        return Vector2.zero;
    }

    public Vector3 GetNewPlayerPosition(Vector3 currentPosition)
    {
        Vector3 newPosition = currentPosition;

        switch (currentMoveAxis)
        {
            case MoveAxis.X:
                newPosition.x = _targetPlayerPos;
                break;
            case MoveAxis.Y:
                newPosition.y = _targetPlayerPos;
                break;
            case MoveAxis.Z:
                newPosition.z = _targetPlayerPos;
                break;
            case MoveAxis.NegX:
                newPosition.x = -_targetPlayerPos;
                break;
            case MoveAxis.NegY:
                newPosition.y = -_targetPlayerPos;
                break;
            case MoveAxis.NegZ:
                newPosition.z = -_targetPlayerPos;
                break;
            default:
                newPosition.x = _targetPlayerPos;
                break; 
        }
        
        return newPosition;
    }
}
