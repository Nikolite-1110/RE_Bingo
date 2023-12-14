using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScene : MonoBehaviour
{
    [SerializeField, Range( 1, 8 )] private int m_useDisplayCount   = 2;

    private void Awake()
    {
        int count   = Mathf.Min( Display.displays.Length, m_useDisplayCount );

        for( int i = 0; i < count; ++i )
        {
            Display.displays[i].Activate();
        }

        SceneManager.LoadScene("SubWindow", LoadSceneMode.Additive);
    }

    // class GameController
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoBingoScene(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed){
            SceneManager.LoadScene("BingoScene");
        }    
    }

    public void QuitGame(InputAction.CallbackContext context){
        if(context.phase == InputActionPhase.Performed){
            Application.Quit();
        }    
    }
}
