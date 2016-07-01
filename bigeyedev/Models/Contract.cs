using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bigeyedev.Models
{
    public class Contract
    {
        public bigeyedev_member memberData { get; set; }
        public bigeyedev_member_address address { get; set; }

        public Contract()
        {
            memberData = new bigeyedev_member();
            address = new bigeyedev_member_address();
        }
    }
    
}