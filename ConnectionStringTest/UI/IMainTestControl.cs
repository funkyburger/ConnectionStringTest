using ConnectionStringTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionStringTest.UI
{
    public interface IMainTestControl
    {
        string ConnectionString { get; }
        string Message { get; }
        bool IsActionButtonEnabled { get; set; }
        void SetStatus(TestStatus status);
        void UpdateTimer(TimeSpan elapsedTime);
        void DisplayMessage(string message, bool success = true);
        void RefreshAutoComplete();
    }
}
