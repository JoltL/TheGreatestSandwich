using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CutFeedBack : MonoBehaviour
{
    [SerializeField] GameObject m_cutGameObject;
    [SerializeField] int m_cutNum = 2;
    void Start()
    {

        for (int i = 0;i < m_cutNum;i++)
        {
            int dir = 0;
            if(i == 0)
            {
                dir = 1;
                m_cutGameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
            }
            else
            {
                dir = -1;
                m_cutGameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
 
            GameObject cutInst = Instantiate(m_cutGameObject, transform.position, transform.rotation);
            Rigidbody2D cutRb = cutInst.GetComponent<Rigidbody2D>();
            cutRb.AddForce(new Vector3(dir, 1,1)*2,ForceMode2D.Impulse); //TODO CHANGE HARD VALUE
            cutRb.AddTorque(Random.Range(-100, 100)*10);
            Destroy(cutInst,5f);
            Debug.Log(i +"= "+dir);
        }     
    }

    #region
    public void SetCutVisual(Sprite newCutSprite)
    {
        m_cutGameObject.GetComponentInChildren<SpriteRenderer>().sprite = newCutSprite;
    }
    #endregion
}
