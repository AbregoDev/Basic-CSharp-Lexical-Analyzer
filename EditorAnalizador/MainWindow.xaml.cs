using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
        string lexms, buffer;

        public MainWindow()
        {
            InitializeComponent();

            tbxCode.Focus();
            btnAnalizar.IsEnabled = false;
        }

        private void Grid_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (tbxCode.IsFocused)
            {
                e.Handled = true;

                if (tbxCode.Text == "")
                    btnAnalizar.IsEnabled = false;

                else
                    btnAnalizar.IsEnabled = true;
            }
        }

        private void btnAnalizar_Click(object sender, RoutedEventArgs e)
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
                            lexms += "Lexema: \".\", Token: Punto\n";

                            //status = 0;
                            k++;
                        }

                        else if (c == ',')
                        {
                            lexms += "Lexema: \",\", Token: Coma\n";

                            //status = 0;
                            k++;
                        }

                        else if (c == '(')
                        {
                            lexms += "Lexema: \"(\", Token: Paréntesis Izquierdo\n";

                            //status = 0;
                            k++;
                        }

                        else if (c == ')')
                        {
                            lexms += "Lexema: \")\", Token: Paréntesis Derecho\n";

                            //status = 0;
                            k++;
                        }

                        else if (c == '{')
                        {
                            lexms += "Lexema: \"{\", Token: Llave Izquierda\n";

                            //status = 0;
                            k++;
                        }

                        else if (c == '}')
                        {
                            lexms += "Lexema: \"}\", Token: Llave Derecha\n";

                            //status = 0;
                            k++;
                        }

                        else if (c == '[')
                        {
                            lexms += "Lexema: \"[\", Token: Corchete Izquierdo\n";

                            //status = 0;
                            k++;
                        }

                        else if (c == ']')
                        {
                            lexms += "Lexema: \"]\", Token: Corchete Derecho\n";

                            //status = 0;
                            k++;
                        }

                        else if (c == ';')
                        {
                            lexms += "Lexema: \";\", Token: Punto y Coma\n";

                            //status = 0;
                            k++;
                        }

                        else if (c == ':')
                        {
                            lexms += "Lexema: \":\", Token: Dos Puntos\n";

                            //status = 0;
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
                            lexms += "Lexema: \"^\", Token: Xor Lógico\n";

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
                            lexms += "Lexema: \"/=\", Token: División Asignada\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"/\", Token: División\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 2:
                        if (c == '=')
                        {
                            lexms += "Lexema: \"*/\", Token: Producto Asignado\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"*\", Token: Producto\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 3:
                        if (Char.IsLetter(c))
                        {
                            buffer += c;

                            //status = 3;
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
                            lexms += "Lexema: \"" + buffer + "\", Token: Dígito\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 5:
                        if (c == '+')
                        {
                            lexms += "Lexema: \"++\", Token: Incremento\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else if (c == '=')
                        {
                            lexms += "Lexema: \"+=\", Token: Suma Asignada\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"+\", Token: Suma\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 6:
                        if (c == '-')
                        {
                            lexms += "Lexema: \"--\", Token: Decremento\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else if (c == '=')
                        {
                            lexms += "Lexema: \"-=\", Token: Resta Asignada\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"-\", Token: Resta\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 7:
                        if (c == '=')
                        {
                            lexms += "Lexema: \"%=\", Token: Módulo Asignado\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"%\", Token: Módulo\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 8:
                        if (c == '&')
                        {
                            lexms += "Lexema: \"&&\", Token: And Lógico\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"&\", Token: And Bitwise\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 9:
                        if (c == '|')
                        {
                            lexms += "Lexema: \"||\", Token: Or Lógico\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"%\", Token: Or Bitwise\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 10:
                        if (c == '=')
                        {
                            lexms += "Lexema: \"!=\", Token: Diferente de\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"!\", Token: Not Lógico\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 11:
                        if (c == '=')
                        {
                            lexms += "Lexema: \"==\", Token: Igual a\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"=\", Token: Asignación\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 12:
                        if (c == '=')
                        {
                            lexms += "Lexema: \"<=\", Token: Menor o Igual\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \"<\", Token: Menor que\n";
                            buffer = string.Empty;

                            status = 0;
                        }

                        break;

                    case 13:
                        if (c == '=')
                        {
                            lexms += "Lexema: \">=\", Token: Mayor o Igual\n";
                            buffer = string.Empty;

                            status = 0;
                            k++;
                        }

                        else
                        {
                            lexms += "Lexema: \">\", Token: Mayor que\n";
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
                            lexms += "Lexema: \"" + buffer + "\"\", Token: Cadena\n";
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
                            lexms += "Lexema: \"" + buffer + "\"\", Token: Cadena\n";
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
                            lexms += "Lexema: \"//" + buffer + "\"\", Token: Comentario\n";
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

                            //status = 17;
                            k++;
                        }

                        else
                        {
                            if (k + 1 != tbxCode.Text.Length)
                            {
                                if (tbxCode.Text[k + 1] == '/')
                                {
                                    lexms += "Lexema: \"/*" + buffer + "*/\", Token: Comentario de Párrafo\n";
                                    buffer = string.Empty;

                                    status = 0;
                                    k += 2;
                                }

                                else
                                {
                                    buffer += c;

                                    //status = 17;
                                    k++;
                                }
                            }

                            else
                            {
                                lexms += "Lexema: \"/*" + buffer + "*\", Token: Error\n";
                                buffer = string.Empty;

                                lblStatus.Content = "Error";
                                lblStatus.Foreground= Brushes.OrangeRed;
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
                if (status == 1)
                {
                    lexms += "Lexema: \"/\", Token: División\n";
                }
                else if (status == 2)
                {
                    lexms += "Lexema: \"*\", Token: Producto\n";
                }
                else if (status == 3)
                {
                    MatcherReservedWords(buffer);
                }
                else if (status == 4)
                {
                    lexms += "Lexema: \"" + buffer + "\", Token: Dígito\n";
                }
                else if (status == 5)
                {
                    lexms += "Lexema: \"+\", Token: Suma\n";
                }
                else if (status == 6)
                {
                    lexms += "Lexema: \"-\", Token: Resta\n";
                }
                else if (status == 7)
                {
                    lexms += "Lexema: \"%\", Token: Módulo\n";
                }
                else if (status == 8)
                {
                    lexms += "Lexema: \"&\", Token: And Bitwise\n";
                }
                else if (status == 9)
                {
                    lexms += "Lexema: \"|\", Token: Or Bitwise\n";
                }
                else if (status == 10)
                {
                    lexms += "Lexema: \"!\", Token: Not Lógico\n";
                }
                else if (status == 11)
                {
                    lexms += "Lexema: \"=\", Token: Asignación\n";
                }
                else if (status == 12)
                {
                    lexms += "Lexema: \"<\", Token: Menor que\n";
                }
                else if (status == 13)
                {
                    lexms += "Lexema: \">\", Token: Mayor que\n";
                }
                else if (status == 17)
                {
                    lexms += "Lexema: \"//" + buffer + "\", Token: Comentario\n";
                }
                else
                {
                    lexms += "Lexema: \"" + buffer + "\", Token: Error\n";

                    lblStatus.Content = "Error";
                    lblStatus.Foreground = Brushes.OrangeRed;
                }

                buffer = string.Empty;
            }

            tbxTokens.Text = lexms;
        }

        private void MatcherReservedWords(string s)
        {
            if (s.Length == 2)
            {
                switch (s)
                {
                    case "if":
                        lexms += "Lexema: \"" + s + "\", Token: If\n";
                        break;

                    case "do":
                        lexms += "Lexema: \"" + s + "\", Token: Do\n";
                        break;

                    case "Ln":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    default:
                        lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        break;
                }
            }

            if (s.Length == 3)
            {
                switch (s)
                {
                    case "int":
                        lexms += "Lexema: \"" + s + "\", Token: Int\n";
                        break;

                    case "for":
                        lexms += "Lexema: \"" + s + "\", Token: For\n";
                        break;

                    case "new":
                        lexms += "Lexema: \"" + s + "\", Token: New\n";
                        break;

                    case "try":
                        lexms += "Lexema: \"" + s + "\", Token: Try\n";
                        break;

                    case "Cos":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    case "Sin":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    case "Tan":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    case "Exp":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    case "Log":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    case "Pow":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    default:
                        lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        break;
                }
            }

            else if (s.Length == 4)
            {
                switch (s)
                {
                    case "bool":
                        lexms += "Lexema: \"" + s + "\", Token: Boolean\n";
                        break;

                    case "long":
                        lexms += "Lexema: \"" + s + "\", Token: Long\n";
                        break;

                    case "char":
                        lexms += "Lexema: \"" + s + "\", Token: Char\n";
                        break;

                    case "case":
                        lexms += "Lexema: \"" + s + "\", Token: Case\n";
                        break;

                    case "else":
                        lexms += "Lexema: \"" + s + "\", Token: Else\n";
                        break;

                    case "main":
                        lexms += "Lexema: \"" + s + "\", Token: Método Principal\n";
                        break;

                    case "null":
                        lexms += "Lexema: \"" + s + "\", Token: Null\n";
                        break;

                    case "this":
                        lexms += "Lexema: \"" + s + "\", Token: This\n";
                        break;

                    case "true":
                        lexms += "Lexema: \"" + s + "\", Token: True\n";
                        break;

                    case "void":
                        lexms += "Lexema: \"" + s + "\", Token: Void\n";
                        break;

                    case "Math":
                        lexms += "Lexema: \"" + s + "\", Token: Clase\n";
                        break;

                    case "Read":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    case "Sqrt":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    default:
                        lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        break;
                }
            }

            else if (s.Length == 5)
            {
                switch (s)
                {
                    case "float":
                        lexms += "Lexema: \"" + s + "\", Token: Float\n";
                        break;

                    case "break":
                        lexms += "Lexema: \"" + s + "\", Token: Break\n";
                        break;

                    case "catch":
                        lexms += "Lexema: \"" + s + "\", Token: Catch\n";
                        break;

                    case "class":
                        lexms += "Lexema: \"" + s + "\", Token: Class\n";
                        break;

                    case "false":
                        lexms += "Lexema: \"" + s + "\", Token: False\n";
                        break;

                    case "while":
                        lexms += "Lexema: \"" + s + "\", Token: While\n";
                        break;

                    case "using":
                        lexms += "Lexema: \"" + s + "\", Token: Using\n";
                        break;

                    default:
                        lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        break;
                }
            }

            else if (s.Length == 6)
            {
                switch (s)
                {
                    case "double":
                        lexms += "Lexema: \"" + s + "\", Token: Double\n";
                        break;

                    case "string":
                        lexms += "Lexema: \"" + s + "\", Token: Tipo String\n";
                        break;

                    case "return":
                        lexms += "Lexema: \"" + s + "\", Token: Return\n";
                        break;

                    case "public":
                        lexms += "Lexema: \"" + s + "\", Token: Public\n";
                        break;

                    case "static":
                        lexms += "Lexema: \"" + s + "\", Token: Static\n";
                        break;

                    case "switch":
                        lexms += "Lexema: \"" + s + "\", Token: Switch\n";
                        break;

                    case "System":
                        lexms += "Lexema: \"" + s + "\", Token: Namespace\n";
                        break;

                    case "Random":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    default:
                        lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        break;
                }
            }

            else if (s.Length == 7)
            {
                switch (s)
                {
                    case "decimal":
                        lexms += "Lexema: \"" + s + "\", Token: Decimal\n";
                        break;

                    case "private":
                        lexms += "Lexema: \"" + s + "\", Token: Private\n";
                        break;

                    case "Console":
                        lexms += "Lexema: \"" + s + "\", Token: Clase\n";
                        break;

                    default:
                        lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        break;
                }
            }

            else if (s.Length == 8)
            {
                switch (s)
                {
                    case "internal":
                        lexms += "Lexema: \"" + s + "\", Token: Internal\n";
                        break;

                    case "ReadLine":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    default:
                        lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        break;
                }
            }

            else if (s.Length == 9)
            {
                switch (s)
                {
                    case "Exception":
                        lexms += "Lexema: \"" + s + "\", Token: Clase\n";
                        break;

                    case "WriteLine":
                        lexms += "Lexema: \"" + s + "\", Token: Método\n";
                        break;

                    default:
                        lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
                        break;
                }
            }

            else
            {
                lexms += "Lexema: \"" + s + "\", Token:  Identificador\n";
            }
        }

        private void Reset()
        {
            lexms = "";
            status = 0;
            k = 0;

            tbxCode.Height = 300;
            tbxTokens.Visibility = Visibility.Visible;
        }
    }
}
