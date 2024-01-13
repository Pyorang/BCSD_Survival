using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Water : MonoBehaviour
{

    [SerializeField]
    private float waterDrag;    //�� �� �߷�
    private float originDrag;

    [SerializeField]
    private Color waterColor;   //���� ����
    [SerializeField]
    private float waterFogDensity;  //�� Ź�� ����.

    [SerializeField]
    private Color waterNightColor;      //�� ������ ���� ����.
    [SerializeField]
    private float waterNightFogDensity;

    private Color originColor;
    private float originFogDensity;

    [SerializeField]
    private Color originNightColor;
    [SerializeField]
    private float originNightFogDensity;

    [SerializeField]
    private string sound_WaterOut;
    [SerializeField]
    private string sound_WaterIn;
    [SerializeField]
    private string sound_WaterBreathe;

    [SerializeField]
    private float breatheTime;
    private float currentBreatheTime;

    [SerializeField]
    private float totalOxygen;
    private float currentOxygen;
    private float temp;

    [SerializeField]
    private GameObject go_BaseUi;
    [SerializeField]
    private Text text_totalOxygen;
    [SerializeField]
    private Text text_currentOxygen;
    [SerializeField]
    private Image image_gauge;

    private Status_Controller thePlayerStat;

    // Start is called before the first frame update
    void Start()
    {
        originColor = RenderSettings.fogColor;
        originFogDensity = RenderSettings.fogDensity;

        originDrag = 0;
        thePlayerStat = FindObjectOfType<Status_Controller>();
        currentOxygen = totalOxygen;
        text_totalOxygen.text = totalOxygen.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.isWater)
        {
            currentBreatheTime += Time.deltaTime;
            if(currentBreatheTime > breatheTime)
            {
                SoundManager.instance.PlaySE(sound_WaterBreathe);
                currentBreatheTime = 0;
            }
        }

        DecreaseOxygen();

    }

    private void DecreaseOxygen()
    {
        if(GameManager.isWater)
        {
            currentOxygen -= Time.deltaTime;
            text_currentOxygen.text = Mathf.Round(currentOxygen).ToString();

            image_gauge.fillAmount = currentOxygen / totalOxygen;

            if(currentOxygen < 0)
            {
                temp += Time.deltaTime;
                if(temp>=1)
                {
                    thePlayerStat.DecreaseHP(1);
                    temp = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetWater(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            GetOutWater(other);
        }
    }

    private void GetWater(Collider _player)
    {
        SoundManager.instance.PlaySE(sound_WaterIn);

        go_BaseUi.SetActive(true);

        GameManager.isWater = true;
        _player.transform.GetComponent<Rigidbody>().drag = waterDrag;
        
        if(!GameManager.isNight)
        {
            RenderSettings.fogColor = waterColor;
            RenderSettings.fogDensity = waterFogDensity;
        }
        else
        {
            RenderSettings.fogColor = waterNightColor;
            RenderSettings.fogDensity = waterNightFogDensity;
        }
    }

    private void GetOutWater(Collider _player)
    {

        if (GameManager.isWater)
        {
            go_BaseUi.SetActive(false);
            currentOxygen = totalOxygen;
            SoundManager.instance.PlaySE(sound_WaterOut);

            GameManager.isWater = false;
            _player.transform.GetComponent<Rigidbody>().drag = originDrag;

            if(!GameManager.isNight)
            {
                RenderSettings.fogColor = originColor;
                RenderSettings.fogDensity = originFogDensity;
            }
            else
            {
                RenderSettings.fogColor = originNightColor;
                RenderSettings.fogDensity = originNightFogDensity;
            }
        }
    }
}
