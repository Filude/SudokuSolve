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

        private List<Location> getAllSquareLocationsInBlock(Location blockLocation) {

            List<Location> locations = new List<Location>();

            for (int x = blockLocation.x; x < blockLocation.x + 3; x++) {
                for (int y = blockLocation.y; y < blockLocation.y + 3; y++) {
                    locations.Add(new Location(x,y));
                }
            }

            return locations;

        }

        private void improveRandomBlock() {

            Random random = new Random();
            Location randomBlockLocation = new Location(random.Next(0, 3), random.Next(0, 3));
            Dictionary<Swap, int> swapScores;
            List<Location> squareLocationsInBlock = getAllSquareLocationsInBlock(randomBlockLocation);

            foreach (squareLocationsInBlock as sqaureLocation) {

                foreach (squareLocationsInBlock as sqaureLocation) {

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
