using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] TMP_Text _rankText;
    [SerializeField] TMP_Text _fileText;

    private SpriteRenderer _spriteRenderer;
    private ChessBoard _board;

    public int Rank { get; private set; }
    public string File { get; private set; }

    public void InitSquare(ChessBoard board, string file, int rank, Color squareColor)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = squareColor;
        _board = board;

        Rank = rank;
        File = file;

        File = File.ToLower();
        _fileText.enabled = false;
        _rankText.enabled = false;

        if (Rank == 1)
        {
            _fileText.enabled = true;
            _fileText.text = File;
        }
        if (File == "a")
        {
            _rankText.enabled = true;
            _rankText.text = Rank.ToString();
        }

        gameObject.name = File + Rank.ToString();

        if (_spriteRenderer.color == Color.white)
        {
            _fileText.color = Color.black;
            _rankText.color = Color.black;
        }
    }

    private void OnMouseDown()
    {
        print($"Selected {File.ToUpper()}{Rank}");
        _board.SelectedSquare = this;
    }
}
