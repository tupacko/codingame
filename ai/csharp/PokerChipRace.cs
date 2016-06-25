using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class ChipRaceGame
{
    private struct Position
    {
        public Position(double x, double y) : this()
        {
            X = x;
            Y = y;
        }
        
        public double X
        {
            get;
            private set;
        }
        
        public double Y
        {
            get;
            private set;
        }
        
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            
            if (!typeof(Position).Equals(other.GetType()))
            {
                return false;
            }
            
            var otherPosition = (Position) other;
            if (!Equals(X, otherPosition.X))
            {
                return false;
            }
            
            if (!Equals(Y, otherPosition.Y))
            {
                return false;
            }
            
            return true;
        }
        
        public override int GetHashCode()
        {
            return X.GetHashCode() * (127 ^ Y.GetHashCode());
        }
    }
    
    private struct Velocity
    {
        public Velocity(double x, double y) : this()
        {
            X = x;
            Y = y;
        }
        
        public double X
        {
            get;
            private set;
        }
        
        public double Y
        {
            get;
            private set;
        }
    }
    
    private abstract class Entity
    {
        protected Entity(int id)
        {
            this.id = id;
        }
        
        public int Id
        {
            get
            {
                return this.id;
            }
        }
        
        public float Radius
        {
            get;
            set;
        }
        
        public Position Position
        {
            get;
            set;
        }
        
        public Velocity Speed
        {
            get;
            set;
        }
        
        public void Update(Entity entity)
        {
            Radius = entity.Radius;
            Position = entity.Position;
            Speed = entity.Speed;
        }
        
        private readonly int id;
    }
    
    private class Droplet : Entity
    {
        public Droplet(int id) : base(id) { }
    }
    
    private class Chip : Entity
    {
        public Chip(int id, int ownerId) : base(id)
        {
            this.ownerId = ownerId;
        }
        
        public bool IsOwned(int playerId)
        {
            return this.ownerId == playerId;
        }
        
        private readonly int ownerId;
    }
    
    private abstract class EntityFactory
    {
        public EntityFactory Next
        {
            get;
            set;
        }
        
        public static EntityFactory CreateChain(int playerId)
        {
            var dropletFactory = new DropletFactory();
            var chipFactory = new ChipFactory();
            
            dropletFactory.Next = chipFactory;
            
            return dropletFactory;
        }
        
        public Entity Create(int id, int ownerId)
        {
            if (!CanHandle(ownerId))
            {
                return HandleNext(id, ownerId);
            }
            
            return CreateInternal(id, ownerId);
        }
        
        protected abstract bool CanHandle(int ownerId);
        
        protected abstract Entity CreateInternal(int id, int ownerId);
        
        private Entity HandleNext(int id, int ownerId)
        {
            if (ReferenceEquals(null, Next))
            {
                throw new Exception("Cannot create entity with factory chain!");
            }
            
            return Next.Create(id, ownerId);
        }
    }
    
    private class ChipFactory : EntityFactory
    {
        protected override bool CanHandle(int ownerId)
        {
            return true;
        }
        
        protected override Entity CreateInternal(int id, int ownerId)
        {
            return new Chip(id, ownerId);
        }
    }
    
    private class DropletFactory : EntityFactory
    {
        protected override bool CanHandle(int ownerId)
        {
            return 0 > ownerId;
        }
        
        protected override Entity CreateInternal(int id, int ownerId)
        {
            return new Droplet(id);
        }
    }
    
    private class Player
    {
        public Player(int id, InputReader reader, OutputWriter writer)
        {
            this.id = id;
            this.entityFactory = EntityFactory.CreateChain(id);
            
            this.reader = reader;
            this.writer = writer;
            
            this.updateMethods = new Dictionary<Type, Action<Entity>>
            {
                { typeof(Droplet), e => UpdateDroplet(e as Droplet) },
                { typeof(Chip), e => UpdateChip(e as Chip) }
            };
            
            this.ownChips = new List<Chip>();
            this.enemies = new List<Chip>();
            this.powers = new List<Droplet>();
        }
        
        public void TakeALook()
        {
            var allEntities = reader.ReadAllEntities(this.entityFactory).ToList();
            UpdateTableTop(allEntities);
        }
        
        public void Attack()
        {
            var strategy = new UniteClosestPairs(this.ownChips);
            
            var targetPositions = strategy.GetTargetPositions().ToList();
            foreach (var targetPosition in targetPositions)
            {
                writer.TargetPosition(targetPosition);
            }
        }
        
        private void UpdateTableTop(List<Entity> entities)
        {
            entities.ForEach(UpdateEntityDetails);
            
            this.powers = entities.OfType<Droplet>().ToList();
            this.enemies = entities.OfType<Chip>().Where(e => !e.IsOwned(this.id)).ToList();
            this.ownChips = entities.OfType<Chip>().Where(e => e.IsOwned(this.id)).ToList();
        }
        
        private void UpdateEntityDetails(Entity entity)
        {
            this.updateMethods[entity.GetType()](entity);
        }
        
        private void UpdateDroplet(Droplet droplet)
        {
            var originalDroplet = this.powers.FirstOrDefault(d => droplet.Id == d.Id);
            if (ReferenceEquals(null, originalDroplet))
            {
                this.powers.Add(droplet);
                return;
            }
            
            originalDroplet.Update(droplet);
        }
        
        private void UpdateChip(Chip chip)
        {
            if (!chip.IsOwned(this.id))
            {
                UpdateEnemy(chip);
                return;
            }
            
            UpdateOwn(chip);
        }
        
        private void UpdateEnemy(Chip chip)
        {
            var originalChip = this.enemies.FirstOrDefault(c => chip.Id == c.Id);
            if (ReferenceEquals(null, originalChip))
            {
                this.enemies.Add(chip);
                return;
            }
            
            originalChip.Update(chip);
        }
        
        private void UpdateOwn(Chip chip)
        {
            var originalChip = this.ownChips.FirstOrDefault(c => chip.Id == c.Id);
            if (ReferenceEquals(null, originalChip))
            {
                this.ownChips.Add(chip);
                return;
            }
            
            originalChip.Update(chip);
        }
        
        private readonly int id;
        
        private readonly InputReader reader;
        
        private readonly OutputWriter writer;
        
        private readonly EntityFactory entityFactory;
        
        private readonly IDictionary<Type, Action<Entity>> updateMethods;
        
        private IList<Chip> ownChips;
        
        private IList<Chip> enemies;
        
        private IList<Droplet> powers;
    }
    
    private class UniteClosestPairs
    {
        public UniteClosestPairs(IEnumerable<Chip> chips)
        {
            this.chips = chips.ToList();
        }
        
        public IEnumerable<Position> GetTargetPositions()
        {
            var result = new Position[this.chips.Count];
            foreach (var pair in GetPairs())
            {
                Console.Error.WriteLine("Calculate and return mid point ...");
                var first = pair.Item1;
                var second = pair.Item2;
                
                if (ReferenceEquals(first, second))
                {
                    SetResult(result, first, first.Position);
                    continue;
                }
                
                var midPoint = CalculateMidPoint(first.Position, second.Position);
                SetResult(result, first, midPoint);
                SetResult(result, second, midPoint);
            }
            
            return result;
        }
        
        private void SetResult(Position[] result, Chip chip, Position position)
        {
            var index = this.chips.IndexOf(chip);
            result[index] = position;
        }
        
        private IEnumerable<Tuple<Chip, Chip>> GetPairs()
        {
            var remainingChips = this.chips.ToList();
            while (0 < remainingChips.Count)
            {
                Console.Error.WriteLine("Calculate pair ...");
                var first = Dequeue(remainingChips);
                var second = GetClosest(first, remainingChips);
                if (Equals(first, second))
                {
                    yield return CreateTuple(first, first);
                }
                
                remainingChips.Remove(second);
                yield return CreateTuple(first, second);
            }
        }
        
        private Chip Dequeue(IList<Chip> list)
        {
            var element = list[0];
            list.RemoveAt(0);
            
            return element;
        }
        
        private Chip GetClosest(Chip first, IEnumerable<Chip> remainingChips)
        {
            if (!remainingChips.Any())
            {
                return first;
            }
            
            if (remainingChips.Count() == 1)
            {
                return remainingChips.Single();
            }
            
            return remainingChips.OrderBy(c => CalculateDistance(first.Position, c.Position)).First();
        }
        
        private double CalculateDistance(Position first, Position second)
        {
            Console.Error.WriteLine("Calculate distance ...");
            
            double x = second.X - first.X;
            double y = second.Y - first.Y;
            
            return Math.Sqrt(x * x + y * y);
        }
        
        private Tuple<Chip, Chip> CreateTuple(Chip first, Chip second)
        {
            return new Tuple<Chip, Chip>(first, second);
        }
        
        private Position CalculateMidPoint(Position first, Position second)
        {
            Console.Error.WriteLine("Calculate mid point ...");
            
            var newX = (first.X + second.X) / 2;
            var newY = (first.Y + second.Y) / 2;
            
            return new Position(newX, newY);
        }
        
        private readonly IList<Chip> chips;
    }
    
    private class InputReader
    {
        public int ReadPlayerId()
        {
            return int.Parse(Console.ReadLine());
        }
        
        public IEnumerable<Entity> ReadAllEntities(EntityFactory factory)
        {
            PrepareReadAllEntities();
            int entityCount = GetEntityCount();
            for (int i = 0; i < entityCount; i++)
            {
                yield return ReadEntity(factory);
            }
        }
        
        private void PrepareReadAllEntities()
        {
            // do not care, have to read to comply with the interface
            int playerChipCount = int.Parse(Console.ReadLine());
        }
        
        private int GetEntityCount()
        {
            return int.Parse(Console.ReadLine());
        }
        
        private Entity ReadEntity(EntityFactory factory)
        {
            string[] inputs = Console.ReadLine().Split(' ');
            
            int id = int.Parse(inputs[0]);
            int ownerId = int.Parse(inputs[1]);
            var entity = factory.Create(id, ownerId);
            
            entity.Radius = float.Parse(inputs[2]);
            entity.Position = new Position(double.Parse(inputs[3]), double.Parse(inputs[4]));
            entity.Speed = new Velocity(double.Parse(inputs[5]), double.Parse(inputs[6]));
            
            return entity;
        }
    }
    
    private class OutputWriter
    {
        public void TargetPosition(Position position)
        {
            Console.WriteLine("{0} {1}", position.X, position.Y);
        }
        
        public void Wait()
        {
            Console.WriteLine("WAIT");
        }
    }
    
    static void Main(string[] args)
    {
        var reader = new InputReader();
        var writer = new OutputWriter();
        
        var playerId = reader.ReadPlayerId();
        var player = new Player(playerId, reader, writer);
        
        while (true)
        {
            player.TakeALook();
            player.Attack();
        }
    }
}