using System;
using System.Runtime.Serialization;
namespace Test
{
    [DataContract]
    public class APIResponse
    {
		[DataMember]
		public bool success { get; set; }
		[DataMember]
        public string message { get; set; }
		[DataMember]
		public string token { get; set; }
    }
}
