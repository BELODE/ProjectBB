using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardController : MonoBehaviour
{
    public bool CharChange = false;
    public GameObject changeUI;
    public int index = 1;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerChange();
        }
        if (CharChange == true)
        {
            changeUI.SetActive(true);
        }
    }

    public void PlayerChange()
    {
        Animator anim = GameObject.Find("Player").GetComponent<Animator>();
        anim.runtimeAnimatorController = gameObject.GetComponent<AnimationContainer>().ani[index];
        if (index == 0)
        {
            index++;
        }else if (index == 1)
        {
            index--;
        }
    }
}
