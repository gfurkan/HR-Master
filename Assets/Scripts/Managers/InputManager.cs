using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    #region Singleton

    private static InputManager _Instance = null;
    public static InputManager Instance
    {
        get
        {
            return _Instance;
        }
    }
    private void Awake()
    {
        if (_Instance == null)
        {
            _Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    #endregion

    #region Fields

    private bool _clicked = false, _dragging = false;
    private Vector3 _touchFirstPos, _touchNextPos, _directionVec;

    #endregion

    #region Properties

    public bool clicked => _clicked;
    public bool dragging => _dragging;

    public Vector3 touchFirstPos => _touchFirstPos;
    public Vector3 touchNetPos => _touchNextPos;
    public Vector3 directionVec
    {
        get
        {
            return _directionVec;
        }
        set
        {
            _directionVec = value;
        }
    }

    #endregion

    public void OnBeginDrag(PointerEventData eventData)
    {
        _dragging = true;
        _touchFirstPos = eventData.position;
    }
    public void OnDrag(PointerEventData eventData)
    {
        _touchNextPos = eventData.position;
        _directionVec = _touchNextPos - _touchFirstPos;
        _touchFirstPos = _touchNextPos;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _dragging = false;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _clicked = true;
    }
    
}
