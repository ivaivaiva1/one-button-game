using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Over : MonoBehaviour
{

    [SerializeField]
    private GameObject overPanel;

    [SerializeField]
    private TMP_Text scoreTXT;

    [SerializeField]
    private TMP_Text highTXT;

    [SerializeField]
    private GameObject sceneTexts;


    private bool canChange = false;

    public void GameOver()
    {
       print("gameover");
       if(Base.HighScore < Base.PlayerPoints)
       {
          Base.HighScore = Base.PlayerPoints;
          PlayerPrefs.SetInt("highscorePref", Base.HighScore);
       } 
       overPanel.SetActive(true);
       scoreTXT.text = "Score  "
       + 
       Base.PlayerPoints.ToString();

       highTXT.text = "High  "
       + 
       Base.HighScore.ToString();
    }

    public void canScene()
    {
      StartCoroutine("callSceneManager");
    }

      IEnumerator callSceneManager()
    {
       yield return new WaitForSeconds(1f);
       canChange = true;
       sceneTexts.SetActive(true);
    }

    private const float _minimumHeldDuration = 0.25f;
    private float _spacePressedTime = 0;
    private bool _spaceHeld = false;

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space) && canChange == true) 
        {
          // Use has pressed the Space key. We don't know if they'll release or hold it, so keep track of when they started holding it.
          _spacePressedTime = Time.timeSinceLevelLoad;
          _spaceHeld = false;
        } else 
        if (Input.GetKeyUp(KeyCode.Space) && canChange == true)
        {
        if (Time.timeSinceLevelLoad - _spacePressedTime > _minimumHeldDuration) 
        {
          // Player has held the Space key for .25 seconds. Consider it "held"
          SceneManager.LoadScene(0);
          _spaceHeld = true;
        } else
        if (!_spaceHeld && canChange == true) 
        {
           SceneManager.LoadScene(1);
          // Player has released the space key without holding it.
          // TODO: Perform the action for when Space is pressed.
        }
        _spaceHeld = false;
        }
    }
    
}
