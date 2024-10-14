namespace MinWebApp.Frameworks
{
    public class DiceIMPL : IDice
    {
        private int _eyes;
        private Random _random = new Random();
        private int _sides;
        public DiceIMPL(int sides = 6) { 
            _sides = sides;
            roll();
        }

        public int getEyes()
        {
            return _eyes;
        }

        public void roll()
        {
            _eyes = _random.Next(_sides)+1;
        }
    }
}
