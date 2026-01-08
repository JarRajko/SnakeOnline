using SnakeOnline.Snake.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeOnline.Snake.Core.Models
{
    public  class Snake
    {
        private readonly List<Position> _body = new List<Position>();

        public IReadOnlyList<Position> Body { get { return _body; } }

        public Direction CurrentDirection { get; private set; }
        public bool IsAlive { get; set; } = true;

        public Snake(Position startPosition, Direction startDirection)
        {
            _body.Add(startPosition);
            CurrentDirection = startDirection;
            _body.Add(startPosition);
            _body.Add(new Position(startPosition.X, startPosition.Y + 1));
            _body.Add(new Position(startPosition.X, startPosition.Y + 2));
        }

        public void SetDirection(Direction newDirection)
        {
            if (CurrentDirection == Direction.UP && newDirection == Direction.DOWN) return;
            if (CurrentDirection == Direction.DOWN && newDirection == Direction.UP) return;
            if (CurrentDirection == Direction.LEFT && newDirection == Direction.RIGHT) return;
            if (CurrentDirection == Direction.RIGHT && newDirection == Direction.LEFT) return;

            CurrentDirection = newDirection;
        }

        public void Move()
        {
            if (!IsAlive) return;

            Position head = _body.First();

            Position newHead = CurrentDirection switch
            {
                Direction.UP => new Position(head.X, head.Y - 1),
                Direction.DOWN => new Position(head.X, head.Y + 1),
                Direction.LEFT => new Position(head.X - 1, head.Y),
                Direction.RIGHT => new Position(head.X + 1, head.Y),
                _ => head
            };

            _body.Insert(0, newHead);
        }

        public void Grow()
        {
            _body.Add(new Position(_body.Last().X, _body.Last().Y));
        }

    }
}
