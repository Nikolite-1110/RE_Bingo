using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleScene : MonoBehaviour
{
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
