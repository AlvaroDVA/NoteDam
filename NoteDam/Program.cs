using NoteDam.models;
using NoteDam.repositories;

namespace NoteDam
{
    internal static class Program
    {

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            var view = new NotedamView();
            var mode = new TextoModel();
            _ = new NotedamPresenter(view, mode);
            Application.Run(view);
        }
    }
}