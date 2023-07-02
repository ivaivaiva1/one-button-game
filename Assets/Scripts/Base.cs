using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{

   public AudioSource damageSound, overSound, keyboard;
    
    public static int PlayerLife = 30;

    public static int PlayerPoints = 0;

    public int RandomBloon = 1;

    [SerializeField]
    private List<Balao> baloes;

    [SerializeField]
    private List<GameObject> balaoPrefab;

    private static Base instance;

   [SerializeField]
    private List<GameObject> BloonSpot;

    [SerializeField]
    private Balao bloonIn;

    [SerializeField]
    private List<bool> CanSpot;

    [SerializeField]
    private float TimeSpawn;

    private const float _minimumHeldDuration = 0.25f;
    private float _spacePressedTime = 0;
    private bool _spaceHeld = false;

    public static bool gameisON = true;

    public static int HighScore;

   public void damageSoundV()
   {
      if(gameisON == true)
      {
        damageSound.Play();
      }
   }

   public void overSoundV()
   {
      overSound.Play();
   }

   private void HeldKey()
   {
    if (Input.GetKeyDown(KeyCode.Space) && gameisON == true) 
    {
      // Use has pressed the Space key. We don't know if they'll release or hold it, so keep track of when they started holding it.
      _spacePressedTime = Time.timeSinceLevelLoad;
      _spaceHeld = false;
    } else 
     if (Input.GetKeyUp(KeyCode.Space) && gameisON == true)
     {
       if (Time.timeSinceLevelLoad - _spacePressedTime > _minimumHeldDuration) 
       {
         // Player has held the Space key for .25 seconds. Consider it "held"
         _spaceHeld = true;
         ActivateBloon();
       } else
       if (!_spaceHeld && gameisON == true) 
       {
         if(bloonIn != null)
         {
           bloonIn.life = bloonIn.life - 1; 
         }   
         // Player has released the space key without holding it.
         // TODO: Perform the action for when Space is pressed.
       }
       _spaceHeld = false;
      }
    }

    // Start is called before the first frame update
    void Start()
    {
        HighScore = PlayerPrefs.GetInt("highscorePref");
        gameisON = true;
        PlayerLife = 30;
        PlayerPoints = 0;
        instance = this;
        SpawnBloon();
    }

    // Update is called once per frame
    void Update()
    {  
         HeldKey();
         if(Input.GetKeyDown(KeyCode.Space))
         {
            keyboard.Play();
         }
         if(PlayerLife < 0)
         {
            PlayerLife = 0;
         }
    }

    IEnumerator CallSpawnBloon()
    {
        yield return new WaitForSeconds(TimeSpawn);
        if(gameisON == true)
        {
           SpawnBloon();
        }  
    }
  
    public void FindBloonIn()
    {
      for(var i = 0; i<baloes.Count; i++)
      {
         if(baloes[i].inGame == true)
         {
            bloonIn = baloes[i];
            return;
         } 
      }
    }

    private void SpawnBloon()
    {
      for(var i = 0; i<BloonSpot.Count;)
      {
         i = Random.Range(0,4);
         if(CanSpot[i] == true)
         {
            CanSpot[i] = false;
            var obj = Instantiate(balaoPrefab[Random.Range(0,RandomBloon)], BloonSpot[i].transform.position, Quaternion.identity).GetComponent<Balao>();
            baloes.Add(obj);
            StartCoroutine("CallSpawnBloon");
            StartCoroutine("CanSpotTrue", i);
            return;
         } 
      } 
    }

    public void ChangeDificcult()
    {
      switch(PlayerPoints)
          {
            case 5:
            print("Change5");
            RandomBloon = 2;
            TimeSpawn = 2.8f;
            break;
            case 15:
            print("Change12");
            RandomBloon = 3;
            TimeSpawn = 2.5f;
            break;
            case 25:
            print("Change19");
            RandomBloon = 4;
            TimeSpawn = 2.2f;
            break;
            case 35:
            print("Change26");
            RandomBloon = 5;
            TimeSpawn = 1.9f;
            break;
            case 45:
            print("Change33");
            RandomBloon = 6;
            TimeSpawn = 1.6f;
            break;
          }   
    }

    IEnumerator CanSpotTrue(int S)
    {
        yield return new WaitForSeconds(TimeSpawn * 1.9f);
        CanSpot[S] = true;
    }

    public void ActivateBloon()
    {
       var lowest = GetLowestBloon();
       foreach(var b in baloes)
       {
          if(lowest == null)
          {
             bloonIn = null;
             b.inGame = false;
             return;
          }
          b.inGame = false;
       }
       if(lowest != null)
       {
         lowest.inGame = true;
         FindBloonIn();
       }   
    }  

    public Balao GetLowestBloon()
    {
       Balao lowest = null;
       foreach(var b in baloes)
       {
          if(b.inGame == true ) continue;
          if(lowest == null || b.transform.position.y < lowest.transform.position.y)
             {
               lowest = b;
             }
          }
          return lowest;
    }

    public static void RemoveBloon(Balao b)
    {
       instance.baloes.Remove(b);
    }
}
