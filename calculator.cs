using System;
using System.Collections.Generic;

namespace CalculatorApp
{
    class Program
    {
        static List<string> calculationHistory = new List<string>();
        
        static void Main(string[] args)
        {
            Console.WriteLine("=== C# Console Calculator ===");
            Console.WriteLine("Available operations: +, -, *, /, %, sqrt, clear, history, exit");
            Console.WriteLine();
            
            bool running = true;
            string currentInput = "";
            
            while (running)
            {
                Console.Write("Enter expression or command: ");
                string input = Console.ReadLine().Trim().ToLower();
                
                if (input == "exit")
                {
                    running = false;
                    Console.WriteLine("Goodbye!");
                    continue;
                }
                else if (input == "clear")
                {
                    currentInput = "";
                    Console.Clear();
                    Console.WriteLine("=== C# Console Calculator ===");
                    Console.WriteLine("Available operations: +, -, *, /, %, sqrt, clear, history, exit");
                    Console.WriteLine();
                    continue;
                }
                else if (input == "history")
                {
                    DisplayHistory();
                    continue;
                }
                else if (input == "sqrt")
                {
                    Console.Write("Enter number: ");
                    if (double.TryParse(Console.ReadLine(), out double number))
                    {
                        if (number >= 0)
                        {
                            double result = Math.Sqrt(number);
                            string calculation = $"√{number} = {result:F8}";
                            calculationHistory.Add(calculation);
                            Console.WriteLine($"Result: {calculation}");
                        }
                        else
                        {
                            Console.WriteLine("Error: Cannot calculate square root of negative number.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid number.");
                    }
                    continue;
                }
                
                // Handle calculations
                try
                {
                    // Check if input contains an operator
                    if (input.Contains("+") || input.Contains("-") || input.Contains("*") || 
                        input.Contains("/") || input.Contains("%"))
                    {
                        double result = EvaluateExpression(input);
                        string calculation = $"{input} = {result:F8}";
                        calculationHistory.Add(calculation);
                        Console.WriteLine($"Result: {result:F8}");
                    }
                    else if (double.TryParse(input, out double num))
                    {
                        // Just a number, store for future use
                        currentInput = input;
                        Console.WriteLine($"Stored: {currentInput}");
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid input. Use format: 5 + 3 or 7 * 2");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                
                Console.WriteLine();
            }
        }
        
        static double EvaluateExpression(string expression)
        {
            // Remove spaces
            expression = expression.Replace(" ", "");
            
            // Find operator
            int operatorIndex = -1;
            char operatorChar = '+';
            
            // Check for each operator (respecting order of operations would be more complex)
            char[] operators = { '+', '-', '*', '/', '%' };
            
            // For simplicity, find the last operator (basic left-to-right evaluation)
            for (int i = expression.Length - 1; i >= 0; i--)
            {
                if (operators.Contains(expression[i]) && (i == 0 || expression[i-1] != 'e')) // Avoid scientific notation
                {
                    operatorIndex = i;
                    operatorChar = expression[i];
                    break;
                }
            }
            
            if (operatorIndex == -1)
            {
                throw new ArgumentException("No operator found in expression");
            }
            
            string leftStr = expression.Substring(0, operatorIndex);
            string rightStr = expression.Substring(operatorIndex + 1);
            
            if (!double.TryParse(leftStr, out double left) || !double.TryParse(rightStr, out double right))
            {
                throw new ArgumentException("Invalid numbers in expression");
            }
            
            switch (operatorChar)
            {
                case '+':
                    return left + right;
                case '-':
                    return left - right;
                case '*':
                    return left * right;
                case '/':
                    if (right == 0)
                        throw new DivideByZeroException("Division by zero is not allowed.");
                    return left / right;
                case '%':
                    return left % right;
                default:
                    throw new ArgumentException($"Unsupported operator: {operatorChar}");
            }
        }
        
        static void DisplayHistory()
        {
            Console.WriteLine("\n=== Calculation History ===");
            if (calculationHistory.Count == 0)
            {
                Console.WriteLine("No calculations yet.");
            }
            else
            {
                for (int i = 0; i < calculationHistory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {calculationHistory[i]}");
                }
            }
            Console.WriteLine();
        }
    }
}