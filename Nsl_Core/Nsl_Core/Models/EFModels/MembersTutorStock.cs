﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Nsl_Core.Models.EFModels
{
    public partial class MembersTutorStock
    {
        public int MemberId { get; set; }
        public int TeacherId { get; set; }
        public int TutorStock { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ModifiedTime { get; set; }

        public virtual Members Member { get; set; }
        public virtual TeacherId Teacher { get; set; }
    }
}