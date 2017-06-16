using System;
using System.Collections.Generic;

namespace PrzegladBazy
{
    /// <summary>
    /// Klasa ta przedstawia grupê s³owników, które potem s¹ ³adowane w g³ónym oknie programu.
    /// </summary>
    [Serializable]
    public class SlownikGroup
    {
        /// <summary>
        /// Lista identyfikatórów dla ka¿dego s³ownika w grupie. Jeden identyfikator 
        /// - jeden s³ownik
        /// </summary>
        public List<string> Slowniki;
        /// <summary>
        /// Tytu³ grupy, po której bêdzie ona identyfikowana (ta nazwa musi byæ unikalna).
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