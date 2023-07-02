using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manu : MonoBehaviour
{

    [SerializeField]
    private GameObject credits;

    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject instructions;

    private int tapTimes = 0;

    private bool CreditoOn = false;

    private bool InstructionsOn = false;

    [SerializeField]
    private AudioSource keyboard;

    private const float _minimumHeldDuration = 0.25f;
    private float _spacePressedTime = 0;
    private bool _spaceHeld = false;

     private void HeldKey()
   {
    if (Input.GetKeyDown(KeyCode.Space)) 
    {
      // Use has pressed the Space key. We don't know if they'll release or hold it, so keep track of when they started holding it.
      _spacePressedTime = Time.timeSinceLevelLoad;
      _spaceHeld = false;
    } else 
     if (Input.GetKeyUp(KeyCode.Space) && InstructionsOn == false && CreditoOn == false)
     {
       if (Time.timeSinceLevelLoad - _spacePressedTime > _minimumHeldDuration) 
       {
         // Player has held the Space key for .25 seconds. Consider it "held"
         _spaceHeld = true;
         if(InstructionsOn == false)
         {
            mainMenu.SetActive(false);
            instructions.SetActive(true);
            InstructionsOn = true;
            tapTimes = 0;
         }
       } else
       if (!_spaceHeld && InstructionsOn == false && CreditoOn == false) 
       {
         tapTimes = tapTimes + 1;
         if(tapTimes == 1)
         {
         StartCoroutine("doGame");
         }
         // Player has released the space key without holding it.
         // TODO: Perform the action for when Space is pressed.
       }
       _spaceHeld = false;
      }
    }

    IEnumerator doGame()
    {
        yield return new WaitForSeconds(0.3f);
        if(CreditoOn == false)
        {
           tapTimes = 0;
           SceneManager.LoadScene(1);
        } else
        tapTimes = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HeldKey();
        if(Input.GetKeyDown(KeyCode.Space))
        {
           if(InstructionsOn == true)
           {
              tapTimes = -1;
              instructions.SetActive(false);
              mainMenu.SetActive(true);
              InstructionsOn = false;
           }
           if(CreditoOn == true)
           {
              tapTimes = -1;
              credits.SetActive(false);
              mainMenu.SetActive(true);
              CreditoOn = false;
           }
        }
        if(tapTimes > 1)
        {
           mainMenu.SetActive(false);
           credits.SetActive(true);
           CreditoOn = true;
           tapTimes = 0;
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
           keyboard.Play();
        }
    }
}
