using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteDam.repositories
{
    internal interface INotedamView
    {

        string TextoNotepad { get; set; }

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

    }
}
