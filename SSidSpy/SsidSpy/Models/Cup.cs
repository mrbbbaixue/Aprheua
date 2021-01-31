using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsidSpy.Models
{
    public class 杯子
    {
        public 颜色 杯子颜色 { get; set; }
        public int 杯子类型 { get; set; }

        private float _水量;
        public 杯子()
        {
            _水量 = 100;
        }
        public float 水量()
        {
            return _水量;
        }

        public void 被喝一口()
        {
            _水量 -= 10;
        }
    }

    public enum 颜色
    {
        黑色,
        白色,
        粉色,
        蓝色,
        黄色
    }
}
