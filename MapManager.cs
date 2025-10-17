using Script.Assets.Script.Common.Generic;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


namespace Script.Assets.Script.Manager
{

    /// <summary>
    /// classe per gestire la mappa
    /// </summary>
    public class MapManager : MonoBehaviour
    {
        public static MapManager Instance;
        private List<Image> _roomSprites;
        private Image _positionPointer;
        private bool _visiblePointer;
        private GameObject _canvas;
        private int _cardboardCompleted;
        private Sprite _lastPosition;

        [SerializeField]
        private bool _isTutorial;
        /// <inheritdoc/>
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Nel metodo Start si ottengono la lista delle stanze e il pointer della posizione
        /// Se ci troviamo in un livello tutorial la mappa non sarà visualizzata
        /// se no tutti gli sprite delle stanze e il pointer vengono inizializzati con alpha a 0
        /// </summary>
        private void Start()
        {
            _roomSprites = gameObject.transform.GetChild(1).GetComponentsInChildren<Image>().ToList();
            _positionPointer = gameObject.transform.GetChild(2).GetComponent<Image>();
            _canvas = gameObject;
            if (_isTutorial)
            {
                _canvas.SetActive(false);
                _visiblePointer = true;
            }
            else
            {
                foreach (Image child in _roomSprites)
                {
                    child.GetComponent<Image>().color = new Color(255, 255, 255, 0);
                }
                _positionPointer.color = new Color(255, 0, 0, 0);
                _visiblePointer = false;
            }
        }

        /// <summary>
        /// Metodo che cicla la lista di sprite dei frame che abbiamo sbloccato data in input
        /// e li imposta nella mappa, Se il pointer è invisible lo rende visibile
        /// </summary>
        /// <param name="spriteList">Lista di sprite dei frame unlocked</param>
        public void SetMapImages(List<Sprite> spriteList)
        {
            if (!_visiblePointer)
            {
                _positionPointer.color = new Color(255, 0, 0, 255);
                _visiblePointer = true;
            }

            for (int i = 0; i < _roomSprites.Count; i++)
            {
                if (i < spriteList.Count)
                {
                    _roomSprites[i].GetComponent<Image>().sprite = spriteList[i];
                    _roomSprites[i].GetComponent<Image>().color = new Color(255, 255, 255, 255);
                }
                else
                {
                    _roomSprites[i].GetComponent<Image>().sprite = null;
                    _roomSprites[i].GetComponent<Image>().color = new Color(255, 255, 255, 0);
                }
            }
        }

        /// <summary>
        /// Metodo che gestisce la posizione del puntatore nella mappa
        /// </summary>
        /// <param name="frame">Frame della stanza in cui siamo entrati</param>
        public void SetPositionPointer(Frame frame)
        {
            Sprite frameSprite;

            if (frame != null)
            {
                frameSprite = frame.GetComponentInChildren<SpriteRenderer>().sprite;
                _lastPosition = frameSprite;
            }
            else
            {
                frameSprite = _lastPosition;
            }

            for (int i = 0; i < _roomSprites.Count; i++)
            {
                if (_roomSprites[i].sprite == frameSprite)
                {
                    _positionPointer.transform.position = new Vector3(_positionPointer.transform.position.x, _roomSprites[i].transform.position.y, _positionPointer.transform.position.z);
                }
            }
        }

        /// <summary>
        /// Metodo che verifica se tutti i puzzle cartonati sono stati risolti
        /// Quando si raggiunge il quinto puzzle e se la mappa è disattivata attiva la mappa 
        /// </summary>
        public void CheckCardboardCompleted()
        {
            _cardboardCompleted++;
            if (_cardboardCompleted == RoomManager.Instance.RoomCount() && !_canvas.activeSelf)
            {
                _canvas.SetActive(true);
            }
        }
    }
}