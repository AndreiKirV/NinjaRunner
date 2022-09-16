namespace dictionaries
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerStates
    {
        public static readonly string IsRun = "IsRun";
        public static readonly string IsJump = "IsJump";
        public static readonly string IsTrickZone = "IsTrickZone";
        public static readonly string JumpObstacle = "JumpObstacle";
        public static readonly string CrashedJump = "CrashedJump";
        public static readonly string DeathByObstacle = "DeathByObstacle";
        public static readonly string IsStopZone = "IsStopZone";
        public static readonly string Hit = "Hit";
        public static readonly string Slide = "Slide";
    }
}