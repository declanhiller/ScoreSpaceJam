namespace Player {
    public class TestAbility : Ability {
        
        public TestAbility(int priority) : base(priority) {
        }

        public override void Tick() {
            throw new System.NotImplementedException();
        }

        public override void Activate(Player player) {
            throw new System.NotImplementedException();
        }

        public override bool IsDone() {
            throw new System.NotImplementedException();
        }

        public override string GetDescription() {
            throw new System.NotImplementedException();
        }

        public override object Clone() {
            return new TestAbility(priority);
        }
    }
}