// ReSharper disable once CheckNamespace
namespace UnityTools.SceneManagement.Style
{
    public static class SceneManagementStyles
    {
        public static Header Header;
        public static PaneOption PaneOption;

        static SceneManagementStyles()
        {
            Header = new Header();
            PaneOption = new PaneOption();
        }
    }
}