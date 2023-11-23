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
    private GameObject _startingCable;
    private int _numberOfConnectedCables;
    

    private static System.Random rng = new System.Random();


    public void Awake()
    {
        _clientManager = GameManager.Instance.clientManager;
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
        _numberOfConnectedCables = 0;
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
        if(_numberOfConnectedCables >= _isCableConnected.Count)
        {
            SendMinigameSuccess();
            EndMinigame();
        }
    }

    private void OnFingerDown(Finger finger)
    {
        if (_cableFinger != null)
            return;

        _startingCable = null;
        foreach(var cable in leftCableList)
        {
            if (_touchHelper.FingerInsideBounds(cable.GetComponent<SpriteRenderer>().bounds, finger))
            {
                _startingCable = cable;
            }
        }

        if (_startingCable == null || _isCableConnected[_startingCable])
            return;
        
        _cableFinger = finger;

        var fingerPos = _touchHelper.ScaleScreenToCanvas(finger.screenPosition);

        _currentlyDrawnLine = new GameObject("Line");
        var lineRenderer = _currentlyDrawnLine.AddComponent<LineRenderer>();
        lineRenderer.material = _startingCable.GetComponent<SpriteRenderer>().material;
        lineRenderer.startColor = _startingCable.GetComponent<SpriteRenderer>().color;
        lineRenderer.endColor = _startingCable.GetComponent<SpriteRenderer>().color;
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;

        var position = _startingCable.GetComponent<SpriteRenderer>().bounds.center;
        lineRenderer.SetPosition(0, new Vector3(position.x, position.y, -_numberOfConnectedCables)); //x,y and z position of the starting point of the line
        OnFingerMove(finger);
    }

    private void OnFingerMove(Finger finger)
    {
        if (finger != _cableFinger)
            return;
        var fingerPos = Camera.main.ScreenToWorldPoint(finger.screenPosition);
        if(_currentlyDrawnLine != null)
        {
            _currentlyDrawnLine.GetComponent<LineRenderer>().SetPosition(1, new Vector3(fingerPos.x, fingerPos.y, -_numberOfConnectedCables));
        }
    }

    private void OnFingerUp(Finger finger)
    {
        if (finger != _cableFinger)
            return;

        _cableFinger = null;

        bool isSuccess = false;
        foreach (var cable in rightCableList)
        {
            if (_touchHelper.FingerInsideBounds(cable.GetComponent<SpriteRenderer>().bounds, finger))
            {
                _currentlyDrawnLine.GetComponent<LineRenderer>().SetPosition(1, cable.GetComponent<SpriteRenderer>().bounds.center);
                _isCableConnected[_startingCable] = true;
                isSuccess = true;
                ++_numberOfConnectedCables;
            }
        }
        if (!isSuccess && _currentlyDrawnLine != null)
        {
            Destroy(_currentlyDrawnLine);            
        }
        _currentlyDrawnLine = null;
        _startingCable = null;
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
