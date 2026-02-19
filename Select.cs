using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using XboxCtrlrInput;

// Ryan Smith 4 Player Local Multiplayer Game Script

public struct CharacterHold
{
    public GameObject model;
    public Sprite alive;
    public Sprite dead;
}

public class Select : MonoBehaviour
{
    [Header("Rewired")]
    public XboxController controller;

    [Header("Player & Choice")]
    public int selectedCharacter = 0;
    [SerializeField] int playerNum;

    [Header("Character Information")]
    public GameObject[] characters;
    public Sprite[] aliveSprites;
    public Sprite[] deadSprites;

    [Header("Show's Character Model")]
    [SerializeField] GameObject currentObject;

    [Header("UI Quality of Life")]
    private bool canMove = true; 
    public GameObject readyCanvas;

    [Header("To begin")]
    private static int playersReady = 0;
    private static int totalPlayers = 4;
    private bool ready;

    [Header("Other")]
    public GameObject Player;
    private Vector3 newPosition;

    void Awake()
    {
        ready = false;

        if (playerNum == 1) // Just being Cautious
        {
            playersReady = 0;
        }

        readyCanvas.SetActive(false);
    }

    public bool Ready
    {
        get { return ready; }
        set
        {
            ready = value;
            if (Ready)
            {
                playersReady++;
            }
            else
            {
                playersReady--;
            }
        }
    }

    //-----------------------------------Start is called once upon creation-------------------------
    void Start()
    {
        switch (controller)
        {
            case XboxController.First: break;
            case XboxController.Second: break;
            case XboxController.Third: break;
            case XboxController.Fourth: break;
        }

        newPosition = transform.position;
    }

    //-----------------------------------Update is called once per frame----------------------------
    void Update()
    {

        if (XCI.GetButtonDown(XboxButton.A, controller) && playersReady == totalPlayers) // Once all players have readied up this will work
        {
            SceneManager.LoadScene(1); // Moves onwards to the gameplay, this was made to stop people from starting the game when they've selected their character. Let's people lock it in
        }

        if (XCI.GetButtonDown(XboxButton.Y, controller))
        {
            if (Ready) // If they're not ready they can cycle through the characters and the ready visual indicator 'button' is not visible 
            {
                canMove = true;
                Ready = false;
                readyCanvas.SetActive(false);
            }

            else
            {
                canMove = false;
                Ready = true;
                readyCanvas.SetActive(true);
                SelectCharcter(); // Once ready it stores the sprites etc corresponding to the index num they are on

            }
        }

        if (!canMove)
            return;

        // Just for moving through the multiple characters
        if (XCI.GetButtonDown(XboxButton.LeftBumper, controller))
        {
            PreviousCharacter();
        }

        if (XCI.GetButtonDown(XboxButton.RightBumper, controller))
        {
            NextCharcter();
        }
    }

    //-----------------------------------Cycling Between Characters----------------------------
    public void NextCharcter()
    {
        Destroy(currentObject); // Removes last model
        selectedCharacter = (selectedCharacter + 1) % characters.Length; // Moves to the next position, while avoiding error
        currentObject = Instantiate(characters[selectedCharacter], transform); // Estentially places the current model 

    }

    public void PreviousCharacter()
    {
        Destroy(currentObject);
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length; // Loops
        }
        characters[selectedCharacter].SetActive(true);
        currentObject = Instantiate(characters[selectedCharacter], transform);
    }

    //-----------------------------------Selecting and Storing Character----------------------------
    public void SelectCharcter() // Extremely simple and understandable for what it's function is. Updates all data in CharacterHold to whatever character they've chosen
    {
        CharacterHold selectedChar;
        selectedChar.model = characters[selectedCharacter];
        selectedChar.alive = aliveSprites[selectedCharacter];
        selectedChar.dead = deadSprites[selectedCharacter];
        Store.AddCharacter(selectedChar, playerNum);
    }
}