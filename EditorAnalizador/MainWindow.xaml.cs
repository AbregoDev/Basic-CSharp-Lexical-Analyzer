using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;

namespace EditorAnalizador
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        char c;
        sbyte status;
        int k;
        string buffer;
        string[,] tabla;
        List<string[]> tokens;
        Stack<char> pila;

        public MainWindow()
        {
            InitializeComponent();

            tokens = new List<string[]>();
            pila = new Stack<char>();

            FillTable();

            tbxCode.Focus();
            btnAnalizar.IsEnabled = false;
            btnDale.IsEnabled = false;
        }

        private void Grid_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (tbxCode.IsFocused)
            {
                e.Handled = true;

                if (tbxCode.Text == "")
                {
                    btnAnalizar.IsEnabled = false;
                    btnDale.IsEnabled = false;
                }

                else
                {
                    btnAnalizar.IsEnabled = true;
                    btnDale.IsEnabled = true;
                }
            }
        }

        private void btnAnalizar_Click(object sender, RoutedEventArgs e)
        {
            GenerateTokens();

            //Limpiar
            tbxTokens.Clear();

            //Impresión
            for(int f = 0; f < tokens.Count; f++)
                tbxTokens.Text += "Lexema: " + tokens[f][0] + ", Token: " + tokens[f][1] + "\n";
        }

        private void GenerateTokens()
        {
            Reset();

            //Ciclo while porque el analizador podría no recorrer todos los caracteres en caso de error
            //(El programador controlará la variable de control)
            while (k < tbxCode.Text.Length)
            {
                c = tbxCode.Text[k];

                switch (status)
                {
                    case 0:
                        //Delimitadores

                        if (c == ',')
                        {
                            tokens.Add(new string[] { ",", "," });

                            //status = 0;
                            k++;
                        }

                        else if (c == '.')
                        {
                            tokens.Add(new string[] { ".", "." });

                            //status = 0;
                            k++;
                        }

                        else if(c == '#')
                        {
                            tokens.Add(new string[] { "#", "#" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '(')
                        {
                            tokens.Add(new string[] { "(", "(" });

                            //status = 0;
                            k++;
                        }

                        else if (c == ')')
                        {
                            tokens.Add(new string[] { ")", ")" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '{')
                        {
                            tokens.Add(new string[] { "{", "{" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '}')
                        {
                            tokens.Add(new string[] { "}", "}" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '[')
                        {
                            tokens.Add(new string[] { "[", "[" });

                            //status = 0;
                            k++;
                        }

                        else if (c == ']')
                        {
                            tokens.Add(new string[] { "]", "]" });

                            //status = 0;
                            k++;
                        }

                        else if (c == ';')
                        {
                            tokens.Add(new string[] { ";", ";" });

                            //status = 0;
                            k++;
                        }

                        //Operadores Aritméticos

                        else if (c == '/')
                        {
                            status = 1;
                            k++;
                        }

                        else if (c == '*')
                        {
                            status = 2;
                            k++;
                        }

                        else if (c == '+')
                        {
                            status = 5;
                            k++;
                        }

                        else if (c == '-')
                        {
                            status = 6;
                            k++;
                        }

                        else if (c == '%')
                        {
                            status = 7;
                            k++;
                        }

                        //Operadores Lógicos

                        else if (c == '&')
                        {
                            status = 8;
                            k++;
                        }

                        else if (c == '|')
                        {
                            status = 9;
                            k++;
                        }

                        else if (c == '!')
                        {
                            status = 10;
                            k++;
                        }

                        else if (c == '=')
                        {
                            status = 11;
                            k++;
                        }

                        //Operadores Relacionales

                        else if (c == '<')
                        {
                            status = 12;
                            k++;
                        }

                        else if (c == '>')
                        {
                            status = 13;
                            k++;
                        }

                        //Secuencias de escape (omitir)

                        else if (c == 9 || c == 10 || c == 13 || c == 32)
                        {
                            //status = 0;
                            k++;
                        }

                        else if (char.IsLetter(c))
                        {
                            buffer = c.ToString();

                            status = 3;
                            k++;
                        }

                        else if (char.IsDigit(c))
                        {
                            buffer = c.ToString();

                            status = 4;
                            k++;
                        }

                        //Cadena

                        else if (c == '"')
                        {
                            buffer = "\"";

                            status = 14;
                            k++;
                        }

                        else
                        {
                            buffer = c.ToString();

                            status = -1;
                        }

                        break;

                    case 1:
                        if (c == '/')
                        {
                            status = 17;
                            k++;
                        }

                        else if (c == '*')
                        {
                            status = 18;
                            k++;
                        }

                        else if (c == '=')
                        {
                            tokens.Add(new string[] { "/=", "/=" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { "/", "/" });

                            status = 0;
                        }

                        break;

                    case 2:
                        if (c == '=')
                        {
                            tokens.Add(new string[] { "*=", "*=" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { "*", "*" });

                            status = 0;
                        }

                        break;

                    case 3:
                        if (char.IsLetter(c))
                        {
                            buffer += c;

                            //status = 3;
                            k++;
                        }

                        else
                        {
                            tokens.Add(MatcherReservedWords(buffer));
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 4:
                        if (char.IsDigit(c))
                        {
                            buffer += c;

                            //status = 4;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { buffer, "num" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 5:
                        if (c == '+')
                        {
                            tokens.Add(new string[] { "++", "++" });

                            status = 0;
                            k++;
                        }

                        else if (c == '=')
                        {
                            tokens.Add(new string[] { "+=", "+=" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { "+", "+" });

                            status = 0;
                        }

                        break;

                    case 6:
                        if (c == '-')
                        {
                            tokens.Add(new string[] { "--", "--" });

                            status = 0;
                            k++;
                        }

                        else if (c == '=')
                        {
                            tokens.Add(new string[] { "-=", "-=" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { "-", "-" });

                            status = 0;
                        }

                        break;

                    case 7:
                        if (c == '=')
                        {
                            tokens.Add(new string[] { "%=", "%=" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { "%", "%" });

                            status = 0;
                        }

                        break;

                    case 8:
                        if (c == '&')
                        {
                            tokens.Add(new string[] { "&&", "&&" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            buffer = "&";

                            status = -1;
                        }

                        break;

                    case 9:
                        if (c == '|')
                        {
                            tokens.Add(new string[] { "||", "||" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            buffer = "|";

                            status = -1;
                        }

                        break;

                    case 10:
                        if (c == '=')
                        {
                            tokens.Add(new string[] { "!=", "!=" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { "!", "!" });

                            status = 0;
                        }

                        break;

                    case 11:
                        if (c == '=')
                        {
                            tokens.Add(new string[] { "==", "==" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { "=", "=" });

                            status = 0;
                        }

                        break;

                    case 12:
                        if (c == '=')
                        {
                            tokens.Add(new string[] { "<=", "<=" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { "<", "<" });

                            status = 0;
                        }

                        break;

                    case 13:
                        if (c == '=')
                        {
                            tokens.Add(new string[] { ">=", ">=" });

                            status = 0;
                            k++;
                        }

                        else
                        {
                            tokens.Add(new string[] { ">", ">" });

                            status = 0;
                        }

                        break;

                    case 14:
                        if (c >= 32)
                        {
                            if (c == 34)
                            {
                                tokens.Add(new string[] { buffer + "\"", "string" });
                                buffer = string.Empty;

                                status = 0;
                                k++;
                            }

                            else
                            {
                                buffer += c;

                                //status = 14;
                                k++;
                            }
                        }

                        else
                        {
                            buffer += c;

                            status = -1;
                        }

                        break;

                    case 17:
                        if (c != '\n' && c != '\r')
                        {
                            //status = 17;
                            k++;
                        }

                        else
                        {
                            //Ignorar comentarios

                            status = 0;
                            k++;
                        }

                        break;

                    case 18:
                        if (c != '*')
                        {
                            //status = 18;
                            k++;
                        }

                        else
                        {
                            if (k + 1 != tbxCode.Text.Length)
                            {
                                if (tbxCode.Text[k + 1] == '/')
                                {
                                    //Ignorar comentarios

                                    status = 0;
                                    k += 2;
                                }

                                else
                                {
                                    buffer += c;

                                    //status = 18;
                                    k++;
                                }
                            }

                            else
                            {
                                status = -1;
                            }
                        }

                        break;
                }

                if (status == -1)
                    break;
            }

            lblStatus.Content = "Hecho";
            lblStatus.Foreground = Brushes.YellowGreen;

            if (buffer != string.Empty)
            {
                switch (status)
                {
                    case 1:
                        tokens.Add(new string[] { "/", "/" });
                        break;

                    case 2:
                        tokens.Add(new string[] { "*", "*" });
                        break;

                    case 3:
                        tokens.Add(MatcherReservedWords(buffer));
                        break;

                    case 4:
                        tokens.Add(new string[] { buffer, "num" });
                        break;

                    case 5:
                        tokens.Add(new string[] { "+", "+" });
                        break;

                    case 6:
                        tokens.Add(new string[] { "-", "-" });
                        break;

                    case 7:
                        tokens.Add(new string[] { "%", "%" });
                        break;

                    case 10:
                        tokens.Add(new string[] { "!", "!" });
                        break;

                    case 11:
                        tokens.Add(new string[] { "=", "=" });
                        break;

                    case 12:
                        tokens.Add(new string[] { "<", "<" });
                        break;

                    case 13:
                        tokens.Add(new string[] { ">", ">" });
                        break;

                    default:
                        tokens.Add(new string[] { buffer, "Error" });

                        lblStatus.Content = "Error";
                        lblStatus.Foreground = Brushes.OrangeRed;
                        break;
                }
            }
        }

        private string[] MatcherReservedWords(string s)
        {
            switch (s)
            {
                case "if":
                    return new string[] { "if", "if" };

                case "int":
                    return new string[] { "int", "int" };

                case "long":
                    return new string[] { "long", "long" };

                case "char":
                    return new string[] { "char", "char" };

                case "else":
                    return new string[] { "else", "else" };

                case "true":
                    return new string[] { "true", "true" };

                case "void":
                    return new string[] { "void", "void" };

                case "float":
                    return new string[] { "float", "float" };

                case "false":
                    return new string[] { "false", "false" };

                case "while":
                    return new string[] { "while", "while" };

                case "double":
                    return new string[] { "double", "double" };

                case "return":
                    return new string[] { "return", "return" };

                case "main":
                    return new string[] { "main", "main" };

                case "include":
                    return new string[] { "include", "include" };

                default:
                    return new string[] { s, "identificador" };
            }
        }

        private void Reset()
        {
            tokens.Clear();
            buffer = string.Empty;
            status = 0;
            k = 0;

            tbxCode.Height = 300;
            tbxTokens.Visibility = Visibility.Visible;
            tbxTokens.Text = string.Empty;
        }

        private void FillTable()
        {
            tabla = new string[4,7];

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 7; j++)
                    tabla[i, j] = "E";

            tabla[0, 1] = tabla[2, 1] = "a";
            tabla[0, 2] = tabla[2, 2] = "c";
            tabla[0, 3] = "n";
            tabla[0, 4] = tabla[1, 4] = "p";
            tabla[0, 5] = tabla[1, 5] = "r";
            tabla[0, 6] = "$";
            tabla[3,1] = tabla[3,2] = "FnD";

            tabla[0, 0] = " ";
            tabla[1, 0] = "D";
            tabla[2, 0] = "F";
            tabla[3, 0] = "S";
        }

        private void BtnDale_Click(object sender, RoutedEventArgs e)
        {
            int k = 0;
            string aux, cadena = tbxCode.Text;
            pila.Clear();

            pila.Push('S');
            while(pila.Count != 0)
            {
                if (char.IsLower(pila.Peek()))
                {
                    if (pila.Peek() == cadena[k])
                    {
                        pila.Pop();
                        k++;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    try
                    {
                        aux = tabla[SearchColumn(tabla, 0, pila.Peek()), SearchRow(tabla, 0, cadena[k])];
                        if (aux != "E")
                        {
                            pila.Pop();
                            for (int j = 0; j < aux.Length; j++)
                                pila.Push(aux[aux.Length - j - 1]);
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch(IndexOutOfRangeException)
                    {
                        break;
                    }
                }
            }

            if (k == cadena.Length && pila.Count == 0)
            {
                lblSt.Foreground = Brushes.YellowGreen;
                lblSt.Content = "Bien";
            }
            else
            {
                lblSt.Foreground = Brushes.OrangeRed;
                lblSt.Content = "Mal";
            }
        }

        private int SearchColumn(string[,] array, int index, char symb)
        {
            for (int i = 0; i < array.GetLength(0); i++)
                if (array[i, index] == symb.ToString())
                    return i;

            return -1;
        }

        private int SearchRow(string[,] array, int index, char symb)
        {
            for (int i = 0; i < array.GetLength(1); i++)
                if (array[index, i] == symb.ToString())
                    return i;

            return -1;
        }

        private void tbxCode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Tab)
            {
                e.Handled = true;
                MessageBox.Show(tbxCode.SelectionStart.ToString());
            }
        }
    }
}
