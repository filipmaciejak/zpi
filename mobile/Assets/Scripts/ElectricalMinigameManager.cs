using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;

public class ElectricalMinigameManager : MonoBehaviour
{
    public Button button;
    public List<GameObject> leftCableList;
    public List<GameObject> rightCableList;
    public TouchHelper _touchHelper;

    private ClientManager _clientManager;
    private Finger _cableFinger;
    private GameObject _currentlyDrawnLine;
    private Dictionary<GameObject, bool> _isCableConnected;
    private Material _cableMaterial;
    

    private static System.Random rng = new System.Random();


    public void Awake()
    {
        //_clientManager = GameManager.Instance.clientManager;
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += OnFingerDown;
        ETouch.Touch.onFingerMove += OnFingerMove;
        ETouch.Touch.onFingerUp += OnFingerUp;
        button.EndedPress += OnButtonEndedPress;
    }

    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= OnFingerDown;
        ETouch.Touch.onFingerMove -= OnFingerMove;
        ETouch.Touch.onFingerUp -= OnFingerUp;
        button.EndedPress -= OnButtonEndedPress;

    }

    void Start()
    {
        _cableMaterial = (Material)Resources.Load("LineMaterial", typeof(Material));
        _isCableConnected = new Dictionary<GameObject, bool>();
        List<Color> colorList = new List<Color>()
        { Color.blue, Color.red, Color.green, Color.cyan, Color.magenta };
        Shuffle(colorList);
        for(int i=0; i< leftCableList.Count; i++)
        {
            leftCableList[i].GetComponent<SpriteRenderer>().color = colorList[i];
            _isCableConnected.Add(leftCableList[i], false);
        }
        Shuffle(colorList);
        for (int i = 0; i < rightCableList.Count; i++)
        {
            rightCableList[i].GetComponent<SpriteRenderer>().color = colorList[i];
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnFingerDown(Finger finger)
    {
        Debug.Log(finger.screenPosition);
        Debug.Log(_touchHelper.ScaleScreenToCanvas(finger.screenPosition));
        if (_cableFinger != null)
            return;

        GameObject startingCable = null;
        foreach(var cable in leftCableList)
        {
            if (_touchHelper.FingerInsideBounds(cable.GetComponent<SpriteRenderer>().bounds, finger))
            {
                startingCable = cable;
                cable.GetComponent<SpriteRenderer>().color=Color.yellow;
            }
        }

        if (startingCable == null || _isCableConnected[startingCable])
            return;
        
        _cableFinger = finger;

        var fingerPos = _touchHelper.ScaleScreenToCanvas(finger.screenPosition);

        _currentlyDrawnLine = new GameObject("Line");
        var lineRenderer = _currentlyDrawnLine.AddComponent<LineRenderer>();
        _cableMaterial.color = Color.red;
        lineRenderer.material = _cableMaterial;
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        var position = startingCable.GetComponent<SpriteRenderer>().bounds.center;
        lineRenderer.SetPosition(0, new Vector3(position.x, position.y, 0)); //x,y and z position of the starting point of the line
        OnFingerMove(finger);
    }

    private void OnFingerMove(Finger finger)
    {
        if (finger != _cableFinger)
            return;
        var fingerPos = _touchHelper.ScaleScreenToCanvas(finger.screenPosition);
        Debug.Log(fingerPos);
        if(_currentlyDrawnLine != null)
            _currentlyDrawnLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(fingerPos.x, fingerPos.y, 0));
    }

    private void OnFingerUp(Finger finger)
    {
        if (finger != _cableFinger)
            return;

        _cableFinger = null;

        //jesli palec jest w koncowym punkcie pozostaw linie i zaktualizuj punkt koncowy
        //jesli nie to usun linie
        if (_currentlyDrawnLine != null)
        {
            Destroy(_currentlyDrawnLine);
            _currentlyDrawnLine = null;
        }
    }

    void SendMinigameSuccess()
    {
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.UPDATE_MINIGAME.ToString() },
            { "repair", "1" },
        };

        _clientManager.SendDict(sentDict);
        EndMinigame();
    }

    void OnButtonEndedPress(string placeholder)
    {
        EndMinigame();
    }

    void EndMinigame()
    {
        SceneManager.LoadScene("MovementScene");
        Dictionary<string, string> sentDict = new Dictionary<string, string>
        {
            { "event", MessageEvent.ABORT_MINIGAME.ToString() },
        };

        _clientManager.SendDict(sentDict);
    }

    
    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
