using System.Collections.Generic;
using System.Linq;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace Site.Library
{
	public class EntityOptionSet : PortalClass
	{
		private readonly IDictionary<int, string> _optionSetLabelCache = new Dictionary<int, string>();

		public string GetOptionSetLabelByValue(string entityLogicalName, string attributeLogicalName, int value)
		{
			if (string.IsNullOrEmpty(entityLogicalName) || string.IsNullOrEmpty(attributeLogicalName))
			{
				return string.Empty;
			}

			string cachedLabel;

			if (_optionSetLabelCache.TryGetValue(value, out cachedLabel))
			{
				return cachedLabel;
			}

			var retrieveAttributeRequest = new RetrieveAttributeRequest
			{
				EntityLogicalName = entityLogicalName,
				LogicalName = attributeLogicalName
			};

			var retrieveAttributeResponse = (RetrieveAttributeResponse)ServiceContext.Execute(retrieveAttributeRequest);

			var retrievedPicklistAttributeMetadata = (EnumAttributeMetadata)retrieveAttributeResponse.AttributeMetadata;

			var option = retrievedPicklistAttributeMetadata.OptionSet.Options.FirstOrDefault(o => o.Value == value);

			if (option == null)
			{
				return string.Empty;
			}

			var label = option.Label.UserLocalizedLabel.Label;

			if (option.Value.HasValue)
			{
				_optionSetLabelCache[option.Value.Value] = label;
			}

			return label;
		}
	}
}