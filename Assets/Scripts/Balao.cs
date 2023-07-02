using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Balao : MonoBehaviour
{
  
    [SerializeField]
    private TMP_Text lifeTXT;

    [SerializeField]
    private float bloonSpeed;

    [SerializeField]
    private float SpeedBase;

    [SerializeField]
    private int maxLife;

    [SerializeField]
    private float porcentLife;

    public bool inGame;

    [SerializeField]
    private int randomLifeMax;

    [SerializeField]
    private int randomLifeMin;

    public int life = 0;

    public bool canActivate = true;

    private SpriteRenderer sprite;

    [SerializeField]
    private int layerP;

    [SerializeField]
    private Canvas canvasB;

    IEnumerator ActivateTimer()
    {
        yield return new WaitForSeconds(0.2f);
        canActivate = true;
    }

     void Start() 
     {
         sprite = GetComponent<SpriteRenderer>();
         life = Random.Range(randomLifeMin, randomLifeMax);
         maxLife = life;
         SpeedBase = bloonSpeed;
     }


    // Start is called before the first frame update
    void Update()
    {
        NewLife();
        ChangeVelocity();
        BloonAnim();
        lifeTXT.text = life.ToString();
        if(life == 0 && inGame == false)
        {
           Base.RemoveBloon(this);
        }
    }

    private void BloonAnim()
    {
        if(inGame == true)
        {
           canvasB.sortingOrder = 10;
           sprite.sortingOrder = 10;
           lifeTXT.color = Color.white;
        } else
        {
          canvasB.sortingOrder = layerP;
          sprite.sortingOrder = layerP;
          lifeTXT.color = Color.red;
        }        
    }

    private void NewLife()
    {
        if(life < 0)
        {
          life = maxLife;
        }
    }

    private void ChangeVelocity()
    {
       porcentLife = (maxLife - life) * 100 / maxLife;
       bloonSpeed = SpeedBase * (1 + 0.02f * porcentLife); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0f, -bloonSpeed, 0f, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D col) 
    {
        if(life == 0)
        {
           Base.PlayerPoints = Base.PlayerPoints + 1;
           FindObjectOfType<Base>().ChangeDificcult();
        } else 
        {
           FindObjectOfType<Base>().damageSoundV();
           Base.PlayerLife = Base.PlayerLife - life;
           if(Base.PlayerLife <= 0)
           {
           Base.gameisON = false;
           FindObjectOfType<Base>().overSoundV();
           FindObjectOfType<Over>().canScene();
           FindObjectOfType<Over>().GameOver();
           }
        }
        Base.RemoveBloon(this);
        Destroy(this.gameObject);
    }
}
