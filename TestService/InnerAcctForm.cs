using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using xQuant.AidSystem.BizDataModel;

namespace TestService
{
    public partial class InnerAcctForm : Form
    {
        public InnerAcctForm()
        {
            InitializeComponent();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            try
            {
                string result;
                if (BizDataHelper.GenerateInnerAcctNO(txtOrgNO.Text.Trim(), txtCurrency.Text.Trim(), txtCheckCode.Text.Trim(), txtInnerAcctSN.Text.Trim(), out result))
                {
                    txtResult.Text = result;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }
    }
}
