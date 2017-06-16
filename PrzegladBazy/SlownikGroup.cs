using System;
using System.Collections.Generic;

namespace PrzegladBazy
{
    /// <summary>
    /// Klasa ta przedstawia grup� s�ownik�w, kt�re potem s� �adowane w g��nym oknie programu.
    /// </summary>
    [Serializable]
    public class SlownikGroup
    {
        /// <summary>
        /// Lista identyfikat�r�w dla ka�dego s�ownika w grupie. Jeden identyfikator 
        /// - jeden s�ownik
        /// </summary>
        public List<string> Slowniki;
        /// <summary>
        /// Tytu� grupy, po kt�rej b�dzie ona identyfikowana (ta nazwa musi by� unikalna).
        /// </summary>
        public string Title;
        /// <summary>
        /// Konstruktor
        /// </summary>
        public SlownikGroup()
        {
            Title = "noname";
            Slowniki = new List<string>();
        }
        /// <summary>
        /// ToString.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Title;
        }
    }
}