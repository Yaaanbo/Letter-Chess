using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton { get; set; }

    [Header("Basic Info")]
    [SerializeField] private int score;
    [SerializeField] private int lives;
    [SerializeField] private float initialTime;
    private float currentTime;

    [Header("Queue System")]
    [SerializeField] private List<ScriptablePawn> pawnList = new List<ScriptablePawn>();
    [SerializeField] private List<ScriptablePawn> queuedPawnsList = new List<ScriptablePawn>();
    [SerializeField] private int queueNumber;
    [SerializeField] private Transform pawnQueuePrefab;
    [SerializeField] private Transform queueParentViewport;

    [SerializeField] private float spacing;
    [SerializeField] private float smoothingTime = .125f;
    [SerializeField] private RectTransform pawnQueueSpawnPos;

    private ScriptablePawn selectedPawn;

    //Updating Timer UI with event
    public delegate void onTimerUpdate(float _currentTime, float _initTime);
    public event onTimerUpdate OnTimerUpdate;

    //Updating Lives UI with event
    public delegate void onLivesUpdate(int _lives);
    public event onLivesUpdate OnLivesUpdate;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentTime = initialTime;
    }

    // Update is called once per frame
    void Update()
    {
        GenerateQueue();
        Timer();
    }

    private void GenerateQueue()
    {
        if (queuedPawnsList.Count < queueNumber)
        {
            List<RectTransform> queueRect = new List<RectTransform>();

            for (int i = queuedPawnsList.Count; i < queueNumber; i++)
            {
                //Instantiate and assign pawn to corresponded variable
                var queuedPawn = Instantiate(pawnQueuePrefab, pawnQueueSpawnPos.position, Quaternion.identity);
                queuedPawn.transform.SetParent(queueParentViewport);
                queueRect.Add(queuedPawn.GetComponent<RectTransform>());

                //Change pawn spawn position
                var spawnPos = pawnQueueSpawnPos.anchoredPosition;
                pawnQueueSpawnPos.anchoredPosition = new Vector3(spawnPos.x, spawnPos.y + spacing);

                int randomizedPawn = Random.Range(0, pawnList.Count);

                //Change prefab apperance according to randomized pawn scriptable object
                queuedPawn.GetComponent<Image>().color = pawnList[randomizedPawn].pawnBackgroundColor;
                foreach (Transform child in queuedPawn.transform)
                {
                    child.GetComponent<Image>().sprite = pawnList[randomizedPawn].pawnLetterSprite;
                }

                //Add pawns to queued list
                queuedPawnsList.Add(pawnList[randomizedPawn]);
            }

            //Set selected pawn to first index of queued list
            selectedPawn = queuedPawnsList[0];

            for (int i = 0; i < queueRect.Count; i++)
            {
                var lerpPos = queueRect[i].anchoredPosition;
                queueRect[i].anchoredPosition = Vector3.Lerp(new Vector3(lerpPos.x, lerpPos.y), new Vector3(lerpPos.x, lerpPos.y - spacing), smoothingTime);
            }
        }
        else
            return;
    }

    private void Timer()
    {
        if (currentTime > 0)
            currentTime -= Time.deltaTime;
        else
        {
            currentTime = initialTime;
            lives--;
            OnLivesUpdate?.Invoke(lives);

            queuedPawnsList.Remove(selectedPawn);

            if (lives <= 0)
                Debug.Log("Game Over");
        }

        OnTimerUpdate?.Invoke(currentTime, initialTime);
    }

    private void RearrangeQueue()
    {

    }

    public void SpawnPawnsOnBoard(Vector3 _pos)
    {
        Instantiate(selectedPawn.pawnPrefab, _pos, Quaternion.identity);
        queuedPawnsList.Remove(selectedPawn);
    }
}
