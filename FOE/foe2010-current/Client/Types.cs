using System;
using System.Collections.Generic;
using System.Text;

namespace Foe.Client
{
    public class FoeClientCatalogItem
    {
        public string Code { get; set; }
        public bool IsSubscribed { get; set; }
        public DateTime DtLastUpdated { get; set; }

        public FoeClientCatalogItem(string code, bool isSubscribed)
        {
            Code = code;  
            IsSubscribed = isSubscribed;
        }

        public FoeClientCatalogItem()
        {
            Code = null;
            IsSubscribed = false;
        }
    }

    public class FoeClientRequestItem
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public DateTime DtRequested { get; set; }
    }

    public class FoeClientRegistryEntry
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
