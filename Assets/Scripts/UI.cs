using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI : MonoBehaviour
{

    [SerializeField]
    private TMP_Text playerLifeTXT;

    [SerializeField]
    private TMP_Text playerPointsTXT;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerLifeTXT.text = Base.PlayerLife.ToString();
        playerPointsTXT.text = Base.PlayerPoints.ToString();
    }
}
