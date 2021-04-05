using ConnectionStringTest.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConnectionStringTest.UI
{
    public class ResultLabel : Label
    {
        private readonly IStringCutter _stringCutter;
        private readonly IThreadSafeHandler _threadSafeHandler;

        private string message = string.Empty;
        public string Message {
            get
            {
                return message;
            }
            set
            {
                message = value;
                _threadSafeHandler.WriteInLabel(this, _stringCutter.Cut(value, 145));
            }
        }

        private bool isErrorMessage = false;
        public bool IsErrorMessage
        {
            get
            {
                return isErrorMessage;
            }
            set
            {
                isErrorMessage = value;
                var color = isErrorMessage ? Color.Red : Color.Black;

                ForeColor = color;
            }
        }

        public ResultLabel(IStringCutter stringCutter, IThreadSafeHandler threadSafeHandler)
        {
            _stringCutter = stringCutter;
            _threadSafeHandler = threadSafeHandler;

            AutoSize = true;
            Location = new System.Drawing.Point(3, 34);
            Name = "testResultLabel";
            Size = new System.Drawing.Size(87, 13);
            TabIndex = 3;
            Text = "-Test result label-";
        }
    }
}
