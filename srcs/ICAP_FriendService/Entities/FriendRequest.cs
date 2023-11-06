﻿using ICAP_AccountService.Entities;

namespace ICAP_FriendService.Entities
{
    public class FriendRequest : IEntity
    {
        public Guid Id { get; set; }
        public string UserFrom { get; set; }
        public string UserTo { get; set; }
        public bool Declined { get; set; }
        public bool Accepted { get; set; }
        public bool Pending { get; set; }
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}