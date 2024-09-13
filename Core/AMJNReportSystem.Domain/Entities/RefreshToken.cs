using AMJNReportSystem.Domain.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMJNReportSystem.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string ChandaNo { get; set; }
        public string Token { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
