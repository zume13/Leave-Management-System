using LeaveManagement.Domain.Entities;
using LeaveManagement.Domain.Shared;

namespace LeaveManagement.Domain.Primitives
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Entity(Guid id) 
        {
            Guid guid = id;
        }

        protected Entity() { }

        public Guid Id { get; }

        public bool Equals(Entity? other)
        {
            if( other == null )
            {
                return false;
            }

            if( other.GetType() != GetType() )
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return (GetType().ToString() + this.Id).GetHashCode();
        }

    }
}
