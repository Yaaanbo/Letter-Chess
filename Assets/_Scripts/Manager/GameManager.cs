using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton { get; set; }

    private const string HIGH_SCORE_KEY = "Highscore";

    [Header("Basic Info")]
    [SerializeField] private int lives;
    [SerializeField] private float initialTime;
    private float currentTime;
    private int score;
    private int hiScore = 0;

    [Header("Queue System")]
    [SerializeField] private List<ScriptablePawn> pawnList = new List<ScriptablePawn>();
    [SerializeField] private List<ScriptablePawn> queuedPawnsList = new List<ScriptablePawn>();
    [SerializeField] private int queueNumber;
    [SerializeField] private Transform pawnQueuePrefab;

    [SerializeField] private float spacing;
    [SerializeField] private RectTransform pawnQueueSpawnPos;
    [SerializeField] private RectTransform queueParentViewport;

    [Header("Pawn Counter")]
    private List<ScriptablePawn> spawnedPawn = new List<ScriptablePawn>();
    private List<GameObject> ARookList = new List<GameObject>();
    private List<GameObject> BKnightList = new List<GameObject>();
    private List<GameObject> CBishopList = new List<GameObject>();

    private ScriptablePawn selectedPawn;

    //Events
    //Updating Timer UI
    public delegate void onTimerUpdate(float _currentTime, float _initTime);
    public event onTimerUpdate OnTimerUpdate;

    //Updating Lives UI
    public delegate void onLivesUpdate(int _lives);
    public event onLivesUpdate OnLivesUpdate;

    //Updating Score UI
    public delegate void onScoreUpdate(int _score);
    public event onScoreUpdate OnScoreUpdate;

    //Displaying Game Over UI
    public delegate void onGameOver(int _finalScore, int _highScore);
    public event onGameOver OnGameOver;

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
        Time.timeScale = 1;

        if (PlayerPrefs.HasKey(HIGH_SCORE_KEY))
           hiScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY);

        currentTime = initialTime;

        GenerateQueue();
    }

    // Update is called once per frame
    void Update()
    {
        CountDown();
    }

    private void GenerateQueue()
    {
        for (int i = queuedPawnsList.Count; i < queueNumber; i++)
        {
            //Instantiate and assign pawn to corresponded variable
            var queuedPawn = Instantiate(pawnQueuePrefab, pawnQueueSpawnPos.position, Quaternion.identity);
            queuedPawn.transform.SetParent(queueParentViewport);

            //Change spawner position
            var spawnPos = pawnQueueSpawnPos.anchoredPosition;
            pawnQueueSpawnPos.anchoredPosition = new Vector3(spawnPos.x, spawnPos.y + spacing);

            //Change prefab apperance according to randomized pawn scriptable object
            int randomizedPawn = Random.Range(0, pawnList.Count);
            queuedPawn.GetComponent<Image>().color = pawnList[randomizedPawn].pawnBackgroundColor;
            queuedPawn.GetChild(0).GetComponent<Image>().sprite = pawnList[randomizedPawn].pawnLetterSprite;
            queuedPawn.name = $"{pawnList[randomizedPawn].pawnType}";

            //Add pawns to queued list
            queuedPawnsList.Add(pawnList[randomizedPawn]);
        }

        //Set selected pawn to first index of queued list
        selectedPawn = queuedPawnsList[0];
    }

    public void ReverseQueue()
    {
        queuedPawnsList.Add(spawnedPawn[^1]);

        //Move last queued pawn list index to first index
        ScriptablePawn temp = queuedPawnsList[^1];
        queuedPawnsList.RemoveAt(queuedPawnsList.Count - 1);
        queuedPawnsList.Insert(0, temp);
        selectedPawn = queuedPawnsList[0];

        //Change queue parent position
        var parentPos = queueParentViewport.anchoredPosition;
        queueParentViewport.anchoredPosition = new Vector3(parentPos.x, parentPos.y + spacing);

    }

    private void CountDown()
    {
        if (currentTime > 0)
            currentTime -= Time.deltaTime;
        else
            OnLiveSubtracted(true);

        OnTimerUpdate?.Invoke(currentTime, initialTime);
    }

    public void OnLiveSubtracted(bool _isTimeOut)
    {
        lives--;
        currentTime = initialTime;
        OnLivesUpdate?.Invoke(lives);

        //Play Wrong Placement Audio
        AudioManager.singleton.PlaySfx(1);

        if (_isTimeOut)
        {
            queuedPawnsList.Remove(selectedPawn);

            //Change queue parent position
            var parentPos = queueParentViewport.anchoredPosition;
            queueParentViewport.anchoredPosition = new Vector3(parentPos.x, parentPos.y - spacing);

            GenerateQueue();
        }

        if (lives <= 0)
        {
            Time.timeScale = 0;
            if (score > hiScore)
            {
                hiScore = score;
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, hiScore);
            }
            OnGameOver?.Invoke(score, hiScore);
        }
    }

    public void SpawnPawnOnBoard(Vector3 _pos)
    {
        if(Time.timeScale == 1 && currentTime >= 0)
        {
            var pawns = Instantiate(selectedPawn.pawnPrefab, _pos, Quaternion.identity);
            pawns.name = $"{pawns.GetComponent<PawnsBehaviour>().GetPawnSO.pawnType}";
            spawnedPawn.Add(pawns.GetComponent<PawnsBehaviour>().GetPawnSO);

            queuedPawnsList.Remove(selectedPawn);

            //Change queue parent position
            var parentPos = queueParentViewport.anchoredPosition;
            queueParentViewport.anchoredPosition = new Vector3(parentPos.x, parentPos.y - spacing);

            GenerateQueue();

            CheckPawnDuplication(pawns);
            currentTime = initialTime;
        }
    }

    private void CheckPawnDuplication(GameObject _target)
    {
        switch (_target.GetComponent<PawnsBehaviour>().GetPawnSO.pawnType)
        {
            case PawnType.ARook:
                ARookList.Add(_target);
                CheckListAmount(ARookList, _target.GetComponent<PawnsBehaviour>().GetPawnSO.pawnScore);
                break;
            case PawnType.BKnight:
                BKnightList.Add(_target);
                CheckListAmount(BKnightList, _target.GetComponent<PawnsBehaviour>().GetPawnSO.pawnScore);
                break;
            case PawnType.CBishop:
                CBishopList.Add(_target);
                CheckListAmount(CBishopList, _target.GetComponent<PawnsBehaviour>().GetPawnSO.pawnScore);
                break;
        }

        void CheckListAmount(List<GameObject> _obj, int _score)
        {
            int pawnLimit = 4;
            if(_obj.Count == pawnLimit)
            {
                for (int i = 0; i < 4; i++)
                {
                    //Spawn particle and destroy in 1 second
                    var fourStackParticle = Instantiate(_obj[i].GetComponent<PawnsBehaviour>().GetPawnSO.fourStackParticle, _obj[i].transform.position, Quaternion.identity);
                    Destroy(fourStackParticle, 1f);

                    //Play 4 stack sfx
                    AudioManager.singleton.PlaySfx(2);

                    //Destroy 4 stack objects
                    Destroy(_obj[i], .2f);
                }

                _obj.Clear();

                score += _score;
                OnScoreUpdate?.Invoke(score);
            }
        }
    }

    public void RemovePawnFromList(GameObject _pawnToRemove)
    {
        switch (_pawnToRemove.GetComponent<PawnsBehaviour>().GetPawnSO.pawnType)
        {
            case PawnType.ARook:
                ARookList.Remove(_pawnToRemove);
                break;
            case PawnType.BKnight:
                BKnightList.Remove(_pawnToRemove);
                break;
            case PawnType.CBishop:
                CBishopList.Remove(_pawnToRemove);
                break;
        }
    }
}
