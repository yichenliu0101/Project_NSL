﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Nsl_Api.Models.EFModels
{
    public partial class MembersConsumptionRecordDetails
    {
        public int Id { get; set; }
        public int MembersConsumptionRecordId { get; set; }
        public int TeacherId { get; set; }
        public int Count { get; set; }
        public decimal CurrentPrice { get; set; }

        public virtual MembersConsumptionRecords MembersConsumptionRecord { get; set; }
        public virtual TeacherId Teacher { get; set; }
    }
}