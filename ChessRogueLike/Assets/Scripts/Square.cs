using System;
using System.Collections;
using System.Collections.Generic;
using ChessPieces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour
{
    [SerializeField] private TMP_Text _rankText;
    [SerializeField] private TMP_Text _fileText;
    [SerializeField] private Image _availableToMoveIcon;
    [SerializeField] private Image _availableToMoveIconWithPiece;

    private SpriteRenderer _spriteRenderer;
    private ChessBoard _board;
    private Color _savedColor;

    public int Rank { get; private set; }
    public string File { get; private set; }
    public ChessPiece CurrentPiece { get; private set; } = null;
    public SquareState SquareState { get; private set; }

    public Action<Square> OnSquareClicked;
    public Action<Square> OnMouseReleased;

    public void InitSquare(ChessBoard board, string file, int rank, Color squareColor)
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _board = board;
        _availableToMoveIcon.enabled = false;
        _availableToMoveIconWithPiece.enabled = false;

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

        if (squareColor == Color.black)
        {
            _spriteRenderer.color = Color.grey;
            _savedColor = Color.grey;
        }
        else
        {
            _spriteRenderer.color = Color.white;
            _savedColor = Color.white;
        }

        if (_spriteRenderer.color == Color.white)
        {
            _fileText.color = Color.black;
            _rankText.color = Color.black;
        }

    }

    public void SetCurrentPiece(ChessPiece newPiece)
    {
        if (CurrentPiece != null)
        {
            //destroy piece
        }

        CurrentPiece = newPiece;
    }

    public void SetSquareState(SquareState state)
    {
        SquareState = state;
        _availableToMoveIcon.enabled = false;
        _availableToMoveIconWithPiece.enabled = false;

        switch (state)
        {
            case SquareState.Default:

                _spriteRenderer.color = _savedColor;

                break;

            case SquareState.Selected:

                _spriteRenderer.color = Color.yellow;

                break;

            case SquareState.AvailableToMove:

                if (CurrentPiece == null)
                {
                    _spriteRenderer.color = _savedColor;
                    _availableToMoveIcon.enabled = true;
                }
                else
                {
                    _spriteRenderer.color = _savedColor;
                    _availableToMoveIconWithPiece.enabled = true;
                }

                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (Math.Abs(mousePosInWorld.x - transform.position.x) < 0.5f && Math.Abs(mousePosInWorld.y - transform.position.y) < 0.5f)
            {
                OnMouseReleased?.Invoke(this);
            }
        }
    }

    private void OnMouseDown()
    {
        OnSquareClicked?.Invoke(this);
    }
}

public enum SquareState
{
    Default,
    Selected,
    AvailableToMove
}
