using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace jvPo.Report
{
    public partial class ViewPODetails : DevExpress.XtraReports.UI.XtraReport
    {
        public ViewPODetails()
        {
            InitializeComponent();
        }

        public ViewPODetails(string poNumber)
        {
            InitializeComponent();
            // You can use the poNumber to fetch data and set it as the DataSource for the report
            // For example:
            // this.DataSource = FetchPODetails(poNumber);
        }
    }
}
