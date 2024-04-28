using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardTests
{
    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator CheckAllAdjecentSquaresTest()
    {
        var gameObject = new GameObject();
        var board = gameObject.AddComponent<ChessBoard>();

        //Checking Adjacent squares to B2
        Square originSquare = board.Board[1, 1];  //B2

        //Checking North
        Square square = board.GetAdjacentSquare(originSquare, Direction.North);

        Assert.AreEqual("b", square.File);
        Assert.AreEqual(3, square.Rank);

        //South
        square = board.GetAdjacentSquare(originSquare, Direction.South);

        Assert.AreEqual("b", square.File);
        Assert.AreEqual(1, square.Rank);

        //East
        square = board.GetAdjacentSquare(originSquare, Direction.East);

        Assert.AreEqual("c", square.File);
        Assert.AreEqual(2, square.Rank);

        //West
        square = board.GetAdjacentSquare(originSquare, Direction.West);

        Assert.AreEqual("a", square.File);
        Assert.AreEqual(2, square.Rank);

        //NorthEast
        square = board.GetAdjacentSquare(originSquare, Direction.NorthEast);

        Assert.AreEqual("c", square.File);
        Assert.AreEqual(3, square.Rank);

        //NorthWest
        square = board.GetAdjacentSquare(originSquare, Direction.NorthWest);

        Assert.AreEqual("a", square.File);
        Assert.AreEqual(3, square.Rank);

        //SouthEast
        square = board.GetAdjacentSquare(originSquare, Direction.SouthEast);

        Assert.AreEqual("c", square.File);
        Assert.AreEqual(1, square.Rank);

        //SouthWest
        square = board.GetAdjacentSquare(originSquare, Direction.SouthWest);

        Assert.AreEqual("a", square.File);
        Assert.AreEqual(1, square.Rank);

        //Checking Adjacent squares to E4
        originSquare = board.Board[3, 4]; //E4

        //Checking North
        square = board.GetAdjacentSquare(originSquare, Direction.North);

        Assert.AreEqual("e", square.File);
        Assert.AreEqual(5, square.Rank);

        //South
        square = board.GetAdjacentSquare(originSquare, Direction.South);

        Assert.AreEqual("e", square.File);
        Assert.AreEqual(3, square.Rank);

        //East
        square = board.GetAdjacentSquare(originSquare, Direction.East);

        Assert.AreEqual("f", square.File);
        Assert.AreEqual(4, square.Rank);

        //West
        square = board.GetAdjacentSquare(originSquare, Direction.West);

        Assert.AreEqual("d", square.File);
        Assert.AreEqual(4, square.Rank);

        //NorthEast
        square = board.GetAdjacentSquare(originSquare, Direction.NorthEast);

        Assert.AreEqual("f", square.File);
        Assert.AreEqual(5, square.Rank);

        //NorthWest
        square = board.GetAdjacentSquare(originSquare, Direction.NorthWest);

        Assert.AreEqual("d", square.File);
        Assert.AreEqual(5, square.Rank);

        //SouthEast
        square = board.GetAdjacentSquare(originSquare, Direction.SouthEast);

        Assert.AreEqual("f", square.File);
        Assert.AreEqual(3, square.Rank);

        //SouthWest
        square = board.GetAdjacentSquare(originSquare, Direction.SouthWest);

        Assert.AreEqual("d", square.File);
        Assert.AreEqual(3, square.Rank);

        yield return null;
    }

    [UnityTest]
    public IEnumerator CheckAllSquaresInAllDirections()
    {
        var gameObject = new GameObject();
        var board = gameObject.AddComponent<ChessBoard>();

        Square originSquare = board.Board[1, 1];  //B2

        List<Square> squares = new List<Square>();

        squares = board.GetSquaresInDirection(originSquare, Direction.North);

        Assert.AreEqual(6, squares.Count);
        Assert.AreEqual("b", squares[0].File);
        Assert.AreEqual(3, squares[0].Rank);
        Assert.AreEqual("b", squares[5].File);
        Assert.AreEqual(8, squares[5].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.South);

        Assert.AreEqual(1, squares.Count);
        Assert.AreEqual("b", squares[0].File);
        Assert.AreEqual(1, squares[0].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.East);

        Assert.AreEqual(6, squares.Count);
        Assert.AreEqual("c", squares[0].File);
        Assert.AreEqual(2, squares[0].Rank);
        Assert.AreEqual("h", squares[5].File);
        Assert.AreEqual(2, squares[5].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.West);

        Assert.AreEqual(1, squares.Count);
        Assert.AreEqual("a", squares[0].File);
        Assert.AreEqual(2, squares[0].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.NorthEast);

        Assert.AreEqual(6, squares.Count);
        Assert.AreEqual("c", squares[0].File);
        Assert.AreEqual(3, squares[0].Rank);
        Assert.AreEqual("h", squares[5].File);
        Assert.AreEqual(8, squares[5].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.NorthWest);

        Assert.AreEqual(1, squares.Count);
        Assert.AreEqual("a", squares[0].File);
        Assert.AreEqual(3, squares[0].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.SouthEast);

        Assert.AreEqual(1, squares.Count);
        Assert.AreEqual("c", squares[0].File);
        Assert.AreEqual(1, squares[0].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.SouthWest);

        Assert.AreEqual(1, squares.Count);
        Assert.AreEqual("a", squares[0].File);
        Assert.AreEqual(1, squares[0].Rank);

        originSquare = board.Board[5, 3]; //D6

        squares = board.GetSquaresInDirection(originSquare, Direction.North);

        Assert.AreEqual(2, squares.Count);
        Assert.AreEqual("d", squares[0].File);
        Assert.AreEqual(7, squares[0].Rank);
        Assert.AreEqual("d", squares[1].File);
        Assert.AreEqual(8, squares[1].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.South);

        Assert.AreEqual(5, squares.Count);
        Assert.AreEqual("d", squares[0].File);
        Assert.AreEqual(5, squares[0].Rank);
        Assert.AreEqual("d", squares[4].File);
        Assert.AreEqual(1, squares[4].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.East);

        Assert.AreEqual(4, squares.Count);
        Assert.AreEqual("e", squares[0].File);
        Assert.AreEqual(6, squares[0].Rank);
        Assert.AreEqual("h", squares[3].File);
        Assert.AreEqual(6, squares[3].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.West);

        Assert.AreEqual(3, squares.Count);
        Assert.AreEqual("c", squares[0].File);
        Assert.AreEqual(6, squares[0].Rank);
        Assert.AreEqual("a", squares[2].File);
        Assert.AreEqual(6, squares[2].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.NorthEast);

        Assert.AreEqual(2, squares.Count);
        Assert.AreEqual("e", squares[0].File);
        Assert.AreEqual(7, squares[0].Rank);
        Assert.AreEqual("f", squares[1].File);
        Assert.AreEqual(8, squares[1].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.NorthWest);

        Assert.AreEqual(2, squares.Count);
        Assert.AreEqual("c", squares[0].File);
        Assert.AreEqual(7, squares[0].Rank);
        Assert.AreEqual("b", squares[1].File);
        Assert.AreEqual(8, squares[1].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.SouthEast);

        Assert.AreEqual(4, squares.Count);
        Assert.AreEqual("e", squares[0].File);
        Assert.AreEqual(5, squares[0].Rank);
        Assert.AreEqual("h", squares[3].File);
        Assert.AreEqual(2, squares[3].Rank);

        squares = board.GetSquaresInDirection(originSquare, Direction.SouthWest);

        Assert.AreEqual(3, squares.Count);
        Assert.AreEqual("c", squares[0].File);
        Assert.AreEqual(5, squares[0].Rank);
        Assert.AreEqual("a", squares[2].File);
        Assert.AreEqual(3, squares[2].Rank);

        yield return null;
    }

    [UnityTest]
    public IEnumerator CheckAllKnightSpaceSquaresTest()
    {
        var gameObject = new GameObject();
        var board = gameObject.AddComponent<ChessBoard>();

        //TODO: Make this test

        ////Checking Adjacent squares to B2
        //Square originSquare = board.Board[1, 1];  //B2

        ////Checking North
        //Square square = board.GetAdjacentSquare(originSquare, Direction.North);

        //Assert.AreEqual("b", square.File);
        //Assert.AreEqual(3, square.Rank);

        ////South
        //square = board.GetAdjacentSquare(originSquare, Direction.South);

        //Assert.AreEqual("b", square.File);
        //Assert.AreEqual(1, square.Rank);

        ////East
        //square = board.GetAdjacentSquare(originSquare, Direction.East);

        //Assert.AreEqual("c", square.File);
        //Assert.AreEqual(2, square.Rank);

        ////West
        //square = board.GetAdjacentSquare(originSquare, Direction.West);

        //Assert.AreEqual("a", square.File);
        //Assert.AreEqual(2, square.Rank);

        ////NorthEast
        //square = board.GetAdjacentSquare(originSquare, Direction.NorthEast);

        //Assert.AreEqual("c", square.File);
        //Assert.AreEqual(3, square.Rank);

        ////NorthWest
        //square = board.GetAdjacentSquare(originSquare, Direction.NorthWest);

        //Assert.AreEqual("a", square.File);
        //Assert.AreEqual(3, square.Rank);

        ////SouthEast
        //square = board.GetAdjacentSquare(originSquare, Direction.SouthEast);

        //Assert.AreEqual("c", square.File);
        //Assert.AreEqual(1, square.Rank);

        ////SouthWest
        //square = board.GetAdjacentSquare(originSquare, Direction.SouthWest);

        //Assert.AreEqual("a", square.File);
        //Assert.AreEqual(1, square.Rank);

        ////Checking Adjacent squares to E4
        //originSquare = board.Board[3, 4]; //E4

        ////Checking North
        //square = board.GetAdjacentSquare(originSquare, Direction.North);

        //Assert.AreEqual("e", square.File);
        //Assert.AreEqual(5, square.Rank);

        ////South
        //square = board.GetAdjacentSquare(originSquare, Direction.South);

        //Assert.AreEqual("e", square.File);
        //Assert.AreEqual(3, square.Rank);

        ////East
        //square = board.GetAdjacentSquare(originSquare, Direction.East);

        //Assert.AreEqual("f", square.File);
        //Assert.AreEqual(4, square.Rank);

        ////West
        //square = board.GetAdjacentSquare(originSquare, Direction.West);

        //Assert.AreEqual("d", square.File);
        //Assert.AreEqual(4, square.Rank);

        ////NorthEast
        //square = board.GetAdjacentSquare(originSquare, Direction.NorthEast);

        //Assert.AreEqual("f", square.File);
        //Assert.AreEqual(5, square.Rank);

        ////NorthWest
        //square = board.GetAdjacentSquare(originSquare, Direction.NorthWest);

        //Assert.AreEqual("d", square.File);
        //Assert.AreEqual(5, square.Rank);

        ////SouthEast
        //square = board.GetAdjacentSquare(originSquare, Direction.SouthEast);

        //Assert.AreEqual("f", square.File);
        //Assert.AreEqual(3, square.Rank);

        ////SouthWest
        //square = board.GetAdjacentSquare(originSquare, Direction.SouthWest);

        //Assert.AreEqual("d", square.File);
        //Assert.AreEqual(3, square.Rank);

        yield return null;
    }
}
