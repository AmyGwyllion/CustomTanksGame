using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Complete
{
    public class GameManager : MonoBehaviour
    {
        public int m_LapsToWin = 3;                             // The number of laps for winning the round
        public float m_StartDelay = 3f;                         // The delay between the start of RoundStarting and RoundPlaying phases.
        public float m_EndDelay = 3f;                           // The delay between the end of RoundPlaying and RoundEnding phases.
        public CameraManager m_CameraManager;                   // Reference to the CameraControl script for control during different phases.
        public Text m_MessageText;                              // Reference to the overlay Text to display winning text, etc.
        public GameObject m_TankPrefab;                         // Reference to the prefab the players will control.
        public TankManager[] m_Tanks;                           // A collection of managers for enabling and disabling different aspects of the tanks.
        public Transform[] m_Checkpoints;                       // All the checkpoints for completing a lap
        
        private int m_RoundNumber;                              // Which round the game is currently on.
        private WaitForSeconds m_StartWait;                     // Used to have a delay whilst the round starts.
        private WaitForSeconds m_EndWait;                       // Used to have a delay whilst the round or game ends.
        private TankManager m_RoundWinner;                      // Reference to the winner of the current round.  Used to make an announcement of who won.
        private TankManager m_GameWinner;                       // Reference to the winner of the game.  Used to make an announcement of who won.
        
        public Button m_Button;                                 // Play Again Button
        private bool m_PlayAgain;                               // Flag for the play again button
        private Dictionary<int, TankManager> m_PlayerByNumber;  // All the players sorted by their player number

        private void Start()
        {
            m_PlayerByNumber = new Dictionary<int, TankManager>();

            // Create the delays so they only have to be made once.
            m_StartWait = new WaitForSeconds (m_StartDelay);
            m_EndWait = new WaitForSeconds (m_EndDelay);

            SpawnAllTanks();

            // Initialize the position, mode, target and zoom for all the player cameras
            m_CameraManager.SetPlayers(m_Tanks);
            m_CameraManager.SetCameraTarget();

            // Once the tanks have been created and the cameras are using them as targets, start the game.
            StartCoroutine (GameLoop ());
        }

        private void SpawnAllTanks()
        {
            // For all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... create them, set their player number and references needed for control.
                m_Tanks[i].m_Instance =
                    Instantiate(m_TankPrefab, m_Tanks[i].m_SpawnPoint.position, m_Tanks[i].m_SpawnPoint.rotation) as GameObject;
                m_Tanks[i].m_PlayerNumber = i + 1;
                m_Tanks[i].Setup(m_Checkpoints);            // Give the players the position of the checkpoints they have to reach
                m_PlayerByNumber.Add(i + 1, m_Tanks[i]);    // Populate the player by number player dictionary
            }
        }

        // This is called from start and will run each phase of the game one after another.
        private IEnumerator GameLoop ()
        {
            // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
            yield return StartCoroutine (RoundStarting ());

            // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
            yield return StartCoroutine (RoundPlaying());

            // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
            yield return StartCoroutine (RoundEnding());

            StartCoroutine (GameLoop ());
        }

        private IEnumerator RoundStarting ()
        {
            //Disable the play again button
            m_Button.gameObject.SetActive(false);

            // As soon as the round starts reset the tanks and make sure they can't move.
            ResetAllTanks ();
            DisableTankControl ();

            // Snap the camera's zoom and position to something appropriate for the reset tanks.
            m_CameraManager.Initialize();

            // Increment the round number and display text showing the players what round it is.
            m_RoundNumber++;
            m_MessageText.text = "ROUND " + m_RoundNumber;

            // Wait for the specified length of time until yielding control back to the game loop.
            yield return m_StartWait;
        }

        private IEnumerator RoundPlaying ()
        {
            // As soon as the round begins playing let the players control the tanks.
            EnableTankControl ();

            // Clear the text from the screen.
            m_MessageText.text = string.Empty;

            // While there is not one tank left...
            while (!WeHaveAWinner())
            {
                // ... return on the next frame.
                yield return null;
            }
        }

        private IEnumerator RoundEnding ()
        {
            m_PlayAgain = false;
            m_Button.gameObject.SetActive(true);
            // Stop tanks from moving.
            DisableTankControl();

            // Clear the winner from the previous round.
            m_RoundWinner = null;

            // See if there is a winner now the round is over.
            m_RoundWinner = GetRoundWinner ();

            // If there is a winner, increment their score.
            if (m_RoundWinner != null)
                m_RoundWinner.m_Wins++;

            // Get a message based on the scores and whether or not there is a game winner and display it.
            string message = EndMessage ();
            m_MessageText.text = message;

            // Wait until the player presses the play again button to yielding control back to the game loop.
            while (!m_PlayAgain) yield return null;
        }

        private bool WeHaveAWinner()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... and if they are active, increment the counter.
                if (m_Tanks[i].m_Laps == m_LapsToWin) return true;
            }

            return false;
        }
        
        // This function is to find out if there is a winner of the round.
        private TankManager GetRoundWinner()
        {
            // Go through all the tanks...
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                // ... and if one of them has completed all laps
                if (m_Tanks[i].m_Laps == m_LapsToWin)
                    return m_Tanks[i];
            }

            return null;
        }

        // Returns a string message to display at the end of each round.
        private string EndMessage()
        {
            // By default when a round ends there are no winners so the default end message is a draw.
            string message = "DRAW!";

            // If there is a winner then change the message to reflect that.
            if (m_RoundWinner != null)
                message = m_RoundWinner.m_ColoredPlayerText + " WINS THE ROUND!";

            // Add some line breaks after the initial message.
            message += "\n";

            // Go through all the tanks and add each of their scores to the message.
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                message += m_Tanks[i].m_ColoredPlayerText + ": " + m_Tanks[i].m_Wins + " WINS\n";
            }

            return message;
        }

        // This is a trigger function for the play again button
        public void PlayAgain()
        {
            m_PlayAgain = true;
        }

        // This function is only called once the player reached all the checkpoints
        // Increments the player laps by one and updates his lap dial on screen
        public void AddLap(int playerNumber)
        {
            TankManager player = m_PlayerByNumber[playerNumber];
            player.m_Laps++;
            PrintLapInfo(player);
        }

        // This function prints the current lap of the player 
        private void PrintLapInfo(TankManager player)
        {
            string info = "<color=#" + ColorUtility.ToHtmlStringRGB(player.m_PlayerColor) + ">LAPS: " + player.m_Laps.ToString() + "/" + m_LapsToWin + "</color>";
            player.m_LapDial.text = info;
        }

        // This function is used to turn all the tanks back on and reset their positions and properties.
        private void ResetAllTanks()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].Reset();
                PrintLapInfo(m_Tanks[i]);
            }
        }


        private void EnableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].EnableControl();
            }
        }


        private void DisableTankControl()
        {
            for (int i = 0; i < m_Tanks.Length; i++)
            {
                m_Tanks[i].DisableControl();
            }
        }
    }
}