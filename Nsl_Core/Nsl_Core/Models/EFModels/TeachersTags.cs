﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Nsl_Core.Models.EFModels
{
    public partial class TeachersTags
    {
        public int TeacherId { get; set; }
        public int TagId { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual Tags Tag { get; set; }
        public virtual TeacherId Teacher { get; set; }
    }
}