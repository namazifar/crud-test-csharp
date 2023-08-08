using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC2.CurdTest.Common
{
        public enum ResponseType
        {
            success = 0,
            badRequest = 1,
            logicalError = 2,
            unauthorized = 3
        }
        public class MC2Response
        {
            public ResponseType ResultCode { get; set; }
            public string ResultDesc { get; set; }

            public MC2Response()
            {
                ResultCode = (int)ResponseType.success;
                ResultDesc = "success";
            }
        }
}
