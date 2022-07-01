using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class MyHelpers
{
    // Camera Reference:
    // Store reference to camera so its only called once
    private static Camera _camera;

    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }


    // Non-Allocating WaitForSeconds
    // Reduce garbage collection by reusing WaitForSeconds if same wait time exists
    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }


    // Is Pointer Over UI?
    // Detects if cursor or touch is over any UI element
    // example: _text.text = Helpers.IsOverUI() ? "Over UI" : "Not Over UI";
    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;

    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }


    // Find World Point Of Canvas Element
    // example: transform.position = Helpers.GetWorldPositionOfCanvasElement(target)
    public static Vector2 GetWorldPosOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }


    // Destroy All Child Objects
    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }

}
