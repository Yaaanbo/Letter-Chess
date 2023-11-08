using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnsBehaviour : MonoBehaviour
{
    [SerializeField] private ScriptablePawn pawnSO;
    public ScriptablePawn GetSetPawnSO { get { return pawnSO; } set { pawnSO = value; } }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckPawnType()
    {
        switch (pawnSO.pawnType)
        {
            case PawnType.ARook:
                break;
            case PawnType.BKnight:
                break;
            case PawnType.CBishop:
                break;
        }
    }
}
