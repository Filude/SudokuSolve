namespace Sudoku {

    class Sudoku {

        private List<List<Value>> values; // two dimensional list, y,x
        private int totalScore; // score of the sudoku, lower is better
        private List<int> rowScores;
        private List<int> columnScores;
        private int S;
        private int viewDistance;
        private int randomWalkDifferenceTrigger;

        public Sudoku(List<List<int>> fixedValues) {

            this.values = addRandomValues(fixedValues);
            this.S = 1;
            this.viewDistance = 5;
            this.randomWalkDifferenceTrigger = 2;

            print();

            this.rowScores = new List<int>();
            this.columnScores = new List<int>();
            calculateScore();

        }

        private void print() {

            Console.WriteLine("");
            int rowNumber = 0;
            foreach (List<Value> row in this.values) {
                int columnNumber = 0;
                string str = "";
                foreach (Value value in row) {
                    if (columnNumber % 3 == 0 && columnNumber > 0) {
                        str += "|";
                    }
                    str += value.value.ToString();
                    columnNumber++;
                }
                if (rowNumber % 3 == 0 && rowNumber > 0) {
                    Console.WriteLine("------------");
                }
                Console.WriteLine(str);
                rowNumber++;
            }
            Console.WriteLine("");

        }

        private List<List<Value>> addRandomValues(List<List<int>> fixedValues) {

            List<List<Value>> values = new List<List<Value>>();

            for (int i = 0; i < 9; i++) {

                List<Value> row = new List<Value>();
                for (int j = 0; j < 9; j++) {
                    row.Add(new Value(0, false));
                }
                values.Add(row);

            }

            for (int i = 0; i < 3; i++) {

                for (int j = 0; j < 3; j++) {

                    List<Location> squareLocationsInBlock = getAllSquareLocationsInBlock(new Location(i, j));
                    List<int> availableNumbers = new List<int>();

                    for (int k = 1; k <= 9; k++) { 
                        availableNumbers.Add(k);
                    }

                    foreach (Location squareLocation in squareLocationsInBlock) {

                        int value = fixedValues[squareLocation.y][squareLocation.x];
                        if (value != 0) {
                            availableNumbers.Remove(value);
                            values[squareLocation.y][squareLocation.x] = new Value(value, true);
                        }

                    }

                    foreach (Location squareLocation in squareLocationsInBlock) {

                        int value = fixedValues[squareLocation.y][squareLocation.x];

                        if (value == 0) {
                            //Console.WriteLine(availableNumbers[0]);
                            //Console.WriteLine(squareLocation.x.ToString() + " " + squareLocation.y.ToString());
                            //Console.WriteLine(squareLocation.y.ToString() + " " + squareLocation.x.ToString());

                            values[squareLocation.y][squareLocation.x] = new Value(availableNumbers[0], false);
                            availableNumbers.RemoveAt(0);
                        }

                    }

                }

            }

            return values;

        }

        private int getAmountOfMissingValues(List<Value> list) {

            List<int> encounteredIntegers = new List<int>();

            foreach (Value value in list) {
                if (!encounteredIntegers.Contains(value.value)) {
                    encounteredIntegers.Add(value.value);
                }
            }

            return 9 - encounteredIntegers.Count;

        }

        private List<Value> getColumn(int columnNumber) {

            List<Value> column = new List<Value>();

            foreach (List<Value> row in values) {
                column.Add(row[columnNumber]);
            }

            return column;

        }

        private void calculateScore() {

            int score = 0;
            List<int> rowScores = new List<int>();
            List<int> columnScores = new List<int>();
             
            // For all rows 
            foreach (List<Value> row in values) {
                int amounfOfMissingValues = getAmountOfMissingValues(row);
                score += amounfOfMissingValues;
                rowScores.Add(amounfOfMissingValues);
            }

            // For all columns
            for (int columnNumber = 0; columnNumber < 9; columnNumber++) {
                List<Value> column = getColumn(columnNumber);
                int amounfOfMissingValues = getAmountOfMissingValues(column);
                score += amounfOfMissingValues;
                columnScores.Add(amounfOfMissingValues);
            }

            this.totalScore = score;
            this.rowScores = rowScores;
            this.columnScores = columnScores;

        }

        // Update the score but only recalculate the rows, and columns where a swap has occured
        private void updateScore(Location swapLocation1, Location swapLocation2) {

            var rowNumbers = swapLocation1.y != swapLocation2.y ? new[] { swapLocation1.y, swapLocation2.y } : new[] { swapLocation1.y };
            var columnNumbers = swapLocation1.x != swapLocation2.x ? new[] { swapLocation1.x, swapLocation2.x } : new[] { swapLocation1.x };

            foreach (int rowNumber in rowNumbers) {
                List<Value> row = this.values[rowNumber];
                int rowScore = getAmountOfMissingValues(row);
                this.rowScores[rowNumber] = rowScore;
            }

            foreach (int columnNumber in columnNumbers) {
                List<Value> column = getColumn(columnNumber);
                int columnScore = getAmountOfMissingValues(column);
                this.columnScores[columnNumber] = columnScore;
            }

            int totalScoreTemp = 0;

            foreach (int score in this.rowScores) {
                totalScoreTemp += score;
            }

            foreach (int score in this.columnScores) {
                totalScoreTemp += score;
            }

            this.totalScore = totalScoreTemp;
            
        }

        private void swap(Swap swap) {

            Location swapLocation1 = swap.location1;
            Location swapLocation2 = swap.location2;

            Value swapValue1 = this.values[swapLocation1.y][swapLocation1.x];
            Value swapValue2 = this.values[swapLocation2.y][swapLocation2.x];

            this.values[swapLocation1.y][swapLocation1.x] = swapValue2;
            this.values[swapLocation2.y][swapLocation2.x] = swapValue1;

        }

        private List<Location> getAllSquareLocationsInBlock(Location blockLocation) {

            List<Location> locations = new List<Location>();

            for (int x = blockLocation.x * 3; x < blockLocation.x * 3 + 3; x++) {
                for (int y = blockLocation.y * 3; y < blockLocation.y * 3 + 3; y++) {
                    locations.Add(new Location(x, y));
                }
            }

            return locations;

        }

        private void improveRandomBlock() {

            Random random = new Random();
            Location randomBlockLocation = new Location(random.Next(0, 3), random.Next(0, 3));
            Dictionary<Swap, int> swapScores = new Dictionary<Swap, int>();
            List<Location> squareLocationsInBlock = getAllSquareLocationsInBlock(randomBlockLocation);

            //Console.WriteLine("Before: " + this.totalScore.ToString());

            foreach (Location swapLocation1 in squareLocationsInBlock) {

                foreach (Location swapLocation2 in squareLocationsInBlock) {

                    if (values[swapLocation1.y][swapLocation1.x].isFixed || values[swapLocation2.y][swapLocation2.x].isFixed) {
                        continue;
                    }

                    swap(new Swap(swapLocation1, swapLocation2)); // Execute swap
                    updateScore(swapLocation1, swapLocation2);
                    swapScores.Add(new Swap(swapLocation1, swapLocation2), this.totalScore);

                    swap(new Swap(swapLocation1, swapLocation2)); // Reverse swap to normal situation
                    updateScore(swapLocation1, swapLocation2);

                }

            }

            KeyValuePair<Swap, int> bestSwapPair = swapScores.FirstOrDefault();
            foreach (KeyValuePair<Swap, int> pair in swapScores) {
                if (pair.Value < bestSwapPair.Value) {
                    bestSwapPair = pair;
                }
            }

            //Console.WriteLine("Swapping " + bestSwapPair.Key.location1.x + ", " + bestSwapPair.Key.location1.y + " with " +
            //    bestSwapPair.Key.location2.x + ", " + bestSwapPair.Key.location2.y);

            if (bestSwapPair.Value <= this.totalScore) {
                swap(bestSwapPair.Key);
                updateScore(bestSwapPair.Key.location1, bestSwapPair.Key.location2);
            }

            //Console.WriteLine("After: " + this.totalScore.ToString());

        }

        private void randomWalk() {

            Random random = new Random();

            Location randomBlockLocation = new Location(random.Next(0, 3), random.Next(0, 3));
            List<Location> squareLocationsInBlock = getAllSquareLocationsInBlock(randomBlockLocation);

            for (int i = 0; i < this.S; i++) {

                Location randomSquareLocation1 = squareLocationsInBlock[random.Next(0, 9)];
                Location randomSquareLocation2 = squareLocationsInBlock[random.Next(0, 9)];

                while (values[randomSquareLocation1.y][randomSquareLocation1.x].isFixed || values[randomSquareLocation2.y][randomSquareLocation2.x].isFixed) {
                    randomSquareLocation1 = squareLocationsInBlock[random.Next(0, 9)];
                    randomSquareLocation2 = squareLocationsInBlock[random.Next(0, 9)];
                }

                swap(new Swap(randomSquareLocation1, randomSquareLocation2));
                updateScore(randomSquareLocation1, randomSquareLocation2);

            }

        }

        public void solve() {

            int counter = 0;
            List<int> lastScores = new List<int>();

            while (this.totalScore != 0) {

                improveRandomBlock();

                int minLastScore = int.MaxValue;
                int maxLastScore = 0;

                foreach (int score in lastScores) {
                    if (score < minLastScore) {
                        minLastScore = score;
                    }
                    else if (score > maxLastScore) {
                        maxLastScore = score;
                    }
                }

                if (lastScores.Count == this.viewDistance && maxLastScore - minLastScore <= randomWalkDifferenceTrigger) {
                    //Console.WriteLine("Random walk");
                    randomWalk();
                    lastScores.Clear();
                }

                if (lastScores.Count < this.viewDistance) {
                    lastScores.Add(this.totalScore);
                }
                else {
                    lastScores.RemoveAt(0);
                    lastScores.Add(this.totalScore);
                }

                counter++;

                //Console.WriteLine(this.totalScore);

            }

            calculateScore();
            //Console.WriteLine(this.totalScore);

            print();

            Console.WriteLine("Solved in " + counter.ToString() + " iterations.");

        }

    }

}