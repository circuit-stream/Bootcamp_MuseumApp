using UnityEditor;

namespace Database.Editor
{
    public static class DatabaseEditor
    {
        [MenuItem("Database/Clear Database")]
        private static void ClearDatabase()
        {
            MuseumApp.Database.ClearDatabase();
        }
    }
}