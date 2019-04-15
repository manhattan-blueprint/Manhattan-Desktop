using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model.State {
    [Serializable]
    public class MapState {
        [SerializeField] private Dictionary<Vector2, MapObject> grid;
        [SerializeField] private Goal goal;

        public MapState() {
            grid = new Dictionary<Vector2, MapObject>();
            goal = new Goal();
        }

        public void addObject(Vector2 position, int id) {
            grid[position] = new MapObject(id);
        }

        public void removeObject(Vector2 position) {
            grid.Remove(position);
        }

        public void AddGoalItem(GoalPosition position) {
            if (position == GoalPosition.Top)
                goal.topInput = true;
            if (position == GoalPosition.Mid)
                goal.midInput = true;
            if (position == GoalPosition.Bot)
                goal.botInput = true;
        }

        public Dictionary<Vector2, MapObject> getObjects() {
            return grid;
        }

        public Goal getGoal() {
            return goal;
        }
    }
}
