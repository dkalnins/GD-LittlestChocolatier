using System;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Used with WaypointMover to horizontally flip the graphics horizontally based on the direction
/// the object is moving.
/// 
/// Place this on the same object as the WaypointMover
/// </summary>
public class WaypointFlipper : MonoBehaviour
{
    private enum MovementDirection { Left, Right};
    
    [SerializeField] private MovementDirection _normalFacing = MovementDirection.Left;
    private MovementDirection _currentFacing;

    private void Start()
    {
        _currentFacing = _normalFacing;
    }

    internal void InformXDirection(float movementDirection)
    {
        if (movementDirection == 0)
            return;

        MovementDirection _newFacing = movementDirection > 0 ? MovementDirection.Right : MovementDirection.Left;
    
        if (_newFacing != _currentFacing)
        {
            _currentFacing = _newFacing;
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
//            Debug.Log("Sprite Flip now");
        }

    }
}
