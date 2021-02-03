using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static GreenAppRider.App;

namespace GreenAppRider.Models
{
    public class TBL_Riders
    {
        public string id { get; set; }
        public string rdr_fullname { get; set; }
        public string rdr_mobile_num { get; set; }
        public string rdr_user_name { get; set; }
        public string rdr_password { get; set; }

        public static async Task Insert(TBL_Riders rider)
        {
            await MobileService.GetTable<TBL_Riders>().InsertAsync(rider);
        }
        public static async Task Update(TBL_Riders rider)
        {
            await MobileService.GetTable<TBL_Riders>().UpdateAsync(rider);
        }

        public static async Task<List<TBL_Riders>> Read()
        {
            var rider = await MobileService.GetTable<TBL_Riders>().ToListAsync();
            return rider;
        }
    }
}
