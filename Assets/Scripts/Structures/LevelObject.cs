using System;
using UnityEngine;

[Serializable]
public class LevelObject {
    public string name;
    public Vector2 position;
    public float time;
    public float speed;
    public bool spawned = false;
}