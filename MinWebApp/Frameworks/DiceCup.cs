namespace MinWebApp.Frameworks
{
    public class DiceCup
    {
        List<IDice> myDice = new List<IDice>();
        public DiceCup(int numberOfDice = 5) {
            for (int i = 0; i < numberOfDice; i++) {
                DiceIMPL d = new();
                myDice.Add(d);
            }
        }
        public int[] getEyes()
        {
            int[] currentRoll = new int[myDice.Count];
            for(int i = 0; i < currentRoll.Length; i++ ) {
                currentRoll[i] = myDice[i].getEyes();
            }
            return currentRoll;
        }

        public void roll()
        {
            foreach (IDice dice in myDice) { 
                dice.roll();
            }
        }

        public void roll(bool[] locked)
        {
            if (locked.Length != myDice.Count) {
                throw new Exception();
            }
            for (int i = 0; i < locked.Length; i++) {
                if (!locked[i]) {
                    myDice[i].roll();
                }
            }
        }
    }
}
