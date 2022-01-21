using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {
        public Calculator()
        {
            InitializeComponent();
        }

        private void Clicked(object sender, EventArgs e)
        {
            string buttonText = ((Button)sender).Text;
            Screen.Text += buttonText;
        }

        private void Cleared(object sender, EventArgs e) => Screen.Text = null;

        private void Calculated(object sender, EventArgs e)
        {
            char[] possibleOperations = new char[] { '+', '-', '*', '/' };
            List<double> numbers = (from string number in Screen.Text.Split(possibleOperations)
                                    select double.Parse(number)).ToList();
            List<char> operations = (from char element in Screen.Text
                                    where Contains<char>(possibleOperations,element)
                                    select element).ToList();

            char operation = default(char);
            double result = default(double);
            while (operations.Count != 0)
            {
                operation = ConsiderPriority(operations, out int index);
                double[] numbersInOperation = new double[] { numbers[index], numbers[index + 1] };
                result = Evaluate(numbersInOperation, operation);

                for (int i = 0; i < 2; i++) numbers.RemoveAt(index);
                numbers.Insert(index, result);
                
                Screen.Text = Screen.Text.Replace($"{numbersInOperation[0]}{operation}{numbersInOperation[1]}", $"{result}");
            }
        }

        private char ConsiderPriority(List<char> operations, out int index)
        {
            char operation = default(char);
            if (operations.Contains('*')) operation = '*';
            else if (operations.Contains('/')) operation = '/';
            else if (operations.Contains('+')) operation = '+';
            else if (operations.Contains('-')) operation = '-';

            index = operations.IndexOf(operation);
            operations.Remove(operation);
            return operation;
        }
        
        private double Evaluate(double[] numbers, char operation)
        {
            double result = default(double);
            switch (operation)
            {
                case '+':
                    result = numbers[0] + numbers[1];
                    break;
                case '-':
                    result = numbers[0] - numbers[1];
                    break;
                case '*':
                    result = numbers[0] * numbers[1];
                    break;
                case '/':
                    result = numbers[0] / numbers[1];
                    break;
            }

            return result;
        }

        private bool Contains<T>(T[] array, T target)
        {
            foreach (T element in array)
            {
                if (element.Equals(target)) return true;
            }

            return false;
        }
    }
}