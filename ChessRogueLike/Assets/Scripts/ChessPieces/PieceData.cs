namespace ChessPieces
{
    public class PieceData
    {
        public PieceType PieceType { get; private set; }
        public int MaterialValue { get; private set; }
        
        //public ChessPiece PiecePrefab { get; private set; }

        public PieceData(PieceType type, int materialValue)
        {
            PieceType = type;
            MaterialValue = materialValue;
            //PiecePrefab = prefabToSpawn;
        }
    }
}
