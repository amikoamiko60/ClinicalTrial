﻿using ClinicalTrial.DataContracts;

namespace ClinicalTrial.DataAccess.Entities
{
    public class ClinicalTrialEntity
    {
        public int Id { get; set; }

        public string TrialId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? Participants { get; set; }

        public ClinicalTrialStatus Status { get; set; }

        public int Duration { get; set; }
    }
}