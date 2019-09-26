using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HD
{
    public static class ProtocolLabels
    {
        // MainScene Protocol Labels
        public const string joinRequest = "L0";
        public const string newClient = "L1";
        public const string clientInfo = "L2";
        public const string roleSelected = "L3";
        public const string clientLeft = "L4";
        public const string clientReady = "L5";
        public const string gameAction = "L6"; 

        // GameScene Protocol Labels
        public const string playerMove = "G0";
        public const string playerRot = "G1"; 
        public const string playerMouse = "G2"; 
        public const string lootState = "G3";
        public const string itemDropped = "G4";  
        public const string playerDeath = "G5"; 
        public const string demonDeath = "G6"; 
    }
}