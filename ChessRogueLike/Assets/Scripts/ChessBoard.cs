using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ChessBoard : MonoBehaviour
{
    [SerializeField]
    private Square _squarePrefab;

    [SerializeField]
    private ChessPiece _pawnPrefab;

    public Square SelectedSquare { get; set; }
    public Square[,] Board { get; set; } = new Square[8, 8]; //first is rank second is file

    private void Awake()
    {
        _createBoard();
    }

    private void _createBoard()
    {
        Color nextSquareColor = Color.black;

        for (int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                Board[i, j] = Instantiate(_squarePrefab, new Vector3(j + 1, i + 1, 0), Quaternion.identity, transform);
                Board[i, j].InitSquare(this, NumberToFile(j + 1), i + 1, nextSquareColor);

                if (nextSquareColor ==  Color.black)
                {
                    nextSquareColor = Color.white;
                }
                else if (nextSquareColor == Color.white)
                {
                    nextSquareColor = Color.black;
                }
            }

            if (nextSquareColor == Color.black)
            {
                nextSquareColor = Color.white;
            }
            else if (nextSquareColor == Color.white)
            {
                nextSquareColor = Color.black;
            }
        }

        var pawn = Instantiate(_pawnPrefab, new Vector3(5, 2, 0), Quaternion.identity, transform);
        pawn.Init(Board[1, 4]);
    }

    public Square GetSquare(string file, int rank)
    {
        if (string.Compare(file, "h") > 0 || rank > 8 || rank == 0 || file == "")
        {
            return null;
        }

        return Board[rank - 1, FileToNumber(file) - 1];
    }

    public List<Square> GetSquaresInDirection(Square origin, Direction direction)
    {
        var availableSquares = new List<Square>();

        var squareToCheck = origin;

        while (squareToCheck != null)
        {
            var nextSquare = GetAdjacentSquare(squareToCheck, direction);

            if (nextSquare != null)
            {
                availableSquares.Add(nextSquare);
            }

            squareToCheck = nextSquare;
        }

        return availableSquares;
    }

    public Square CheckSquareKnightSpaceAway(Square origin, KnightDirection direction)
    {
        Square knightSquare = null;

        switch (direction)
        {
            case KnightDirection.OneOClock:

                knightSquare = GetSquare(NumberToFile(FileToNumber(origin.File) + 1), origin.Rank + 2);

                break;

            case KnightDirection.TwoOClock:

                knightSquare = GetSquare(NumberToFile(FileToNumber(origin.File) + 2), origin.Rank + 1);

                break;

            case KnightDirection.FourOClock:

                knightSquare = GetSquare(NumberToFile(FileToNumber(origin.File) + 2), origin.Rank - 1);

                break;

            case KnightDirection.FiveOClock:

                knightSquare = GetSquare(NumberToFile(FileToNumber(origin.File) + 1), origin.Rank - 2);

                break;

            case KnightDirection.SevenOClock:

                knightSquare = GetSquare(NumberToFile(FileToNumber(origin.File) - 1), origin.Rank - 2);

                break;

            case KnightDirection.EightOClock:

                knightSquare = GetSquare(NumberToFile(FileToNumber(origin.File) - 2), origin.Rank - 1);

                break;

            case KnightDirection.TenOClock:

                knightSquare = GetSquare(NumberToFile(FileToNumber(origin.File) - 2), origin.Rank + 1);

                break;

            case KnightDirection.ElevenOCLock:

                knightSquare = GetSquare(NumberToFile(FileToNumber(origin.File) - 1), origin.Rank + 2);

                break;
        }

        return knightSquare;
    }

    public Square GetAdjacentSquare(Square origin, Direction direction)
    {
        Square adjacentSquare = null;

        switch (direction)
        {
            case Direction.North:

                adjacentSquare = GetSquare(origin.File, origin.Rank + 1);

                break;

            case Direction.East:

                adjacentSquare = GetSquare(NumberToFile(FileToNumber(origin.File) + 1), origin.Rank);

                break;

            case Direction.South:

                adjacentSquare = GetSquare(origin.File, origin.Rank -  1);

                break;

            case Direction.West:

                adjacentSquare = GetSquare(NumberToFile(FileToNumber(origin.File) - 1), origin.Rank);

                break;

            case Direction.NorthEast:

                adjacentSquare = GetSquare(NumberToFile(FileToNumber(origin.File) + 1), origin.Rank + 1);

                break;

            case Direction.NorthWest:

                adjacentSquare = GetSquare(NumberToFile(FileToNumber(origin.File) - 1), origin.Rank + 1);

                break;

            case Direction.SouthEast:

                adjacentSquare = GetSquare(NumberToFile(FileToNumber(origin.File) + 1), origin.Rank - 1);

                break;

            case Direction.SouthWest:

                adjacentSquare = GetSquare(NumberToFile(FileToNumber(origin.File) - 1), origin.Rank - 1);

                break;
        }

        if (adjacentSquare == null)
        {
            print("No Adjacent Square Found");
        }

        return adjacentSquare;
    }

    public static int FileToNumber(string file)
    {
        file = file.ToLower();
        int number = 0;

        switch (file)
        {
            case "a":

                number = 1;

                break;

            case "b":

                number = 2;

                break;

            case "c":

                number = 3;

                break;

            case "d":

                number = 4;

                break;

            case "e":

                number = 5;

                break;

            case "f":

                number = 6;

                break;

            case "g":

                number = 7;

                break;

            case "h":

                number = 8;

                break;

            default:

                number = 0;

                break;
        }

        return number;
    }

    public static string NumberToFile(int number)
    {
        string file = "";

        switch (number)
        {
            case 1:

                file = "a";

                break;

            case 2:

                file = "b";

                break;

            case 3:

                file = "c";

                break;

            case 4:

                file = "d";

                break;

            case 5:

                file = "e";

                break;

            case 6:

                file = "f";

                break;

            case 7:

                file = "g";

                break;

            case 8:

                file = "h";

                break;

            default:

                file = "";

                break;
        }

        return file;
    }
}

public enum Direction
{
    North,
    East,
    South,
    West,
    NorthEast,
    NorthWest,
    SouthEast,
    SouthWest
}

public enum KnightDirection
{
    OneOClock,
    TwoOClock,
    FourOClock,
    FiveOClock,
    SevenOClock,
    EightOClock,
    TenOClock,
    ElevenOCLock
}
