using UnityEngine;

namespace Player {
    public class TestAbility : Ability {
        
        public TestAbility(int priority) : base(priority) {
        }

        public override void Tick() {
            
        }

        public override void Activate(Player player) {
            Debug.Log("Activate test ability");
        }

        public override bool IsDone() {
            return true;
        }

        public override string GetDescription() {
            return "This is a test ability";
        }

        public override object Clone() {
            return new TestAbility(priority);
        }
    }
}