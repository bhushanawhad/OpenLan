using System;
using System.Xml;

namespace Site.Library
{
	public class Asirra
	{
		public static bool ValidateAsirraChallenge(string asirraTicket)
		{
			var validationUrl = "http://challenge.asirra.com/cgi/Asirra?action=ValidateTicket&ticket=" + asirraTicket;
			var validationTextReader = new XmlTextReader(validationUrl) { DtdProcessing = DtdProcessing.Prohibit };
			var validationDocument = new XmlDocument();

			try
			{
				validationDocument.Load(validationTextReader);

				var validationValue = validationDocument.GetElementsByTagName("Result")[0].ChildNodes[0].Value;

				return string.Equals(validationValue, "Pass", StringComparison.InvariantCulture);
			}
			catch
			{
				return false;
			}
		}
	}
}
