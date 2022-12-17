using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EDamageData : MonoBehaviour
{
    public float damage = 20.0f; //기본 공격력
    public enum EType
    {
        PAWN,
        KNIGHT,
        BISHOP,
        ROOK,
        QCARD,
        JCARD,
        CARD,
        BOSS
    }
    public EType etype;
}
