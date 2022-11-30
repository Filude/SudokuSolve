namespace Sudoku {

    class Sudoku {

        private List<List<int>> values; // two dimensional list, x,y
        private int score; // score of the sudoku, lower is better

        public Sudoku(List<List<int>> fixedValues) {

            this.values = addRandomValues(fixedValues);
            this.score = calculateScore();

        }

        private List<List<int>> addRandomValues(List<List<int>> fixedValues) {
            // TODO
            return new List<List<int>>();
        }

        private int calculateScore() {
            // TODO
            return 0;
        }

        // Update the score but only recalculate the rows, and columns where a swap has occured
        private int getUpdatedScore(Location swapLocation1, Location swapLocation2) {
            // TODO
            return 0;
        }

        private void swap(Location swapLocation1, Location swapLocation2) {
            // TODO
        }

        private void improveRandomBlock() {

            // TODO: Choose random block

            Dictionary<Swap, int> swapScores;

            foreach (/*TODO: square in block that is not fixed*/) {

                foreach (/*TODO: square in block except self that is not fixed*/) {

                    swap(swapLocation1, swapLocation2); // Execute swap
                    int newScore = getUpdatedScore(swapLocation1, swapLocation2);
                    swap(swapLocation1, swapLocation2) // Reverse swap to normal situation

                    swapScores.Add(/* TODO: Swap, newScore */);

                }

            }

            // TODO: Execute best swap

        }

        public void solve() {

            while (true) {
                improveRandomBlock();
            }

        }

    }

}
