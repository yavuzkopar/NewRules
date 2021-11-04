using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RPG.Core
{
    public sealed class GameEnvironment
    {
        private static GameEnvironment instance;
        private List<GameObject> checkpoints = new List<GameObject>();
        private List<GameObject> goals = new List<GameObject>();
        public List<GameObject> Checkpoints { get { return checkpoints; } }
        public List<GameObject> Goalpoints { get { return goals; } }
        public static GameEnvironment Singleton
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameEnvironment();
                    instance.Checkpoints.AddRange(GameObject.FindGameObjectsWithTag("Checkpoint"));
                    instance.Goalpoints.AddRange(GameObject.FindGameObjectsWithTag("goal"));
                    instance.checkpoints = instance.checkpoints.OrderBy(waypoint => waypoint.name).ToList();
                }
                return instance;
            }
        }
    }

}