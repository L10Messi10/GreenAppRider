using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static GreenAppRider.App;

namespace GreenAppRider.Models
{
    public class TBL_Delivery
    {
        #region Fieldnames

        public string id { get; set; }
        public string order_id { get; set; }
        public string users_id { get; set; }
        public string del_datetime { get; set; }
        public string del_stat { get; set; }

        #endregion

        public static async Task Insert(TBL_Delivery order)
        {
            await MobileService.GetTable<TBL_Delivery>().InsertAsync(order);
        }
        public static async Task Update(TBL_Delivery orders)
        {
            await MobileService.GetTable<TBL_Delivery>().UpdateAsync(orders);
        }
        public static async Task Delete(TBL_Delivery deleteDelivery)
        {
            await MobileService.GetTable<TBL_Delivery>().DeleteAsync(deleteDelivery);
        }
    }
}
