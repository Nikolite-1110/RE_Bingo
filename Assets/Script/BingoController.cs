using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class BingoController : MonoBehaviour
{

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject paddingImage;
    [SerializeField] private List<GameObject> lateTextList;
    [SerializeField] private ShowNumberDrawer drawer;

    public Color firstRowColor;
    public Color secondRowColor;
    public Color therdRowColor;
    public Color fourthRowColor;
    public Color fifthRowColor;

    private List<GameObject> objectList = new List<GameObject>(); //ゲームオブジェクト格納
    private List<int> numberList = new List<int>();

    private Queue<string> lateNumberQueue = new Queue<string>();

    public GameObject numberPrefab;

    private Vector2 prefabSize;
    private Vector2 canvasSize;

    private bool allowPush = true;
    

    private const int MAX_NUMBER = 75;
    private const int MAX_COLUMN = 15;
    private const int MAX_ROW = 5;
    private const float SIDE_PADDING = 15;
    private const float START_POS_Y = 700;

    //////////////////////////////////
    //表示のアニメーションに関するフィールド

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private Transform fadeTransForm;

    public GameObject ShowNumberPrefab;
    [SerializeField] private TextMeshProUGUI textTmp;

    //////////////////////////////////

    private void Start()
    {
        prefabSize = numberPrefab.GetComponent<RectTransform>().sizeDelta;
        canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        GenerateNumbers();
    }

    private void GenerateNumbers(){

        int count = 1;

        float genx = SIDE_PADDING + prefabSize.x / 2;
        float genxStart = genx;
        float geny = START_POS_Y - prefabSize.y / 2;

        float padding_x = ((canvasSize.x - SIDE_PADDING * 2) / MAX_COLUMN - prefabSize.x) / 2;
        float padding_y = ((START_POS_Y - SIDE_PADDING) / MAX_ROW - prefabSize.y) / 2; //思った以上にpaddingが低いバグ発生中...

        for(int i = 0; i < MAX_NUMBER; i++){

            genx += padding_x;

            GameObject genObj = Instantiate(numberPrefab, paddingImage.transform) as GameObject;
            RectTransform genRectTrans = genObj.GetComponent<RectTransform>();
            genRectTrans.anchoredPosition = new Vector2(genx, geny);
            
            Image img = genObj.GetComponent<Image>();

            switch(count){
                case 1:
                    img.color = firstRowColor;
                    break;
                case 2:
                    img.color = secondRowColor;
                    break;
                case 3:
                    img.color = therdRowColor;
                    break;
                case 4:
                    img.color = fourthRowColor;
                    break;
                case 5:
                    img.color = fifthRowColor;
                    break;
                default:
                    img.color = Color.black;
                    break;
            }


            GameObject textObj = genObj.transform.Find("NumberText").gameObject;

            int textNum = i + 1;
            textObj.GetComponent<TextMeshProUGUI>().text = textNum.ToString();
            genObj.GetComponent<CanvasGroup>().alpha = 0;

            objectList.Add(genObj);
            numberList.Add(i);

            genx += padding_x + prefabSize.x;

            //改行処理
            if(i % MAX_COLUMN == MAX_COLUMN - 1){
                genx = genxStart;
                geny -= padding_y + prefabSize.y;
                count++;
            }
        }
    }

    public void DisplayNumberBase(int num){
        CanvasGroup canvasGroup =  objectList[num - 1].GetComponent<CanvasGroup>();
        canvasGroup.DOFade(1.0f, 0.2f).SetDelay(0.2f);

        List<string> tempList = new List<string>(lateNumberQueue);
        tempList.Reverse();
        //lateText表示用の個数の入っていない場合、足りない分0を入れる
        for(int i = tempList.Count; i < lateTextList.Count; i++){
            tempList.Add("00");
        }
        Debug.Log("-----------");
        foreach(string str in tempList){
            Debug.Log(str);
        }
        Debug.Log("-----------");

        for(int i = 0; i < lateTextList.Count; i++){
            lateTextList[i].GetComponent<TextMeshProUGUI>().text = tempList[i];
        }
        
        allowPush = true;
    }

    public void GenerateRandomNumber(){
        int randMax = numberList.Count;
        int randNum = UnityEngine.Random.Range(0, randMax);

        int genNum = numberList[randNum];
        numberList.Remove(genNum);

        lateNumberQueue.Enqueue((genNum + 1).ToString());

        if(lateNumberQueue.Count > lateTextList.Count){
            lateNumberQueue.Dequeue();
        }        

        Debug.Log(genNum);

        drawer.SetSlot(genNum + 1);
    }

    public void OnPushEnter(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed && allowPush){
            allowPush = false;
            GenerateRandomNumber();
        }
    }

    public void ShowNumber(){
        Debug.Log("数字表示");
        GameObject genText = Instantiate(ShowNumberPrefab, fadeTransForm) as GameObject;
        genText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    public void ShowNumberChanger(int i){
        textTmp.text = i.ToString();
    }

    public void GoTitleScene(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed){
            SceneManager.LoadScene("TitleScene");
        }
    }
}
