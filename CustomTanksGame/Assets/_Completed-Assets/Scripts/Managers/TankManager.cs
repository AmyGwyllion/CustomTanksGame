﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Complete
{
    [Serializable]
    public class TankManager
    {
        // This class is to manage various settings on a tank.
        // It works with the GameManager class to control how the tanks behave
        // and whether or not players have control of their tank in the 
        // different phases of the game.

        [HideInInspector] public int m_PlayerNumber;            // This specifies which player this the manager for.
        [HideInInspector] public string m_ColoredPlayerText;    // A string that represents the player with their number colored to match their tank.
        [HideInInspector] public GameObject m_Instance;         // A reference to the instance of the tank when it is created.
        [HideInInspector] public int m_Wins;                    // The number of wins this player has so far.
        [HideInInspector] public int m_Laps;                    // The laps the player has completed

        public Color m_PlayerColor;                             // This is the color this tank will be tinted.
        public Transform m_SpawnPoint;                          // The position and direction the tank will have when it spawns.
        public Text m_LapDial;                                  // This is the text UI element where to display the player laps
        
        private TankMovement m_Movement;                        // Reference to tank's movement script, used to disable and enable control.
        private TankShooting m_Shooting;                        // Reference to tank's shooting script, used to disable and enable control.
        private TankCompass m_Compass;                          // Reference to tank's moving compass script.
        private TankStatus m_TankStatus;                        // Reference to tank's checking status script.
        private GameObject m_CanvasGameObject;                  // Used to disable the world space UI during the Starting and Ending phases of each round.

        public void Setup (Transform[] checkpoints)
        {
            // Reset the laps and his value in the UI dial
            m_Laps = 0;
            m_LapDial.text = string.Empty;

            // Get references to the components.
            m_Movement = m_Instance.GetComponent<TankMovement>();
            m_Shooting = m_Instance.GetComponent<TankShooting>();
            m_Compass = m_Instance.GetComponent<TankCompass>();
            m_TankStatus = m_Instance.GetComponent<TankStatus>();
            m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

            // Set the player numbers to be consistent across the scripts.
            m_Movement.m_PlayerNumber = m_PlayerNumber;
            m_Shooting.m_PlayerNumber = m_PlayerNumber;
            m_Compass.m_PlayerNumber = m_PlayerNumber;
            m_TankStatus.m_PlayerNumber = m_PlayerNumber;

            //Set the player colors for the UI
            m_Compass.m_PlayerColor = m_PlayerColor;
            m_TankStatus.m_PlayerColor = m_PlayerColor;
            
            // Create a string using the correct color that says 'PLAYER 1' etc based on the tank's color and the player's number.
            m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

            // Get all of the renderers of the tank.
            MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer> ();

            // Go through all the renderers...
            for (int i = 0; i < renderers.Length; i++)
            {
                // ... set their material color to the color specific to this tank.
                renderers[i].material.color = m_PlayerColor;
            }

            //Set all the checkpoints to be reached
            m_Compass.SetCheckpoints(checkpoints);
        }


        // Used during the phases of the game where the player shouldn't be able to control their tank, with a flag in case we don't want to disable tue UI too (when the tanks is quiet for example)
        public void DisableControl (bool canvasToo = true)
        {
            m_Movement.enabled = false;
            m_Shooting.enabled = false;

            if(canvasToo) m_CanvasGameObject.SetActive (false);
        }


        // Used during the phases of the game where the player should be able to control their tank.
        public void EnableControl ()
        {
            m_Movement.enabled = true;
            m_Shooting.enabled = true;

            m_CanvasGameObject.SetActive (true);
        }

        // Used at the start of each round to put the tank into it's default state.
        public void Reset ()
        {
            m_Laps = 0;

            m_Instance.transform.position = m_SpawnPoint.position;
            m_Instance.transform.rotation = m_SpawnPoint.rotation;

            m_Instance.SetActive (false);
            m_Instance.SetActive (true);
        }
    }
}