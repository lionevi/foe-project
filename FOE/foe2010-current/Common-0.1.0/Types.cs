using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Foe.Common
{
    public enum RequestType { Unknown, Content, Registration, Catalog };

    public class FoeMessage
    {
        private List<FoeMessageItem> _items = new List<FoeMessageItem>();

        /// <summary>
        /// Get the FoeMessageItem list.
        /// </summary>
        /// <returns>FoeMessageItem list</returns>
        public List<FoeMessageItem> GetList()
        {
            return _items;
        }

        /// <summary>
        /// Get all FoeMessageItem objects with the matching name.
        /// </summary>
        /// <param name="name">Name to search for</param>
        /// <returns>FoeMessageItem list with matching name</returns>
        public List<FoeMessageItem> GetItemByName(string name)
        {
            List<FoeMessageItem> results = new List<FoeMessageItem>();

            // search for the item by name
            foreach (FoeMessageItem item in _items)
            {
                if (item.Name.CompareTo(name) == 0)
                {
                    results.Add(item);
                }
            }

            return results;
        }

        /// <summary>
        /// Add a new FoeMessageItem
        /// </summary>
        /// <param name="item">FoeMessageItem</param>
        public void Add(FoeMessageItem item)
        {
            _items.Add(item);
        }

        /// <summary>
        /// Get the XML representation of the message.
        /// </summary>
        /// <returns>XML representation of the message</returns>
        public string ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);
            XmlNode root = doc.CreateElement("Message");
            doc.AppendChild(root);

            foreach (FoeMessageItem item in _items)
            {
                XmlElement node = doc.CreateElement(item.Name);
                node.AppendChild(doc.CreateTextNode(item.Value));
                root.AppendChild(node);
            }

            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            xw.Formatting = Formatting.Indented;
            doc.WriteContentTo(xw);

            return sw.ToString();
        }

        /// <summary>
        /// Import XML
        /// </summary>
        /// <param name="xml">XML containing all the FoeMessageItem objects</param>
        public void ImportXml(string xml)
        {
            // delete all existing items
            _items.Clear();

            // import xml
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList nodes = doc.GetElementsByTagName("Message");
            foreach (XmlNode node in nodes)
            {
                XmlNodeList children = node.ChildNodes;
                foreach (XmlNode child in children)
                {
                    FoeMessageItem item = new FoeMessageItem(child.Name, child.InnerText);
                    _items.Add(item);
                }
            }
        }
    }

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
        public string Name { get; set; }
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
