using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    #region Property
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string Category { get; private set; }
    public string Description { get; private set; }
    public float Power { get; private set; }
    public float Duration { get; set; }
    public bool IsActivate { get; set; }
    #endregion

    #region Initialization
    public void Initialize(string id, string itemName, string category, string description, float power, float duration)
    {
        Id = id;
        Name = itemName;
        Category = category;
        Description = description;
        Power = power;
        Duration = duration;
        IsActivate = false; // 기본적으로 아이템은 비활성 상태로 시작
    }
    #endregion
}