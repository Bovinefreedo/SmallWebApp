namespace MinWebApp.Frameworks
{
    public class YatzyIMPL
    {
        private DiceCup _diceCup = new();
        private int[] _currentRoll = new int[5];
        private int[,] _scoreBoard = new int[18, 4];
        private bool[,] _usedRule = new bool[18, 4];
        private int _playerInTurn = 0;
        private int _numPlayers = 4;

        public YatzyIMPL(int numberOfPlayers)
        {
            _numPlayers = numberOfPlayers;
            _scoreBoard = new int[18, numberOfPlayers];
            _usedRule = new bool[18, numberOfPlayers];
            roll(new bool[] { false, false, false, false, false });
        }
        public void roll(bool[] locked)
        {
            _diceCup.roll(locked);
            _currentRoll = _diceCup.getEyes();
        }
        public int getEyes(int id)
        {
            int eyes = _currentRoll[id];
            return eyes;
        }

        public void nextPlayer() {
            if (_playerInTurn < _numPlayers - 1)
            {
                _playerInTurn++;
            }
            else
            {
                _playerInTurn = 0;
            }
        }

        public int scoreSum()
        {
            int sum = 0;
            for (int i = 0; i < 6; i++)
            {
                sum += _scoreBoard[i, _playerInTurn];
            }
            return sum;
        }

        public int scoreTotal()
        {
            int total = 0;
            for (int i = 0; i < 15; i++)
            {
                total += _scoreBoard[i, _playerInTurn];
            }
            total += scoreBonus();
            return total;
        }

        public int scoreBonus()
        {
            if (scoreSum() >= 63)
                return 50;
            else
                return 0;
        }

        public int scoreCup(int rule)
        {
            switch (rule)
            {
                case 1: //1'ere
                    return scoreNumbers(1);

                case 2: //2'ere
                    return scoreNumbers(2);

                case 3: //3'ere
                    return scoreNumbers(3);

                case 4://4'ere
                    return scoreNumbers(4);

                case 5://5'ere
                    return scoreNumbers(5);

                case 6://6'ere
                    return scoreNumbers(6);
                case 7:
                    return scoreSum();
                case 8:
                    return scoreBonus();
                case 9://par
                    return scoreMultiple(2);

                case 10: //to par
                    return scoreMultiplePairs(2, 2);

                case 11://tre ens 
                    return scoreMultiple(3);

                case 12: //fire ens
                    return scoreMultiple(4);

                case 13: //fuldt hus 
                    return scoreMultiplePairs(3, 2);

                case 14: //lav straight
                    return scoreStraight(1);

                case 15: //høj straight
                    return scoreStraight(2);

                case 16: //Yatzy
                    return scoreYatzy();

                case 17: //chance
                    return scoreChance();
                case 18: //
                    return scoreTotal();
                default:
                    throw new Exception();
            }
        }

        public int scoreChance()
        {
            int sum = 0;
            foreach (int i in _currentRoll)
            {
                sum += i;
            }
            return sum;
        }

        public int scoreNumbers(int num)
        {
            int total = 0;
            int[] rolls = _diceCup.getEyes();
            for (int i = 0; i < rolls.Length; i++)
            {
                if (rolls[i] == num) { total = total + num; }
            }
            return total;
        }

        // scoring multiple of a kind (pair, three or four of a kind)
        public int scoreMultiple(int num)
        {
            int total = 0;
            int[] appearances = appearancesOfEyes();
            for (int i = 0; i < appearances.Length; i++)
            {
                if (appearances[i] >= num) total = num * i;
            }
            return total;
        }

        //Helping method that gives an array with the number of appearances a roll has
        // if the roll is {3,3,3,5,5} it will return {0,0,0,3,0,2,0}
        // the index 0 will always be 0 and can be skipped.
        public int[] appearancesOfEyes()
        {
            int[] rolls = _diceCup.getEyes();
            int[] appearances = new int[7];
            for (int i = 0; i < rolls.Length; i++)
            {
                appearances[rolls[i]]++;
            }
            return appearances;
        }

        //scoring two pair and full house, it is important to put the bigger of the two
        //input integers first, if you don't the input {3,3,3,5,5} will fail.  
        public int scoreMultiplePairs(int bigMult, int smallMult)
        {
            int firstMult = 0;
            int secondMult = 0;
            int[] appearances = appearancesOfEyes();
            for (int i = 1; i < appearances.Length; i++)
            {
                if (appearances[i] >= bigMult)
                {
                    firstMult = i * bigMult;
                    appearances[i] -= bigMult;
                    break;
                }
            }
            for (int i = 1; i < appearances.Length; i++)
            {
                if (appearances[i] >= smallMult)
                {
                    secondMult = i * smallMult;
                    appearances[i] -= smallMult;
                    break;
                }
            }

            if (firstMult == 0 || secondMult == 0) return 0;
            return firstMult + secondMult;
        }

        //scoring a straights
        public int scoreStraight(int start)
        {
            int[] roll = _diceCup.getEyes();
            bool legalStraight = true;
            Array.Sort(roll);
            for (int i = 0; i < 5; i++)
            {
                if (roll[i] != start + i) legalStraight = false;
            }
            if (!legalStraight) return 0;
            else if (start == 1) return 15;
            else return 20;
        }

        //Scoring Yatzy
        public int scoreYatzy()
        {
            int[] appearances = appearancesOfEyes();
            foreach (int i in appearances)
            {
                if (i == 5)
                {
                    return 50;
                }
            }
            return 0;
        }

        //Responsible for awarding points when a turn has been finalized by choosing
        //a rule
        public void awardPoints(int rule)
        {
            _scoreBoard[rule-1, _playerInTurn] = scoreCup(rule);
            _usedRule[rule, _playerInTurn] = true;
        }
    }
}

