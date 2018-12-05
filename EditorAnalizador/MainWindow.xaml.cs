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

            //Impresión
            for(int f = 0; f < tokens.Count; f++)
                tbxTokens.Text += "Lexema: \"" + tokens[f][0] + "\", Token: " + tokens[f][1] + "\n";
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

                        if (c == '.')
                        {
                            ////lexms += "Lexema: \".\", Token: Punto\n";
                            tokens.Add(new string[] { ".", "Punto" });

                            //status = 0;
                            k++;
                        }

                        else if (c == ',')
                        {
                            ////lexms += "Lexema: \",\", Token: Coma\n";
                            tokens.Add(new string[] { ",", "Coma" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '(')
                        {
                            ////lexms += "Lexema: \"(\", Token: Paréntesis Izquierdo\n";
                            tokens.Add(new string[] { "(", "P Izq" });

                            //status = 0;
                            k++;
                        }

                        else if (c == ')')
                        {
                            //lexms += "Lexema: \")\", Token: Paréntesis Derecho\n";
                            tokens.Add(new string[] { ")", "P Der" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '{')
                        {
                            //lexms += "Lexema: \"{\", Token: Llave Izquierda\n";
                            tokens.Add(new string[] { "{", "LlaveI" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '}')
                        {
                            //lexms += "Lexema: \"}\", Token: Llave Derecha\n";
                            tokens.Add(new string[] { "}", "LlaveD" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '[')
                        {
                            //lexms += "Lexema: \"[\", Token: Corchete Izquierdo\n";
                            tokens.Add(new string[] { "[", "CorchI" });

                            //status = 0;
                            k++;
                        }

                        else if (c == ']')
                        {
                            //lexms += "Lexema: \"]\", Token: Corchete Derecho\n";
                            tokens.Add(new string[] { "]", "CorchD" });

                            //status = 0;
                            k++;
                        }

                        else if (c == ';')
                        {
                            //lexms += "Lexema: \";\", Token: Punto y Coma\n";
                            tokens.Add(new string[] { ";", "PtoyCma" });

                            //status = 0;
                            k++;
                        }

                        else if (c == ':')
                        {
                            //lexms += "Lexema: \":\", Token: Dos Puntos\n";
                            tokens.Add(new string[] { ":", "DosPuntos" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '?')
                        {
                            //lexms += "Lexema: \"?\", Token: Interrogación\n";
                            tokens.Add(new string[] { "?", "Interrog" });

                            //status = 0;
                            k++;
                        }

                        else if (c == '_')
                        {
                            buffer = c.ToString();

                            status = 19;
                            k++;
                        }

                        //Operadores Aritméticos

                        else if (c == '/')
                        {
                            buffer = c.ToString();

                            status = 1;
                            k++;
                        }

                        else if (c == '*')
                        {
                            buffer = c.ToString();

                            status = 2;
                            k++;
                        }

                        else if (c == '+')
                        {
                            buffer = c.ToString();

                            status = 5;
                            k++;
                        }

                        else if (c == '-')
                        {
                            buffer = c.ToString();

                            status = 6;
                            k++;
                        }

                        else if (c == '%')
                        {
                            buffer = c.ToString();

                            status = 7;
                            k++;
                        }

                        //Operadores Lógicos

                        else if (c == '&')
                        {
                            buffer = c.ToString();

                            status = 8;
                            k++;
                        }

                        else if (c == '|')
                        {
                            buffer = c.ToString();

                            status = 9;
                            k++;
                        }

                        else if (c == '!')
                        {
                            buffer = c.ToString();

                            status = 10;
                            k++;
                        }

                        else if (c == '=')
                        {
                            buffer = c.ToString();

                            status = 11;
                            k++;
                        }

                        else if (c == '^')
                        {
                            //lexms += "Lexema: \"^\", Token: Xor Lógico\n";
                            tokens.Add(new string[] { "^", "Xor" });

                            //status = 0;
                            k++;
                        }

                        //Operadores Relacionales

                        else if (c == '<')
                        {
                            buffer = c.ToString();

                            status = 12;
                            k++;
                        }

                        else if (c == '>')
                        {
                            buffer = c.ToString();

                            status = 13;
                            k++;
                        }

                        //Secuencias de escape (omitir)
                        else if (c == 9 || c == 10 || c == 13 || c == 32)
                        {
                            //status = 0;
                            k++;
                        }

                        else if (Char.IsLetter(c))
                        {
                            buffer = c.ToString();

                            status = 3;
                            k++;
                        }

                        else if (Char.IsDigit(c))
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
                            //lexms += "Lexema: \"/=\", Token: División Asignada\n";
                            tokens.Add(new string[] { "/=", "DivAs" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"/\", Token: División\n";
                            tokens.Add(new string[] { "/", "Div" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 2:
                        if (c == '=')
                        {
                            //lexms += "Lexema: \"*=\", Token: Producto Asignado\n";
                            tokens.Add(new string[] { "*=", "ProdAs" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"*\", Token: Producto\n";
                            tokens.Add(new string[] { "*", "Prod" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 3:
                        if (Char.IsLetter(c) || Char.IsDigit(c))
                        {
                            buffer += c;

                            //status = 3;
                            k++;
                        }

                        else if (c == '_')
                        {
                            buffer += c;

                            status = 19;
                            k++;
                        }

                        else
                        {
                            MatcherReservedWords(buffer);
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 4:
                        if (Char.IsDigit(c))
                        {
                            buffer += c;

                            //status = 4;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"" + buffer + "\", Token: Dígito\n";
                            tokens.Add(new string[] { buffer, "Dig" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 5:
                        if (c == '+')
                        {
                            //lexms += "Lexema: \"++\", Token: Incremento\n";
                            tokens.Add(new string[] { "++", "Incr" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else if (c == '=')
                        {
                            //lexms += "Lexema: \"+=\", Token: Suma Asignada\n";
                            tokens.Add(new string[] { "+=", "SumAs" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"+\", Token: Suma\n";
                            tokens.Add(new string[] { "+", "Suma" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 6:
                        if (c == '-')
                        {
                            //lexms += "Lexema: \"--\", Token: Decremento\n";
                            tokens.Add(new string[] { "--", "Decr" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else if (c == '=')
                        {
                            //lexms += "Lexema: \"-=\", Token: Resta Asignada\n";
                            tokens.Add(new string[] { "-=", "RestAs" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"-\", Token: Resta\n";
                            tokens.Add(new string[] { "-", "Resta" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 7:
                        if (c == '=')
                        {
                            //lexms += "Lexema: \"%=\", Token: Módulo Asignado\n";
                            tokens.Add(new string[] { "%=", "ModAs" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"%\", Token: Módulo\n";
                            tokens.Add(new string[] { "%", "Mod" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 8:
                        if (c == '&')
                        {
                            //lexms += "Lexema: \"&&\", Token: And Lógico\n";
                            tokens.Add(new string[] { "&&", "AndL" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"&\", Token: And Bitwise\n";
                            tokens.Add(new string[] { "&", "AndB" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 9:
                        if (c == '|')
                        {
                            //lexms += "Lexema: \"||\", Token: Or Lógico\n";
                            tokens.Add(new string[] { "||", "OrL" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"%\", Token: Or Bitwise\n";
                            tokens.Add(new string[] { "|", "OrB" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 10:
                        if (c == '=')
                        {
                            //lexms += "Lexema: \"!=\", Token: Diferente de\n";
                            tokens.Add(new string[] { "!=", "Diff" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"!\", Token: Not Lógico\n";
                            tokens.Add(new string[] { "!", "Not" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 11:
                        if (c == '=')
                        {
                            //lexms += "Lexema: \"==\", Token: Igual a\n";
                            tokens.Add(new string[] { "==", "Igual" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"=\", Token: Asignación\n";
                            tokens.Add(new string[] { "=", "Asign" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 12:
                        if (c == '=')
                        {
                            //lexms += "Lexema: \"<=\", Token: Menor o Igual\n";
                            tokens.Add(new string[] { "<=", "MenorIgual" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"<\", Token: Menor que\n";
                            tokens.Add(new string[] { "<", "Menor" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 13:
                        if (c == '=')
                        {
                            //lexms += "Lexema: \">=\", Token: Mayor o Igual\n";
                            tokens.Add(new string[] { ">=", "MayorIgual" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \">\", Token: Mayor que\n";
                            tokens.Add(new string[] { ">", "Mayor" });
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 14:
                        if (c == 32 || c == 33 || c >= 35 && c <= 91 || c >= 93 && c <= 255)
                        {
                            buffer += c;

                            //status = 14;
                            k++;
                        }

                        else if (c == 34)
                        {
                            //lexms += "Lexema: \"" + buffer + "\"\", Token: Cadena\n";
                            tokens.Add(new string[] { buffer + "\"", "Cadena" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else if (c == 92)
                        {
                            buffer += c;

                            status = 15;
                            k++;
                        }

                        else
                        {
                            buffer += c;

                            status = -1;
                        }

                        break;

                    case 15:
                        if (c == '"' || c == 'n' || c == 'r' || c == 't' || c == '\\')
                        {
                            buffer += c;

                            status = 16;
                            k++;
                        }

                        else
                        {
                            buffer += c;

                            status = -1;
                        }

                        break;

                    case 16:
                        if (c == 32 || c == 33 || c >= 35 && c <= 91 || c >= 93 && c <= 126)
                        {
                            buffer += c;

                            status = 14;
                            k++;
                        }

                        else if (c == 34)
                        {
                            //lexms += "Lexema: \"" + buffer + "\"\", Token: Cadena\n";
                            tokens.Add(new string[] { buffer + "\"", "Cadena" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
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
                            buffer += c;

                            //status = 17;
                            k++;
                        }

                        else
                        {
                            //lexms += "Lexema: \"/" + buffer + "\", Token: Comentario\n";
                            tokens.Add(new string[] { "/" + buffer, "Coment" });
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        break;

                    case 18:
                        if (c != '*')
                        {
                            switch (c)
                            {
                                case '\n':
                                    buffer += "|\\n|";
                                    break;

                                case '\r':
                                    buffer += "|\\r|";
                                    break;

                                default:
                                    buffer += c;
                                    break;
                            }

                            //status = 18;
                            k++;
                        }

                        else
                        {
                            if (k + 1 != tbxCode.Text.Length)
                            {
                                if (tbxCode.Text[k + 1] == '/')
                                {
                                    //lexms += "Lexema: \"/*" + buffer + "*/\", Token: Comentario de Párrafo\n";
                                    tokens.Add(new string[] { "/*" + buffer + "*/", "ComentP" });
                                    buffer = string.Empty;

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
                                buffer += c;

                                status = -1;
                            }
                        }
                        break;

                    case 19:
                        if (Char.IsLetter(c) || Char.IsDigit(c))
                        {
                            buffer += c;

                            status = 3;
                            k++;
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
                if (status == 1)
                {
                    //lexms += "Lexema: \"/\", Token: División\n";
                    tokens.Add(new string[] { "/", "Div" });
                }
                else if (status == 2)
                {
                    //lexms += "Lexema: \"*\", Token: Producto\n";
                    tokens.Add(new string[] { "*", "Prod" });
                }
                else if (status == 3)
                {
                    MatcherReservedWords(buffer);
                }
                else if (status == 4)
                {
                    //lexms += "Lexema: \"" + buffer + "\", Token: Dígito\n";
                    tokens.Add(new string[] { buffer, "Dig" });
                }
                else if (status == 5)
                {
                    //lexms += "Lexema: \"+\", Token: Suma\n";
                    tokens.Add(new string[] { "+", "Suma" });
                }
                else if (status == 6)
                {
                    //lexms += "Lexema: \"-\", Token: Resta\n";
                    tokens.Add(new string[] { "-", "Resta" });
                }
                else if (status == 7)
                {
                    //lexms += "Lexema: \"%\", Token: Módulo\n";
                    tokens.Add(new string[] { "%", "Mod" });
                }
                else if (status == 8)
                {
                    //lexms += "Lexema: \"&\", Token: And Bitwise\n";
                    tokens.Add(new string[] { "&", "AndB" });
                }
                else if (status == 9)
                {
                    //lexms += "Lexema: \"|\", Token: Or Bitwise\n";
                    tokens.Add(new string[] { "|", "OrB" });
                }
                else if (status == 10)
                {
                    //lexms += "Lexema: \"!\", Token: Not Lógico\n";
                    tokens.Add(new string[] { "!", "Not" });
                }
                else if (status == 11)
                {
                    //lexms += "Lexema: \"=\", Token: Asignación\n";
                    tokens.Add(new string[] { "=", "Asign" });
                }
                else if (status == 12)
                {
                    //lexms += "Lexema: \"<\", Token: Menor que\n";
                    tokens.Add(new string[] { "<", "Menor" });
                }
                else if (status == 13)
                {
                    //lexms += "Lexema: \">\", Token: Mayor que\n";
                    tokens.Add(new string[] { ">", "Mayor" });
                }
                else if (status == 17)
                {
                    //lexms += "Lexema: \"/" + buffer + "\", Token: Comentario\n";
                    tokens.Add(new string[] { "/" + buffer, "Coment" });
                }
                else
                {
                    //lexms += "Lexema: \"" + buffer + "\", Token: Error\n";
                    tokens.Add(new string[] { buffer, "Error" });

                    lblStatus.Content = "Error";
                    lblStatus.Foreground = Brushes.OrangeRed;
                }

                buffer = string.Empty;
            }
        }

        private void MatcherReservedWords(string s)
        {
            if (s.Length == 2)
            {
                switch (s)
                {
                    case "if":
                        //lexms += "Lexema: \"" + s + "\", Token: If\n";
                        tokens.Add(new string[] { s, "if" });
                        break;

                    case "do":
                        //lexms += "Lexema: \"" + s + "\", Token: Do\n";
                        tokens.Add(new string[] { s, "do" });
                        break;

                    case "Ln":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    default:
                        //lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        tokens.Add(new string[] { s, "id" });
                        break;
                }
            }

            else if (s.Length == 3)
            {
                switch (s)
                {
                    case "int":
                        //lexms += "Lexema: \"" + s + "\", Token: Int\n";
                        tokens.Add(new string[] { s, "int" });
                        break;

                    case "for":
                        //lexms += "Lexema: \"" + s + "\", Token: For\n";
                        tokens.Add(new string[] { s, "for" });
                        break;

                    case "new":
                        //lexms += "Lexema: \"" + s + "\", Token: New\n";
                        tokens.Add(new string[] { s, "new" });
                        break;

                    case "try":
                        //lexms += "Lexema: \"" + s + "\", Token: Try\n";
                        tokens.Add(new string[] { s, "try" });
                        break;

                    case "Cos":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    case "Sin":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    case "Tan":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    case "Exp":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    case "Log":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    case "Pow":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    default:
                        //lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        tokens.Add(new string[] { s, "id" });
                        break;
                }
            }

            else if (s.Length == 4)
            {
                switch (s)
                {
                    case "bool":
                        //lexms += "Lexema: \"" + s + "\", Token: Boolean\n";
                        tokens.Add(new string[] { s, "bool" });
                        break;

                    case "long":
                        //lexms += "Lexema: \"" + s + "\", Token: Long\n";
                        tokens.Add(new string[] { s, "long" });
                        break;

                    case "char":
                        //lexms += "Lexema: \"" + s + "\", Token: Char\n";
                        tokens.Add(new string[] { s, "char" });
                        break;

                    case "case":
                        //lexms += "Lexema: \"" + s + "\", Token: Case\n";
                        tokens.Add(new string[] { s, "case" });
                        break;

                    case "else":
                        //lexms += "Lexema: \"" + s + "\", Token: Else\n";
                        tokens.Add(new string[] { s, "else" });
                        break;

                    case "main":
                        //lexms += "Lexema: \"" + s + "\", Token: Método Principal\n";
                        tokens.Add(new string[] { s, "Main" });
                        break;

                    case "null":
                        //lexms += "Lexema: \"" + s + "\", Token: Null\n";
                        tokens.Add(new string[] { s, "null" });
                        break;

                    case "this":
                        //lexms += "Lexema: \"" + s + "\", Token: This\n";
                        tokens.Add(new string[] { s, "this" });
                        break;

                    case "true":
                        //lexms += "Lexema: \"" + s + "\", Token: True\n";
                        tokens.Add(new string[] { s, "true" });
                        break;

                    case "void":
                        //lexms += "Lexema: \"" + s + "\", Token: Void\n";
                        tokens.Add(new string[] { s, "void" });
                        break;

                    case "Math":
                        //lexms += "Lexema: \"" + s + "\", Token: Clase\n";
                        tokens.Add(new string[] { s, "clase" });
                        break;

                    case "Read":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    case "Sqrt":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    default:
                        //lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        tokens.Add(new string[] { s, "id" });
                        break;
                }
            }

            else if (s.Length == 5)
            {
                switch (s)
                {
                    case "float":
                        //lexms += "Lexema: \"" + s + "\", Token: Float\n";
                        tokens.Add(new string[] { s, "float" });
                        break;

                    case "break":
                        //lexms += "Lexema: \"" + s + "\", Token: Break\n";
                        tokens.Add(new string[] { s, "break" });
                        break;

                    case "catch":
                        //lexms += "Lexema: \"" + s + "\", Token: Catch\n";
                        tokens.Add(new string[] { s, "catch" });
                        break;

                    case "class":
                        //lexms += "Lexema: \"" + s + "\", Token: Class\n";
                        tokens.Add(new string[] { s, "class" });
                        break;

                    case "false":
                        //lexms += "Lexema: \"" + s + "\", Token: False\n";
                        tokens.Add(new string[] { s, "false" });
                        break;

                    case "while":
                        //lexms += "Lexema: \"" + s + "\", Token: While\n";
                        tokens.Add(new string[] { s, "while" });
                        break;

                    case "using":
                        //lexms += "Lexema: \"" + s + "\", Token: Using\n";
                        tokens.Add(new string[] { s, "using" });
                        break;

                    default:
                        //lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        tokens.Add(new string[] { s, "id" });
                        break;
                }
            }

            else if (s.Length == 6)
            {
                switch (s)
                {
                    case "double":
                        //lexms += "Lexema: \"" + s + "\", Token: Double\n";
                        tokens.Add(new string[] { s, "double" });
                        break;

                    case "string":
                        //lexms += "Lexema: \"" + s + "\", Token: Tipo String\n";
                        tokens.Add(new string[] { s, "string" });
                        break;

                    case "return":
                        //lexms += "Lexema: \"" + s + "\", Token: Return\n";
                        tokens.Add(new string[] { s, "return" });
                        break;

                    case "public":
                        //lexms += "Lexema: \"" + s + "\", Token: Public\n";
                        tokens.Add(new string[] { s, "public" });
                        break;

                    case "static":
                        //lexms += "Lexema: \"" + s + "\", Token: Static\n";
                        tokens.Add(new string[] { s, "static" });
                        break;

                    case "switch":
                        //lexms += "Lexema: \"" + s + "\", Token: Switch\n";
                        tokens.Add(new string[] { s, "switch" });
                        break;

                    case "System":
                        //lexms += "Lexema: \"" + s + "\", Token: Namespace\n";
                        tokens.Add(new string[] { s, "ns" });
                        break;

                    case "Random":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    default:
                        //lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        tokens.Add(new string[] { s, "id" });
                        break;
                }
            }

            else if (s.Length == 7)
            {
                switch (s)
                {
                    case "decimal":
                        //lexms += "Lexema: \"" + s + "\", Token: Decimal\n";
                        tokens.Add(new string[] { s, "dec" });
                        break;

                    case "private":
                        //lexms += "Lexema: \"" + s + "\", Token: Private\n";
                        tokens.Add(new string[] { s, "priv" });
                        break;

                    case "Console":
                        //lexms += "Lexema: \"" + s + "\", Token: Clase\n";
                        tokens.Add(new string[] { s, "clase" });
                        break;

                    default:
                        //lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        tokens.Add(new string[] { s, "id" });
                        break;
                }
            }

            else if (s.Length == 8)
            {
                switch (s)
                {
                    case "internal":
                        //lexms += "Lexema: \"" + s + "\", Token: Internal\n";
                        tokens.Add(new string[] { s, "internal" });
                        break;

                    case "ReadLine":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    default:
                        //lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        tokens.Add(new string[] { s, "id" });
                        break;
                }
            }

            else if (s.Length == 9)
            {
                switch (s)
                {
                    case "Exception":
                        //lexms += "Lexema: \"" + s + "\", Token: Clase\n";
                        tokens.Add(new string[] { s, "clase" });
                        break;

                    case "WriteLine":
                        //lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        tokens.Add(new string[] { s, "met" });
                        break;

                    default:
                        //lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        tokens.Add(new string[] { s, "id" });
                        break;
                }
            }

            else
            {
                //lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                tokens.Add(new string[] { s, "id" });
            }
        }

        private void Reset()
        {
            tokens.Clear();
            status = 0;
            k = 0;

            tbxCode.Height = 300;
            tbxTokens.Visibility = Visibility.Visible;
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
                if (Char.IsLower(pila.Peek()))
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
    }
}
