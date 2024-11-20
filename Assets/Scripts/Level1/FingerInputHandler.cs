using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerInputHandler : MonoBehaviour
{


    bool m_canTouch = true;
    Vector3 m_slideVector;
    Vector3 m_firstPoint;
    Vector3 m_secondPoint;

    void Start()
    {
        
    }








    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (m_canTouch)
            {
                m_canTouch = false;
                m_firstPoint = pos;
               
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_canTouch = true;
            m_secondPoint = pos;
            m_slideVector = (m_secondPoint - m_firstPoint).normalized;
            //Vector3 vectDebug = new Vector3(Mathf.Sign(m_slideVector.x), Mathf.Sign(m_slideVector.y));
           
            Debug.Log(ClampVector(m_slideVector));
        }
        
        Debug.DrawLine(m_firstPoint, m_secondPoint, Color.cyan);

    }

    Vector3 ClampVector(Vector3 vector)
    {
        // Si x ou y est inférieur à 1 (en absolu), définir à 0
        vector.x = Mathf.Abs(vector.x) < 1 ? 0 : vector.x;
        vector.y = Mathf.Abs(vector.y) < 1 ? 0 : vector.y;

        return vector;
    }
}
