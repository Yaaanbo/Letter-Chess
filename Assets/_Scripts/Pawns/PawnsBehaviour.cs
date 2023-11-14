using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PawnsBehaviour : MonoBehaviour
{
    [Header("Pawn Scriptable Object")]
    [SerializeField] private ScriptablePawn pawnSO;
    public ScriptablePawn GetPawnSO { get { return pawnSO; } }

    [Header("Attack area")]
    [SerializeField] private Transform targetAreaHighlight;
    private int width;
    private int height;
    private List<Transform> highlightObjectList = new List<Transform>();

    [Header("Pawn attack area checker")]
    [SerializeField] private float checkerRadius = .3f;
    [SerializeField] private LayerMask areaMask;
    // Start is called before the first frame update
    void Start()
    {
        width = BoardManager.instace.GetWidth;
        height = BoardManager.instace.GetHeight;

        CheckPawnType();

        if (IsOnAttackArea())
        {
            //Game Subtract lives and reverse queue
            GameManager.singleton.OnLiveSubtracted(false);
            GameManager.singleton.RemovePawnFromList(this.gameObject);
            GameManager.singleton.ReverseQueue();

            Destroy(this.gameObject, .2f);
        }
        else
        {
            //Play Correct Placement Audio
            AudioManager.singleton.PlaySfx(0);
        }
    }

    private void CheckPawnType()
    {
        switch (pawnSO.pawnType)
        {
            case PawnType.ARook:
                ARook();
                break;
            case PawnType.BKnight:
                BKnight();
                break;
            case PawnType.CBishop:
                CBishop();
                break;
        }
    }

    private void ARook()
    {
        Vector3 thisPos = this.transform.position;
        Vector3[] targetPos = new Vector3[2];
        for (int xy = 0; xy < width; xy++)
        {
            targetPos[0] = new Vector3(xy, thisPos.y); //Horizontal highlight
            targetPos[1] = new Vector3(thisPos.x, xy); //Vertical highlight

            //Spawning highlight
            for (int i = 0; i < targetPos.Length; i++)
            {
                SpawnHighlight(targetPos[i]);
            }
        }
    }

    private void BKnight()
    {
        Vector3[] targetPos = new Vector3[8];
        Vector3 thisPos = this.transform.position;
        
        //Assign vertical+ target pos
        targetPos[0] = new Vector3(thisPos.x + 1, thisPos.y + 2, thisPos.z);
        targetPos[1] = new Vector3(thisPos.x - 1, thisPos.y + 2, thisPos.z);

        //Assign vertical- target pos
        targetPos[2] = new Vector3(thisPos.x + 1, thisPos.y - 2, thisPos.z);
        targetPos[3] = new Vector3(thisPos.x - 1, thisPos.y - 2, thisPos.z);

        //Assign horizontal+ target pos
        targetPos[4] = new Vector3(thisPos.x + 2, thisPos.y + 1, thisPos.z);
        targetPos[5] = new Vector3(thisPos.x + 2, thisPos.y - 1, thisPos.z);

        //Assign horizontal- target pos
        targetPos[6] = new Vector3(thisPos.x - 2, thisPos.y + 1, thisPos.z);
        targetPos[7] = new Vector3(thisPos.x - 2, thisPos.y - 1, thisPos.z);

        for (int i = 0; i < targetPos.Length; i++)
        {
            SpawnHighlight(targetPos[i]);
        }
    }

    private void CBishop()
    {
        Vector3 thisPos = this.transform.position;
        Vector3[] targetPos = new Vector3[4];
        for (int xy = 0; xy < height; xy++)
        {
            targetPos[0] = new Vector3(thisPos.x + xy, thisPos.y + xy); //Top Right Diagonal highlight
            targetPos[1] = new Vector3(thisPos.x - xy, thisPos.y + xy); //Top Left Diagonal highlight
            targetPos[2] = new Vector3(thisPos.x + xy, thisPos.y - xy); //Bottom Right Diagonal highlight
            targetPos[3] = new Vector3(thisPos.x - xy, thisPos.y - xy); //Bottom Left Diagonal highlight

            for (int i = 0; i < targetPos.Length; i++)
            {
                SpawnHighlight(targetPos[i]);
            }
        }
    }

    private void SpawnHighlight(Vector3 _pos)
    {
        if ((_pos != this.transform.position) && (_pos.x >= 0 && _pos.x < width) && (_pos.y >= 0 && _pos.y < height))
        {
            Vector3 higlightObjectPos = new Vector3(_pos.x, _pos.y, .2f);
            var highlightObject = Instantiate(targetAreaHighlight, higlightObjectPos, Quaternion.identity);
            highlightObject.SetParent(this.transform);
            highlightObjectList.Add(highlightObject);
        }
    }

    private void ToggleHighlight(bool _isHovering)
    {
        if(highlightObjectList.Count > 0)
        {
            for (int i = 0; i < highlightObjectList.Count; i++)
            {
                highlightObjectList[i].GetComponent<SpriteRenderer>().enabled = _isHovering;
            }
        }
    }

    private bool IsOnAttackArea()
    {
        return Physics2D.OverlapCircle(this.transform.position, checkerRadius, areaMask);
    }

    private void OnMouseEnter()
    {
        ToggleHighlight(true);
    }

    private void OnMouseExit()
    {
        ToggleHighlight(false); 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(this.transform.position, checkerRadius);
    }
}
