// Calculator WFA (Applcation), Developed by iDeveloper_.
using System;
using System.Windows.Forms;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CalculatorWFA
{
    public partial class Caculator : Form
    {
        public Caculator()
        {
            InitializeComponent();
            ResetDetails();
        }
        // Global Variables & Settings
        private readonly Random objRandom = new Random();
        const string
            mUsername = "username",
            mPassword = "password",
            mMailFrom = "username@mail.com";
        bool
            g_isResulted = false,
            g_DigitsGrouping = false,
            g_DigitsByPoint = false;
        int g_AMO = -1, /* Advanced mathematical operation */
            g_Option = -1;
        /*
         * Calculator value buttons
         * Fucos: 0-9, .
         */
        private void button0_Click(object sender, EventArgs e) { setValueCalculator('0'); }
        private void button1_Click(object sender, EventArgs e) { setValueCalculator('1'); }
        private void button2_Click(object sender, EventArgs e) { setValueCalculator('2'); }
        private void button3_Click(object sender, EventArgs e) { setValueCalculator('3'); }
        private void button4_Click(object sender, EventArgs e) { setValueCalculator('4'); }
        private void button5_Click(object sender, EventArgs e) { setValueCalculator('5'); }
        private void button6_Click(object sender, EventArgs e) { setValueCalculator('6'); }
        private void button7_Click(object sender, EventArgs e) { setValueCalculator('7'); }
        private void button8_Click(object sender, EventArgs e) { setValueCalculator('8'); }
        private void button9_Click(object sender, EventArgs e) { setValueCalculator('9'); }
        private void Point_Click(object sender, EventArgs e) { setValueCalculator('.'); }

        /*
         * Calculator symbol buttons
         * Fucos: +, -, *, /, =
         */
        // Plus Click (+)
        private void Plus_Click(object sender, EventArgs e)
        { setSymbolCalculator('+', 1); }
        // Minus option (-)
        private void Minus_Click(object sender, EventArgs e)
        { setSymbolCalculator('-', 2); }
        // Multipcation option (*)
        private void Multiplication_Click(object sender, EventArgs e)
        { setSymbolCalculator('*', 3); }
        // Divide option (/)
        private void Divide_Click(object sender, EventArgs e)
        { setSymbolCalculator('/', 4); }

        // Result option (=)
        private void Calculate_Click(object sender, EventArgs e)
        {
            string[] arr = splitResulter();
            if (String.IsNullOrEmpty(arr[0]))
            {
                MessageBox.Show("Value cannot be null", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            double 
                n1 = 0, n2 = 0;
            bool tp = double.TryParse(arr[0], out n1);
            if (!tp) double.TryParse(arr[1], out n1);
            double.TryParse(arr[arr.Length - 1], out n2);
            if (g_AMO != -1) Resulter.Text += ") = ";
            int d1 = (int) n1;
            switch (g_AMO)
            {
                case 7: (Resulter.Text) += DecimalToAnyBase((int)n2, d1); return;
                case 11: (Resulter.Text) += (d1 % 2 == 0 ? ("even") : ("odd")); return;
                case 12: (Resulter.Text) += ((bool)isPrime(d1)); return;
                case 13: (Resulter.Text) += ((bool)isPalindrome(d1.ToString())); return;
            }
            double c = Calculator(n1, n2, g_Option);
            if (c == -1.13782312) return;
            double result = (g_AMO != -1 ? AMOperation(g_AMO, n1, n2) : c);
            string fixedPoint = String.Format((!g_DigitsByPoint ? ("{0}") : ("{0:F3}")), result);
            if (g_AMO == -1) Resulter.Text += " = ";
            string res = (result == 0 ? ("0") : (g_AMO != -1 || !g_DigitsGrouping) ? fixedPoint : result.ToString("#,#", CultureInfo.InvariantCulture));
            Resulter.Text += res;
            CaluculateAll();
            g_isResulted = true;
        }
        // Clear button
        private void Clear_Click(object sender, EventArgs e) 
        { ResetDetails(); }

        // Remove last input
        private void Delete_Click(object sender, EventArgs e)
        {
            string s = Resulter.Text;
            s =
                (s.Length > 1) ? s.Substring(0, s.Length - 1) : null;
            Resulter.Text = s;
        }

        // Comma button (move to next element)
        private void Comma_Click(object sender, EventArgs e)
        { Resulter.Text += ", "; }

        /*
         * Convert Decimal To Any Base
         * Fucos: 2 (Binary), 8 (Octal), 16 (Hexadecimal), etc..
         */
        private void button27_Click(object sender, EventArgs e)
        { setAMOCalculator(7, "Base"); }

        public static string DecimalToAnyBase(int decimalNumber, int nBase)
        {
            if (nBase == 10) return Convert.ToString(decimalNumber);
            string result = string.Empty;
            if (nBase == 2 || nBase == 8 || nBase == 16)
                return (result = Convert.ToString(decimalNumber, nBase));
            int remainder;
            while (decimalNumber > 0)
            {
                remainder = decimalNumber % nBase;
                decimalNumber /= nBase;
                result = remainder.ToString() + result;
            }
            return result;
        }

        /*
         * Advanced mathematical operation
         * Fucos: Max, Min, Round, Abs, Pow, Sqrt
         */
        private void Abs_Click(object sender, EventArgs e)
        { setAMOCalculator(3, "Abs"); }
        private void Max_Click(object sender, EventArgs e)
        { setAMOCalculator(0, "Max"); }
        private void Min_Click(object sender, EventArgs e)
        { setAMOCalculator(1, "Min"); }
        private void Sqrt_Click(object sender, EventArgs e)
        { setAMOCalculator(5, "Sqrt"); }
        private void Round_Click(object sender, EventArgs e)
        { setAMOCalculator(2, "Round"); }
        private void Pow_Click(object sender, EventArgs e)
        { setAMOCalculator(4, "Pow"); }
        private void Modulo_Click(object sender, EventArgs e) 
        { setAMOCalculator(6, "Modulo"); }
        private void Tangents_Click(object sender, EventArgs e)
        { setAMOCalculator(7, "Tan"); }
        private void Sines_Click(object sender, EventArgs e)
        { setAMOCalculator(8, "Sin"); }
        private void Cosines_Click(object sender, EventArgs e)
        { setAMOCalculator(9, "Cos"); }
        private void Random_Click(object sender, EventArgs e)
        { setAMOCalculator(10, "Random"); }
        private void EONumber_Click(object sender, EventArgs e)
        { setAMOCalculator(11, "EoN"); }
        private void PrimeNumber_Click(object sender, EventArgs e)
        { setAMOCalculator(12, "PN"); }
        private void Palindrome_Click(object sender, EventArgs e)
        { setAMOCalculator(13, "Palindrome"); }

        /*
         * tool-Strip
         * about ToolStrip Menu Item
         */
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text =
                "iDEV Calculator - Support Windows OS Only!\nVersion: 1.0 (Beta version)\nCopyright (c) 2013 iDEVDevelopers. All rights reserved\nSemi-Advanced calculator application, based on C# (WFA)\n\n\t\tOptions:\n\nArithmetic operators (+, -, *, /)\nMath class:\n\tAbs(decimal value)\n\tMax(byte val1, byte val2)\n\tMin(byte val1, byte val2)\n\tSqrt(double n)\n\tPow(double x, double y)\n\nTrigonometry functions:\n\tSin(double angle{radinas})\n\tCos(double angle{radinas})\n\tTan(double angle{radinas})\n\nModulo (%(decimal value, decimal divison))\nConvert Base-10 (Decimal) To Any base\n\t{Format: <base>, <decimal value>}\n\nEnjoy!";
            MessageBox.Show(text, "About calculator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void digitsGroupingToolStripMenuItem_Click(object sender, EventArgs e)
        { digitsGroupingToolStripMenuItem.Checked = ((g_DigitsGrouping = !g_DigitsGrouping)); }

        private void digitsByPointToolStripMenuItem_Click(object sender, EventArgs e)
        { digitsByPointToolStripMenuItem.Checked = ((g_DigitsByPoint = !g_DigitsByPoint)); }
        /*
         * Sub-Functions
         * Fucos: Calculator(...), setSymbol/ValueCalculator(c'' i0-9 s""), isNumeric(""), ResetDetails()
         */
        private double Calculator(double n1, double n2, int method)
        {
            string[] arr = splitResulter();
            if (String.IsNullOrEmpty(arr[0]) || String.IsNullOrEmpty(arr[2]))
            {
                MessageBox.Show("Value cannot be null", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1.13782312;
            }
            return
                (double) ((method == 1)  ? n1 + n2 : (method == 2) ? n1 - n2 : (method == 3) ? n1 * n2 : n1 / n2);
        }
        private void setSymbolCalculator(char c, int n = -1)
        {
            Resulter.Text += " " + c.ToString() + " ";
            g_Option = (g_Option != -1) ? n : returnOptionByChar(c);
        }
        private int returnOptionByChar(char oper)
        { return  (oper == '+') ? 1 : (oper == '-') ? 2 : (oper == '*') ? 3 : 4; }
        private void setValueCalculator(char c)
        {
            if (g_isResulted == true) 
            { 
                ResetDetails(); 
                return; 
            }
            Resulter.Text += c;
        }
        private bool isNumeric(string n)
        {
            int n1;
            return int.TryParse(n, out n1);
        }
        private void ResetDetails()
        {
            Resulter.Clear();
            g_Option = -1; 
            g_AMO = -1;
            g_isResulted = false;
            toolStripProgressBar1.Value = 0;
        }
        public void setAMOCalculator(int n, string s = null)
        {
            ResetDetails();
            g_AMO = n;
            if(s != null) Resulter.Text = s + "(";
        }
        private string[] splitResulter()
        {
            return (Resulter.Text).Split(new char[] { ' ', '(' });
        }
        private double AMOperation(int oper/*g_AMO value*/, double NecasseryI, double OptionalI = 0)
        {
            switch (oper)
            {
                case 0: return Math.Max(NecasseryI, OptionalI);
                case 1: return Math.Min(NecasseryI, OptionalI);
                case 2: return Math.Round(NecasseryI);
                case 3: return Math.Abs(NecasseryI);
                case 4: return Math.Pow(NecasseryI, OptionalI);
                case 5: return Math.Sqrt(NecasseryI);
                case 6: return (NecasseryI % OptionalI);
                case 7: return Math.Tan(NecasseryI);
                case 8: return Math.Sin(NecasseryI);
                case 9: return Math.Cos(NecasseryI);
                case 10: return objRandom.Next(Convert.ToInt32(NecasseryI) + 1);
                default:
                    return -1;
            }
        }
        private bool isPalindrome(string n)
        {
            char[] charArray = n.ToCharArray();
            char[] reverseArray = n.ToCharArray();
            Array.Reverse(reverseArray);

            return (new string(charArray) == new string(reverseArray)) ? true : false;
        }
        private bool isPrime(int n)
        {
            for (int i = 2; i < n; i++)
                if (n % i == 0) return false;
            return true;
        }
        private void CaluculateAll()
        {
            toolStripProgressBar1.Maximum = 100000;
            toolStripProgressBar1.Step = 1;

            for (int j = 0; j < 100000; j++)
                toolStripProgressBar1.PerformStep();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string mailTo = toolStripTextBox1.Text;
            if (mailTo == null || !IsValidEmail(mailTo))
                MessageBox.Show("Enter E-Mail / Invalid E-Mail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                if (SendGmail(mUsername, mPassword, mMailFrom, mailTo, "Subject",
                "message blablababla"))
                    MessageBox.Show("E-Mail sended sucessfully", "E-Mail Sender", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                else MessageBox.Show("E-Mail don't sended", "E-Mail Sender", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool SendGmail(string userName, string password, string mailFrom, string mailTo, string subject, string message)
        {
            System.Net.Mail.MailMessage msg = new
System.Net.Mail.MailMessage(mailFrom, mailTo,
            subject, message);
            System.Net.NetworkCredential cred = new
            System.Net.NetworkCredential(userName, password);
            System.Net.Mail.SmtpClient mailClient = new
System.Net.Mail.SmtpClient("smtp.gmail.com", 587);
            mailClient.EnableSsl = true;
            mailClient.UseDefaultCredentials = false;
            mailClient.Credentials = cred;
            mailClient.Send(msg);
            return true;
        }
        private bool IsValidEmail(string strIn)
        {
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strIn, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
    }
}
