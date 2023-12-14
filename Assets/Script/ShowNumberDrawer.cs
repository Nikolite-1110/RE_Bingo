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

    public GameObject ShowNumberPrefab;
    [SerializeField] private TextMeshProUGUI textTmp;

    //////////////////////////////////

    void Start(){
        textTransfrom = gameObject.GetComponent<RectTransform>();
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();

        slotTime = 0.25f;
        count = 0;
        fadeCanvasGroup.alpha = 0.0f;
    }

    public void SetSlot(int target){
        slotTime = 0.25f;
        count = 0;

        textTransfrom.anchoredPosition = fromPosition;
        targetNum = target;
        fadeCanvasGroup.DOFade(1.0f, 1.0f).OnComplete(() => {
            SlotNumberAnimation();
        });
    }
 
    public void ChangeSlotNumber(){
        int rand = UnityEngine.Random.Range(1, 76);
        textMesh.text = rand.ToString();
    }

    public void SlotNumberAnimation(){
        if (count < 5){
            textTransfrom.anchoredPosition = fromPosition;
            ChangeSlotNumber();
            textTransfrom.DOAnchorPos(toPosition, slotTime).SetEase(Ease.Linear).SetDelay(0.2f).OnComplete(() => {
                SlotNumberAnimation();
                count++;
            });
            slotTime *= 1.20f;
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
        textTransfrom.DOAnchorPos(Vector3.zero, slotTime * 1.5f).SetEase(Ease.OutBack).OnComplete(() => {
            fadeCanvasGroup.DOFade(0.0f, 1.0f).SetDelay(3f).OnComplete(() => {
                bingo.DisplayNumberBase(targetNum);
            });
        });
    } 
}
