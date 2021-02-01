using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static GreenAppRider.App;

namespace GreenAppRider.Models
{
    public class V_Confirmed_Orders
    {
        public string id { get; set; }
        public string users_id { get; set; }
        public string full_name { get; set; }
        public string mobile_num { get; set; }
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
        public string propic { get; set; }
        public static async Task Update(V_Confirmed_Orders orders)
        {
            await MobileService.GetTable<V_Confirmed_Orders>().UpdateAsync(orders);
        }

        public static async Task Void(V_Confirmed_Orders voidorder)
        {
            await MobileService.GetTable<V_Confirmed_Orders>().DeleteAsync(voidorder);
        }
        public static async Task<List<V_Confirmed_Orders>> Read()
        {
            var orders = await MobileService.GetTable<V_Confirmed_Orders>().ToListAsync();
            return orders;
        }
    }
}
