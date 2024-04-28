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
        board.SelectedSquare = board.Board[1, 1];  //B2

        //Checking North
        Square square = board.GetAdjacentSquare(Direction.North);

        Assert.AreEqual("b", square.File);
        Assert.AreEqual(3, square.Rank);

        //South
        square = board.GetAdjacentSquare(Direction.South);

        Assert.AreEqual("b", square.File);
        Assert.AreEqual(1, square.Rank);

        //East
        square = board.GetAdjacentSquare(Direction.East);

        Assert.AreEqual("c", square.File);
        Assert.AreEqual(2, square.Rank);

        //West
        square = board.GetAdjacentSquare(Direction.West);

        Assert.AreEqual("a", square.File);
        Assert.AreEqual(2, square.Rank);

        //NorthEast
        square = board.GetAdjacentSquare(Direction.NorthEast);

        Assert.AreEqual("c", square.File);
        Assert.AreEqual(3, square.Rank);

        //NorthWest
        square = board.GetAdjacentSquare(Direction.NorthWest);

        Assert.AreEqual("a", square.File);
        Assert.AreEqual(3, square.Rank);

        //SouthEast
        square = board.GetAdjacentSquare(Direction.SouthEast);

        Assert.AreEqual("c", square.File);
        Assert.AreEqual(1, square.Rank);

        //SouthWest
        square = board.GetAdjacentSquare(Direction.SouthWest);

        Assert.AreEqual("a", square.File);
        Assert.AreEqual(1, square.Rank);

        //Checking Adjacent squares to E4
        board.SelectedSquare = board.Board[3, 4]; //E4

        //Checking North
        square = board.GetAdjacentSquare(Direction.North);

        Assert.AreEqual("e", square.File);
        Assert.AreEqual(5, square.Rank);

        //South
        square = board.GetAdjacentSquare(Direction.South);

        Assert.AreEqual("e", square.File);
        Assert.AreEqual(3, square.Rank);

        //East
        square = board.GetAdjacentSquare(Direction.East);

        Assert.AreEqual("f", square.File);
        Assert.AreEqual(4, square.Rank);

        //West
        square = board.GetAdjacentSquare(Direction.West);

        Assert.AreEqual("d", square.File);
        Assert.AreEqual(4, square.Rank);

        //NorthEast
        square = board.GetAdjacentSquare(Direction.NorthEast);

        Assert.AreEqual("f", square.File);
        Assert.AreEqual(5, square.Rank);

        //NorthWest
        square = board.GetAdjacentSquare(Direction.NorthWest);

        Assert.AreEqual("d", square.File);
        Assert.AreEqual(5, square.Rank);

        //SouthEast
        square = board.GetAdjacentSquare(Direction.SouthEast);

        Assert.AreEqual("f", square.File);
        Assert.AreEqual(3, square.Rank);

        //SouthWest
        square = board.GetAdjacentSquare(Direction.SouthWest);

        Assert.AreEqual("d", square.File);
        Assert.AreEqual(3, square.Rank);

        yield return null;
    }
}
