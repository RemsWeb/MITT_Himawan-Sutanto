using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace MITT_HIMAWAN_SUTANTO.REST
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IIInsertUser" in both code and config file together.
    [ServiceContract]
    public interface IIInsertUser
    {
        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/InsertUser", Method = "POST")]
        List<InsertMessage> InsertUser(string Usernames, string Names, string Passwords, string Addresss, string DOBs, string Emails);
    }

    [DataContract]
    public class GetDetail
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string Address { get; set; }
        [DataMember]
        public string DOB { get; set; }
        [DataMember]
        public string Email { get; set; }
    }

    [DataContract]
    public class InsertMessage
    {
        [DataMember]
        public string MSG { get; set; }

    }
}
