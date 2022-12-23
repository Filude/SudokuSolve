namespace Sudoku {

    class Sudoku {

        private List<List<Value>> values; // two dimensional list, y,x (because we index by row first)
        private int totalScore; // total score of the sudoku, lower is better
        private List<int> rowScores; // score per row
        private List<int> columnScores; // score per column
        private int S; // how many random search operations should be performed with a random walk
        private int viewDistance; // how far back do we look in the score values to determine when to random walk
        private int randomWalkDifferenceTrigger; // minimum difference between last scores needed to trigger a random walk
        private bool randomWalkAcrossBlocks; // if set to true the random walk will choose a different block every time,
                                             // otherwise it will choose one block and stay there

        // NOTE: A block is considered a block of 9x9 squares, and a square is one cell with a number.

        public Sudoku(List<List<int>> fixedValues) {

            this.values = addRandomValues(fixedValues); // Add random values in the non fixed squares

            // PARAMATERS
            this.S = 2;
            this.viewDistance = 5;
            this.randomWalkDifferenceTrigger = 2;
            this.randomWalkAcrossBlocks = true;

            print();

            this.rowScores = new List<int>();
            this.columnScores = new List<int>();
            calculateScore(); // Calculate the initial score the sudoku has

        }

        // Print the sudoku in a readable way
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

        // Add random values in the non fixed squares
        private List<List<Value>> addRandomValues(List<List<int>> fixedValues) {

            // Create an empty list with new values
            List<List<Value>> values = new List<List<Value>>();

            // Populate the newly created list with zeros
            for (int i = 0; i < 9; i++) {

                List<Value> row = new List<Value>();
                for (int j = 0; j < 9; j++) {
                    row.Add(new Value(0, false));
                }
                values.Add(row);

            }

            // For every block
            for (int i = 0; i < 3; i++) {

                for (int j = 0; j < 3; j++) {

                    // Get the sqaure locations in the block
                    List<Location> squareLocationsInBlock = getAllSquareLocationsInBlock(new Location(i, j));
                    List<int> availableNumbers = new List<int>();
                     
                    // Popualte the 'availableNumbers' list with all possible numbers (1 to 9)
                    for (int k = 1; k <= 9; k++) { 
                        availableNumbers.Add(k);
                    }

                    foreach (Location squareLocation in squareLocationsInBlock) {

                        // Get the value that is currently in the square
                        int value = fixedValues[squareLocation.y][squareLocation.x];

                        // If the value is a fixed value, remove the number from the available numbers and add to the 'values' list.
                        if (value != 0) {
                            availableNumbers.Remove(value);
                            values[squareLocation.y][squareLocation.x] = new Value(value, true);
                        }

                    }

                    // Now populate the rest with random values that are still available,
                    // i.e. all the numbers 1-9 minus the numbers that are fixed
                    foreach (Location squareLocation in squareLocationsInBlock) {

                        int value = fixedValues[squareLocation.y][squareLocation.x];

                        if (value == 0) {
                            values[squareLocation.y][squareLocation.x] = new Value(availableNumbers[0], false);
                            availableNumbers.RemoveAt(0); // Remove from available numbers
                        }

                    }

                }

            }

            return values;

        }

        // Get the amount of missing numbers in either a row or column
        private int getAmountOfMissingValues(List<Value> list) {

            List<int> encounteredIntegers = new List<int>();

            foreach (Value value in list) {
                // Add if not yet seen
                if (!encounteredIntegers.Contains(value.value)) {
                    encounteredIntegers.Add(value.value);
                }
            }

            return 9 - encounteredIntegers.Count;

        }

        // Get a list with all the values of one column
        private List<Value> getColumn(int columnNumber) {

            List<Value> column = new List<Value>();

            foreach (List<Value> row in values) {
                column.Add(row[columnNumber]);
            }

            return column;

        }

        // Calculate the score of the whole sudoku
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

            // Get the row and column numbers where a swap has occured
            var rowNumbers = swapLocation1.y != swapLocation2.y ? new[] { swapLocation1.y, swapLocation2.y } : new[] { swapLocation1.y };
            var columnNumbers = swapLocation1.x != swapLocation2.x ? new[] { swapLocation1.x, swapLocation2.x } : new[] { swapLocation1.x };

            foreach (int rowNumber in rowNumbers) {
                List<Value> row = this.values[rowNumber];
                int rowScore = getAmountOfMissingValues(row); // Calculate new score
                this.totalScore = this.totalScore - this.rowScores[rowNumber] + rowScore; // Withdraw the old score and add the new score
                this.rowScores[rowNumber] = rowScore;
            }

            foreach (int columnNumber in columnNumbers) {
                List<Value> column = getColumn(columnNumber);
                int columnScore = getAmountOfMissingValues(column);
                this.totalScore = this.totalScore - this.columnScores[columnNumber] + columnScore;
                this.columnScores[columnNumber] = columnScore;
            }
            
        }

        // Swap the values of two square locations
        private void swap(Swap swap) {

            Location swapLocation1 = swap.location1;
            Location swapLocation2 = swap.location2;

            Value swapValue1 = this.values[swapLocation1.y][swapLocation1.x];
            Value swapValue2 = this.values[swapLocation2.y][swapLocation2.x];

            this.values[swapLocation1.y][swapLocation1.x] = swapValue2;
            this.values[swapLocation2.y][swapLocation2.x] = swapValue1;

        }

        // Get all the square locations in a given block
        private List<Location> getAllSquareLocationsInBlock(Location blockLocation) {

            List<Location> locations = new List<Location>();

            for (int x = blockLocation.x * 3; x < blockLocation.x * 3 + 3; x++) {
                for (int y = blockLocation.y * 3; y < blockLocation.y * 3 + 3; y++) {
                    locations.Add(new Location(x, y));
                }
            }

            return locations;

        }

        // Try to improve a randomly chosen block by trying all possible swaps and choosing the best one
        private void improveRandomBlock() {

            Random random = new Random();
            Location randomBlockLocation = new Location(random.Next(0, 3), random.Next(0, 3));
            Dictionary<Swap, int> swapScores = new Dictionary<Swap, int>();
            List<Location> squareLocationsInBlock = getAllSquareLocationsInBlock(randomBlockLocation);

            foreach (Location swapLocation1 in squareLocationsInBlock) {

                foreach (Location swapLocation2 in squareLocationsInBlock) {

                    // Continue if one of the values is fixed
                    if (values[swapLocation1.y][swapLocation1.x].isFixed || values[swapLocation2.y][swapLocation2.x].isFixed) {
                        continue;
                    }

                    // Execute the swap and add the new score to the scores list
                    swap(new Swap(swapLocation1, swapLocation2)); 
                    updateScore(swapLocation1, swapLocation2);
                    swapScores.Add(new Swap(swapLocation1, swapLocation2), this.totalScore);
                    
                    // Reverse the swap again and reupdate the score
                    swap(new Swap(swapLocation1, swapLocation2));
                    updateScore(swapLocation1, swapLocation2);

                }

            }

            // Get the swap with the best score
            KeyValuePair<Swap, int> bestSwapPair = swapScores.FirstOrDefault();
            foreach (KeyValuePair<Swap, int> pair in swapScores) {
                if (pair.Value < bestSwapPair.Value) {
                    bestSwapPair = pair;
                }
            }

            // If the score is an improvement then execute the swap
            if (bestSwapPair.Value <= this.totalScore) {
                swap(bestSwapPair.Key);
                updateScore(bestSwapPair.Key.location1, bestSwapPair.Key.location2);
            }

        }

        // Execute a random walk of S long
        private void randomWalk() {

            Random random = new Random();
            Location randomBlockLocation;
            List<Location> squareLocationsInBlock = new List<Location>();

            // If "randomWalkAcrossBlocks" is set to false, choose one random block now and stay with that one
            if (!randomWalkAcrossBlocks) {
                randomBlockLocation = new Location(random.Next(0, 3), random.Next(0, 3));
                squareLocationsInBlock = getAllSquareLocationsInBlock(randomBlockLocation);
            }

            for (int i = 0; i < this.S; i++) {

                // If "randomWalkAcrossBlocks" is set to true, choose a new random block every time
                if (randomWalkAcrossBlocks) {
                    randomBlockLocation = new Location(random.Next(0, 3), random.Next(0, 3));
                    squareLocationsInBlock = getAllSquareLocationsInBlock(randomBlockLocation);
                }

                Location randomSquareLocation1 = squareLocationsInBlock[random.Next(0, 9)];
                Location randomSquareLocation2 = squareLocationsInBlock[random.Next(0, 9)];

                // Choose two values that are not fixed
                while (values[randomSquareLocation1.y][randomSquareLocation1.x].isFixed || values[randomSquareLocation2.y][randomSquareLocation2.x].isFixed) {
                    randomSquareLocation1 = squareLocationsInBlock[random.Next(0, 9)];
                    randomSquareLocation2 = squareLocationsInBlock[random.Next(0, 9)];
                }

                swap(new Swap(randomSquareLocation1, randomSquareLocation2));
                updateScore(randomSquareLocation1, randomSquareLocation2);

            }

        }

        public void solve() {

            int counter = 0; // Iteration counter
            List<int> lastScores = new List<int>(); // List with last seen scores

            while (this.totalScore != 0) {

                improveRandomBlock();

                int minLastScore = int.MaxValue;
                int maxLastScore = 0;

                // Determine the min and max scores
                foreach (int score in lastScores) {
                    if (score < minLastScore) {
                        minLastScore = score;
                    }
                    else if (score > maxLastScore) {
                        maxLastScore = score;
                    }
                }

                // If the differnce between the last scores is too small execute a random walk
                if (lastScores.Count == this.viewDistance && maxLastScore - minLastScore <= randomWalkDifferenceTrigger) {
                    randomWalk();
                    lastScores.Clear();
                }

                // If the 'lastScores' list is not yet full just add the score to it
                if (lastScores.Count < this.viewDistance) {
                    lastScores.Add(this.totalScore);
                }
                // Otherwise remove the oldest score and add the new score
                else {
                    lastScores.RemoveAt(0);
                    lastScores.Add(this.totalScore);
                }

                counter++;

            }

            // Print the solved sudoku
            print();

            Console.WriteLine("Solved in " + counter.ToString() + " iterations.");

        }

    }

}