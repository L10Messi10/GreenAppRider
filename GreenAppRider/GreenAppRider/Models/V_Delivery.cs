using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GreenAppRider.Models
{
    public class V_Delivery
    {
        public string id { get; set; }
        public string order_id { get; set; }
        public string users_id { get; set; }
        public string rider_id { get; set; }
        public string full_name { get; set; }
        public string mobile_num { get; set; }
        public string emailadd { get; set; }
        public string del_datetime { get; set; }
        public string tot_payable { get; set; }
        public string cash_rendered { get; set; }
        public string cash_change { get; set; }
        public string order_date { get; set; }
        public string itms_qty { get; set; }
        public string cart_datetime { get; set; }
        public string order_status { get; set; }
        public string stat { get; set; }
        public string order_choice { get; set; }
        public string del_address { get; set; }
        public string del_lat { get; set; }
        public string del_long { get; set; }
        public string notes { get; set; }
        public string building_name { get; set; }
        public string propic { get; set; }
        public string del_stat { get; set; }

        public static async Task<List<V_Delivery>> Read()
        {
            var orderdelivery = await App.MobileService.GetTable<V_Delivery>().ToListAsync();
            return orderdelivery;
        }
    }
}
