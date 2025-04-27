using Newtonsoft.Json;
using System.Collections.Generic;

namespace DataModel.Common
{
	public class TeamsWebhookRequestModel
	{
		public class AttachmentModel {
			public class ContentModel {
				[JsonProperty("$schema")]
				public string schema { get; set; }
				public string type { get; set; }
				public string version { get; set; }
				public List<BodyModel> body { get; set; }
				public class BodyModel { 
				public string type { get; set; }
				}
			}
			public string contentType { get; set; }
			public ContentModel content { get; set; }
		}
		public List<AttachmentModel> attachments { get; set; }
	}
}
