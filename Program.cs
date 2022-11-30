using System;

namespace Sudoku {

    struct Location {

        public int x, y;

    }

    struct Swap {

        public Location location1, location2;

    }

    class Program {

        public static List<List<int>> convertRawSudokuInput(String string) {
            // TODO
            return return new List<List<int>>();
        }

        public static void Main(String[] args) {

            String rawSudokuInput = // TODO: Get input
            Sudoku sudoku = new Sudoku(convertRawSudokuInput(rawSudokuInput));
            Sudoku.solve();

        }

    }

}