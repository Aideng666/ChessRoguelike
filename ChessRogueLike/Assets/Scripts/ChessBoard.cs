using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessBoard : MonoBehaviour
{
    [SerializeField]
    private Square _squarePrefab;

    [SerializeField]
    private ChessPiece _pawnPrefab;

    [SerializeField]
    private ChessPiece _bishopPrefab;

    [SerializeField]
    private ChessPiece _knightPrefab;

    [SerializeField]
    private ChessPiece _rookPrefab;

    [SerializeField]
    private ChessPiece _queenPrefab;

    [SerializeField]
    private ChessPiece _kingPrefab;

    public Square SelectedSquare { get; set; }
    public Square[,] Board { get; set; } = new Square[8, 8]; //first is rank second is file

    public Action<Square> OnSquareClicked;
    public Action<Square> OnMouseReleased;

    private Player _player;

    private void Awake()
    {
        _createBoard();

        foreach (var square in Board)
        {
            square.OnSquareClicked += _onSquareClicked;
            square.OnMouseReleased += _onMouseReleased;
        }

        _player = new Player(this);
    }

    private void OnDestroy()
    {
        foreach (var square in Board)
        {
            square.OnSquareClicked -= _onSquareClicked;
            square.OnMouseReleased -= _onMouseReleased;
        }
    }

    private void Update()
    {
        _player.Tick();

        if (Input.GetMouseButtonUp(0))
        {
            var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePosInWorld.x < 0.5f || mousePosInWorld.x > 8.5f || mousePosInWorld.y < 0.5f || mousePosInWorld.y > 8.5f)
            {
                OnMouseReleased?.Invoke(null);
            }
        }
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


        //Creating the white pieces
        for (int i = 1; i <= 8; i++)
        {
            _spawnPiece(_pawnPrefab, Board[1, i - 1], Color.white);
        }

        _spawnPiece(_bishopPrefab, Board[0, 2], Color.white);
        _spawnPiece(_bishopPrefab, Board[0, 5], Color.white);

        _spawnPiece(_knightPrefab, Board[0, 1], Color.white);
        _spawnPiece(_knightPrefab, Board[0, 6], Color.white);

        _spawnPiece(_rookPrefab, Board[0, 0], Color.white);
        _spawnPiece(_rookPrefab, Board[0, 7], Color.white);

        _spawnPiece(_queenPrefab, Board[0, 3], Color.white);
        _spawnPiece(_kingPrefab, Board[0, 4], Color.white);

        //black pieces
        for (int i = 1; i <= 8; i++)
        {
            _spawnPiece(_pawnPrefab, Board[6, i - 1], Color.black);
        }

        _spawnPiece(_bishopPrefab, Board[7, 2], Color.black);
        _spawnPiece(_bishopPrefab, Board[7, 5], Color.black);

        _spawnPiece(_knightPrefab, Board[7, 1], Color.black);
        _spawnPiece(_knightPrefab, Board[7, 6], Color.black);

        _spawnPiece(_rookPrefab, Board[7, 0], Color.black);
        _spawnPiece(_rookPrefab, Board[7, 7], Color.black);

        _spawnPiece(_queenPrefab, Board[7, 3], Color.black);
        _spawnPiece(_kingPrefab, Board[7, 4], Color.black);

    }

    private void _spawnPiece(ChessPiece piece, Square square, Color color)
    {
        var spawnedPiece = Instantiate(piece, square.transform.position, Quaternion.identity, transform);
        spawnedPiece.Init(Board[square.Rank - 1, FileToNumber(square.File) - 1], color);
    }

    private void _onSquareClicked(Square square)
    {
        OnSquareClicked?.Invoke(square);
    }

    private void _onMouseReleased(Square square)
    {
        OnMouseReleased?.Invoke(square);
    }

    public Square GetSquare(string file, int rank)
    {
        if (string.Compare(file, "h") > 0 || rank > 8 || rank <= 0 || file == "")
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
