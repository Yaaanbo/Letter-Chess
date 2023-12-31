using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pawn", menuName = "Create Pawn")]
public class ScriptablePawn : ScriptableObject
{
    [Header("Pawn Info")]
    public PawnType pawnType;
    public string pawnName;
    public Sprite pawnLetterSprite;
    public Color pawnBackgroundColor;
    public int pawnScore;

    [Header("Pawn Object")]
    public GameObject pawnPrefab;
    public GameObject fourStackParticle;
}

public enum PawnType
{
    ARook,
    BKnight,
    CBishop
}
