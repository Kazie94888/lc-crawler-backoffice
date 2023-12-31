using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;
using MongoDB.Bson.Serialization.Attributes;
using Volo.Abp;

namespace LC.Crawler.BackOffice.Medias
{
    public class Media : Entity<Guid>
    {
        [NotNull]
        public virtual string Name { get; set; }

        [NotNull]
        public virtual string ContentType { get; set; }

        [NotNull]
        public virtual string Url { get; set; }

        [CanBeNull]
        public virtual string Description { get; set; }

        public virtual bool IsDowloaded { get; set; }
        
        [BsonElement("ExternalUrl")]
        public virtual string ExternalUrl { get; set; }
        [BsonElement("ExternalId")]
        public virtual string ExternalId { get; set; }

        public Media()
        {

        }

        public Media(Guid id, string name, string contentType, string url, string description, bool isDowloaded)
        {

            Id = id;
            Check.NotNull(name, nameof(name));
            Check.NotNull(contentType, nameof(contentType));
            Check.NotNull(url, nameof(url));
            Name = name;
            ContentType = contentType;
            Url = url;
            Description = description;
            IsDowloaded = isDowloaded;
        }

    }
}