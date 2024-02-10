using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[Flags] public enum RoomAccess
{
    None = 0,
    North = 1,
    East = 2,
    South = 4,
    West = 8
}

public class Room : MonoBehaviour
{
    [SerializeField] RoomAccess[] entryPoints;
    RoomAccess _totalAccess = RoomAccess.None;
    public RoomAccess totalAccess => _totalAccess;

    private void Awake() {
        foreach (RoomAccess access in entryPoints)
        {
            _totalAccess |= access;
        }
    }

    private void Update() {
        foreach(RoomAccess access in entryPoints)
        {
            Vector2 debugDirection = AccessValueToVector2(access);
            Debug.DrawRay(transform.position, new Vector2(debugDirection.x, debugDirection.y) * 10, Color.red); 
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawCube(transform.position, new Vector3(5, 5, 5));
    }

    Vector2Int AccessValueToVector2(RoomAccess accessValue)
    {
        switch (accessValue)
        {
            case RoomAccess.North:
                return new Vector2Int(0, 1);
            case RoomAccess.East:
                return new Vector2Int(1, 0);
            case RoomAccess.South:
                return new Vector2Int(0, -1);
            case RoomAccess.West:
                return new Vector2Int(-1, 0);
            default:
                return Vector2Int.zero;
        }
    }

    public RoomAccess GetRandomAccess()
    {
        List<RoomAccess> possibleAccess = new List<RoomAccess>();
        foreach (RoomAccess access in entryPoints)
        {
            if ((_totalAccess & access) != 0)
            {
                possibleAccess.Add(access);
            }
        }
        return possibleAccess[UnityEngine.Random.Range(0, possibleAccess.Count)];
    }

    public RoomAccess[] GetAllAccess() => entryPoints;

    private void OnValidate() {
        _totalAccess = RoomAccess.None;
        foreach (RoomAccess access in entryPoints)
        {
            _totalAccess |= access;
        }
        gameObject.name = _totalAccess.ToString();
    }
}
