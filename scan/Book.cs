using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace scan
{
    public class Book

    {
        public string title1 { get; set; }
        public string authors1 { get; set; }
        public int pageCount1 { get; set; }
        public string kind { get; set; }
        public item[] items{ get; set; }
        public int totalitems { get; set; }
        public bool source = false;
        public string ReadStatus { get; set; }
        public string comment { get; set; }

    }
    public class item
    {
        public VolumeInfo volumeInfo { get; set; }
        
    }
    public class VolumeInfo
    {
        public string title { get; set; }
        public IList<string> authors { get; set; }
        public int pageCount { get; set; }
        public IList<IndustryIdentifier> industryIdentifiers { get; set; }
    }

    public class IndustryIdentifier
    {
        public string type { get; set; }
        public string identifier { get; set; }
    }
}