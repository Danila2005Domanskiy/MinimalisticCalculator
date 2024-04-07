using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace Лабораторная_2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            foreach (var child in MainGrid.Children)
            {
                if (child is Button button)
                {
                    button.Click += Button_Click;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string buttonText = button.Content.ToString();

                try
                {
                    switch (buttonText)
                    {
                        case "AC":
                            ClearDisplay();
                            break;

                        case "C":
                            RemoveLastCharacter();
                            break;

                        case "=":
                            CalculateExpression();
                            break;

                        default:
                            AppendToDisplay(buttonText);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }    
        }

        private void ClearDisplay()
        {
            textLabel.Text = string.Empty;
            priviosLabel.Text = string.Empty;
        }

        private void RemoveLastCharacter()
        {
            if (textLabel.Text.Length > 0)
                textLabel.Text = textLabel.Text.Substring(0, textLabel.Text.Length - 1);
        }

        private void CalculateExpression()
        {
            string expression = textLabel.Text;

            if (string.IsNullOrEmpty(expression))
                return;

            if (expression.Contains("/0"))
                throw new DivideByZeroException("Деление на ноль недопустимо!");

            if (expression.Contains("="))
                return;

            var result = new DataTable().Compute(ReplaceCommasWithDots(expression), null);
            priviosLabel.Text = expression;
            textLabel.Text = "=" + result.ToString();
        }

        private void AppendToDisplay(string text)
        {
            if (textLabel.Text.Contains("=") && char.IsDigit(text[0]))
                textLabel.Text = text;
            else if (textLabel.Text.Contains("="))
                textLabel.Text = textLabel.Text.Split('=')[1] + text;
            else
                textLabel.Text += text;
        }

        private string ReplaceCommasWithDots(string input)
        {
            return input.Replace(',', '.');
        }
    }
}
