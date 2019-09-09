using System.Collections.Generic;

namespace Valeo.Bot.Data.Repository
{
    public static class UziInfo
    {
        private static readonly Dictionary<string, string> _descriptions = new Dictionary<string, string>();
        static UziInfo()
        {
            _descriptions.Add("usibrush", "Опис УЗД органів черевної порожнини");
            _descriptions.Add("usizabrush", "Опис УЗД органів зачеревної порожнини");
            _descriptions.Add("usishitov", "Опис УЗД органів щитовидної залози");
            _descriptions.Add("usimoloch", "Опис УЗД молочної залози");
            _descriptions.Add("usimyah", "Опис УЗД м'яких тканин");
            _descriptions.Add("usilimfous", "Опис УЗД лімфовузлів");
            _descriptions.Add("usitasa", "Опис УЗД органців тазу");
            _descriptions.Add("usiserdsa", "Опис УЗД серця");
            _descriptions.Add("usisosudniz", "Опис УЗД судин нижніх кінцівок");
            _descriptions.Add("usisosudverh", "Опис УЗД судин верхніх кінцівок");
            _descriptions.Add("usisosudshei", "Опис УЗД судин шиї та голови");
            _descriptions.Add("usineyro", "Опис УЗД нейросонографія");
        }

        public static string GetDescription(string command)
        {
            return _descriptions[command];
        }
    }
}