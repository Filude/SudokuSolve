﻿using System;

namespace Sudoku {

    struct Location {

        public int x, y;
        public Location(int x, int y) {
            this.x = x;
            this.y = y;
        }

    }

    struct Swap {

        public Location location1, location2;
        public Swap(Location location1, Location location2) {
            this.location1 = location1;
            this.location2 = location2;
        }

    }

    struct Value {

        public int value;
        public bool isFixed;
        public Value(int value, bool isFixed) {
            this.value = value;
            this.isFixed = isFixed;
        }

    }

    class Program {

        public static List<List<int>> convertRawSudokuInput(String rawInput) {
            
            // Split input on spaces and initialize a variable for the rows
            string[] input = rawInput.Trim().Split(' ');
            List<List<int>> rows = new List<List<int>>();

            // Populate the 'rows' variable with the input data
            for (int row = 0; row < 9; row++) {

                rows.Add(new List<int>());
            
                for (int column = 0; column < 9; column++) {

                    int inputValue = int.Parse(input[row * 9 + column]);
                    rows[row].Add(inputValue);

                }
            
            }

            return rows;

        }

        public static void Main(String[] args) {

            String rawSudokuInput = Console.ReadLine();
            List<List<int>> sudokuInput = convertRawSudokuInput(rawSudokuInput);
            Sudoku sudoku = new Sudoku(sudokuInput);
            sudoku.solve();

        }

    }

}