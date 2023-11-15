using NoteDam.Properties;
using NoteDam.repositories;

namespace NoteDam
{
    public partial class NotedamView : Form, INotedamView
    {
        public string TextoNotepad { get => textoNotepad.Text; set => textoNotepad.Text = value; }
        public string LabelLineaColumna { get => lineaColumnaStatusStrip.Text; set => lineaColumnaStatusStrip.Text = value; }
        public Boolean EstadoBarra { get => barraMenuButton.Checked; set => barraMenuButton.Checked = value; }
        public Boolean AjusteLineaEstado { get => ajusteLineaMenuButton.Checked; set => ajusteLineaMenuButton.Checked = value; }
        public NotedamView()
        {
            InitializeComponent();
            Icon = new Icon("lapiz.ico");
            textoNotepad.ContextMenuStrip = menuContextMenu;
            this.Text = "Nuevo";
            AtachEventHandlers();

        }

        public event EventHandler SaveFile;
        public event EventHandler NewFile;
        public event EventHandler OpenFile;
        public event EventHandler CloseFile;
        public event EventHandler SaveFileAs;
        public event EventHandler PrintFile;
        public event EventHandler CopyText;
        public event EventHandler DeleteText;
        public event EventHandler CutText;
        public event EventHandler PasteText;
        public event EventHandler ChangeFont;
        public event EventHandler UpdatePosition;
        public event EventHandler EnableStatus;
        public event EventHandler AboutModel;
        public event EventHandler HelpLink;
        public event EventHandler ChangeBackground;
        public event EventHandler AjusteLinea;
        public event EventHandler UndoText;

        public event EventHandler CutRightMenu;
        public event EventHandler CopyRigtMenu;
        public event EventHandler DeleteRigtMenu;
        public event EventHandler PasteRightMenu;
        public event EventHandler UndoRightMenu;
        public event EventHandler SelectAllRightMenu;

        public event EventHandler CloseApp;

        private void AtachEventHandlers()
        {
            // Menu sobre texto
            copiarMenuButton.Click += (sender, e) => CopyText.Invoke(textoNotepad, e); ;
            cortarMenuButton.Click += (sender, e) => CutText.Invoke(textoNotepad, e);
            eliminarMenuButton.Click += (sender, e) => DeleteText.Invoke(textoNotepad, e);
            fuenteMenuButton.Click += (sender, e) => ChangeFont.Invoke(textoNotepad, e);
            pegarMenuButton.Click += (sender, e) => PasteText.Invoke(textoNotepad, e);
            textoNotepad.SelectionChanged += (sender, e) => UpdatePosition.Invoke(textoNotepad, e);
            imprimirMenuButton.Click += (sender, e) => PrintFile.Invoke(textoNotepad, e);
            barraMenuButton.Click += (sender, e) => EnableStatus.Invoke(statusStrip1, e);
            fondoMenuButton.Click += (sender, e) => ChangeBackground.Invoke(textoNotepad, e);
            ajusteLineaMenuButton.Click += (sender, e) => AjusteLinea.Invoke(textoNotepad, e);
            deshacerMenuButton.Click += (sender, e) => UndoText.Invoke(textoNotepad, e);

            // Menu sobre Archivo
            nuevoMenuButton.Click += (sender, e) => NewFile.Invoke(sender, e);
            guardarMenuButton.Click += (sender, e) => SaveFile.Invoke(sender, e);
            abrirMenuButton.Click += (sender, e) => OpenFile.Invoke(sender, e);
            guardarComoMenuButton.Click += (sender, e) => SaveFileAs.Invoke(sender, e);
            acercaMenuButton.Click += (sender, e) => AboutModel.Invoke(sender, e);
            ayudaMenuButton.Click += (sender, e) => HelpLink.Invoke(sender, e);

            // Menu contextual
            copiarRightMenu.Click += (sender, e) => CopyText.Invoke(textoNotepad, e); ;
            cortarRightMenu.Click += (sender, e) => CutText.Invoke(textoNotepad, e);
            eliminarRightMenu.Click += (sender, e) => DeleteText.Invoke(textoNotepad, e);
            pegarRightMenu.Click += (sender, e) => PasteText.Invoke(textoNotepad, e);
            deshacerRightMenu.Click += (sender, e) => UndoText.Invoke(textoNotepad, e);
            seleccionarTodoRightMenu.Click += (sender, e) => SelectAllRightMenu.Invoke(textoNotepad, e);

            FormClosing += (sender, e) => CloseApp(sender,e);

        }
    }
}