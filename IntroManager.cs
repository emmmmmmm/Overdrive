using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class IntroManager : MonoBehaviour {

    [FMODUnity.EventRef]
    public string soundSelectEvent;
    [FMODUnity.EventRef]
    public string soundHoverEvent;
    private bool isLoading = false;
    private bool start = false;
    public GameObject loadingScreen;
    public Text loadingText;
    public Text tipsText;
    public GameObject Buttons;
    public Animation anim;
    private string[] tips;
    private int thisTip;
    public int startTimeOut = 3;
    public float menuTime = 0;
    public bool menuActive = false;
    private void Start()
    {
        Cursor.visible = false;
        loadingScreen.SetActive(false);
        Time.timeScale = 1; // needs to be reset when returning from pause screen!

        setTipsMessage();
        
    }
    //------------------------------------------------
    private void Update()
    {
        menuTime += Time.deltaTime;
        if (menuTime > 0.5f) menuActive = true;
        if (Input.anyKeyDown) {
            setTipsMessage();
        }
        if (start && !isLoading) {
            isLoading = true;
            StartCoroutine(StartGameRoutine());
        }
        if (isLoading) {
            loadingScreen.SetActive(true);
            Buttons.SetActive(false);
          
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
    }
    //------------------------------------------------
    public void StartGame(){
        start = true;
        isLoading = false;
        anim.Rewind();
        anim.Play();
        FMODUnity.RuntimeManager.PlayOneShot(soundSelectEvent);
      
    }

    //------------------------------------------------
    public void QuitGame(){
		StartCoroutine (QuitGameRoutine ());
        anim.Rewind();
        anim.Play();
        Buttons.SetActive(false);
        FMODUnity.RuntimeManager.PlayOneShot(soundSelectEvent);

    }
    //------------------------------------------------
    public IEnumerator StartGameRoutine(){

        //        GetComponent<Animator>().SetTrigger("animateLight");
		yield return new WaitForSeconds (startTimeOut);
        AsyncOperation async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        while (!async.isDone)
            yield return null;


    }
    //------------------------------------------------
    public IEnumerator QuitGameRoutine()
    {
        yield return new WaitForSeconds (1);
		Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }
    //------------------------------------------------
    public void OnHover()
    {
        if(menuActive)
            FMODUnity.RuntimeManager.PlayOneShot(soundHoverEvent);
    }
    //------------------------------------------------
    void setTipsMessage()
    {
        // set the tips-message!
        tips = new string[4];
        tips[0] = "Press 'A' to unsuspend Gravity!";
        tips[1] = "Use pitching (right stick) to stay on the track.";
        tips[2] = "Straving (right stick) helps you to position yourself on the track.";
        tips[3] = "Theoretically you could use your breaks (left trigger)";
        thisTip = (int)Random.Range(0, tips.Length ); // pick at random
        tipsText.text = tips[thisTip];
    }
    //------------------------------------------------
    public void deselectButton(Button button) {
      //  EventSystem.current.SetSelectedGameObject(null);
       // button.OnDeselect(null);
        button.enabled = false;
        button.enabled = true;
        button.GetComponent<Animator>().SetTrigger("Normal");
        //Why doesn't anything of this work!?
    }
    //------------------------------------------------
}
