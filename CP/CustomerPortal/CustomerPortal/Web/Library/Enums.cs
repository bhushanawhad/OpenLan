namespace Site.Library
{
	public class Enums
	{
		public enum ActivityPointerState
		{
			Open = 0,
			Completed = 1,
			Canceled = 2,
			Scheduled = 3,
		}

		public class Adx_caseaccess
		{
			/// <summary>
			///  (Enumeration for adx_scope)
			/// </summary>
			public enum ScopeOption { Self = 1, Account = 2 }
		}

		public class Campaign
		{
			public enum StatusOption
			{
				Proposed = 0,
				ReadyToLaunch = 1,
				Launched = 2,
				Completed = 3,
				Canceled = 4,
				Susspended = 5,
				Inactive = 6,
				RegistrationOpen = 200000,
				WaitlistOnly = 200001,
				SoldOut = 200002
			}
		}

		public enum IncidentState
		{
			Active = 0,
			Resolved = 1,
			Canceled = 2
		}
        public enum ContractState
        {
            Invoiced = 1,
            Active = 2,
            Canceled = 4,
            Expired = 5
        }

        public enum ContractDetailState
        {
            Existing = 0,
            Renewed = 1,
            Canceled = 2,
            Expired = 3
        }

		public enum KbArticleState
		{
			Draft = 1,
			Unapproved = 2,
			Published = 3,
		}

		public enum OpportunityState
		{
			Open = 0,
			Won = 1,
			Lost = 2,
		}

		public enum ServiceAppointmentState
		{
			Open = 0,
			Closed = 1,
			Canceled = 2,
			Scheduled = 3,
		}
	}
}
