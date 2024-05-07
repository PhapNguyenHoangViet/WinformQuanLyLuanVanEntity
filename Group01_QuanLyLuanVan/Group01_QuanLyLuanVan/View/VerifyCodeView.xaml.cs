using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Group01_QuanLyLuanVan.View
{
    /// <summary>
    /// Interaction logic for VerifyCodeView.xaml
    /// </summary>
    public partial class VerifyCodeView : Window
    {
        public VerifyCodeView()
        {
            InitializeComponent();
        }
        private void OnDigitTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!string.IsNullOrEmpty(textBox.Text))
            {
                MoveToNextTextBox(textBox);
            }
        }

        private void OnDigitKeyUp(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox.Text.Length >= 1 && textBox.Text.Length < textBox.MaxLength)
            {
                MoveToNextTextBox(textBox);
            }
        }

        private void OnDigitPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (e.Key == Key.Back && string.IsNullOrEmpty(textBox.Text))
            {
                MoveToPreviousTextBox(textBox);
            }
        }

        private void MoveToNextTextBox(TextBox currentTextBox)
        {
            var nextTextBox = GetNextTextBox(currentTextBox);
            if (nextTextBox != null)
            {
                nextTextBox.Focus();
            }
        }

        private void MoveToPreviousTextBox(TextBox currentTextBox)
        {
            var previousTextBox = GetPreviousTextBox(currentTextBox);
            if (previousTextBox != null)
            {
                previousTextBox.Focus();
            }
        }

        private TextBox GetNextTextBox(TextBox currentTextBox)
        {
            var nextIndex = int.Parse(currentTextBox.Name.Substring(5)) + 1; // Assuming the TextBox names are Digit1, Digit2, ..., Digit6
            var nextTextBoxName = "Digit" + nextIndex;
            return FindName(nextTextBoxName) as TextBox;
        }

        private TextBox GetPreviousTextBox(TextBox currentTextBox)
        {
            var previousIndex = int.Parse(currentTextBox.Name.Substring(5)) - 1;
            if (previousIndex >= 1)
            {
                var previousTextBoxName = "Digit" + previousIndex;
                return FindName(previousTextBoxName) as TextBox;
            }
            return null;
        }


    }
}
