using System;
using UnityEngine;

namespace Player
{
    public abstract class Ability : ICloneable {
        public int priority { get; private set; }

        protected Ability(int priority) {
            this.priority = priority;
        }

        public abstract void Tick();

        public abstract void Activate(Player player);

        public abstract bool IsDone();

        public abstract string GetDescription();

        public abstract object Clone();
    }
}