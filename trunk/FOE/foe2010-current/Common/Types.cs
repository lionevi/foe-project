using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Foe.Common
{
    public enum RequestType { Unknown, Content, Registration, Catalog, Feed };


    /// <summary>
    /// Every FOE message contains a list of FOE message items.
    /// Examples of message items include:
    ///   <RequestId>12345</RequestId>
    ///   <UserId>ABCDEFG</UserId>
    /// Where "Name" are the XML tag names such as "RequestId" and "UserId" above.
    /// "Value" are the content of the XML tag such as "12345" and "ABCDEFG" above.
    /// </summary>
    public class FoeMessageItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public FoeMessageItem()
        {
            Name = null;
            Value = null;
        }

        public FoeMessageItem(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }


    /// <summary>
    /// FOE User
    /// </summary>
    public class FoeUser
    {
        public int? Id { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public DateTime? DtCreated { get; set; }
        public string VerificationCode { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? DtVerified { get; set; }
        public string ProcessorEmail { get; set; }
    }

    public class CatalogItem
    {
        public string Code { get; set; }
        public string ContentType { get; set; }        
        public string Description { get; set; }
        public string Location { get; set; }
    }

    public class PopServer
    {
        public string ServerName { get; set; }
        public int Port { get; set; }
        public bool SslEnabled { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SmtpServer
    {
        public string ServerName { get; set; }
        public int Port { get; set; }
        public bool AuthRequired { get; set; }
        public bool SslEnabled { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
