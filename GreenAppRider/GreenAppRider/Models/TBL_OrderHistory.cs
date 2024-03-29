﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static GreenAppRider.App;

namespace GreenAppRider.Models
{
    public class TBL_OrderHistory
    {
        public string id { get; set; }
        public string users_id { get; set; }
        public string order_id { get; set; }
        public double tot_payable { get; set; }
        public double cash_rendered { get; set; }
        public double cash_change { get; set; }
        public string itms_qty { get; set; }
        public string cart_datetime { get; set; }
        public string order_status { get; set; }
        public string order_choice { get; set; }

        public static async Task Insert(TBL_OrderHistory orderHistory)
        {
            await MobileService.GetTable<TBL_OrderHistory>().InsertAsync(orderHistory);
        }
    }
}
