using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace studia_kalkulator
{
    public partial class MainForm : Form
    {
        Kalkulator kalkulator;
        public MainForm()
        {
          
            InitializeComponent();
            kalkulator = new Kalkulator(this.labelOutput,this.labelLED);
            // kalkulator.labelOutput = this.labelOutput;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            Button input = sender as Button;
            if (input == null) return;

            if(input.Name=="btnSeparator")
            {
                kalkulator.TryAddSeparator();
                return;
            }
            if (input.Text != "ON/OFF" && !kalkulator.IsOn) return; //kalkulator jest wylaczony, nie mozna nic wpisywac
            switch(input.Text)
            {
                case "ON/OFF":
                    kalkulator.IsOn = !kalkulator.IsOn;
                  break;
                   
                case "C":
                    kalkulator.ResetValue();
                    break;

                case "=":
                    kalkulator.Count();
                    break;
                case "-":
                    kalkulator.TryAddOperator('-');
                    break;
                case "*":
                    kalkulator.TryAddOperator('*');
                    break;
                case "+":
                    kalkulator.TryAddOperator('+');
                    break;
                case "➗":
                    kalkulator.TryAddOperator('➗');
                    break;
                default:
                    //kliknięto w liczbę
                    kalkulator.TryAddDigit(Convert.ToChar( input.Text));
                    break;
            }

        }
    }

 
}
