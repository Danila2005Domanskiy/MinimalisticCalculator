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
            CreateButtons();
        }

        private void CreateButtons()
        {
            string[,] buttons = 
            {
                { "(", ")", "C", "AC" },
                { "1", "2", "3", "+" },
                { "4", "5", "6", "-" },
                { "7", "8", "9", "*" },
                { "0", ",", "=", "/" }
            };

            for (int row = 0; row < buttons.GetLength(0); row++)
            {
                for (int col = 0; col < buttons.GetLength(1); col++)
                {
                    Button button = new Button
                    {
                        Content = buttons[row, col],
                        FontSize = 25
                    };

                    button.Click += Button_Click;

                    Grid.SetRow(button, row + 2);
                    Grid.SetColumn(button, col);

                    if (buttons[row, col] == "=")
                    {
                        button.Background = System.Windows.Media.Brushes.LightSkyBlue;
                    }

                    MainGrid.Children.Add(button);
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
            {
                textLabel.Text = textLabel.Text.Substring(0, textLabel.Text.Length - 1);
            }
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
            {
                ClearDisplay();
                textLabel.Text = text;
            }
            else if (textLabel.Text.Contains("="))
            {
                textLabel.Text = textLabel.Text.Split('=')[1] + text;
            }
            else
            {
                textLabel.Text += text;
            }
        }

        private string ReplaceCommasWithDots(string input)
        {
            return input.Replace(',', '.');
        }
    }
}
