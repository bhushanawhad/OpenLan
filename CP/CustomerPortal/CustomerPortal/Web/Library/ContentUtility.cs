using System;
using System.Linq;
using System.Text;
using Xrm;
using Encoder = Microsoft.Security.Application.Encoder;

namespace Site.Library
{
	public static class ContentUtility
	{
		public static string FormatEventDate(XrmServiceContext context, Campaign campaign, DateTime? value)
		{
			if (value == null)
			{
				return string.Empty;
			}

			var formatter = DateTimeFormatter.ForEvent(context, campaign);

			var output = new StringBuilder(HtmlEncode(formatter.Format(value.Value)));

			if (!string.IsNullOrEmpty(formatter.TimeZoneLabel))
			{
				output.AppendFormat(" ({0})", HtmlEncode(formatter.TimeZoneLabel));
			}

			return output.ToString();
		}

		public static string FormatEventDateRange(XrmServiceContext context, Campaign campaign)
		{
			var formatter = DateTimeFormatter.ForEvent(context, campaign);

			if (campaign.MSA_StartDateTime == null)
			{
				return string.Empty;
			}

			var output = new StringBuilder(HtmlEncode(formatter.Format(campaign.MSA_StartDateTime.Value)));

			if (campaign.MSA_EndDateTime.HasValue)
			{
				output.AppendFormat(" &ndash; {0}", HtmlEncode(formatter.Format(campaign.MSA_EndDateTime.Value)));
			}

			if (!string.IsNullOrEmpty(formatter.TimeZoneLabel))
			{
				output.AppendFormat(" ({0})", HtmlEncode(formatter.TimeZoneLabel));
			}

			return output.ToString();
		}

		public static string FormatSequence(string separator, params object[] values)
		{
			return HtmlEncode(string.Join(separator,
				values
					.Where(v => v != null)
					.Select(v => v.ToString())
					.Where(v => !string.IsNullOrWhiteSpace(v)
				).ToArray()));
		}

		public static string HtmlEncode(object value)
		{
			return value == null ? string.Empty : Encoder.HtmlEncode(value.ToString());
		}

		private class DateTimeFormatter
		{
			private const string _dateTimeFormat = "dddd, MMMM d, yyyy h:mm tt";

			private readonly int _minutesFromGmt;

			private DateTimeFormatter(string timeZoneLabel, int minutesFromGmt)
			{
				TimeZoneLabel = timeZoneLabel;
				_minutesFromGmt = minutesFromGmt;
			}

			public string TimeZoneLabel { get; private set; }

			public string Format(DateTime value)
			{
				return value.AddMinutes(_minutesFromGmt).ToString(_dateTimeFormat);
			}

			public static DateTimeFormatter ForEvent(XrmServiceContext context, Campaign campaign)
			{
				if (context == null || campaign == null || campaign.MSA_EventTimezone == null)
				{
					return new DateTimeFormatter(null, 0);
				}

				var timeZone = context.TimeZoneDefinitionSet.FirstOrDefault(t => t.TimeZoneCode == campaign.MSA_EventTimezone);

				if (timeZone == null)
				{
					return new DateTimeFormatter(null, 0);
				}

				var start = campaign.MSA_StartDateTime.HasValue ? campaign.MSA_StartDateTime.Value : DateTime.UtcNow;

				var rule = timeZone.lk_timezonerule_timezonedefinitionid
					.OrderByDescending(r => r.EffectiveDateTime)
					.FirstOrDefault(r => r.EffectiveDateTime.HasValue && r.EffectiveDateTime <= start.AddMinutes(r.Bias.GetValueOrDefault(0) * -1));

				return rule == null ? new DateTimeFormatter(null, 0) : new DateTimeFormatter(timeZone.StandardName, rule.Bias.GetValueOrDefault(0) * -1);
			}
		}
	}
}