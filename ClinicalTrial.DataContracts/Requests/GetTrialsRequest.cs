namespace ClinicalTrial.DataContracts.Requests
{
    public class GetTrialsRequest
    {
        public int? Id { get; set; }

        public string TrialId { get; set; }

        public string Title { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string Status { get; set; }
    }
}
