using UnityEngine;
using UnityEngine.UI;


public class GameUI : MonoBehaviour
{

    public GameObject PositionText;
    public GameObject LapText;
    public GameObject TimeText;
    public GameObject DirectionText;
    public GameObject OverDriveBoostText;

    public Slider speedSlider;
    public Slider healthSlider;
    public Slider boostSlider;


    public RaceManagerSingleton RM;
    public Vehicle VS;
    private Player player;
    public CurrentPositionManager PS;
    public Compass compass;


    // WHY isnt this called when in Start method!? I have NO idea...
    void Awake()
    {
        player = VS.GetComponent<Player>();
        if (!player) Debug.LogError("no Player!?");
    }

    // Update is called once per frame
    public void UpdateUI()
    {

        PositionText.GetComponent<Text>().text = "Pos: " + PS.currentPosition + " / " + RM.getHighScore().Length;
        LapText.GetComponent<Text>().text = "Lap: " + (PS.currentLap + 1) + " / " + RM.laps;
        TimeText.GetComponent<Text>().text = "Lap:      " + PS.currentLapTime.ToString("F4") + " \nTotal: " + PS.totalTime.ToString("F4") + " ";

        speedSlider.SetMax(player.vehicle.maxSpeed);
        speedSlider.SetValue(VS.currentSpeed);

        healthSlider.SetMax(player.vehicle.maxHealth);
        healthSlider.SetValue(player.health);

        boostSlider.SetMax(player.vehicle.maxOverDriveBoost);
        boostSlider.SetValue(VS.overDriveBoost);

        if (boostSlider.current >= boostSlider.max) OverDriveBoostText.SetActive(true);
        else OverDriveBoostText.SetActive(false);



        if (PS.wrongDirection)
            DirectionText.SetActive(true);
        else
            DirectionText.SetActive(false);


        compass.updateCompass(
            VS.transform,
            VS.GetComponent<CurrentPositionManager>().GetNextWayPoint()
            );

    }
}