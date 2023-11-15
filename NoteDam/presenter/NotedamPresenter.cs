using NoteDam.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteDam.repositories
{
    internal class NotedamPresenter
    {
        string? FilePath { get; set; }
        string Titulo { get => view.Text ; set => view.Text = value; }
        string TextoEscrito { get => model.TextoNotepad; set => model.TextoNotepad = value; }
        string PosicionTexto { get => view.LabelLineaColumna; set => view.LabelLineaColumna = value; }
        public Boolean AjusteLineaEstado { get => view.AjusteLineaEstado; set => view.AjusteLineaEstado = value; }

        private NotedamView view;
        private TextoModel model;

        public NotedamPresenter(NotedamView view, TextoModel model)
        {
            this.view = view;
            this.model = model;

            AtachEventHandlers();

            PosicionTexto = $"Línea 0 , Columna 0 ";
            model.TextoNotepad = view.TextoNotepad;
        }

        private void AtachEventHandlers()
        {
            view.NewFile += (_, _) => NewFile();
            view.SaveFile += (_, _) => SaveFile();
            view.OpenFile += (_, _) => OpenFile();
            view.SaveFileAs += (_, _) => SaveFileAs();
            view.AboutModel += (_, _) => AboutModel();
            view.HelpLink += (_, _) => HelpLink();

            view.PrintFile += (sender, _) => PrintFile(sender);
            view.ChangeFont += (sender, _) => ChangeFont(sender);
            view.ChangeBackground += (sender, _) => ChangeBackground(sender);
            view.PasteText += (sender, _) => PasteText(sender);
            view.CutText += (sender, _) => CutText(sender);
            view.DeleteText += (sender, _) => DeleteText(sender);
            view.CopyText += (sender, _) => CopyText(sender);
            view.UndoText += (sender, _) => UndoTexto(sender);
            view.UpdatePosition += (sender, _) => UpdatePosition(sender);
            view.EnableStatus += (sender, _) => EnableSatus(sender);
            view.AjusteLinea += (sender, _) => AjusteLinea(sender);

            view.PasteRightMenu += (sender, _) => PasteText(sender);
            view.CutRightMenu += (sender, _) => CutText(sender);
            view.DeleteRigtMenu += (sender, _) => DeleteText(sender);
            view.CopyRigtMenu += (sender, _) => CopyText(sender);
            view.UndoRightMenu += (sender, _) => UndoTexto(sender);
            view.SelectAllRightMenu += (sender, _) => SelectAllText(sender);

            view.CloseApp += (sender, e) => CloseApp(sender,e as FormClosingEventArgs);
        }

        private void CloseApp(object sender, FormClosingEventArgs e)
        {
            TextoEscrito = view.TextoNotepad;
            if (!string.IsNullOrEmpty(TextoEscrito))
            {
                var res = PreguntarGuardar();
                if (res == DialogResult.Yes)
                {

                    if (!SaveFile())
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }


        private void SelectAllText(object sender)
        {
            var richBox = sender as RichTextBox;
            richBox.SelectAll();
        }

        private void UndoTexto(object sender)
        {
            var richBox = sender as RichTextBox;
            richBox.Undo();
        }

        private void AjusteLinea(object sender)
        {
            var richBox = sender as RichTextBox;
            if (AjusteLineaEstado)
            {
                richBox.WordWrap = false;
                AjusteLineaEstado = false;
            }
            else
            {
                richBox.WordWrap = true;
                AjusteLineaEstado = true;
            }
        }

        private void ChangeBackground(object sender)
        {
            var richBox = sender as RichTextBox;

            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color selectedColor = colorDialog.Color;

                richBox.BackColor = selectedColor;
            }
        }

        private void AboutModel() => MessageBox.Show("Creado por Alvaro Del Val Arce", "Acerca de" , MessageBoxButtons.OK);

        private void HelpLink()
        {
            
            ProcessStartInfo proceso = new ProcessStartInfo
            {
                FileName = "https://apps.microsoft.com/detail/9MSMLRH6LZF3?hl=es-es&gl=ES",
                UseShellExecute = true
            };

            Process.Start(proceso);
           
        }

        private void EnableSatus(object sender)
        {
            var grip = sender as StatusStrip;
            if (grip.Visible)
            {
                grip.Visible = false;
                view.EstadoBarra = false;
            }else
            {
                grip.Visible = true;
                view.EstadoBarra = true;
            }
        }

        private void UpdatePosition(object sender)
        {
            var richBox = sender as RichTextBox;

            int posicionCursor = richBox.SelectionStart;
            int linea = richBox.GetLineFromCharIndex(posicionCursor);
            int columna = posicionCursor - richBox.GetFirstCharIndexFromLine(linea);

            PosicionTexto = $"Línea {linea} , Columna {columna} ";
        }

        private void CopyText(object sender)
        {
            var richBox = sender as RichTextBox;
            richBox.Copy();
        }

        private void DeleteText(object sender)
        {
            var richBox = sender as RichTextBox;
            richBox.SelectedText = string.Empty;
        }

        private void CutText(object sender)
        {
            var richBox = sender as RichTextBox;
            richBox.Cut();
        }

        private void PasteText (object sender)
        {
            // Confirmamos que vamos a pegar solo texto
                if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text))
                {
                    string? textoPegado = Clipboard.GetData(DataFormats.Text) as string;

                    if (textoPegado != null)
                    {
                    var textoNotepad = sender as RichTextBox;
                    textoNotepad.Paste();
                    }
                }
        }

        private void ChangeFont(object sender)
        {
            var richText = sender as RichTextBox;
            using (FontDialog fontDialog = new FontDialog())
            {
                fontDialog.ShowColor = true;

                DialogResult result = fontDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    richText.ForeColor = fontDialog.Color;
                    richText.Font = fontDialog.Font;
                }
            }
        }

        private void PrintFile(object sender)
        {
            TextoEscrito = view.TextoNotepad;
            var richText = sender as RichTextBox;
            using (PrintDialog printDialog = new PrintDialog())
            {
                printDialog.AllowSomePages = true;
                printDialog.ShowHelp = true;

                DialogResult result = printDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    using (PrintDocument printDocument = new PrintDocument())
                    {
                        printDocument.PrintPage += (sender, e) =>
                        {
                            e.Graphics.DrawString(TextoEscrito, richText.Font, new SolidBrush(richText.ForeColor), new PointF(100, 100));
                        };

                        printDocument.PrinterSettings = printDialog.PrinterSettings;

                        printDocument.Print();
                    }
                }
            }
        }

        private Boolean SaveFileAs()
        {
            TextoEscrito = view.TextoNotepad;
            SaveFileDialog saveDialog = new SaveFileDialog();
            if (TextoEscrito.Length > 0)
            {
                saveDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                saveDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                saveDialog.FilterIndex = 1;
                saveDialog.RestoreDirectory = false;
                var resultado = saveDialog.ShowDialog() == DialogResult.OK;
                if (resultado)
                {
                    File.WriteAllText(saveDialog.FileName, TextoEscrito);
                    Titulo = Path.GetFileNameWithoutExtension(saveDialog.FileName);
                    FilePath = saveDialog.FileName;
                    return true;
                }
            }
            return false;
        }

        private void OpenFile()
        {
            TextoEscrito = view.TextoNotepad;
           if (!string.IsNullOrEmpty(TextoEscrito))
           {
                var respuestaDialog = PreguntarGuardar();
                if (respuestaDialog == DialogResult.Yes)
                {

                    SaveFile();
                } 
                else if (respuestaDialog == DialogResult.Cancel)
                {
                    return;
                }
            }
            
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.Title = "Seleccionar archivo de texto";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    FilePath = openFileDialog.FileName;
                    TextoEscrito = File.ReadAllText(FilePath);
                    view.TextoNotepad = TextoEscrito;
                    Titulo = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                }
            }
        }

        private DialogResult PreguntarGuardar() => MessageBox.Show(
                        "¿Quieres guardar el archivo actual?",
                        "Guardar",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question
                    );

        private void NewFile()
        {
            var res1 = TextoEscrito;
            TextoEscrito = view.TextoNotepad;
            // Comparar el resultado anterior con el nuevo, si no son iguales , se pedira si se quiere guardar
            var res2 = TextoEscrito;
            // Necesario inicializarlo en Cancel para que no haya problemas al darle a cancelar
            DialogResult resultado = DialogResult.Cancel;
            if (!string.IsNullOrEmpty(TextoEscrito))
            {
                if ((res1 == res2 && resultado == DialogResult.Cancel))
                {
                    LimpiarInterfaz();
                }else
                {
                    resultado = PreguntarGuardar();

                    if (resultado == DialogResult.Yes)
                    {
                        if (SaveFile())
                        {
                            LimpiarInterfaz();
                        }
                    }
                    else if (resultado == DialogResult.No)
                    {
                        LimpiarInterfaz();
                    }
                }
               
            }
        }

        void LimpiarInterfaz ()
        {
            Titulo = "Nuevo";
            TextoEscrito = string.Empty;
            FilePath = null;
            view.TextoNotepad = TextoEscrito;
        }

        Boolean SaveFile()
        {
           
            TextoEscrito = view.TextoNotepad;

            SaveFileDialog saveDialog = new SaveFileDialog();
            if (FilePath is null || Titulo == "Nuevo")
            {
                return SaveFileAs();
            }
            else
            {
                if (File.Exists(FilePath))
                {
                    File.WriteAllText(FilePath, TextoEscrito);
                    return true;
                }else
                {
                    FilePath = null;
                    SaveFile();
                }
            }
            view.TextoNotepad = TextoEscrito;
            return false;

        }
    }
}
