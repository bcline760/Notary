using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Notary.Contract
{
    [DataContract]
    [KnownType(typeof(Certificate))]
    [KnownType(typeof(ApiToken))]
    public abstract class Entity : IComparable<Entity>, IEquatable<Entity>, ISluggable
    {
        /// <summary>
        /// Get or set identifying slug of the entit
        /// </summary>
        [DataMember]
        public string Slug
        {
            get => default;
            set
            {
            }
        }

        /// <summary>
        /// Get or set in which the entity was created
        /// </summary>
        [DataMember]
        public DateTime Created
        {
            get => default;
            set
            {
            }
        }

        [DataMember]
        public string CreatedBySlug { get; set; }

        /// <summary>
        /// Get or set the last time the entity was updated
        /// </summary>
        [DataMember]
        public DateTime? Updated
        {
            get => default;
            set
            {
            }
        }

        [DataMember]
        public string UpdatedBySlug { get; set; }

        /// <summary>
        /// Get or set whether this entity is active
        /// </summary>
        [DataMember]
        public bool Active
        {
            get => default;
            set
            {
            }
        }

        public int CompareTo(Entity other)
        {
            var result = string.Compare(Slug, other.Slug, true);
            return result;
        }

        public bool Equals(Entity other)
        {
            if (other == null)
                return false;

            return string.Compare(Slug, other.Slug, true) == 0;
        }

        public abstract string[] SlugProperties();
    }
}