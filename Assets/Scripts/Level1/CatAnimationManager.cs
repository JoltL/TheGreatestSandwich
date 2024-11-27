using PixelSunsetStudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationManager : MonoBehaviour
{
    [SerializeField] Animator m_animator;
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        IngredientEntity.OnHCut += HorizontalSlice;
        IngredientEntity.OnVCut += VerticalSlice;   
    }
    private void OnDisable()
    {
        IngredientEntity.OnHCut -= HorizontalSlice;
        IngredientEntity.OnVCut -= VerticalSlice;
        //SimpleSwipeDetector.OnSwipeUp -= VerticalSlice;
        //SimpleSwipeDetector.OnSwipeDown -= VerticalSlice;
        //SimpleSwipeDetector.OnSwipeLeft -= HorizontalSlice;
        //SimpleSwipeDetector.OnSwipeRight -= HorizontalSlice;
    }
    void Update()
    {
        
    }

    void HorizontalSlice()
    {
        m_animator.SetTrigger("HSliced");
        GetComponent<Oscillator>().StartOscillator(1);
    }

    void VerticalSlice() 
    {
        m_animator.SetTrigger("VSliced");
        GetComponent<Oscillator>().StartOscillator(1);
    }

}
