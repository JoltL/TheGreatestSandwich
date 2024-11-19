using PixelSunsetStudio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerInputs : MonoBehaviour
{
    // Start is called before the first frame update

    Vector2 m_currentVector;

    public event Action<Vector2> OnSwipe;

    private void OnEnable()
    {
        SimpleSwipeDetector.OnSwipeUp += SwipeUp;
        SimpleSwipeDetector.OnSwipeDown += SwipeDown;
        SimpleSwipeDetector.OnSwipeLeft += SwipeLeft;
        SimpleSwipeDetector.OnSwipeRight += SwipeRight;
    }
    private void OnDisable()
    {
        SimpleSwipeDetector.OnSwipeUp -= SwipeUp;
        SimpleSwipeDetector.OnSwipeDown -= SwipeDown;
        SimpleSwipeDetector.OnSwipeLeft -= SwipeLeft;
        SimpleSwipeDetector.OnSwipeRight -= SwipeRight;
    }

    void SwipeUp()
    {
        SetCurrentVector(Vector2.up);
    }
    void SwipeDown()
    {
        SetCurrentVector(Vector2.down);
    }
    void SwipeLeft()
    {
        SetCurrentVector(Vector2.left);
    }
    void SwipeRight()
    {
        SetCurrentVector(Vector2.right);
    }

    public void SetCurrentVector(Vector2 newVect)
    {
        m_currentVector = newVect;
        OnSwipe?.Invoke(m_currentVector);
    }
}
