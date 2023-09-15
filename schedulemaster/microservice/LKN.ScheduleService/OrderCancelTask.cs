using Hos.ScheduleMaster.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LKN.ScheduleService
{
    public class OrderCancelTask : TaskBase
    {
        public override void Run(TaskContext context)
        {
            // 1、超时定时逻辑
            context.WriteLog("回收超时订单......成功");
        }
    }
}
