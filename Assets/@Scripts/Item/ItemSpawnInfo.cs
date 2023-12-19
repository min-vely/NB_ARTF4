using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템 스폰 위치 정보를 담은 클래스
public class ItemSpawnInfo : MonoBehaviour
{
    [SerializeField] private int _id;

    public int Id => _id;
   
}
