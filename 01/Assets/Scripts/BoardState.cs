using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Diag=System.Diagnostics;


public class BoardState {
    public static int BoardSide = 5;
    public static int BoardSize = BoardSide * BoardSide;
    // safe togles are the indeces of squares that we can toggle individually
    // without affecting the solvability of the board
    public static List<int> safeToggles;
    // static patterns are toggling sequences that don't alter final state.
    // Used for determining solvability, and minimizing solutions.
    public static ulong[] silentPatterns;
    static BoardState() {
        safeToggles = new List<int>() { 6, 8, 12, 16, 18 };

        silentPatterns = new ulong[] {
            // XX.XX
            // .....
            // XX.XX
            // .....
            // XX.XX
            IntListToUlong(new List<int>() {
                0,  1,  3,  4,
                10, 11, 13, 14,
                20, 21, 23, 24
            }),
            // X.X.X
            // X.X.X
            // .....
            // X.X.X
            // X.X.X
            IntListToUlong(new List<int>() {
                0,  2,  4,
                5,  7,  9,
                15, 17, 19,
                20, 22, 24
            }),
            // 3 is combination of first two.  Not used in solvability,
            // but can be used in minimizing solutions.
            // .XXX.
            // X.X.X
            // XX.XX
            // X.X.X
            // .XXX.
            IntListToUlong(new List<int>() {
                1, 2, 3,
                5, 7, 9,
                10, 11, 13, 14,
                15, 17, 19,
                21, 22, 23
            })
        };
    }
    // returns the total number of asserted bits
    public static int Bitcount(ulong n)
    {
	    int count = 0;
	    while (n != 0)
	    {
	        count++;
	        n &= (n - 1);
        }
	    return count;
    }
    public static BoardState FromToggles(List<int> toggles) {
        BoardState state = new BoardState();
        foreach(int i in toggles) {
            state.ToggleArea(i);
        }
        return state;
    }

    // Converts a list of integers specifying asserted bits
    // into a ulong
    public static ulong IntListToUlong(List<int> list) {
        ulong val = 0UL;
        foreach(int i in list) {
            val |= 1UL << i;
        }
        return val;
    }
    public static int IndexToX(int index) {
        return index / BoardState.BoardSide; // integer divison, floors
    }
    public static int IndexToY(int index) {
        return index % BoardSide;
    }

    public ulong bits;

    public BoardState(List<int> list = null) {
        Diag.Debug.Assert(BoardSide < 8, "Can't store board that big in long");
        bits = 0;
        if(list != null) {
            foreach(int i in list) {
                Toggle(i);
            }
        }
    }

    public bool IsClear() {
        return bits == 0UL;
    }

    public bool IsSolvable() {
        return (Bitcount(bits & silentPatterns[0]) % 2) == 0
            && (Bitcount(bits & silentPatterns[1]) % 2) == 0;
    }

    public bool Get(int index) {
        return (bits & (1UL << index)) != 0;
    }

    public string GetBinaryString() {
        int size = BoardSize;
        char[] b = new char[size];

        for(int i=0;i<size;i++) {
            if((bits & (1UL << i)) != 0) {
                b[size-i-1] = '1';
            } else {
                b[size-i-1] = '0';
            }
        }
        return new string(b);
    }

    public void MakeSolvable() {
        bool pass1 = (Bitcount(bits & silentPatterns[0]) % 2) == 0;
        bool pass2 = (Bitcount(bits & silentPatterns[1]) % 2) == 0;
        if(!pass1 && !pass2) {
            Toggle(0);
        }
        if(!pass1 && pass2) {
            Toggle(1);
        }
        if(pass1 && !pass2) {
            Toggle(5);
        }
    }

    public void RandomizeBoard() {
        byte[] buffer = new byte[sizeof(ulong)];
        System.Random rnd = new System.Random();
        rnd.NextBytes(buffer);
        bits = BitConverter.ToUInt64(buffer, 0);
        MakeSolvable();
    }

    public void Set(List<int> list) {
        BoardState newstate = new BoardState(list);
        Diag.Debug.Assert(newstate.IsSolvable());
        bits = newstate.bits;
    }

    public override string ToString() {
        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < BoardSize; i++) {
            if((bits & (1UL << i)) != 0) {
                if(sb.Length != 0) {
                    sb.Append(", ");
                }
                sb.Append(i);
            }
        }
        return sb.ToString();
    }

    public void Toggle(int index) {
        Diag.Debug.Assert(index < BoardSize);
        bits ^= (1UL << index);
    }

    /// <summary>
    /// Toggles a given square, and all surrounding squares.
    /// </summary>
    public void ToggleArea(int index) {
        System.Diagnostics.Debug.Assert(index < BoardSize);
        ulong toggle = (1UL << index);
        if(IndexToY(index) > 0) {
            toggle |= (1UL << (index - 1));
        }
        if(IndexToY(index) < BoardSide -1) {
            toggle |= (1UL << (index + 1));
        }
        if(IndexToX(index) > 0) {
            toggle |= (1UL << (index - BoardSide));
        }
        if(IndexToX(index) < BoardSide - 1) {
            toggle |= (1UL << (index + BoardSide));
        }
        bits ^= toggle;
    }
}
