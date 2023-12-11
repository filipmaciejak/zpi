using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBar : MonoBehaviour
{
    public Color noPlayerColor;
    public Color playerOneColor;
    public Color playerTwoColor;
    public Color playerThreeColor;
    public Color playerFourColor;

    private Dictionary<int, Color> _playerColorMapping;
    
    private ClientManager _clientManager;
    public Image IndicatorImage;
    
    public void Awake()
    {
        _clientManager = GameManager.Instance.clientManager;
        IndicatorImage = GetComponent<Image>();
        
        // an unfortunate way to serialize colors in the unity editor
        _playerColorMapping = new Dictionary<int, Color>
        {
            { -1, noPlayerColor },
            { 0, playerOneColor },
            { 1, playerTwoColor },
            { 2, playerThreeColor },
            { 3, playerFourColor }
        };
    }

    public void Start()
    {
        IndicatorImage.color = _playerColorMapping[_clientManager.playerId];
    }
}
