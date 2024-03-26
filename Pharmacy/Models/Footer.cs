using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmacy.Models
{
    public class FooterViewModel
    {
        public List<QuickLink> QuickLinks { get; set; }
        public List<ContactInfo> ContactInfoes { get; set; }
    }
}