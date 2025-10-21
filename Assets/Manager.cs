using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject PAGE_Home;
    public GameObject PAGE_Word;
    public GameObject PAGE_Arrow;


    public GameObject WordGame;
    public GameObject ArrowGame;
    public GameObject HomePanel;

    public void Button_WordGame()
    {
        PAGE_Home.SetActive(false);
        PAGE_Word.SetActive(true);
        WordGame.SetActive(true);
        ArrowGame.SetActive(false);
        HomePanel.SetActive(false);
        
    }
    
    public void Button_ArrowGame()
    {
        PAGE_Home.SetActive(false);
        PAGE_Arrow.SetActive(true);
        WordGame.SetActive(false);
        ArrowGame.SetActive(true);
        HomePanel.SetActive(false);

    }
    public void Button_Home()
    {
        PAGE_Word.SetActive(false);
        PAGE_Arrow.SetActive(false);
        PAGE_Home.SetActive(true);
        WordGame.SetActive(false);
        ArrowGame.SetActive(false);
        HomePanel.SetActive(true);

    }
}
