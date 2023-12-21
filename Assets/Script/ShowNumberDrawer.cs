using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class ShowNumberDrawer : MonoBehaviour
{
    [SerializeField] BingoController bingo;
    private RectTransform textTransfrom;
    private Vector3 fromPosition = new Vector3(0, 900, 0);
    private Vector3 toPosition = new Vector3(0, -900, 0);
    private Sequence sequence;
    private TextMeshProUGUI textMesh;
    private float slotTime;
    private int count;
    private int targetNum;

   //////////////////////////////////
    //表示のアニメーションに関するフィールド

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Transform fadeTransForm;
    [SerializeField] private CanvasGroup spotLightFadeGroup;

    [SerializeField] private AudioSource symbal;
    [SerializeField] private AudioSource drum;

    public GameObject ShowNumberPrefab;
    [SerializeField] private TextMeshProUGUI textTmp;

    //////////////////////////////////

    const int MAX_COUNT = 6;
    const float TIME_DELAU_MULTI = 1.20f;
    public float reqTime = 0.0f;

    void Start(){
        textTransfrom = gameObject.GetComponent<RectTransform>();
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();

        slotTime = 0.25f;
        count = 0;
        fadeCanvasGroup.alpha = 0.0f;

        float tmpTime = slotTime;
        for(int i = 0; i < MAX_COUNT; i++){
            reqTime += tmpTime + 0.2f;
            tmpTime *= TIME_DELAU_MULTI; 
        }
    }

    public void SetSlot(int target){
        slotTime = 0.25f;
        count = 0;

        spotLightFadeGroup.alpha = 0.0f;

        textTransfrom.anchoredPosition = fromPosition;
        targetNum = target;
        fadeCanvasGroup.DOFade(1.0f, 1.0f).OnComplete(() => {
            SlotNumberAnimation();

            drum.volume = 1.0f;

            drum.Play();
            drum.DOFade(0.0f, reqTime).SetEase(Ease.Linear);
        });
    }
 
    public void ChangeSlotNumber(){
        int rand = UnityEngine.Random.Range(1, 76);
        textMesh.text = rand.ToString();
    }

    public void SlotNumberAnimation(){
        if (count < MAX_COUNT){
            textTransfrom.anchoredPosition = fromPosition;
            ChangeSlotNumber();
            textTransfrom.DOAnchorPos(toPosition, slotTime).SetEase(Ease.Linear).SetDelay(0.2f).OnComplete(() => {
                count++;
                SlotNumberAnimation();
            });
            slotTime *= TIME_DELAU_MULTI;
        } else {
            ShowNumberAnimation();
        }
    }

    public void ChangeShowNumber(){
        textMesh.text = targetNum.ToString();
    }

    public void ShowNumberAnimation(){
        textTransfrom.anchoredPosition = fromPosition;
        ChangeShowNumber();
        
        Sequence sequence = DOTween.Sequence();

        sequence.Append(textTransfrom.DOAnchorPos(Vector3.zero, slotTime * 1.5f).SetEase(Ease.OutBack))
                .Append(spotLightFadeGroup.DOFade(1.0f,  0.5f))
                .AppendCallback(() => {
                    symbal.Play();
                })
                .AppendInterval(3.0f)
                .Append(fadeCanvasGroup.DOFade(0.0f, 1.0f))
                .AppendCallback(() => {
                    bingo.DisplayNumberBase(targetNum);
                });

    } 
}
