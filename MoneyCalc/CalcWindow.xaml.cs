using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace MoneyCalc
{
    /// <summary>
    /// Логика взаимодействия для CalcWindow.xaml
    /// </summary>
    public partial class CalcWindow : Window
    {
        public CalcWindow()
        {
            InitializeComponent();
        }
        private void Grid_Click(object sender, RoutedEventArgs e)
        {
            Button btn = e.Source as Button;
            if (btn == null)
                return;
            string value = btn.Content.ToString();
            if (String.IsNullOrEmpty(value))
                return;
            switch (value)
            {
                case "C":
                    TextBoxMain.Text = "0";
                    break;

                case "=":
                    try
                    {                     
                        TextBoxMain.Text = Calc.Calculate(TextBoxMain.Text).ToString();
                    }
                    catch
                    {
                        TextBoxMain.Text = "Error";
                    }
                    break;

                case "Ок":

                default:
                    TextBoxMain.Text = TextBoxMain.Text.TrimStart('0') + value;
                    break;
            }

        }
    }

}
