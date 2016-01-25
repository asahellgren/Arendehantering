﻿using System;
using System.Collections;

namespace DAL.Entities
{
    public sealed class WorkItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateDone { get; set; }
        public bool Reviewed { get; set; }
        public int PriorityIndex { get; set; }
        public int? UserId { get; set; }
        public int CreatedByUserId { get; set; }
        public int? TeamId { get; set; }
        public ICollection Issues { get; set; }
    }
}
