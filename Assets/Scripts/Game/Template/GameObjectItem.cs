using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Object", menuName ="Create Game Object")]
public class GameObjectItem : ScriptableObject
{
    public ulong id;
    public Vector2 attackScope;
    public List<Vector2> extendPoint = new List<Vector2>();
}
