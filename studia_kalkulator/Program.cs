using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace studia_kalkulator
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public class Kalkulator
    {

        private Label labelOutput;
        private Label labelLED;
        private char usedOparator = ' ';
        private bool isOn = false;
        private byte separatorsPlacedBeforeOperator = 0;
        private byte separatorsPlacedAfterOperator = 0;
       // private bool isOperatorPlaced = false;
        private string outputValue = "0"; //glowny string z wartoscią wyjściową pokazywaną na ekranie

        #region PROPERTIES
        public string OutputValue
        {
            get { return outputValue; }
            set
            {
                outputValue = value;
                labelOutput.Text = outputValue;
            }
        }

        public bool IsOn
        {
            get { return isOn; }
            set
            {

                if (value)
                {
                    //włączanie
                    labelOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(135)))), ((int)(((byte)(124)))));
                    labelLED.ForeColor = System.Drawing.Color.Lime;
                    labelOutput.Text = OutputValue;
                }
                else
                {
                    ResetValue(); //ustawianie wartosci na 0 przy wylaczeniu kalkulatora
                    labelOutput.Text = "";
                    labelOutput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(67)))), ((int)(((byte)(62)))));
                    labelLED.ForeColor = System.Drawing.Color.DarkGreen;
                }

                isOn = value;
            }

        }
        public char currentSeparator { get; } = Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        #endregion

        #region FORMAL_OPERATIONS
        private void ThrowError()
        {
            OutputValue = "ERROR";
            separatorsPlacedAfterOperator = 0;
            separatorsPlacedBeforeOperator = 0;
            usedOparator = ' ';
            labelLED.ForeColor = System.Drawing.Color.Red;

        }

        public void ResetValue()
        {
            // isOperatorPlaced = false;
            usedOparator = ' ';
            OutputValue = "0";
            separatorsPlacedAfterOperator = 0;
            separatorsPlacedBeforeOperator = 0;
            usedOparator = ' ';
            labelLED.ForeColor = System.Drawing.Color.Lime;
        }
        #endregion

        #region MATHS_AND_INTEGRITY
        private bool CanAddSeparator
        {
            get
            {
               // if (OutputValue == "0") return false; //wartosc jest pusta
                if (OutputValue[OutputValue.Length - 1] == usedOparator) return false; //ostatnim znakiem jest znak działania

                if (usedOparator!=' ')
                {
                    if (separatorsPlacedAfterOperator > 0) return false;
                }
                else
                {
                    if (separatorsPlacedBeforeOperator > 0) return false;
                }

                return true;
            }
        }

        public void TryAddDigit(char digit)
        {
            if( usedOparator!=' ' && separatorsPlacedAfterOperator==0 && outputValue[outputValue.Length-1]=='0')
            {
                return; //zapobiega wpisaniu wielu 0 przed przecinkiem w drugiej liczbie
            }
            if (OutputValue == "0") OutputValue = digit.ToString();
            else
                OutputValue += digit;
        }
        public void TryAddSeparator()
        {
            if (CanAddSeparator)
            {

                if (usedOparator!=' ') { separatorsPlacedAfterOperator++; }
                else { separatorsPlacedBeforeOperator++; }

                OutputValue += currentSeparator;
            }
        }

        public void TryAddOperator(char input)
        {
            if (usedOparator == ' ' && OutputValue[OutputValue.Length - 1] != currentSeparator)
            {
                //isOperatorPlaced = true;
                usedOparator = input;
                OutputValue += input;
            }
        }

        public void Count()
        {
            double[] numbers = new double[2];
            try
            {
                string[] sNumbers = OutputValue.Split(usedOparator);

                if (OutputValue.Contains("E"+usedOparator))
                {
                    //mamy do czynienia z notacją wykładniczą dodatnią, a wiec dwa razy '+' lub '-' na wejściu
                    sNumbers[0] += usedOparator + sNumbers[1];
                    sNumbers[1] = sNumbers[2];
                }

                if (sNumbers[0] == "" || sNumbers[1] == "" || sNumbers.Count()<2 ) return;
                numbers[0] = Convert.ToDouble(sNumbers[0]);
                numbers[1] = Convert.ToDouble(sNumbers[1]);
            }
            catch
            {
                return;
            }
            double solution = 0;
            //rozpocznij liczenie
            try
            {
                switch (usedOparator)
                {
                    case '+':
                        solution = numbers[0] + numbers[1];
                        break;
                    case '-':
                        solution = numbers[0] - numbers[1];
                        break;
                    case '*':
                        solution = numbers[0] * numbers[1];
                        break;
                    case '➗':
                        if(numbers[1]==0)
                        {
                            ThrowError();
                            return;
                        }
                        solution = numbers[0] / numbers[1];
                        break;

                }
                // OutputValue = ((decimal)solution).ToString();
                OutputValue = solution.ToString();
            }
            catch
            {

                ThrowError(); //w razie gdyby z jakiegoś dziwnego powodu sobie nie poradził z liczeniem
                return;

            }



            separatorsPlacedAfterOperator = 0;

            if (OutputValue.Contains(currentSeparator)) separatorsPlacedBeforeOperator = 1; //jeśli wynik ma w sobie separator, informacja zostaje
            else separatorsPlacedBeforeOperator = 0;
            usedOparator = ' ';
        }

        #endregion

        public Kalkulator(Label externalOutputLabel, Label externalLabelLED)
        {
            labelOutput = externalOutputLabel;
            labelLED = externalLabelLED;
        }


    }

}
