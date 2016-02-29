using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CandPCI_1.Algorithms
{
    public class Position : ICloneable
    {
        public int Row { get; set; }
        public int Column { get; set; }

        public Position() { }

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public object Clone()
        {
            return new Position(Row, Column);
        }
    }

    public class CardanGrilleKey
    {
        public Position[] Positions { get; set; }
        public int MatrixOrder { get; set; }
    }

    public class CardanGrilleCipher
    {
        public string Encrypt(string message, CardanGrilleKey key)
        {
            // проверка ключа на четность
            // проверка вмещается ли сообщение

            return new Encrypter().Encrypt(message, key);
        }

        public string Decrypt(string message, CardanGrilleKey key)
        {
            // проверка ключа на четность
            // проверка вмещается ли сообщение

            return new Decrypter().Decrypt(message, key);
        }

        private char GetLetter(string message, int keyLength, int i, int j)
        {
            return message[i * keyLength + j];
        }

        

        private void ValidKey(Position[] key)
        {
            if (key.Length < 2)
                throw new ArgumentException();
            if (key.Length != 4)
                throw new ArgumentException();
            if (key.Max(p => Math.Max(p.Row, p.Column)) != 4)
                throw new ArgumentException();
            
        }

        private class TableMessage
        {
            private StringBuilder message;
            private int order;

            public int Shift { get; set; }

            public string Result { get { return message.ToString(); } }

            public TableMessage(string message, int order)
            {
                if (order < 2)
                    throw new ArgumentException();
                Shift = 0;
                this.message = new StringBuilder(message);
                this.order = order;
            }

            private int GetLineIndex(int i, int j)
            {
                int halfOrder = order / 2;
                i += halfOrder;
                j += halfOrder;

                int index = i * order + j;
                if (order % 2 == 0)
                {
                    int additionalRow = i >= halfOrder ? 1 : 0;
                    int additionalColumn = j >= halfOrder ? 1 : 0;
                    index -= additionalRow * order + additionalColumn;
                }
                else
                {
                    if ((i == halfOrder && j >= halfOrder) || i > halfOrder)
                        index--;
                }
                return index;
            }

            public char this[int i, int j]
            {
                get
                {
                    return message[GetLineIndex(i, j) + Shift];
                }

                set
                {
                    message[GetLineIndex(i, j) + Shift] = value;
                }
            }

            public char this[Position position]
            {
                get
                {
                    return this[position.Row, position.Column];
                }
                
                set 
                {
                    this[position.Row, position.Column] = value;
                }
            }
        }

        private abstract class CardanGrilleTraveller
        {
            protected void Travel(string message, CardanGrilleKey key)
            {
                //Position[] currentPositions = new Position[key.Positions.Length];
                //Array.Copy(key.Positions, currentPositions, key.Positions.Length);
                Position[] currentPositions = key.Positions.Select(p => (Position)p.Clone()).ToArray();
                TransformeIndexes(currentPositions, key.MatrixOrder);
                for (var i = 0; i < message.Length; i++)
                {
                    // учесть первый поворот
                    if (i != 0 && i % currentPositions.Length == 0)
                        TurnPositions(currentPositions);
                    var position = currentPositions[i % currentPositions.Length];
                    ElementAction(position);
                }
            }

            private void TransformeIndexes(Position[] positions, int order)
            {
                var halfOrder = order / 2;
                var orderEven = order % 2 == 0;
                for (var i = 0; i < positions.Length; i++)
                {
                    positions[i].Row = positions[i].Row - halfOrder;
                    if (orderEven && positions[i].Row >= 0)
                        positions[i].Row = positions[i].Row + 1;

                    positions[i].Column = positions[i].Column - halfOrder;
                    if (orderEven && positions[i].Column >= 0)
                        positions[i].Column = positions[i].Column + 1;
                }
            }

            private void TurnPositions(Position[] positions)
            {
                for (var i = 0; i < positions.Length; i++)
                    TurnPosition(positions[i]);
            }

            private void TurnPosition(Position position)
            {
                int row = position.Row;
                position.Row = position.Column;
                position.Column = -row;
            }

            protected abstract void ElementAction(Position tablePosition);
        }

        private class Encrypter : CardanGrilleTraveller
        {
            private TableMessage encryptMessage;
            private string message;
            private int messagePosition;
            private int matrixSize;

            public string Encrypt(string message, CardanGrilleKey key)
            {
                matrixSize = (key.MatrixOrder * key.MatrixOrder) / 4 * 4;
                var additionalTable = message.Length % matrixSize == 0 ? 0 : 1;
                var encryptMessageSize = (message.Length / matrixSize + additionalTable) * matrixSize;
                encryptMessage = new TableMessage(new string('_', encryptMessageSize), key.MatrixOrder);
                this.message = message;
                
                messagePosition = 0;
                Travel(message, key);
                return encryptMessage.Result;
            }

            protected override void ElementAction(Position tablePosition)
            {
                if (messagePosition != 0 && messagePosition % matrixSize == 0)
                    encryptMessage.Shift += matrixSize;
                encryptMessage[tablePosition] = message[messagePosition++];
            }
        }

        private class Decrypter : CardanGrilleTraveller
        {
            private StringBuilder decryptMessage;
            private TableMessage message;
            private int messagePosition;
            private int matrixSize;

            public string Decrypt(string message, CardanGrilleKey key)
            {
                decryptMessage = new StringBuilder(new string('_', message.Length));
                this.message = new TableMessage(message, key.MatrixOrder);
                matrixSize = (key.MatrixOrder * key.MatrixOrder) / 4 * 4;
                messagePosition = 0;
                Travel(message, key);
                return decryptMessage.ToString().TrimEnd('_');
            }

            protected override void ElementAction(Position tablePosition)
            {
                if (messagePosition != 0 && messagePosition % matrixSize == 0)
                    message.Shift += matrixSize;
                 decryptMessage[messagePosition++] = message[tablePosition];
            }
        }
    }
}
