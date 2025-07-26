using System.Collections.Generic;

namespace Project.Scripts.Services
{
    public static class EnemyNames
    {
        private static List<string> _englishNames = new List<string>
        {
            "Antony_124",
            "John_being",
            "Darchi^_^",
            "YourAstro",
            "Killer_8",
            "Porshe911",
            "S1mple",
            "Y0urWay",
            "ILovU",
            "\\UWU//",
        };
        
        private static List<string> _russianNames = new List<string>
        {
            "Вован_2007",
            "Пушистик^^",
            "Киллер_007",
            "Танкист_",
            "Няшка_",
            "Череп_666",
            "БоТаныЧ",
            "Максимка",
            "Люся<3",
            "\\ОГО//",
            "Санек_",
            "Капитан",
            "Гриша_ТТ",
            "Валерчик",
            "Zайка",
        };

        private static List<string> _turkishNames = new List<string>
        {
            "Kral_TR",
            "Delikanlı",
            "Aşkım_<3",
            "Ölümcül",
            "Fırtına",
            "Kaplan_",
            "Şahin_01",
            "Yıldız_",
            "Kuzu_^^",
            "//ASLAN\\",
            "Reis_",
            "Türkoglu",
            "Savaşçı",
            "Bebek_",
            "Karanlık",
        };
        
        public static string GetRandomEnglishName()
        {
            return _englishNames[UnityEngine.Random.Range(0, _englishNames.Count)];
        }

        public static string GetRandomRussianName()
        {
            return _russianNames[UnityEngine.Random.Range(0, _russianNames.Count)];
        }

        public static string GetRandomTurkishName()
        {
            return _turkishNames[UnityEngine.Random.Range(0, _turkishNames.Count)];
        }
    }
}