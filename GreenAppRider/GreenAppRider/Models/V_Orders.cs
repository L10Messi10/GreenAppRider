﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static GreenAppRider.App;

namespace GreenAppRider.Models
{
    public class V_Orders
    {
        public string id { get; set; }
        public string order_id { get; set; }
        public string users_id { get; set; }
        public string prod_id { get; set; }
        public string full_name { get; set; }
        public string prod_name { get; set; }
        public string prod_desc { get; set; }
        public string qty { get; set; }
        public string prod_price { get; set; }
        public string unit_desc { get; set; }
        public string category_name { get; set; }
        public string img_uri { get; set; }
        public double sub_total { get; set; }
        public string discount { get; set; }
        public string tot_payable { get; set; }
        public string cash_rendered { get; set; }
        public string cash_change { get; set; }
        public string order_date { get; set; }
        public string cart_datetime { get; set; }
        public string itms_qty { get; set; }
        public string order_choice { get; set; }
        public static async Task<List<V_Orders>> Read()
        {
            var orderdetails = await MobileService.GetTable<V_Orders>().ToListAsync();
            return orderdetails;
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //[NotifyPropertyChangedInvocator]
        //protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
