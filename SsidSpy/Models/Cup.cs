using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsidSpy.Models
{
    public class Cup
    {
        public Color Color { get; set; }
        public int CupType { get; set; }

        private float _percent;
        public Cup()
        {
            _percent = 100;
        }
        public float Percent()
        {
            return _percent;
        }

        public void Drink()
        {
            _percent -= 10;
        }
    }

    public enum Color
    {
        Black,
        Blue,
        Pink,
        Red,
        White
    }
}
